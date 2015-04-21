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
    class MappingIfcSystemToSystem : XbimMappings<XbimModel, List<Facility>, string, IfcSystem, Xbim.COBieLiteUK.System>
    {
        protected override Xbim.COBieLiteUK.System Mapping(IfcSystem ifcSystem, Xbim.COBieLiteUK.System target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcSystem);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSystem);
            target.ExternalSystem = helper.ExternalSystemName(ifcSystem);
            target.Name = ifcSystem.Name;
            target.Description = ifcSystem.Description;
            target.CreatedBy = helper.GetCreatedBy(ifcSystem);
            target.CreatedOn = helper.GetCreatedOn(ifcSystem);
            target.Categories = helper.GetCategories(ifcSystem);
            //Add Assets
            var systemAssignments = helper.GetSystemAssignments(ifcSystem);
            var ifcObjectDefinitions = systemAssignments as IList<IfcObjectDefinition> ?? systemAssignments.ToList();
            if (ifcObjectDefinitions.Any())
            {
                target.Components = new List<AssetKey>();
                foreach (var ifcObjectDefinition in ifcObjectDefinitions)
                {
                    var assetKey = new AssetKey { Name = ifcObjectDefinition.Name };
                    target.Components.Add(assetKey);
                }
            }

            //Attributes
            target.Attributes = helper.GetAttributes(ifcSystem);

            //TODO:
            //System Issues
            //System Documents
            return target;
        }
    }
}
