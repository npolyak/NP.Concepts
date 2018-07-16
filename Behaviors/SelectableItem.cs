using NP.Concepts.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NP.Concepts.Behaviors
{
    public interface ISelectableItem<T>
        where T : ISelectableItem<T>
    {
        bool IsSelected { get; set; }

        [EventThisIdx]
        event Action<ISelectableItem<T>> IsSelectedChanged;

        void Select();
    }

    public static class SelectableItemExtensions
    {
        public static void SelectItem<T>(this ISelectableItem<T> selectableItem)
            where T : ISelectableItem<T>
        {
            selectableItem.IsSelected = true;
        }

        public static void ToggleSelection<T>(this ISelectableItem<T> selectableItem) 
            where T : ISelectableItem<T>
        {
            selectableItem.IsSelected = !selectableItem.IsSelected;
        }
    }


    public class SelectableItem<T> : VMBase, ISelectableItem<T>, INotifyPropertyChanged
        where T : ISelectableItem<T>
    {
        bool _isSelected = false;

        [XmlIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;

                IsSelectedChanged?.Invoke(this);

                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event Action<ISelectableItem<T>> IsSelectedChanged;

        public void Select()
        {
            this.SelectItem();
        }
    }

    [WrapperInterface(typeof(ISelectableItem<>))]
    public interface ISelectableItemWrapper<T>
        where T : ISelectableItem<T>
    {
        SelectableItem<T> TheSelectableItem { get; }
    }
}
