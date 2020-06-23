namespace NP.Concepts.CodeMetaData
{
    public class MethodParamInfo : ParamInfo
    {
        object _paramValue;
        public object ParamValue
        {
            get => _paramValue;
            set
            {
                if (_paramValue == value)
                {
                    return;
                }

                _paramValue = value;
            }
        }
    }
}
