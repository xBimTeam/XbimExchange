using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingSpaceTypeToIfcSpace : CoBieLiteIfcMappings<string, SpaceType, IfcSpace>
    {
        protected override IfcSpace Mapping(SpaceType spaceType, IfcSpace ifcSpace)
        {
            ifcSpace.Name = spaceType.SpaceName;
            ifcSpace.Description = spaceType.SpaceDescription;
            ifcSpace.CompositionType=IfcElementCompositionEnum.ELEMENT;
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceGrossAreaValue, "SpaceGrossAreaValue", Exchanger.DefaultAreaUnit);
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceNetAreaValue, "SpaceNetAreaValue", Exchanger.DefaultAreaUnit);
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceSignageName, "SpaceSignageName");
            Exchanger.TryCreatePropertySingleValue(ifcSpace, spaceType.SpaceUsableHeightValue, "SpaceUsableHeightValue", Exchanger.DefaultAreaUnit);

            #region Attributes
            if (spaceType.SpaceAttributes != null)
            {
                foreach (var attribute in spaceType.SpaceAttributes)
                    Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcSpace, attribute);
            }
            #endregion


           
            return ifcSpace;
        }
    }
}
