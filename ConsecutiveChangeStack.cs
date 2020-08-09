using NP.Concepts.DatumAttributes;
using NP.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NP.Concepts
{
    [DatumProcessor]
    public class ConsecutiveChangeStack<T> : VMBase, IEnumerable<T>
    {
        private ObservableCollection<T> _items;
        [DatumProperty(DatumPropertyDirection.In)]
        public ObservableCollection<T> Items
        {
            get => _items;

            set
            {
                if (ReferenceEquals(_items, value))
                {
                    return;
                }

                _items = value;

                if (_items.Count > 0)
                {
                    CurrentIdx = 0;
                }
                else
                {
                    CurrentIdx = -1;
                }

                UpdateProps();
            }
        }

        T _unsetItem = default;
        public T UnsetItem // unset item is what showing as current item when Stack Is not set at all
        {
            get => _unsetItem; 
            set
            {
                if (_unsetItem.ObjEquals(value))
                    return;

                _unsetItem = value;

                OnPropertyChanged(nameof(CurrentItem));
            }
        } 

        public event Action<T> BeforeBackwardEvent;

        public event Action<T> AfterForwardEvent;

        int _currentIdx = 0;
        [DatumProperty(DatumPropertyDirection.Out)]
        public int CurrentIdx
        {
            get => _currentIdx;
            private set
            {
                if (_currentIdx == value)
                    return;

                _currentIdx = value;

                OnPropertyChanged(nameof(CurrentIdx));

                UpdateProps();
            }
        }

        private void UpdateProps()
        {
            OnPropertyChanged(nameof(CurrentItem));

            OnPropertyChanged(nameof(CanBackward));

            OnPropertyChanged(nameof(CanForward));
        }

        [DatumProperty(DatumPropertyDirection.Out)]
        public virtual bool CanBackward => CurrentIdx > 0;

        public int NextIdx => CurrentIdx + 1;

        [DatumProperty(DatumPropertyDirection.Out)]
        public bool CanForward => NextIdx < TotalNumberItems;

        [DatumProperty(DatumPropertyDirection.Out)]
        public int TotalNumberItems => _items?.Count ?? 0;

        [DatumProperty(DatumPropertyDirection.Out)]
        public T CurrentItem =>
            CurrentIdx >= 0 && TotalNumberItems > 0 ? _items[CurrentIdx] : UnsetItem;



        public T NextItem =>
            CanForward ? _items[NextIdx] : UnsetItem;

        bool _isWithinBackwardForwardStack = false;

        [DatumCallMethod]
        public void GoBackward()
        {
            try
            {
                _isWithinBackwardForwardStack = true;
                if (CanBackward)
                {
                    BeforeBackwardEvent?.Invoke(CurrentItem);

                    CurrentIdx--;
                }
            }
            finally
            {
                _isWithinBackwardForwardStack = false;
            }
        }

        [DatumCallMethod]
        public void GoForward()
        {
            try
            {
                _isWithinBackwardForwardStack = true;
                if (CanForward)
                {
                    CurrentIdx++;

                    AfterForwardEvent?.Invoke(CurrentItem);
                }
            }
            finally
            {
                _isWithinBackwardForwardStack = false;
            }
        }

        [DatumCallMethod]
        public void AddNewItem(T newItem)
        {
            if (_isWithinBackwardForwardStack)
                return;

            while (CanForward)
            {
                _items.RemoveAt(NextIdx);
            }

            _items.Add(newItem);
            CurrentIdx++;

            OnPropertyChanged(nameof(TotalNumberItems));
        }

        [DatumCallMethod]
        public void Reset()
        {
            CurrentIdx = -1;

            _items.Clear();

            OnPropertyChanged(nameof(CanForward));
            OnPropertyChanged(nameof(CanBackward));
        }


        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public ConsecutiveChangeStack()
        {
            
        }
    }
}
