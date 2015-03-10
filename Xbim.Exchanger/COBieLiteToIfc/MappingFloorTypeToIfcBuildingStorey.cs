using System.Linq;
using Xbim.COBieLite;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using XbimExchanger.COBieLiteHelpers;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    
    public class MappingFloorTypeToIfcBuildingStorey : CoBieLiteIfcMappings<string, FloorType, IfcBuildingStorey>
    {
        protected override IfcBuildingStorey Mapping(FloorType floorType, IfcBuildingStorey buildingStorey)
        {
            buildingStorey.Name = floorType.FloorName;
            buildingStorey.Description = floorType.FloorDescription;
            buildingStorey.CompositionType = IfcElementCompositionEnum.ELEMENT;
            if (floorType.FloorElevationValue != null && floorType.FloorElevationValue.HasValue())
                buildingStorey.Elevation = floorType.FloorElevationValue.DecimalValue;
            
            Exchanger.TryCreatePropertySingleValue(buildingStorey, floorType.FloorHeightValue, "FloorHeightValue", Exchanger.DefaultLinearUnit);
           
            //write out the spaces
            if (floorType.Spaces != null)
            {
                var spaceMapping = Exchanger.GetOrCreateMappings<MappingSpaceTypeToIfcSpace>();
                foreach (var space in floorType.Spaces)
                {
                    var ifcSpace = spaceMapping.AddMapping(space, spaceMapping.GetOrCreateTargetObject(space.externalID));
                    buildingStorey.AddToSpatialDecomposition(ifcSpace);
                    Exchanger.AddToSpaceMap(buildingStorey, ifcSpace);
                }
            }

            #region Attributes

            if (floorType.FloorAttributes != null)
            {

                foreach (var attribute in floorType.FloorAttributes)
                {
                   Exchanger.ConvertAttributeTypeToIfcObjectProperty(buildingStorey, attribute);
                }
            }
            #endregion
            return buildingStorey;
        }
    }
}
