using System;

namespace NP.Concepts.DatumAttributes.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DatumTypeAttribute : Attribute
    {
    }
}
