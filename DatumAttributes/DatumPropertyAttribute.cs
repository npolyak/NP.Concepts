using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DatumPropertyAttribute : Attribute
    {
        public DatumPropertyDirection Direction { get; }

        public Type DatumType { get; }

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
            Type datumType = null)
        {
            Direction = datumDirection;

            IsSaveable = isSaveable;

            PropagateWhenConnected = propagateWhenConnected;
            IsNotifiable = isNotifiable;

            DatumType = datumType;
        }
    }
}
