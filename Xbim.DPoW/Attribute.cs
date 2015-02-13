using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.DPoW
{
    public class Attribute
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Value { get; set; }
        public ValueTypeEnum ValueType { get; set; }       
    }
}
