using NP.Concepts.ComponentFolders;
using System;

namespace NP.Concepts.DatumAttributes
{
    public class ComponentMetadataAttributeBase : Attribute, IComponentDisplayMetadata
    {
        public string Icon { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public ComponentMetadataAttributeBase
        (
            string icon = null,
            string displayName = null,
            string description = null)
        {
            Icon = icon;

            DisplayName = displayName;

            Description = description;
        }
    }
}
