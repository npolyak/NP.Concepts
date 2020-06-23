using NP.Concepts.DatumAttributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using static NP.Concepts.DatumAttributes.DatumPropertyDirection;
namespace NP.Concepts.Behaviors
{
    public abstract class MultiCollectionsChangeBehavior<T> : VMBase, IForEachItemCollectionBehavior<IEnumerable<T>>
    {
        [DatumProperty(In, IsCollection = true)]
        public ObservableCollection<IEnumerable<T>> Collections { get; } =
            new ObservableCollection<IEnumerable<T>>();

        protected abstract void OnCollectionItemAdded(T item);

        protected abstract void OnCollectionItemRemoved(T item);


        void ICollectionItemBehavior<IEnumerable<T>>.OnItemAdded(IEnumerable<T> item)
        {
            _individualCollectionItemBehavior.Attach(item);
        }

        void ICollectionItemBehavior<IEnumerable<T>>.OnItemRemoved(IEnumerable<T> item)
        {
            _individualCollectionItemBehavior.Detach(item);
        }

        protected IForEachItemCollectionBehavior<T> _individualCollectionItemBehavior;

        [DatumProperty(In, isPullAlways:true)]
        public SynchronizationContext TheSyncContext { get; set; }

        public void AttachCollectionImpl()
        {
            (this as IForEachItemCollectionBehavior<IEnumerable<T>>).Attach(Collections);
        }

        [DatumCallMethod]
        public virtual void AttachCollections()
        {
            TheSyncContext.RunWithinContext(AttachCollectionImpl);
        }

        public void DetachCollectionsImpl()
        {
            (this as IForEachItemCollectionBehavior<IEnumerable<T>>).Detach(Collections);
        }

        [DatumCallMethod]
        public virtual void DetachCollections()
        {
            TheSyncContext.RunWithinContext(DetachCollectionsImpl);
        }

        public MultiCollectionsChangeBehavior()
        {
            _individualCollectionItemBehavior = 
                new DoForEachItemCollectionBehavior<T>(OnCollectionItemAdded, OnCollectionItemRemoved);
        }
    }
}
