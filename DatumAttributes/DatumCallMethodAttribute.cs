using System;

namespace NP.Concepts.DatumAttributes
{
    /// <summary>
    /// can only be an input DatumProcessor connector
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DatumCallMethodAttribute : Attribute, IDatumMember
    {        
        /// <summary>
        /// lists the names of properties or methods for which it triggers changed
        /// </summary>
        public string[] TriggersChange { get; }

        public DatumPropertyDirection Direction => DatumPropertyDirection.In;

        public Type DatumType => null;

        public bool IsSaveable => false;

        public bool PropagateWhenConnected => false;

        public bool IsNotifiable => false;

        public DatumCallMethodAttribute(string[] triggersChange = null)
        {
            TriggersChange = triggersChange;
        }
    }
}
