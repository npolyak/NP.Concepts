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

namespace NP.Concepts.Behaviors
{
    public interface IStatelessBehavior<T>
    {
        void Attach(T obj);

        void Detach(T obj);
    }

    public static class StatelessBehaviorUtils
    {
        public static void Reset<T>(this IStatelessBehavior<T> behavior, T obj)
        {
            behavior.Detach(obj);
            behavior.Attach(obj);
        }
    }
}
