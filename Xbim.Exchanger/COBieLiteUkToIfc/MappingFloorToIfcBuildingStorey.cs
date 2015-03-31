using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteUkToIfc
{
    
    /// <summary>
    /// 
    /// </summary>
    public class MappingFloorToIfcBuildingStorey : CoBieLiteUkIfcMappings<string, Floor, IfcBuildingStorey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorType"></param>
        /// <param name="buildingStorey"></param>
        /// <returns></returns>
        protected override IfcBuildingStorey Mapping(Floor floorType, IfcBuildingStorey buildingStorey)
        {
            buildingStorey.Name = floorType.Name;
            buildingStorey.Description = floorType.Description;
            buildingStorey.CompositionType = IfcElementCompositionEnum.ELEMENT;
            buildingStorey.Elevation = floorType.Elevation;

            Exchanger.TryCreatePropertySingleValue(buildingStorey, new DecimalAttributeValue { Value = floorType.Height}, "FloorHeightValue", Exchanger.DefaultLinearUnit);
            
            //write out the spaces
            if (floorType.Spaces != null)
            {
                var spaceMapping = Exchanger.GetOrCreateMappings<MappingSpaceToIfcSpace>();
                foreach (var space in floorType.Spaces)
                {
                    var ifcSpace = spaceMapping.AddMapping(space, spaceMapping.GetOrCreateTargetObject(space.ExternalId));
                    buildingStorey.AddToSpatialDecomposition(ifcSpace);
                    Exchanger.AddToSpaceMap(ifcSpace);
                    
                }
            }

            #region Attributes

            if (floorType.Attributes != null)
            {

                foreach (var attribute in floorType.Attributes)
                {
                   Exchanger.ConvertAttributeTypeToIfcObjectProperty(buildingStorey, attribute);
                }
            }
            #endregion
            return buildingStorey;
        }
    }
}
