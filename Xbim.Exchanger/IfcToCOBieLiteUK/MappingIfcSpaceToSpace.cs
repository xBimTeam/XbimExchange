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
    class MappingIfcSpaceToSpace : XbimMappings<XbimModel, List<Facility>, string, IfcSpace, Space>
    {
        protected override Space Mapping(IfcSpace ifcSpace, Space target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcSpace);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSpace);
            target.ExternalSystem = helper.ExternalSystemName(ifcSpace);
            target.Name = ifcSpace.Name;
            target.Categories = helper.GetCategories(ifcSpace);
            target.Description = ifcSpace.Description;
            target.RoomTag = helper.GetCoBieAttribute<StringAttributeValue>("SpaceSignageName", ifcSpace).Value;
            target.UsableHeight = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceUsableHeightValue", ifcSpace).Value;
            target.GrossArea = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceGrossAreaValue", ifcSpace).Value;
            target.NetArea = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceNetAreaValue", ifcSpace).Value;


            //Attributes
            target.Attributes = helper.GetAttributes(ifcSpace);

            //TODO:
            //Space Issues
            //Space Documents
            return target;
        }
    }
}
