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
using NP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NP.Concepts.Behaviors
{
    // used to dispose of behaviors
    public class BehaviorsDisposable<T> : IDisposable
    {
        List<DisposableBehaviorContainer<T>> _disposableBehaviors = new List<DisposableBehaviorContainer<T>>();


        public T TheObjectTheBehaviorsAreAttachedTo =>
            _disposableBehaviors.LastOrDefault().TheObjectTheBehaviorIsAttachedTo;

        public BehaviorsDisposable
        (
            DisposableBehaviorContainer<T> disposableBehaviorToAdd,
            BehaviorsDisposable<T> previousBehavior = null
        )
        {
            if (previousBehavior != null)
            {
                _disposableBehaviors.AddAll(previousBehavior._disposableBehaviors);
            }

            _disposableBehaviors.Add(disposableBehaviorToAdd);
        }

        public void Dispose()
        {
            foreach(DisposableBehaviorContainer<T> behaviorContainer in _disposableBehaviors)
            {
                behaviorContainer.Dispose();
            }
        }
    }
}
