using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NP.Concepts.Behaviors
{
    public class SingleSelectionBehavior<T> : VMBase
        where T : class, ISelectableItem<T>
    {
        IDisposable _behaviorDisposable = null;
        public event Action SelectedItemChangedEvent = null;


        #region TheCollection Property
        private IEnumerable<T> _collection;
        public IEnumerable<T> TheCollection
        {
            get => _collection;
            set
            {
                if (ReferenceEquals(_collection, value))
                    return;

                _collection = value;

                _behaviorDisposable =
                    _collection?.AddBehavior
                    (
                        (item) => item.IsSelectedChanged += Item_IsSelectedChanged,
                        (item) => item.IsSelectedChanged -= Item_IsSelectedChanged
                    );
            }
        }
        #endregion TheCollection Property

        private void Item_IsSelectedChanged(ISelectableItem<T> item)
        {
            if (item.IsSelected)
            {
                this.TheSelectedItem = (T)item;
            }
            else
            {
                TheSelectedItem = null;
            }
        }

        #region TheSelectedItem Property
        private T _selectedItem;
        [XmlIgnore]
        public T TheSelectedItem
        {
            get
            {
                return this._selectedItem;
            }
            set
            {
                if (ReferenceEquals(_selectedItem, value))
                {
                    return;
                }

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                this._selectedItem = value;

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }

                SelectedItemChangedEvent?.Invoke();
                this.OnPropertyChanged(nameof(TheSelectedItem));
            }
        }
        #endregion TheSelectedItem Property
    }
}
