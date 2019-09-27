// (c) Nick Polyak 2018 - http://awebpros.com/
// License: Apache License 2.0 (http://www.apache.org/licenses/LICENSE-2.0.html)
//
// short overview of copyright rules:
// 1. you can use this framework in any commercial or non-commercial 
//    product as long as you retain this copyright message
// 2. Do not blame the author of this software if something goes wrong. 
// 
// Also, please, mention this software in any documentation for the 
// products that use it.
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NP.Concepts.Behaviors
{
    public abstract class ForEachItemCollectionBehaviorBase<TCollItem, TBehaviorItem> :
        IStatelessBehavior<IEnumerable>
        where TBehaviorItem : TCollItem
    {
        protected abstract void UnsetItem(TBehaviorItem item);
        protected abstract void SetItem(TBehaviorItem item);

        private void UnsetItems(IEnumerable items)
        {
            if (items == null)
                return;

            foreach (TCollItem item in items)
            {
                if (item is TBehaviorItem behaviorItem)
                {
                    UnsetItem(behaviorItem);
                }
            }
        }

        private void SetItems(IEnumerable items)
        {
            if (items == null)
                return;

            foreach (TCollItem item in items)
            {
                if (item is TBehaviorItem behaviorItem)
                {
                    SetItem(behaviorItem);
                }
            }
        }


        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UnsetItems(e.OldItems);
            SetItems(e.NewItems);
        }

        public void Detach(IEnumerable collection)
        {
            if (collection == null)
                return;

            INotifyCollectionChanged notifiableCollection =
                collection as INotifyCollectionChanged;

            if (notifiableCollection != null)
            {
                notifiableCollection.CollectionChanged -= Collection_CollectionChanged;
            }

            UnsetItems(collection);
        }

        public void Attach(IEnumerable collection)
        {
            if (collection == null)
                return;

            SetItems(collection);

            INotifyCollectionChanged notifiableCollection =
                collection as INotifyCollectionChanged;

            if (notifiableCollection != null)
            {
                notifiableCollection.CollectionChanged += Collection_CollectionChanged;
            }
        }
    }
}
