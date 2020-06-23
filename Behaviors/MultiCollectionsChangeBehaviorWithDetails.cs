using NP.Concepts.DatumAttributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using static NP.Concepts.DatumAttributes.DatumPropertyDirection;
namespace NP.Concepts.Behaviors
{
    public abstract class MultiCollectionsChangeBehaviorWithDetails<T> 
        : VMBase, IForEachItemCollectionBehavior<IEnumerable<T>>
    {
        [DatumProperty(In, IsCollection = true)]
        public ObservableCollection<IEnumerable<T>> Collections { get; } =
            new ObservableCollection<IEnumerable<T>>();

        void ICollectionItemBehavior<IEnumerable<T>>.OnItemAdded(IEnumerable<T> item)
        {
            _individualCollectionItemBehavior.Attach(item);
        }

        void ICollectionItemBehavior<IEnumerable<T>>.OnItemRemoved(IEnumerable<T> item)
        {
            _individualCollectionItemBehavior.Detach(item);
        }

        protected abstract void OnCollectionItemAdded(IEnumerable<T> collection, T item, int idx);

        protected abstract void OnCollectionItemRemoved(IEnumerable<T> collection, T item, int oldIdx);

        protected IForEachItemDetailedCollectionBehavior<T> _individualCollectionItemBehavior;
        
        [DatumProperty(In, isPullAlways: true)]
        public SynchronizationContext TheSyncContext { get; set; }


        private void DettachCollectionImpl()
        {
            (this as IForEachItemCollectionBehavior<IEnumerable<T>>).Detach(Collections);
        }


        private void AttachCollectionImpl()
        {
            (this as IForEachItemCollectionBehavior<IEnumerable<T>>).Attach(Collections);
        }

        [DatumCallMethod]
        public virtual void DettachCollections()
        {
            TheSyncContext.RunWithinContext(AttachCollectionImpl);
        }

        [DatumCallMethod]
        public virtual void AttachCollections()
        {
            TheSyncContext.RunWithinContext(AttachCollectionImpl);
        }

        public MultiCollectionsChangeBehaviorWithDetails()
        {
            _individualCollectionItemBehavior = 
                new DoForEachItemDetailedCollectionBehavior<T>(OnCollectionItemAdded, OnCollectionItemRemoved);

            AttachCollections();
        }
    }
}
