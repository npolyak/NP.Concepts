using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NP.Concepts
{
    public class GenericParamInfo : GenericParamInfoBase, IGenericParamInfo, INotifyPropertyChanged
    {
        #region PluggedInType Property
        private Type _pluggedInType;

        public event PropertyChangedEventHandler PropertyChanged;

        public override Type PluggedInType
        {
            get
            {
                return this._pluggedInType;
            }
            set
            {
                if (this._pluggedInType == value)
                {
                    return;
                }

                this._pluggedInType = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PluggedInType)));
            }
        }
        #endregion PluggedInType Property

        public override bool Equals(object obj)
        {
            if (obj is GenericParamInfo paramObserver)
            {
                return GenericParameterType == paramObserver.GenericParameterType;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return GenericParameterType.GetHashCode();
        }

        public GenericParamInfo
        (
            Type genericParamType, 
            Type pluggedInType = null) : base(genericParamType, pluggedInType)
        {
        }
    }

}
