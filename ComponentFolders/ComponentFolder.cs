using NP.Concepts.Behaviors;
using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentFolder<TId> : ComponentDisplayMetadata
        where TId : INameContainer
    {
        public override bool IsFolder => true;

        public ObservableCollection<ComponentFolder<TId>> SubFolders { get; } =
            new ObservableCollection<ComponentFolder<TId>>();

        public ObservableCollection<ComponentIdWithDisplayMetadata<TId>> ComponentInfos { get; } =
            new ObservableCollection<ComponentIdWithDisplayMetadata<TId>>();

        public ObservableCollection<ComponentDisplayMetadata> FoldersAndComponents { get; } =
            new ObservableCollection<ComponentDisplayMetadata>();

        public ComponentFolder<TId> FindSubFolder(string subFolderName)
        {
            return SubFolders
                       .FirstOrDefault(f => f.DisplayName == subFolderName);
        }

        public void Add(TId componentId)
        {
            ComponentIdWithDisplayMetadata<TId> componentInfo =
                new ComponentIdWithDisplayMetadata<TId>(componentId);

            ComponentInfos.Add(componentInfo);
        }

        public ComponentFolder<TId> AddSubFolder
        (
            string name,
            string icon = null,
            string description = null)
        {
            ComponentFolder<TId> subFolder =
                new ComponentFolder<TId>(name, icon, description);

            SubFolders.Add(subFolder);

            return subFolder;
        }

        public ComponentFolder<TId> GetOrAddFolder
        (
            string name,
            string icon = null,
            string description = null)
        {
            ComponentFolder<TId> subFolder =
                FindSubFolder(name) ?? AddSubFolder(name, icon, description);

            return subFolder;
        }

        IDisposable _subFoldersBehavior;
        IDisposable _bbsBehavior;
        public ComponentFolder
        (
            string name,
            string icon = null,
            string description = null
        )
            :
            base(name, icon, description)
        {
            _subFoldersBehavior = SubFolders.AddBehavior<ComponentDisplayMetadata>(OnAddItem, OnRemoveItem);
            _bbsBehavior = ComponentInfos.AddBehavior<ComponentDisplayMetadata>(OnAddItem, OnRemoveItem);
        }

        public ComponentFolder(ComponentDisplayMetadata md) :
            this(md.DisplayName, md.Icon, md.Description)
        {

        }

        private void OnAddItem(ComponentDisplayMetadata item)
        {
            if (item.IsFolder)
            {
                int lastFolderIdx = FoldersAndComponents.LastIndexOf(md => md.IsFolder);

                FoldersAndComponents.Insert(lastFolderIdx + 1, item);
            }
            else
            {
                FoldersAndComponents.Add(item);
            }
        }

        private void OnRemoveItem(ComponentDisplayMetadata item)
        {
            FoldersAndComponents.Remove(item);
        }
    }
}
