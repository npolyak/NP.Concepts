using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DatumPropertyAttribute : Attribute, IDatumMember
    {
        public DatumPropertyDirection Direction { get; }

        public Type DatumType { get; }

        /// <summary>
        /// lists the names of properties or methods for which it triggers changed
        /// </summary>
        public string[] TriggersChange { get; }

        #region IN Props
        public bool IsSaveable { get; }
        #endregion IN Props

        #region OUT Props
        public bool PropagateWhenConnected { get; }

        public bool IsNotifiable { get; }
        #endregion OUT Props

        public DatumPropertyAttribute
        (
            DatumPropertyDirection datumDirection, 
            bool isSaveable = false, 
            bool propagateWhenConnected = true,
            bool isNotifiable = true,
            string[] triggersChange = null,
            Type datumType = null)
        {
            Direction = datumDirection;

            IsSaveable = isSaveable;

            PropagateWhenConnected = propagateWhenConnected;
            IsNotifiable = isNotifiable;

            TriggersChange = triggersChange;

            DatumType = datumType;
        }
    }
}
