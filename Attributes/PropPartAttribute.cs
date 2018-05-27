using NP.Utilities.IoCUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropPartAttribute : Attribute, IPropNameAndKeyObj
    {
        public string PropName { get; }

        public object KeyObj { get; }

        public PropPartAttribute(string propName, object keyObject = null)
        {
            PropName = propName;
            KeyObj = keyObject;
        }

    }
}
