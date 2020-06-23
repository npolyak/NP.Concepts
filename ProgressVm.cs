using NP.Concepts.DatumAttributes;
using System;
using System.Threading.Tasks;

namespace NP.Concepts
{
    public class ProgressVm : VMBase
    {
        #region IsInProgress Property
        private bool _isInProgress;
        [DatumProperty(DatumPropertyDirection.Out)]
        public bool IsInProgress
        {
            get
            {
                return this._isInProgress;
            }
            set
            {
                if (this._isInProgress == value)
                {
                    return;
                }

                this._isInProgress = value;
                this.OnPropertyChanged(nameof(IsInProgress));
            }
        }
        #endregion IsInProgress Property

        protected async Task PerformActionWithProgress(Func<Task> action)
        {
            try
            {
                IsInProgress = true;

                await action();
            }
            finally
            {
                IsInProgress = false;
            }
        }
    }
}
