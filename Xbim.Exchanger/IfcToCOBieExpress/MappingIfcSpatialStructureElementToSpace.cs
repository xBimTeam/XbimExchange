using System.Collections.Generic;
using Xbim.CobieExpress;
using Xbim.COBieLiteUK;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    class MappingIfcSpatialStructureElementToSpace : XbimMappings<IfcStore, List<Facility>, string, IIfcSpatialStructureElement, Space>
    {
        protected override Space Mapping(IIfcSpatialStructureElement ifcSpatialElement, Space target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
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
            target.RoomTag = helper.GetCoBieAttribute<StringValue>("SpaceSignageName", ifcSpatialElement);
            target.UsableHeight = helper.GetCoBieAttribute<FloatValue>("SpaceUsableHeightValue", ifcSpatialElement);
            target.GrossArea = helper.GetCoBieAttribute<FloatValue>("SpaceGrossAreaValue", ifcSpatialElement);
            target.NetArea = helper.GetCoBieAttribute<FloatValue>("SpaceNetAreaValue", ifcSpatialElement);

            //Attributes
            target.Attributes = helper.GetAttributes(ifcSpatialElement);

            //Documents
            var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
            helper.AddDocuments(docsMappings, target, ifcSpatialElement as IIfcSpace);
            //TODO:
            //Space Issues
            
            return target;
        }


        public override Space CreateTargetObject()
        {
            return new Space();
        }
    }
}
