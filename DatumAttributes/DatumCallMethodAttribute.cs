using System;

namespace NP.Concepts.DatumAttributes
{
    /// <summary>
    /// can only be an input DatumProcessor connector
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DatumCallMethodAttribute : Attribute
    {
    }
}
