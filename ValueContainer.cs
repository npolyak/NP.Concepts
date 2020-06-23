using NP.Concepts.DatumAttributes;
using NP.Utilities;

namespace NP.Concepts
{
    [DatumProcessor]
    public class ValueContainer<T> : VMBase
    {
        T _value = default;

        [DatumProperty(DatumPropertyDirection.In, isSaveable: true)]
        public T Input
        {
            get => Output;
            set => Output = value;
        }

        [DatumProperty(DatumPropertyDirection.Out)]
        public T Output
        {
            get => _value;

            set
            {
                if (_value.ObjEquals(value))
                    return;

                _value = value;

                OnPropertyChanged(nameof(Output));
            }
        }
    }

}
