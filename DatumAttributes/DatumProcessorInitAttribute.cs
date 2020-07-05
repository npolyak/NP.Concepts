using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class DatumProcessorInitAttribute : Attribute
    {
    }
}
