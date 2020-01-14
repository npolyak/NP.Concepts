using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class DatumAssemblyAttribute : ComponentMetadataAttributeBase
    {
        public DatumAssemblyAttribute
        (
            string icon = null,
            string displayName = null,
            string description = null)
            : base(icon, displayName, description)
        {
        }
    }
}
