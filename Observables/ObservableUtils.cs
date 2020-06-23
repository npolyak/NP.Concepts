using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;

namespace NP.Concepts.Observables
{
    public static class ObservableUtils
    {
        public static IObservable<TEventArgs> FromEventPattern<TEventArgs>
        (
           object instance,
           string eventName,
           IScheduler scheduler = null)
        {
            IObservable<EventPattern<TEventArgs>> eventPatternObservable =
                scheduler == null ?
                    Observable.FromEventPattern<TEventArgs>(instance, eventName) :
                    Observable.FromEventPattern<TEventArgs>(instance, eventName, scheduler);

            return eventPatternObservable.Select(eventPattern => eventPattern.EventArgs);
        }
    }
}
