using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace NP.Concepts.ComponentFolders
{
    public static class ComponentFolderExtensions
    {
        private static IEnumerable<IComponentMetaDataContainer> ToChildren<TId, TFolderMetaData, TComponentMetaData>(this IComponentMetaDataContainer componentDisplayMetadata)
            where TId : INameContainer
            where TFolderMetaData : class, IComponentDisplayMetadata
            where TComponentMetaData : class, IComponentDisplayMetadata
        {
            if (componentDisplayMetadata is ComponentFolder<TId, TFolderMetaData, TComponentMetaData> componentFolder)
            {
                return componentFolder.FoldersAndComponents;
            }

            return Enumerable.Empty<IComponentMetaDataContainer>();
        }

        public static IEnumerable<IComponentIdWithDisplayMetadata<TId, TComponentMetaData>> GetAllNonFolderComponents<TId, TFolderMetaData, TComponentMetaData>(this ComponentFolder<TId, TFolderMetaData, TComponentMetaData> folder)
            where TId : INameContainer
            where TFolderMetaData : class, IComponentDisplayMetadata
            where TComponentMetaData : class, IComponentDisplayMetadata
        {
            return folder.Descendants<IComponentMetaDataContainer, IComponentIdWithDisplayMetadata<TId, TComponentMetaData>> (ToChildren<TId, TFolderMetaData, TComponentMetaData>);
        }

        public static (ComponentFolder<TId, TFolderMetaData, TComponentMetaData> componentFolder, IComponentIdWithDisplayMetadata<TId, TComponentMetaData> componentIdWithDisplayMetadata)
            GetItemAndContainingFolder<TId, TFolderMetaData, TComponentMetaData>(this ComponentFolder<TId, TFolderMetaData, TComponentMetaData> folder, string componentName)
            where TId : INameContainer
            where TFolderMetaData : class, IComponentDisplayMetadata
            where TComponentMetaData : class, IComponentDisplayMetadata
        {
            TreeNodeInfo<IComponentMetaDataContainer> treeNodeInfo =
                folder.SelfAndDescendantsWithLevelInfo<IComponentMetaDataContainer>(null, ToChildren<TId, TFolderMetaData, TComponentMetaData >)
                    .FirstOrDefault(item => item.Node is IComponentIdWithDisplayMetadata<TId, TComponentMetaData> compId && compId.TheComponentId.Name == componentName);

            return (treeNodeInfo?.Parent as ComponentFolder<TId, TFolderMetaData, TComponentMetaData>, treeNodeInfo?.Node as IComponentIdWithDisplayMetadata<TId, TComponentMetaData>);
        }

        public static void RemoveDescendantNodeByName<TId, TFolderMetaData, TComponentMetaData>(this ComponentFolder<TId, TFolderMetaData, TComponentMetaData> folder, string bbName)
            where TId : INameContainer
            where TFolderMetaData : class, IComponentDisplayMetadata
            where TComponentMetaData : class, IComponentDisplayMetadata
        {
            (ComponentFolder<TId, TFolderMetaData, TComponentMetaData> bbFolder, IComponentIdWithDisplayMetadata<TId, TComponentMetaData> bbIdWithDisplayMetadata) =
                folder.GetItemAndContainingFolder(bbName);

            if (bbIdWithDisplayMetadata != null)
            {
                bbFolder?.ComponentInfos?.Remove(bbIdWithDisplayMetadata);
            }
        }
    }
}
