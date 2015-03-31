using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcBuildingStoreyToFloor : XbimMappings<XbimModel, List<Facility>, string, IfcBuildingStorey, Floor>
    {
        protected override Floor Mapping(IfcBuildingStorey ifcBuildingStorey, Floor target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcBuildingStorey);
            target.ExternalId = helper.ExternalEntityIdentity(ifcBuildingStorey);
            target.ExternalSystem = helper.ExternalSystemName(ifcBuildingStorey);
            target.Name = ifcBuildingStorey.Name;
            target.Categories = helper.GetCategories(ifcBuildingStorey);
            target.Description = ifcBuildingStorey.Description;
            //set the fall backs

            if (ifcBuildingStorey.Elevation.HasValue)
            {
                target.Elevation = ifcBuildingStorey.Elevation.Value;
            }
            target.Height = helper.GetCoBieAttribute<DecimalAttributeValue>("FloorHeightValue", ifcBuildingStorey).Value;


            var spaces = ifcBuildingStorey.GetSpaces();
            var ifcSpaces = spaces as IList<IfcSpace> ?? spaces.ToList();
            if (ifcSpaces.Any())
            {
                target.Spaces = new List<Space>(ifcSpaces.Count);
                var spaceMappings = Exchanger.GetOrCreateMappings<MappingIfcSpaceToSpace>();
                for (var i = 0; i < ifcSpaces.Count; i++)
                {
                    var space = new Space();
                    space = spaceMappings.AddMapping(ifcSpaces[i], space);
                    target.Spaces.Add(space);
                }
            }

            //Attributes
            target.Attributes = helper.GetAttributes(ifcBuildingStorey);

            //TODO:
            //Space Issues
            //Space Documents
            return target;
        }
    }
}
