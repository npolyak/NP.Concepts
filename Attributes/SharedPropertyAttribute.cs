using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Concepts.Attributes
{
    public class SharedPropertyAttribute : Attribute
    {
        public Type InitType { get; set; }
    }
}
