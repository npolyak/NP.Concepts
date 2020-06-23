using NP.Concepts.DatumAttributes;
using System.Collections.Generic;
using System.ComponentModel;

namespace NP.Concepts.Behaviors
{
    [DatumProcessor]
    public abstract class DoForEachItemNotifiableCollectionBehavior<TItem> : 
        VMBase, 
        IForEachItemCollectionBehavior<TItem>
    {
        protected abstract void OnItemAdded(TItem item);

        protected abstract void OnItemRemoved(TItem item);


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

                (this as ICollectionStatelessBehavior<TItem>).Detach(_collection);

                this._collection = value;

                (this as ICollectionStatelessBehavior<TItem>).Attach(_collection);
            }
        }
        #endregion TheCollection Property


        void ICollectionItemBehavior<TItem>.OnItemAdded(TItem item)
        {
            OnItemAdded(item);
        }

        void ICollectionItemBehavior<TItem>.OnItemRemoved(TItem item)
        {
            OnItemRemoved(item);
        }
    }
}
