using NP.Concepts.DatumAttributes;
using System.Collections.Generic;

namespace NP.Concepts.Behaviors
{
    [DatumProcessor]
    public abstract class DoForEachItemNotifiableDetailedCollectionBehavior<TItem> :
        VMBase,
        IForEachItemDetailedCollectionBehavior<TItem>
    {
        #region TheCollection Property
        private IEnumerable<TItem> _collection;
        [DatumProperty(DatumPropertyDirection.In)]
        public IEnumerable<TItem> TheCollection
        {
            get
            {
                return this._collection;
            }
            set
            {
                if (this._collection == value)
                {
                    return;
                }

                (this as IForEachItemDetailedCollectionBehavior<TItem>).Detach(_collection);

                this._collection = value;

                (this as IForEachItemDetailedCollectionBehavior<TItem>).Attach(_collection);
            }
        }
        #endregion TheCollection Property

        protected abstract void OnItemAdded(IEnumerable<TItem> collection, TItem item, int newIdx);

        protected abstract void OnItemRemoved(IEnumerable<TItem> collection, TItem item, int oldIdx);

        void IForEachItemDetailedCollectionBehavior<TItem>.OnItemAdded(IEnumerable<TItem> collection, TItem item, int newIdx)
        {
            OnItemAdded(collection, item, newIdx);
        }

        void IForEachItemDetailedCollectionBehavior<TItem>.OnItemRemoved(IEnumerable<TItem> collection, TItem item, int oldIdx)
        {
            OnItemRemoved(collection, item, oldIdx);
        }
    }
}
