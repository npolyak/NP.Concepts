using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class DatumProcessorAttribute : ComponentMetadataAttributeBase
    {
        public DatumProcessorAttribute
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
