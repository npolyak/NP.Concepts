using System;

namespace NP.Concepts.DatumAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DatumPropertyAttribute : Attribute, IDatumMember
    {
        public DatumPropertyDirection Direction { get; }

        #region IN Props
        public bool IsSaveable { get; }
        
        /// <summary>
        /// marks the attribute as 'Type'
        /// only should be used for properties whose
        /// type is Type
        /// </summary>
        public bool IsType { get; }

        public bool IsCollection { get; set; } = false;

        public bool IsPullAlways { get; }
        #endregion IN Props

        #region OUT Props
        public bool PropagateWhenConnected { get; }

        #endregion OUT Props

        public DatumPropertyAttribute
        (
            DatumPropertyDirection datumDirection,
            bool isSaveable = false,
            bool propagateWhenConnected = true,
            bool isType = false, 
            bool isPullAlways = false)
        {
            Direction = datumDirection;

            IsSaveable = isSaveable;

            PropagateWhenConnected = propagateWhenConnected;

            IsType = isType;

            IsPullAlways = isPullAlways;
        }
    }
}
