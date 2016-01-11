using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcSpatialElementToFloor : XbimMappings<IfcStore, IModel, int, IIfcSpatialElement, CobieFloor>
    {
        private MappingStringToCategory _stringToCategory;
        private MappingIfcSpatialElementToSpace _spatialStructureToSpace;

        public MappingStringToCategory StringToCategory
        {
            get {
                return _stringToCategory ??
                       (_stringToCategory = Exchanger.GetOrCreateMappings<MappingStringToCategory>());
            }
        }

        public MappingIfcSpatialElementToSpace SpatialStructureToSpace
        {
            get { return _spatialStructureToSpace ?? (_spatialStructureToSpace = Exchanger.GetOrCreateMappings<MappingIfcSpatialElementToSpace>()); }
        }


        protected override CobieFloor Mapping(IIfcSpatialElement ifcSpatialStructureElement, CobieFloor target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            target.ExternalObject = helper.GetExternalObject(ifcSpatialStructureElement);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSpatialStructureElement);
            target.AltExternalId = ifcSpatialStructureElement.GlobalId;
            target.ExternalSystem = helper.GetExternalSystem(ifcSpatialStructureElement);
            target.Name = ifcSpatialStructureElement.Name;

            //Attributes
            foreach (var attr in helper.GetAttributes(ifcSpatialStructureElement))
                target.Attributes.Add(attr);
            
            IEnumerable<IIfcSpatialElement> spaces = null;
            var site = ifcSpatialStructureElement as IIfcSite;
            var building = ifcSpatialStructureElement as IIfcBuilding;
            var storey = ifcSpatialStructureElement as IIfcBuildingStorey;
            var spaceElement = ifcSpatialStructureElement as IIfcSpace;
            if (site != null)
            {
                target.Categories.Add(StringToCategory.GetOrCreate("Site"));
                //upgrade code below to use extension method GetSpaces()

                if (site.IsDecomposedBy != null)
                {
                    var decomp = site.IsDecomposedBy;
                    var objs = decomp.SelectMany(s => s.RelatedObjects);
                    spaces = objs.OfType<IIfcSpace>();
                }

            }
            else if (building != null)
            {
                target.Categories.Add(StringToCategory.GetOrCreate("Building"));
                spaces = building.Spaces;
            }
            else if (storey != null)
            {
                target.Categories.Add(StringToCategory.GetOrCreate("Floor"));
                if (storey.Elevation.HasValue)
                {
                    target.Elevation = storey.Elevation.Value;
                }
                spaces = storey.Spaces;
            }
            else if (spaceElement != null)
            {
                target.Categories.Add(StringToCategory.GetOrCreate("Space"));
                spaces = spaceElement.Spaces;
            }

            target.Description = ifcSpatialStructureElement.Description;
            target.Created = helper.GetCreatedInfo(ifcSpatialStructureElement);
            //set the fall backs

           
            target.Height = helper.GetCoBieAttribute<FloatValue>("FloorHeightValue", ifcSpatialStructureElement);

            //Documents
            var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
            helper.AddDocuments(docsMappings, target, ifcSpatialStructureElement as IIfcBuildingStorey);

            //Add spaces
            var ifcSpatialStructureElements = spaces != null ? spaces.ToList() : new List<IIfcSpatialElement>();
            ifcSpatialStructureElements.Add(ifcSpatialStructureElement);

            foreach (var element in ifcSpatialStructureElements)
            {
                CobieSpace space;
                if (!SpatialStructureToSpace.GetOrCreateTargetObject(element.EntityLabel, out space)) continue;

                space = SpatialStructureToSpace.AddMapping(element, space);
                space.Floor = target;
            }

            //Attributes
            target.Attributes.AddRange(helper.GetAttributes(ifcSpatialStructureElement));

            //TODO: Floor Issues

            return target;
        }

        public override CobieFloor CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieFloor>();
        }
    }
}
