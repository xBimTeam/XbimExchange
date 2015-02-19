using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.DPoW;
using Space = Xbim.COBieLite.SpaceType;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    class MappingSpaceTypeToSpaceType : DPoWToCOBieLiteMapping<SpaceType, Space>
    {
        protected override Space Mapping(SpaceType source, Space target)
        {
            throw new NotImplementedException();
        }
    }
}
