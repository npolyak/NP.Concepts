using NP.Concepts.Attributes;
using NP.Utilities.BasicInterfaces;
using System.ComponentModel;

namespace NP.Concepts
{
    public interface IVMBase : INotifyPropertyChanged, IPropChangedContainer
    {
    }

    public class VMBase : IVMBase
    {
        #region INotifyPropertyChanged Members
        [EventThisIdx]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        protected void OnPropertyChanged(string propertyName)
        {
            (this as IVMBase).OnPropertyChanged(propertyName);
        }

        void IPropChangedContainer.OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IVMBaseContainer<TExtra> : IVMBase
        where TExtra : INotifyPropertyChanged, new()
    {
        TExtra Extra { get; set; }

        private void Connect(INotifyPropertyChanged notifiable)
        {
            notifiable.PropertyChanged += Notifiable_PropertyChanged;
        }

        void Notifiable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        void Init()
        {
            Extra = new TExtra();

            Connect(Extra);
        }
    }
}
