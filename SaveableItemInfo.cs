﻿using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System.Xml.Serialization;

namespace NP.Concepts
{
    public class SaveableItemInfo : VMBase, ICopyable<SaveableItemInfo>
    {
        #region IsComponent Property
        private bool _isComposite;
        [XmlAttribute]
        public bool IsComponent
        {
            get
            {
                return this._isComposite;
            }
            set
            {
                if (this._isComposite == value)
                {
                    return;
                }

                this._isComposite = value;
                this.OnPropertyChanged(nameof(IsComponent));
            }
        }
        #endregion IsComponent Property

        #region ItemName Property
        private string _itemName;
        [XmlAttribute]
        public string ItemName
        {
            get
            {
                return this._itemName;
            }
            set
            {
                if (this._itemName == value)
                {
                    return;
                }

                this._itemName = value;
                this.OnPropertyChanged(nameof(ItemName));
            }
        }
        #endregion ItemName Property


        public override bool Equals(object obj)
        {
            if (obj is SaveableItemInfo layoutInfo)
            {
                return this.ItemName == layoutInfo.ItemName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ItemName.GetHashCodeExtension();
        }

        public override string ToString()
        {
            return ItemName ?? "Default Item";
        }

        public void CopyFrom(SaveableItemInfo layoutInfo)
        {
            this.ItemName = layoutInfo.ItemName;
            this.IsComponent = layoutInfo.IsComponent;
        }
    }
}
