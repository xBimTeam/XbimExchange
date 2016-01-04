using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    class MappingIfcSpatialStructureElementToFloor : XbimMappings<IfcStore, List<Facility>, string, IIfcSpatialStructureElement, Floor>
    {
        protected override Floor Mapping(IIfcSpatialStructureElement ifcSpatialStructureElement, Floor target)
        {
            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcSpatialStructureElement);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSpatialStructureElement);
            target.AltExternalId = ifcSpatialStructureElement.GlobalId;
            target.ExternalSystem = helper.ExternalSystemName(ifcSpatialStructureElement);
            target.Name = ifcSpatialStructureElement.Name;

            //Attributes
            target.Attributes = helper.GetAttributes(ifcSpatialStructureElement);
            if (target.Categories == null)
                target.Categories = new List<Category>();


            IEnumerable<IIfcSpatialStructureElement> spaces = null;
            var site = ifcSpatialStructureElement as IIfcSite;
            var building = ifcSpatialStructureElement as IIfcBuilding;
            var storey = ifcSpatialStructureElement as IIfcBuildingStorey;
            var spaceElement = ifcSpatialStructureElement as IIfcSpace;
            if (site != null)
            {
                target.Categories.Add(new Category() { Code = "Site" });
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
                target.Categories.Add(new Category() { Code = "Building" });
                spaces = building.Spaces;
            }
            else if (storey != null)
            {
                target.Categories.Add(new Category() { Code = "Floor" });
                if (storey.Elevation.HasValue)
                {
                    target.Elevation = storey.Elevation.Value;
                }
                spaces = storey.Spaces;
            }
            else if (spaceElement != null)
            {
                target.Categories.Add(new Category() { Code = "Space" });
                spaces = spaceElement.Spaces;
            }


            target.Description = ifcSpatialStructureElement.Description;
            target.CreatedBy = helper.GetCreatedBy(ifcSpatialStructureElement);
            target.CreatedOn = helper.GetCreatedOn(ifcSpatialStructureElement);
            //set the fall backs

           
            target.Height = helper.GetCoBieAttribute<DecimalAttributeValue>("FloorHeightValue", ifcSpatialStructureElement).Value;

            //Documents
            var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
            helper.AddDocuments(docsMappings, target, ifcSpatialStructureElement as IIfcBuildingStorey);

            //Add spaces
            var ifcSpatialStructureElements = spaces.ToList();
            ifcSpatialStructureElements.Add(ifcSpatialStructureElement);

            target.Spaces = new List<Space>(ifcSpatialStructureElements.Count);
            var spaceMappings = Exchanger.GetOrCreateMappings<MappingIfcSpatialStructureElementToSpace>();
            for (var i = 0; i < ifcSpatialStructureElements.Count; i++)
            {
                var space = new Space();
                space = spaceMappings.AddMapping(ifcSpatialStructureElements[i], space);
                target.Spaces.Add(space);
            }


            //Attributes
            target.Attributes = helper.GetAttributes(ifcSpatialStructureElement);

            //TODO:
            //Floor Issues
            return target;
        }

        public override Floor CreateTargetObject()
        {
            return new Floor();
        }
    }
}
