using System;
using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingSpaceTypeToIfcSpace : CoBieLiteIfcMappings<string, SpaceType, IfcSpace>
    {
        protected override IfcSpace Mapping(SpaceType spaceType, IfcSpace ifcSpace)
        {
            ifcSpace.Name = spaceType.SpaceName;
            ifcSpace.Description = spaceType.SpaceDescription;
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceGrossAreaValue, "SpaceGrossAreaValue", Exchanger.DefaultAreaUnit);
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceNetAreaValue, "SpaceNetAreaValue", Exchanger.DefaultAreaUnit);
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceSignageName, "SpaceSignageName");
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceUsableHeightValue, "SpaceUsableHeightValue", Exchanger.DefaultAreaUnit);
            return ifcSpace;
        }
    }
}
