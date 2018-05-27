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
    public abstract class ForEachItemCollectionBehaviorBase<T> :
        IStatelessBehavior<IEnumerable<T>>
    {
        protected abstract void UnsetItem(T item);
        protected abstract void SetItem(T item);

        private void UnsetItems(IEnumerable items)
        {
            if (items == null)
                return;

            foreach (T item in items)
            {
                UnsetItem(item);
            }
        }

        private void SetItems(IEnumerable items)
        {
            if (items == null)
                return;

            foreach (T item in items)
            {
                SetItem(item);
            }
        }


        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UnsetItems(e.OldItems);
            SetItems(e.NewItems);
        }

        public void Detach(IEnumerable<T> collection)
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

        public void Attach(IEnumerable<T> collection)
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
