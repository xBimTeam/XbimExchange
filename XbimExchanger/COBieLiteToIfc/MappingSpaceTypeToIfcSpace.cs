using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingSpaceTypeToIfcSpace : CoBieLiteIfcMappings<string, SpaceType, IfcSpace>
    {
        protected override IfcSpace Mapping(SpaceType source, IfcSpace target)
        {
            target.Name = source.SpaceName;
            target.Description = source.SpaceDescription;
            return target;
        }
    }
}
