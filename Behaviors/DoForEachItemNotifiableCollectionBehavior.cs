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

                DetachCollection();

                this._collection = value;

                AttachCollection();
            }
        }
        #endregion TheCollection Property

        [DatumCallMethod]
        public void DetachCollection()
        {
            (this as ICollectionStatelessBehavior<TItem>).Detach(_collection);
        }

        [DatumCallMethod]
        public virtual void AttachCollection()
        {
            (this as ICollectionStatelessBehavior<TItem>).Attach(_collection);
        }

        [DatumCallMethod]
        public void Reattach()
        {
            DetachCollection();
            AttachCollection();
        }

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
