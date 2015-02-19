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
        protected override IfcBuildingStorey Mapping(FloorType floorType, IfcBuildingStorey buildingStory)
        {
            buildingStory.Name = floorType.FloorName;
            buildingStory.Description = floorType.FloorDescription;
            if (floorType.FloorElevationValue != null && floorType.FloorElevationValue.HasValue())
                buildingStory.Elevation = floorType.FloorElevationValue.DecimalValue;
            
            Exchanger.TryCreatePropertySingleValue(buildingStory, floorType.FloorHeightValue, "FloorHeightValue", Exchanger.DefaultLinearUnit);
           
            //write out the spaces
            if (floorType.Spaces != null)
            {
                var spaceMapping = Exchanger.GetOrCreateMappings<MappingSpaceTypeToIfcSpace>();
                foreach (var space in floorType.Spaces)
                {
                    var ifcSpace = spaceMapping.AddMapping(space, spaceMapping.GetOrCreateTargetObject(space.externalID));
                    buildingStory.AddToSpatialDecomposition(ifcSpace);
                }
            }

            #region Attributes

            if (floorType.FloorAttributes != null)
            {

                foreach (var attribute in floorType.FloorAttributes)
                {
                   Exchanger.ConvertAttributeTypeToIfcObjectProperty(buildingStory, attribute);
                }
            }
            #endregion
            return buildingStory;
        }
    }
}
