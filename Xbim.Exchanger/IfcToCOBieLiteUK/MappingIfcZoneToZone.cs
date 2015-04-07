using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcZoneToZone : XbimMappings<XbimModel, List<Facility>, string, IfcZone, Zone>
    {
        protected override Zone Mapping(IfcZone ifcZone, Zone target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcZone);
            target.ExternalId = helper.ExternalEntityIdentity(ifcZone);
            target.ExternalSystem = helper.ExternalSystemName(ifcZone);
            target.Description = ifcZone.Description;
            target.Categories = helper.GetCategories(ifcZone);
            target.Name = ifcZone.Name;
            //Attributes
            target.Attributes = helper.GetAttributes(ifcZone);

            //get spaces in zones

            var spaces = helper.GetSpaces(ifcZone);
            var ifcSpaces = spaces as IList<IfcSpace> ?? spaces.ToList();
            if (ifcSpaces.Any())
            {
                target.Spaces = new List<SpaceKey>();
                foreach (var space in ifcSpaces)
                {
                    var spaceKey = new SpaceKey { Name = space.Name };
                    target.Spaces.Add(spaceKey);
                }
            }
            //TODO:
            //Space Issues
            //Space Documents
            return target;
        }
    }
}
