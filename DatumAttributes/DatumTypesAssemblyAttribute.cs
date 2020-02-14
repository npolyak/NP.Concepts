using NP.Concepts.DatumAttributes;
using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class DatumTypesAssemblyAttribute : ComponentMetadataAttributeBase
    {
        public DatumTypesAssemblyAttribute
        (
            string icon = null,
            string displayName = null,
            string description = null)
            : base(icon, displayName, description)
            {
            }
    }
}
