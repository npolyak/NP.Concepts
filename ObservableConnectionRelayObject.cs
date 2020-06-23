using System.ComponentModel;

namespace NP.Concepts
{
    public interface IObservableConnectionRelayObject : INotifyPropertyChanged
    {
        object Input { get; set; }

        object Output { get; set; }

        void SetOutputWithoutNotification(object val);
    }

    public class ObservableConnectionRelayObject : VMBase, IObservableConnectionRelayObject
    {
        public object Input
        {
            get => Output;

            set => Output = value;
        }

        object _output;
        public object Output 
        {
            get => _output;
            set
            {
                SetOutputWithoutNotification(value);

                OnPropertyChanged(nameof(Output));
            }
        }

        public void SetOutputWithoutNotification(object val)
        {
            _output = val;
        }
    }
}
