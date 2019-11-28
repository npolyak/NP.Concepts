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

        public ObservableCollection<ComponentIdWithDisplayMetadata<TId, TMetaData>> ComponentInfos { get; } =
            new ObservableCollection<ComponentIdWithDisplayMetadata<TId, TMetaData>>();

        public ObservableCollection<IComponentMetaDataContainer<TMetaData>> FoldersAndComponents { get; } =
            new ObservableCollection<IComponentMetaDataContainer<TMetaData>>();

        public ComponentFolder<TId, TMetaData> FindSubFolder(string subFolderName)
        {
            return SubFolders
                       .FirstOrDefault(f => f.MetaData.DisplayName == subFolderName);
        }

        public void Add(TId componentId)
        {
            ComponentIdWithDisplayMetadata<TId, TMetaData> componentInfo =
                new ComponentIdWithDisplayMetadata<TId, TMetaData>(componentId);

            ComponentInfos.Add(componentInfo);
        }

        public ComponentFolder<TId, TMetaData> AddSubFolder
        (
            TMetaData subFolderMetaData    
        )
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
            _subFoldersBehavior = SubFolders.AddBehavior(OnAddItem, OnRemoveItem);
            _bbsBehavior = ComponentInfos.AddBehavior<IComponentMetaDataContainer<TMetaData>>(OnAddItem, OnRemoveItem);
        }

        private void OnAddItem(IComponentMetaDataContainer<TMetaData> item)
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

        private void OnRemoveItem(IComponentMetaDataContainer<TMetaData> item)
        {
            FoldersAndComponents.Remove(item);
        }

        public void CheckMatching(string strToMatch)
        {
            
        }
    }
}
