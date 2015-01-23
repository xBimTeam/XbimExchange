using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingSpaceTypeToIfcSpace : XbimIfcMappings<string, SpaceType, IfcSpace>
    {
        protected override IfcSpace Mapping(SpaceType source, IfcSpace target)
        {
            target.Name = source.SpaceName;
            target.Description = source.SpaceDescription;
            return target;
        }
    }
}
