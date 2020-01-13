using NP.Concepts.Attributes;
using System.ComponentModel;

namespace NP.Concepts
{
    public interface IVMBase : INotifyPropertyChanged
    {
        void OnPropertyChanged(string propertyName);
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

        void IVMBase.OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
