using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Concepts.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ClassEventThisIdxAttribute : Attribute
    {
        public string EventName { get; }
        public int ThisIdx { get; }

        public ClassEventThisIdxAttribute(string eventName, int thisIdx = 0)
        {
            EventName = eventName;
            ThisIdx = thisIdx;
        }
    }
}
