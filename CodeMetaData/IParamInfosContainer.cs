using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NP.Concepts.CodeMetaData
{
    public interface IParamInfosContainer
    {
        ParamInfo[] InputParamInfos { get; }

        public bool NoInputParams => InputParamInfos.IsNullOrEmpty();

        public bool SingleParamInfo => InputParamInfos?.Length == 1;

        public Type ReturnType { get; }

        public IEnumerable<Type> GetTypes()
        {
            return InputParamInfos?.Select(paramInfo => paramInfo.ParamType);
        }

        public object[] ToArgs(object val)
        {
            object[] args;
            if (NoInputParams)
            {
                args = new object[] { };
            }
            else if (SingleParamInfo && (!(val is IEnumerable)))
            {
                args = new object[] { val };
            }
            else
            {
                args = ((IEnumerable)val).Cast<object>().ToArray();
            }

            return args;
        }

        public string TypeAndNamesStr
        {
            get
            {
                string result = "";
                if (!NoInputParams)
                {
                    result = string.Join(", ", InputParamInfos.Select(paramInfo => paramInfo.DisplayStr));
                }

                return result;
            }
        }

        public string TypeAndNameStrWithReturnType
        {
            get
            {
                string postFix = "";
                if (!ReturnType.IsVoid())
                {
                    postFix = $": {ReturnType.Name.UnBox()}";
                }

                return $"({TypeAndNamesStr}){postFix}";
            }
        }

        public string InputTypesStr
        {
            get
            {
                string result = "";
                if (!NoInputParams)
                {
                    result = string.Join(", ", this.GetTypes().Select(t => t.Name.UnBox()));
                }

                return result;
            }
        }

        public string DisplayTypeStr
        {
            get
            {
                string prefix = "Action";
                string postFix = "";
                string separator = "";

                if (!ReturnType.IsVoid())
                {
                    prefix = "Func";
                    postFix = $"{ReturnType.Name.UnBox()}";

                    if (!NoInputParams)
                    {
                        separator = ", ";
                    }
                }


                string result = prefix;
                if (!NoInputParams || !ReturnType.IsVoid())
                {
                    result += $"<{InputTypesStr}{separator}{postFix}> ";
                }

                return result;
            }
        }
    }

    public interface IParamInfosContainerAndTrigger : IParamInfosContainer, ITrigger
    {

    }

    public interface ITriggerableParamInfosContainerAndTrigger
        :
        IParamInfosContainerAndTrigger, 
        ITriggerable
    {

    }
}
