using NP.Concepts.DatumAttributes;
using NP.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace NP.Concepts.Behaviors
{
    [DatumProcessor]
    public class ObservableCollectionGrouper<T, TGroupKey, TGroupedItem> :
        DoForEachItemNotifiableCollectionBehavior<T>
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
        public override void AttachCollection()
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

            base.AttachCollection();
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

        [DatumProperty(DatumPropertyDirection.In)]
        public SynchronizationContext TheSyncContext { get; set; }


        private void OnItemAddedImpl(T item)
        {
            (TGroupedItem currentGroupedItem, TGroupKey groupKey) = GetGroupedItemByKey(item);

            bool shouldInsertGroupItem = false;
            if (currentGroupedItem == null)
            {
                currentGroupedItem = GroupItemCreator(groupKey);

                shouldInsertGroupItem = true;
            }

            currentGroupedItem.InsertInOrder(item, ItemsComparer);

            if (shouldInsertGroupItem)
            {
                GroupedItems.InsertInOrder(currentGroupedItem, groupedItem => groupedItem.TheKey, GroupKeyItemsComparer);
            }

            if (IsPostNotifiable)
            {
                (item as INotifyPropertyChanged).PropertyChanged +=
                    CollectionGrouper_PropertyChanged;
            }
        }

        protected override void OnItemAdded(T item)
        {
            TheSyncContext.RunWithinContext(() => OnItemAddedImpl(item));
        }


        private void OnItemRemovedImpl(T item)
        {
            (TGroupedItem currentGroupedItem, _) = GetGroupedItemByKey(item);

            if (currentGroupedItem != null)
            {
                if (currentGroupedItem.Contains(item) && currentGroupedItem.Count == 1)
                {
                    GroupedItems.Remove(currentGroupedItem);
                }

                currentGroupedItem.Remove(item);
            }

            if (IsPostNotifiable)
            {
                (item as INotifyPropertyChanged).PropertyChanged -=
                    CollectionGrouper_PropertyChanged;
            }
        }

        protected override void OnItemRemoved(T item)
        {
            TheSyncContext.RunWithinContext(() => OnItemRemovedImpl(item));
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
