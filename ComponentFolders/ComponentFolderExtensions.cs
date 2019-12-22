using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace NP.Concepts.ComponentFolders
{
    public static class ComponentFolderExtensions
    {
        private static IEnumerable<IComponentMetaDataContainer<TMetaData>> ToChildren<TId, TMetaData>(this IComponentMetaDataContainer<TMetaData> componentDisplayMetadata)
            where TId : INameContainer
            where TMetaData : class, IComponentDisplayMetadata
        {
            if (componentDisplayMetadata is ComponentFolder<TId, TMetaData> componentFolder)
            {
                return componentFolder.FoldersAndComponents;
            }

            return Enumerable.Empty<IComponentMetaDataContainer<TMetaData>>();
        }

        public static IEnumerable<IComponentIdWithDisplayMetadata<TId, TMetaData>> GetAllNonFolderComponents<TId, TMetaData>(this IComponentMetaDataContainer<TMetaData> folder)
            where TId : INameContainer
            where TMetaData : class, IComponentDisplayMetadata 
        {
            return folder.Descendants <IComponentMetaDataContainer<TMetaData>, IComponentIdWithDisplayMetadata<TId, TMetaData>> (ToChildren<TId, TMetaData>);
        }

        public static (ComponentFolder<TId, TMetaData> componentFolder, IComponentIdWithDisplayMetadata<TId, TMetaData> componentIdWithDisplayMetadata)
            GetItemAndContainingFolder<TId, TMetaData>(this ComponentFolder<TId, TMetaData> folder, string componentName)
            where TId : INameContainer
            where TMetaData : class, IComponentDisplayMetadata
        {
            TreeNodeInfo<IComponentMetaDataContainer<TMetaData>> treeNodeInfo =
                folder.SelfAndDescendantsWithLevelInfo<IComponentMetaDataContainer<TMetaData>>(null, ToChildren<TId, TMetaData>)
                    .FirstOrDefault(item => item.Node is IComponentIdWithDisplayMetadata<TId, TMetaData> compId && compId.TheComponentId.Name == componentName);

            return (treeNodeInfo?.Parent as ComponentFolder<TId, TMetaData>, treeNodeInfo?.Node as IComponentIdWithDisplayMetadata<TId, TMetaData>);
        }

        public static void RemoveDescendantNodeByName<TId, TMetaData>(this ComponentFolder<TId, TMetaData> folder, string bbName)
            where TId : INameContainer
            where TMetaData : class, IComponentDisplayMetadata
        {
            (ComponentFolder<TId, TMetaData> bbFolder, IComponentIdWithDisplayMetadata<TId, TMetaData> bbIdWithDisplayMetadata) =
                folder.GetItemAndContainingFolder(bbName);

            if (bbIdWithDisplayMetadata != null)
            {
                bbFolder?.ComponentInfos?.Remove(bbIdWithDisplayMetadata);
            }
        }
    }
}
