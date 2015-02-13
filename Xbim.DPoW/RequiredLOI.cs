using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.DPoW
{
    public class RequiredLOI
    {
        public String Code { get; set; }

        public int Description { get; set; }

        public List<RequiredAttribute> RequiredAttributes { get; set; }
    }
}
