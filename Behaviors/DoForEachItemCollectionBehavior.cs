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
using System;
using System.Collections;
using System.Collections.Generic;

namespace NP.Concepts.Behaviors
{
    public class DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem> : 
        ForEachItemCollectionBehaviorBase<TCollItem, TBehaviorItem>
        where TBehaviorItem : TCollItem
    {
        Action<TBehaviorItem> UnsetItemDelegate { get; }
        Action<TBehaviorItem> SetItemDelegate { get; }

        protected override void UnsetItem(TBehaviorItem item)
        {
            UnsetItemDelegate?.Invoke(item);
        }

        protected override void SetItem(TBehaviorItem item)
        {
            SetItemDelegate?.Invoke(item);
        }

        public DoForEachItemCollectionBehavior
        (
            Action<TBehaviorItem> OnAdd, 
            Action<TBehaviorItem> OnRemove = null)
        {
            SetItemDelegate = OnAdd;
            UnsetItemDelegate = OnRemove;
        }
    }

    public static class DoForEachBehaviorUtils
    {
        private static BehaviorsDisposable<IEnumerable> AddBehaviorImpl<TCollItem, TBehaviorItem>
        (
            this IEnumerable collection,
            DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem> behavior,
            BehaviorsDisposable<IEnumerable> previousBehavior = null
        ) where TBehaviorItem : TCollItem
        {
            if (collection == null)
                return null;

            DisposableBehaviorContainer<IEnumerable> behaviorContainer =
                new DisposableBehaviorContainer<IEnumerable>(behavior, collection);

            BehaviorsDisposable<IEnumerable> behaviorsDisposable =
                new BehaviorsDisposable<IEnumerable>(behaviorContainer, previousBehavior);

            return behaviorsDisposable;
        }

        private static BehaviorsDisposable<IEnumerable> AddBehaviorImpl<TCollItem, TBehaviorItem>
        (
            this IEnumerable collection,
            Action<TBehaviorItem> onAdd,
            Action<TBehaviorItem> onRemove = null,
            BehaviorsDisposable<IEnumerable> previousBehavior = null
        ) where TBehaviorItem : TCollItem
        {
            DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem> behavior =
                new DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem>(onAdd, onRemove);

            return collection?.AddBehaviorImpl<TCollItem, TBehaviorItem>(behavior, previousBehavior);
        }

        public static BehaviorsDisposable<IEnumerable> AddBehavior<TCollItem, TBehaviorItem>
        (
            this IEnumerable collection,
            DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem> behavior
        ) where TBehaviorItem : TCollItem
        {
            return collection.AddBehaviorImpl(behavior);
        }

        public static BehaviorsDisposable<IEnumerable> AddBehavior<TCollItem>
        (
            this IEnumerable collection,
            DoForEachItemCollectionBehavior<TCollItem, TCollItem> behavior
        ) => AddBehavior<TCollItem, TCollItem>(collection, behavior);


        public static BehaviorsDisposable<IEnumerable> AddBehavior<TCollItem, TBehaviorItem>
        (
            this IEnumerable collection, 
            Action<TBehaviorItem> onAdd,
            Action<TBehaviorItem> onRemove = null
        ) where TBehaviorItem : TCollItem
        {
            return collection.AddBehaviorImpl<TCollItem, TBehaviorItem>(onAdd, onRemove);
        }

        public static BehaviorsDisposable<IEnumerable> AddBehavior<TCollItem>
        (
            this IEnumerable<TCollItem> collection,
            Action<TCollItem> onAdd,
            Action<TCollItem> onRemove = null
        ) => AddBehavior<TCollItem, TCollItem>(collection, onAdd, onRemove);

        public static BehaviorsDisposable<IEnumerable> AddBehavior<TCollItem, TBehaviorItem>
        (
            this BehaviorsDisposable<IEnumerable> previousBehaviors,
            Action<TBehaviorItem> onAdd,
            Action<TBehaviorItem> onRemove = null
        ) where TBehaviorItem : TCollItem
        {
            IEnumerable collection = previousBehaviors.TheObjectTheBehaviorsAreAttachedTo;

            return collection.AddBehaviorImpl<TCollItem, TBehaviorItem>(onAdd, onRemove, previousBehaviors);
        }

        public static BehaviorsDisposable<IEnumerable> AddBehavior<TCollItem>
        (
            this BehaviorsDisposable<IEnumerable> previousBehaviors,
            Action<TCollItem> onAdd,
            Action<TCollItem> onRemove = null
        )
            => AddBehavior<TCollItem, TCollItem>(previousBehaviors, onAdd, onRemove);
    }
}
