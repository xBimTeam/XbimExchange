using System.Collections.Generic;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcSpatialStructureElementToSpace : XbimMappings<XbimModel, List<Facility>, string, IfcSpatialStructureElement, Space>
    {
        protected override Space Mapping(IfcSpatialStructureElement ifcSpatialElement, Space target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcSpatialElement);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSpatialElement);
            target.AltExternalId = ifcSpatialElement.GlobalId;
            target.ExternalSystem = helper.ExternalSystemName(ifcSpatialElement);
            target.Name = ifcSpatialElement.Name;
            target.Categories = helper.GetCategories(ifcSpatialElement);
            target.Description = ifcSpatialElement.LongName;
            if(string.IsNullOrWhiteSpace(target.Description))
                target.Description = ifcSpatialElement.Description;
            target.CreatedBy = helper.GetCreatedBy(ifcSpatialElement);
            target.CreatedOn = helper.GetCreatedOn(ifcSpatialElement);
            target.RoomTag = helper.GetCoBieAttribute<StringAttributeValue>("SpaceSignageName", ifcSpatialElement).Value;
            target.UsableHeight = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceUsableHeightValue", ifcSpatialElement).Value;
            target.GrossArea = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceGrossAreaValue", ifcSpatialElement).Value;
            target.NetArea = helper.GetCoBieAttribute<DecimalAttributeValue>("SpaceNetAreaValue", ifcSpatialElement).Value;

            //Attributes
            target.Attributes = helper.GetAttributes(ifcSpatialElement);

            //Documents
            var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
            helper.AddDocuments(docsMappings, target, ifcSpatialElement as IfcSpace);
            //TODO:
            //Space Issues
            
            return target;
        }
    }
}
