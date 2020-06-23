using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodDatumProcessorAttribute : ComponentMetadataAttributeBase
    {
        public MethodDatumProcessorAttribute
        (
            string icon = null,
            string displayName = null,
            string description = null
        ) :
            base(icon, displayName, description)
        {

        }
    }
}
