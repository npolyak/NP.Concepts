using System;
using System.Threading;

namespace NP.Concepts
{
    public static class SynchContextContainer
    {
        public static SynchronizationContext UISynchContext { get; set; }

        public static void RunWithinContext(this SynchronizationContext context, Action actionToRun)
        {
            if (context == SynchronizationContext.Current || context == null)
            {
                actionToRun?.Invoke();
            }
            else
            {
                context.Send(_ => actionToRun?.Invoke(), null);
            }
        }

        public static void RunWithinUiContext(this Action actionToRun) =>
            UISynchContext.RunWithinContext(actionToRun); 
    }
}
