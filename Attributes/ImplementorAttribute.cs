using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Concepts.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class ImplementorAttribute : Attribute
    {
        public Type ImplementorType { get; }

        public ImplementorAttribute(Type implementorType)
        {

        }
    }
}
