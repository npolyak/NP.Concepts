using NP.Concepts.Behaviors;
using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentFolder<TId, TMetaData> : 
        IComponentMetaDataContainer<TMetaData>, 
        INotifyPropertyChanged
        where TId : INameContainer
        where TMetaData : class, IComponentDisplayMetadata
    {
        public bool IsFolder => true;

        public TMetaData MetaData { get; }

        public ObservableCollection<ComponentFolder<TId, TMetaData>> SubFolders { get; } =
            new ObservableCollection<ComponentFolder<TId, TMetaData>>();

        public ObservableCollection<IComponentIdWithDisplayMetadata<TId, TMetaData>> ComponentInfos { get; } =
            new ObservableCollection<IComponentIdWithDisplayMetadata<TId, TMetaData>>();

        public ObservableCollection<IComponentMetaDataContainer<TMetaData>> FoldersAndComponents { get; } =
            new ObservableCollection<IComponentMetaDataContainer<TMetaData>>();

        public ComponentFolder<TId, TMetaData> FindSubFolder(string subFolderName)
        {
            return SubFolders
                       .FirstOrDefault(f => f.MetaData.DisplayName == subFolderName);
        }

        public void Add(IComponentIdWithDisplayMetadata<TId, TMetaData> componentInfo)
        {
            ComponentInfos.Add(componentInfo);
        }

        public ComponentFolder<TId, TMetaData>
            AddSubFolder(TMetaData subFolderMetaData)
        {
            ComponentFolder<TId, TMetaData> subFolder =
                new ComponentFolder<TId, TMetaData>(subFolderMetaData);

            SubFolders.Add(subFolder);

            return subFolder;
        }

        public ComponentFolder<TId, TMetaData> GetOrAddFolder
        (
            TMetaData subFolderMetaData    
        )
        {
            ComponentFolder<TId, TMetaData> subFolder =
                FindSubFolder(subFolderMetaData.DisplayName) ?? AddSubFolder(subFolderMetaData);

            return subFolder;
        }

        IDisposable _subFoldersBehavior;
        IDisposable _bbsBehavior;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public bool IsMatching => true;

        #region SearchStr Property
        private string _searchStr;
        public string SearchStr
        {
            get
            {
                return this._searchStr;
            }
            set
            {
                if (this._searchStr == value)
                {
                    return;
                }

                this._searchStr = value;
                this.OnPropertyChanged(nameof(SearchStr));

                var allComponents = this.GetAllNonFolderComponents<TId, TMetaData>().ToList();
                foreach (var descendant in allComponents)
                {
                    if (!descendant.IsFolder)
                    {
                        descendant.CheckMatching(_searchStr);
                    }
                }
            }
        }
        #endregion SearchStr Property


        public ComponentFolder
        (
            TMetaData metaData
        )
        {
            MetaData = metaData;
            _subFoldersBehavior = SubFolders.AddBehavior(OnAddSubFolderItem, OnRemoveSubFolderItem);
            _bbsBehavior = ComponentInfos.AddBehavior(OnAddComponentItem, OnRemoveComponentItem);
        }

        private void OnAddComponentItem(IComponentIdWithDisplayMetadata<TId, TMetaData> componentItem)
        {
            FoldersAndComponents.Add(componentItem);
        }

        private void OnRemoveComponentItem(IComponentIdWithDisplayMetadata<TId, TMetaData> componentItem)
        {
            FoldersAndComponents.Add(componentItem);
        }

        private void OnAddSubFolderItem(IComponentMetaDataContainer<TMetaData> folderItem)
        {
            int lastFolderIdx = FoldersAndComponents.LastIndexOf(md => md.IsFolder);

            FoldersAndComponents.Insert(lastFolderIdx + 1, folderItem);
        }

        private void OnRemoveSubFolderItem(IComponentMetaDataContainer<TMetaData> folderItem)
        {
            FoldersAndComponents.Remove(folderItem);
        }

        public void Clear()
        {
            ComponentInfos.RemoveAll();
            SubFolders.RemoveAll();
        }

        public void CheckMatching(string strToMatch)
        {
            
        }
    }
}
