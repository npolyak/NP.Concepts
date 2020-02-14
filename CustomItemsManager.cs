using NP.Concepts;
using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using NP.Utilities.FolderUtils;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace NP.Concepts
{
    public class CustomItemsManager : VMBase
    {
        public string ListOfItemsFileName { get; }

        ItemSaverRestorer TheItemSaverRestorer { get; }

        ItemToFolderSaverRestorer ListOfItemsSaverRestorer =>
            TheItemSaverRestorer.SaverRestorer;

        IStrSaveableRestorableClearable _saveableRestorable;
        // interface representing the assembly
        public IStrSaveableRestorableClearable SaveableRestorable 
        {
            get => _saveableRestorable;
            set
            {
                if (ReferenceEquals(_saveableRestorable, value))
                    return;

                _saveableRestorable = value;

                if (_saveableRestorable is INotifyPropertyChanged notifiable)
                {
                    notifiable.PropertyChanged += Notifiable_PropertyChanged;
                }
            }
        }

        private void Notifiable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IStrSaveableRestorableClearable.CanSave))
            {
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public event Action SaveItemEvent;

        public event Action ClearItemEvent;

        public event Action<string> DeleteItemEvent;

        public event Action IsComponentChangedEvent;

        public bool CanSave => (SaveableRestorable?.CanSave == true) && 
                             (this.ItemName.IsNullOrWhiteSpace() == false);

        // this is the assembly info that is specified
        // by the name within the text box. 
        public SaveableItemInfo TheNamedItemInfo { get; } = 
            new SaveableItemInfo();

        public string ItemName =>
            TheNamedItemInfo.ItemName;

        public CustomItemsManager
        (
             string baseDirName, 
             string defaultItemFileName,
             string listOfItemsFileName
        )
        {
            TheItemSaverRestorer = 
                new ItemSaverRestorer(baseDirName, defaultItemFileName);

            ListOfItemsFileName = listOfItemsFileName;

            string assemblyInfosStr = ListOfItemsSaverRestorer.RestoreStr(ListOfItemsFileName);

            if (assemblyInfosStr != null)
            {
                TheItemsInfos =
                    XmlSerializationUtils
                        .Deserialize<ObservableCollection<SaveableItemInfo>>
                        (
                            assemblyInfosStr);
            }
            else
            {
                TheItemsInfos = 
                    new ObservableCollection<SaveableItemInfo>();
            }

            TheItemsInfos.CollectionChanged += 
                TheLayoutInfos_CollectionChanged;

            this.PropertyChanged +=
                CustomItemsManager_PropertyChanged;

            TheNamedItemInfo.PropertyChanged +=
                TheNamedItemInfo_PropertyChanged;
        }

        private void TheNamedItemInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CanSave));
        }

        private void CustomItemsManager_PropertyChanged
        (
            object sender, 
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentItemInfo))
            {
                this.RestoreItem();
            }
        }

        public ObservableCollection<SaveableItemInfo> TheItemsInfos { get; }

        #region CurrentItemInfo Property
        private SaveableItemInfo _currentItemInfo;
        public SaveableItemInfo CurrentItemInfo
        {
            get
            {
                return this._currentItemInfo;
            }   
            set
            {
                if (this._currentItemInfo.ObjEquals(value))
                {
                    return;
                }

                if (_currentItemInfo != null)
                {
                    _currentItemInfo.PropertyChanged -= _currentItemInfo_PropertyChanged;
                }

                this._currentItemInfo = value;

                if (_currentItemInfo != null)
                {
                    this.TheNamedItemInfo.CopyFrom(_currentItemInfo);

                    OnPropertyChanged(nameof(ItemName));

                    _currentItemInfo.PropertyChanged += _currentItemInfo_PropertyChanged;
                }

                FireIsComponentChanged();

                this.OnPropertyChanged(nameof(CurrentItemInfo));
            }
        }

        private void _currentItemInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SaveableItemInfo.IsComponent))
            {
                FireIsComponentChanged();
            }
        }
        #endregion CurrentItemInfo Property

        private void FireIsComponentChanged()
        {
            IsComponentChangedEvent?.Invoke();
        }

        private void TheLayoutInfos_CollectionChanged
        (
            object sender, 
            NotifyCollectionChangedEventArgs e)
        {
            Serialize();
        }

        public void SetCurrentItemInfo(string itemName)
        {
            CurrentItemInfo = 
                TheItemsInfos.FirstOrDefault(itemInfo => itemInfo.ItemName == itemName);
        }

        public void Serialize()
        {
            string layoutInfosString = TheItemsInfos.Serialize();

            ListOfItemsSaverRestorer.SaveStr(ListOfItemsFileName, layoutInfosString);
        }

        private SaveableItemInfo GetMatchingItemInfo(string itemName)
        {
            SaveableItemInfo matchingItemInfo =
                this.TheItemsInfos
                    .FirstOrDefault
                    (
                        itemInfo => itemInfo.ItemName == itemName);

            return matchingItemInfo;
        }


        private void SaveListOfItems()
        {
            if (ItemName.IsNullOrWhiteSpace())
                return;

            SaveableItemInfo matchingItemInfo = GetMatchingItemInfo(this.ItemName);

            if (matchingItemInfo != null)
            {
                matchingItemInfo.CopyFrom(TheNamedItemInfo);
            }
            else // new 
            {
                matchingItemInfo = new SaveableItemInfo();
                matchingItemInfo.CopyFrom(TheNamedItemInfo);

                TheItemsInfos.Add(matchingItemInfo);
            }

            Serialize();

            CurrentItemInfo = matchingItemInfo;
        }

        public void SaveItem ()
        {
            TheItemSaverRestorer.Save(SaveableRestorable, ItemName);

            SaveItemEvent?.Invoke();

            SaveListOfItems();
            
            OnPropertyChanged(nameof(CanSave));
        }

        public void RestoreItem()
        {
            TheItemSaverRestorer.Restore(SaveableRestorable, ItemName);
        }

        public void ClearItem()
        {
            SaveableRestorable.ClearSelf();
            ClearItemEvent?.Invoke();
        }

        public void ClearItemAndName()
        {
            ClearItem();

            this.TheNamedItemInfo.ItemName = null;
            this.TheNamedItemInfo.IsComponent = false;
            this.CurrentItemInfo = null;
        }

        public void DeleteItem()
        {
            string itemName = ItemName;

            TheItemSaverRestorer.Delete(SaveableRestorable, itemName);

            ClearItemAndName();

            SaveableItemInfo matchingItemInfo = GetMatchingItemInfo(itemName);

            if (matchingItemInfo != null)
            {
                this.TheItemsInfos.Remove(matchingItemInfo);
                Serialize();
            }
            this?.DeleteItemEvent?.Invoke(itemName);
        }
    }
}
