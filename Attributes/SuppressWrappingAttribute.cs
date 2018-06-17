using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Concepts.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class SuppressWrappingAttribute : Attribute
    {
        public string MemberName { get; }

        public SuppressWrappingAttribute(string memberName)
        {
            MemberName = memberName;
        }
    }
}
