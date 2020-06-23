using NP.Concepts.DatumAttributes;
using NP.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace NP.Concepts.Behaviors
{
    [DatumProcessor]
    public class ObservableCollectionGrouper<T, TGroupKey, TGroupedItem> :
        MultiCollectionsChangeBehavior<T>
        where TGroupedItem : ObservableGroupedItems<T, TGroupKey>
    {
        [DatumProperty(DatumPropertyDirection.Out)]
        public ObservableCollection<TGroupedItem> GroupedItems { get; } =
            new ObservableCollection<TGroupedItem>();

        [DatumProperty(DatumPropertyDirection.In)]
        public Func<T, TGroupKey> GroupKeyGetter
        {
            get; set;
        }

        [DatumProperty(DatumPropertyDirection.In)]
        public Func<TGroupKey, TGroupedItem> GroupItemCreator
        {
            get; set;
        }

        [DatumProperty(DatumPropertyDirection.In)]
        public Func<T, T, int> ItemsComparer { get; set; }

        [DatumProperty(DatumPropertyDirection.In)]
        public Func<TGroupKey, TGroupKey, int> GroupKeyItemsComparer { get; set; }


        [DatumCallMethod]
        public void Reattach()
        {
            DetachCollections();

            AttachCollections();
        }

        protected override void OnCollectionItemAdded(T item)
        {
            (TGroupedItem currentGroupedItem, TGroupKey groupKey) = GetGroupedItemByKey(item);

            if (currentGroupedItem == null)
            {
                currentGroupedItem = GroupItemCreator(groupKey);
                GroupedItems.InsertInOrder(currentGroupedItem, groupedItem => groupedItem.TheKey, GroupKeyItemsComparer);
            }

            currentGroupedItem.InsertInOrder(item, ItemsComparer);

            if (IsPostNotifiable)
            {
                (item as INotifyPropertyChanged).PropertyChanged +=
                    CollectionGrouper_PropertyChanged;
            }
        }

        [DatumCallMethod]
        public override void AttachCollections()
        {
            if (GroupKeyGetter == null)
                return;

            if (typeof(TGroupedItem) == typeof(ObservableGroupedItems<T, TGroupKey>) && GroupItemCreator == null)
            {
                GroupItemCreator = (TGroupKey key) => (TGroupedItem)new ObservableGroupedItems<T, TGroupKey>(key);
            }

            if (GroupItemCreator == null)
                return;

            GroupKeyItemsComparer = GroupKeyItemsComparer.GetTrivialComparerIfNull();

            ItemsComparer = ItemsComparer.GetTrivialComparerIfNull();

            base.AttachCollections();
        }

        private (TGroupedItem, TGroupKey) GetGroupedItemByKey(T item)
        {
            TGroupKey groupKey = GroupKeyGetter(item);
            var currentGroupedItem = GroupedItems.FirstOrDefault(groupedItem => groupedItem.TheKey.ObjEquals(groupKey));

            return (currentGroupedItem, groupKey);
        }

        private void CollectionGrouper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected override void OnCollectionItemRemoved(T item)
        {
            (TGroupedItem currentGroupedItem, _) = GetGroupedItemByKey(item);

            if (currentGroupedItem != null)
            {
                currentGroupedItem.Remove(item);

                if (currentGroupedItem.Count == 0)
                {
                    GroupedItems.Remove(currentGroupedItem);
                }
            }

            if (IsPostNotifiable)
            {
                (item as INotifyPropertyChanged).PropertyChanged -=
                    CollectionGrouper_PropertyChanged;
            }
        }

        public bool IsPostNotifiable { get; }
        public ObservableCollectionGrouper()
        {
            Type tType = typeof(T);
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(tType))
            {
                IsPostNotifiable = true;
            }
        }
    }
}
