using System.Linq;
using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    
    public class MappingFloorTypeToIfcBuildingStorey : CoBieLiteIfcMappings<string, FloorType, IfcBuildingStorey>
    {
        protected override IfcBuildingStorey Mapping(FloorType source, IfcBuildingStorey target)
        {
            target.Name = source.FloorName;
            target.Description = source.FloorDescription;
            
            if (source.Spaces != null && source.Spaces.Any())
            {
                //map spaces
                var spaceMappings = Exchanger.GetOrCreateMappings<MappingSpaceTypeToIfcSpace>();
                foreach (var sSpace in source.Spaces)
                {
                    var tSpace = spaceMappings.GetOrCreateTargetObject(sSpace.externalID);
                    spaceMappings.AddMapping(sSpace, tSpace);
                }
            }

            return target;
        }
    }
}
