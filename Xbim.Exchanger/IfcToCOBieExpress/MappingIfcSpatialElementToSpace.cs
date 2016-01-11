using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcSpatialElementToSpace : XbimMappings<IfcStore, IModel, int, IIfcSpatialElement, CobieSpace>
    {
        protected override CobieSpace Mapping(IIfcSpatialElement ifcSpatialElement, CobieSpace target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            target.ExternalObject = helper.GetExternalObject(ifcSpatialElement);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSpatialElement);
            target.AltExternalId = ifcSpatialElement.GlobalId;
            target.ExternalSystem = helper.GetExternalSystem(ifcSpatialElement);
            target.Name = ifcSpatialElement.Name;
            target.Categories.AddRange(helper.GetCategories(ifcSpatialElement));
            target.Description = ifcSpatialElement.LongName;
            if(string.IsNullOrWhiteSpace(target.Description))
                target.Description = ifcSpatialElement.Description;
            target.Created = helper.GetCreatedInfo(ifcSpatialElement);
            //Attributes
            target.Attributes.AddRange(helper.GetAttributes(ifcSpatialElement));

            //use some of the attributes to fill in properties
            target.RoomTag = helper.GetCoBieAttribute<StringValue>("SpaceSignageName", ifcSpatialElement);
            target.UsableHeight = helper.GetCoBieAttribute<FloatValue>("SpaceUsableHeightValue", ifcSpatialElement);
            target.GrossArea = helper.GetCoBieAttribute<FloatValue>("SpaceGrossAreaValue", ifcSpatialElement);
            target.NetArea = helper.GetCoBieAttribute<FloatValue>("SpaceNetAreaValue", ifcSpatialElement);

            //Documents
            var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
            helper.AddDocuments(docsMappings, target, ifcSpatialElement);
            
            //TODO: Space Issues
            
            return target;
        }


        public override CobieSpace CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieSpace>();
        }
    }
}
