using Xbim.COBieLite;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingFloorTypeToIfcBuildingStorey : XbimIfcMappings<string, FloorType, IfcBuildingStorey>
    {
        protected override IfcBuildingStorey Mapping(FloorType floorType, IfcBuildingStorey buildingStory)
        {
            buildingStory.Name = floorType.FloorName;
            buildingStory.Description = floorType.FloorDescription;
            if (floorType.FloorElevationValue != null && floorType.FloorElevationValue.DecimalValueSpecified)
                buildingStory.Elevation = floorType.FloorElevationValue.DecimalValue;
            if (floorType.FloorHeightValue != null && floorType.FloorHeightValue.DecimalValueSpecified)
            {
                var valueHelper = new XbimCoBieValueHelper(floorType.FloorHeightValue, IfcSIUnitName.METRE);
            }

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
            return buildingStory;
        }
    }
}
