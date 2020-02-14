using NP.Utilities;
using System;
using System.Xml.Serialization;

namespace NP.Concepts.CodeGeneration
{
    public class ParamInfo : VMBase
    {
        Type _paramType;
        [XmlIgnore]
        public Type ParamType
        {
            get => _paramType;
            set
            {
                //if (_paramType.ObjEquals(value))
                //{
                //    return;
                //}

                _paramType = value;

                OnPropertyChanged(nameof(ParamType));
            }
        }

        string _paramName;
        [XmlAttribute]
        public string ParamName 
        {
            get => _paramName;
            
            set
            {
                if (_paramName.ObjEquals(value))
                {
                    return;
                }

                _paramName = value;

                OnPropertyChanged(nameof(ParamName));
            }
        }

        [XmlElement]
        public string ParamAssemblyQualifiedTypeName
        {
            get => ParamType?.AssemblyQualifiedName;

            set
            {
                ParamType = value?.GetTypeByAssemblyQualifiedName();
            }
        }

        public string ParamStr =>
            GetParamStr();

        public string GetParamStr(Func<Type, string> typeToStr = null)
        {
            return $"{ParamType?.GetFullTypeName(typeToStr)} {ParamName}";
        }

        public bool Matches(ParamInfo paramInfo)
        {
            return this.ParamType == paramInfo.ParamType &&
                   this.ParamName == paramInfo.ParamName;
        }

        public ParamInfo()
        {

        }

        public ParamInfo(Type paramType, string paramName)
        {
            ParamType = paramType;
            ParamName = paramName;
        }

        public ParamInfo(ParamInfo paramInfo) : this(paramInfo.ParamType, paramInfo.ParamName)
        {

        }
    }
}
