using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NP.Concepts.Behaviors
{
    public class MultiSelectionBehavior<T>
        where T : ISelectableItem<T>
    {
        private IEnumerable<T> _collection;

        private ObservableCollection<T> _selectedItems = new ObservableCollection<T>();

        IDisposable _behaviorDisposable = null;

        public IEnumerable<T> Collection
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

        private void Item_IsSelectedChanged(T selectedOrUnselectedItem)
        {
            if (selectedOrUnselectedItem.IsSelected)
            {
                _selectedItems.Add(selectedOrUnselectedItem);
            }
            else
            {
                _selectedItems.Remove(selectedOrUnselectedItem);
            }
        }

        public IEnumerable<T> SelectedItems { get => _selectedItems; } 
    }
}
