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
        private static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehaviorImpl<TCollItem, TBehaviorItem>
        (
            this IEnumerable<TCollItem> collection,
            DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem> behavior,
            BehaviorsDisposable<IEnumerable<TCollItem>> previousBehavior = null
        ) where TBehaviorItem : TCollItem
        {
            if (collection == null)
                return null;

            DisposableBehaviorContainer<IEnumerable<TCollItem>> behaviorContainer =
                new DisposableBehaviorContainer<IEnumerable<TCollItem>>(behavior, collection);

            BehaviorsDisposable<IEnumerable<TCollItem>> behaviorsDisposable =
                new BehaviorsDisposable<IEnumerable<TCollItem>>(behaviorContainer, previousBehavior);

            return behaviorsDisposable;
        }

        private static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehaviorImpl<TCollItem, TBehaviorItem>
        (
            this IEnumerable<TCollItem> collection,
            Action<TBehaviorItem> onAdd,
            Action<TBehaviorItem> onRemove = null,
            BehaviorsDisposable<IEnumerable<TCollItem>> previousBehavior = null
        ) where TBehaviorItem : TCollItem
        {
            DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem> behavior =
                new DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem>(onAdd, onRemove);

            return collection?.AddBehaviorImpl<TCollItem, TBehaviorItem>(behavior, previousBehavior);
        }

        public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem, TBehaviorItem>
        (
            this IEnumerable<TCollItem> collection,
            DoForEachItemCollectionBehavior<TCollItem, TBehaviorItem> behavior
        ) where TBehaviorItem : TCollItem
        {
            return collection.AddBehaviorImpl(behavior);
        }

        public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
        (
            this IEnumerable<TCollItem> collection,
            DoForEachItemCollectionBehavior<TCollItem, TCollItem> behavior
        ) => AddBehavior<TCollItem, TCollItem>(collection, behavior);


        public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem, TBehaviorItem>
        (
            this IEnumerable<TCollItem> collection, 
            Action<TBehaviorItem> onAdd,
            Action<TBehaviorItem> onRemove = null
        ) where TBehaviorItem : TCollItem
        {
            return collection.AddBehaviorImpl(onAdd, onRemove);
        }

        public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
        (
            this IEnumerable<TCollItem> collection,
            Action<TCollItem> onAdd,
            Action<TCollItem> onRemove = null
        ) => AddBehavior<TCollItem, TCollItem>(collection, onAdd, onRemove);

        public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem, TBehaviorItem>
        (
            this BehaviorsDisposable<IEnumerable<TCollItem>> previousBehaviors,
            Action<TBehaviorItem> onAdd,
            Action<TBehaviorItem> onRemove = null
        ) where TBehaviorItem : TCollItem
        {
            IEnumerable<TCollItem> collection = previousBehaviors.TheObjectTheBehaviorsAreAttachedTo;

            return collection.AddBehaviorImpl<TCollItem, TBehaviorItem>(onAdd, onRemove, previousBehaviors);
        }

        public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
        (
            this BehaviorsDisposable<IEnumerable<TCollItem>> previousBehaviors,
            Action<TCollItem> onAdd,
            Action<TCollItem> onRemove = null
        )
            => AddBehavior<TCollItem, TCollItem>(previousBehaviors, onAdd, onRemove);
    }
}
