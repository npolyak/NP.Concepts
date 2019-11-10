using NP.Utilities.BasicInterfaces;
using System.Collections.Generic;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentIdWithDisplayMetadata<TId> : ComponentDisplayMetadata
        where TId : INameContainer
    {
        internal static Dictionary<TId, ComponentDisplayMetadataBase> MetadataMap { get; } =
            new Dictionary<TId, ComponentDisplayMetadataBase>();

        public override bool IsFolder => false;

        public TId TheComponentId { get; }

        public ComponentIdWithDisplayMetadata(TId componentId)
            :
            base(componentId.GetDisplayMetadata())
        {
            TheComponentId = componentId;
        }
    }

    public static class ComponentIdExtensions
    {
        public static void AddDisplayMetaData<TId>
        (
            this TId componentId,
            string icon,
            string displayName = null,
            string description = null
        )
            where TId : INameContainer
        {
            ComponentDisplayMetadataBase componentDisplayMetadata =
                new ComponentDisplayMetadataBase(displayName ?? componentId.Name, icon, description);

            ComponentIdWithDisplayMetadata<TId>.MetadataMap[componentId] = componentDisplayMetadata;
        }


        public static ComponentDisplayMetadataBase GetDisplayMetadata<TId>(this TId componentId) 
            where TId : INameContainer
        {
            if ( ComponentIdWithDisplayMetadata<TId>.MetadataMap.TryGetValue
                 (
                     componentId,
                     out ComponentDisplayMetadataBase md))
            {
                return md;
            }

            return null;
        }
    }
}
