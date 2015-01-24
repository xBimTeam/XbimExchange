using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.COBieLiteToIfc
{
    
    public class MappingFloorTypeToIfcBuildingStorey : COBieLiteIfcMappings<string, FloorType, IfcBuildingStorey>
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
