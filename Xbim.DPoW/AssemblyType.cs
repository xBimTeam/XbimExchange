using System;
using System.Collections.Generic;

namespace Xbim.DPoW
{
    public class AssemblyType : DPoWObject
    {
        public List<Guid> AggregationOfAssetTypes { get; set; }

        public AssemblyType()
        {
            AggregationOfAssetTypes = new List<Guid>();
        }
    }
}
