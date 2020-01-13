using NP.Concepts.Behaviors;
using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentFolder<TId, TFolderMetaData, TComponentMetaData> : 
        IComponentMetaDataContainer<TFolderMetaData>, 
        INotifyPropertyChanged
        where TId : INameContainer
        where TFolderMetaData : class, IComponentDisplayMetadata
        where TComponentMetaData : class, IComponentDisplayMetadata
    {
        public bool IsFolder => true;

        public TFolderMetaData MetaData { get; }

        public ObservableCollection<ComponentFolder<TId, TFolderMetaData, TComponentMetaData>> SubFolders { get; } =
            new ObservableCollection<ComponentFolder<TId, TFolderMetaData, TComponentMetaData>>();

        public ObservableCollection<IComponentIdWithDisplayMetadata<TId, TComponentMetaData>> ComponentInfos { get; } =
            new ObservableCollection<IComponentIdWithDisplayMetadata<TId, TComponentMetaData>>();

        public ObservableCollection<IComponentMetaDataContainer> FoldersAndComponents { get; } =
            new ObservableCollection<IComponentMetaDataContainer>();

        public ComponentFolder<TId, TFolderMetaData, TComponentMetaData> FindSubFolder(string subFolderName)
        {
            return SubFolders
                       .FirstOrDefault(f => f.MetaData.DisplayName == subFolderName);
        }

        public void Add(IComponentIdWithDisplayMetadata<TId, TComponentMetaData> componentInfo)
        {
            ComponentInfos.Add(componentInfo);
        }

        public ComponentFolder<TId, TFolderMetaData, TComponentMetaData>
            AddSubFolder(TFolderMetaData subFolderMetaData)
        {
            ComponentFolder<TId, TFolderMetaData, TComponentMetaData> subFolder =
                new ComponentFolder<TId, TFolderMetaData, TComponentMetaData>(subFolderMetaData);

            SubFolders.Add(subFolder);

            return subFolder;
        }

        public ComponentFolder<TId, TFolderMetaData, TComponentMetaData> GetOrAddFolder
        (
            TFolderMetaData subFolderMetaData    
        )
        {
            ComponentFolder<TId, TFolderMetaData, TComponentMetaData> subFolder =
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

                var allComponents = 
                    this.GetAllNonFolderComponents<TId, TFolderMetaData, TComponentMetaData>().ToList();
                
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
            TFolderMetaData metaData
        )
        {
            MetaData = metaData;
            _subFoldersBehavior = SubFolders.AddBehavior(OnAddSubFolderItem, OnRemoveSubFolderItem);
            _bbsBehavior = ComponentInfos.AddBehavior(OnAddComponentItem, OnRemoveComponentItem);
        }

        private void OnAddComponentItem(IComponentIdWithDisplayMetadata<TId, TComponentMetaData> componentItem)
        {
            FoldersAndComponents.Add(componentItem);
        }

        private void OnRemoveComponentItem(IComponentIdWithDisplayMetadata<TId, TComponentMetaData> componentItem)
        {
            FoldersAndComponents.Add(componentItem);
        }

        private void OnAddSubFolderItem(IComponentMetaDataContainer<TFolderMetaData> folderItem)
        {
            int lastFolderIdx = FoldersAndComponents.LastIndexOf(md => md.IsFolder);

            FoldersAndComponents.Insert(lastFolderIdx + 1, folderItem);
        }

        private void OnRemoveSubFolderItem(IComponentMetaDataContainer<TFolderMetaData> folderItem)
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
