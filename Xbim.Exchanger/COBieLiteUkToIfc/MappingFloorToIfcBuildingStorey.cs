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
        /// <param name="floor"></param>
        /// <param name="buildingStorey"></param>
        /// <returns></returns>
        protected override IfcBuildingStorey Mapping(Floor floor, IfcBuildingStorey buildingStorey)
        {
            buildingStorey.Name = floor.Name;
            buildingStorey.Description = floor.Description;
            buildingStorey.CompositionType = IfcElementCompositionEnum.ELEMENT;
            buildingStorey.Elevation = floor.Elevation;

            #region Categories

            if (floor.Categories != null)
                foreach (var category in floor.Categories)
                {
                    Exchanger.ConvertCategoryToClassification(category, buildingStorey);
                }

            #endregion

            Exchanger.TryCreatePropertySingleValue(buildingStorey, new DecimalAttributeValue { Value = floor.Height}, "FloorHeightValue", Exchanger.DefaultLinearUnit);
            
            //write out the spaces
            if (floor.Spaces != null)
            {
                var spaceMapping = Exchanger.GetOrCreateMappings<MappingSpaceToIfcSpace>();
                foreach (var space in floor.Spaces)
                {
                    var ifcSpace = spaceMapping.AddMapping(space, spaceMapping.GetOrCreateTargetObject(space.ExternalId));
                    buildingStorey.AddToSpatialDecomposition(ifcSpace);
                    //Exchanger.AddToSpaceMap(ifcSpace);
                }
            }

            #region Attributes

            if (floor.Attributes != null)
            {

                foreach (var attribute in floor.Attributes)
                {
                   Exchanger.ConvertAttributeTypeToIfcObjectProperty(buildingStorey, attribute);
                }
            }
            #endregion
            return buildingStorey;
        }
    }
}
