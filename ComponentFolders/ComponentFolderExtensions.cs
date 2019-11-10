using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace NP.Concepts.ComponentFolders
{
    public static class ComponentFolderExtensions
    {
        private static IEnumerable<ComponentDisplayMetadata> ToChildren<TId>(this ComponentDisplayMetadata componentDisplayMetadata)
            where TId : INameContainer
        {
            if (componentDisplayMetadata is ComponentFolder<TId> componentFolder)
            {
                return componentFolder.FoldersAndComponents;
            }

            return Enumerable.Empty<ComponentDisplayMetadata>();
        }

        public static IEnumerable<ComponentIdWithDisplayMetadata<TId>> GetAllComponents<TId>(this ComponentFolder<TId> folder)
             where TId : INameContainer
        {
            return folder.Descendants<ComponentDisplayMetadata, ComponentIdWithDisplayMetadata<TId>>(ToChildren<TId>);
        }

        public static (ComponentFolder<TId> componentFolder, ComponentIdWithDisplayMetadata<TId> componentIdWithDisplayMetadata)
            GetItemAndContainingFolder<TId>(this ComponentFolder<TId> folder, string componentName)
            where TId : INameContainer
        {
            TreeNodeInfo<ComponentDisplayMetadata> treeNodeInfo =
                folder.SelfAndDescendantsWithLevelInfo<ComponentDisplayMetadata>(null, ToChildren<TId>)
                    .FirstOrDefault(item => item.Node is ComponentIdWithDisplayMetadata<TId> compId && compId.TheComponentId.Name == componentName);

            return (treeNodeInfo?.Parent as ComponentFolder<TId>, treeNodeInfo?.Node as ComponentIdWithDisplayMetadata<TId>);
        }

        public static void RemoveDescendantNodeByName<TId>(this ComponentFolder<TId> folder, string bbName)
             where TId : INameContainer
        {
            (ComponentFolder<TId> bbFolder, ComponentIdWithDisplayMetadata<TId> bbIdWithDisplayMetadata) =
                folder.GetItemAndContainingFolder(bbName);

            if (bbIdWithDisplayMetadata != null)
            {
                bbFolder?.ComponentInfos?.Remove(bbIdWithDisplayMetadata);
            }
        }
    }
}
