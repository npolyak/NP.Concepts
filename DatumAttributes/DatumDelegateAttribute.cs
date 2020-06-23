using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DatumDelegateAttribute : Attribute
    {
    }
}
