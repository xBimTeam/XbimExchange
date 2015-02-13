using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.DPoW
{
    public class Role:DPoWAttributableObject
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public string Description { get; set; }
    }
}
