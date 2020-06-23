using System;

namespace NP.Concepts.CodeMetaData
{
    public interface IMethodSignatureInfo
    {
        public ParamInfo[] InputParamInfos
        {
            get;
            set;
        }

        public Type OutputType { get; }

        public void SetOutputType(Type outputType);
    }
}
