using NP.Concepts.DatumAttributes;
using NP.Utilities;

namespace NP.Concepts.Behaviors
{
    public abstract class SingleValueDetailedCollectionBehavior<TItem, TValue> :
        DoForEachItemNotifiableDetailedCollectionBehavior<TItem>
    {
        #region TheResult Property
        private TValue _result;
        [DatumProperty(DatumPropertyDirection.Out)]
        public TValue TheResult
        {
            get
            {
                return this._result;
            }
            protected set
            {
                if (this._result.ObjEquals(value))
                {
                    return;
                }

                this._result = value;
                this.OnPropertyChanged(nameof(TheResult));
            }
        }
        #endregion TheResult Property
    }
}
