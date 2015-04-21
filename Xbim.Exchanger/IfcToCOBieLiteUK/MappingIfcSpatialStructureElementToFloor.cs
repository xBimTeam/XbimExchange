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
    class MappingIfcSpatialStructureElementToFloor : XbimMappings<XbimModel, List<Facility>, string, IfcSpatialStructureElement, Floor>
    {
        protected override Floor Mapping(IfcSpatialStructureElement ifcSpatialStructureElement, Floor target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcSpatialStructureElement);
            target.ExternalId = helper.ExternalEntityIdentity(ifcSpatialStructureElement);
            target.ExternalSystem = helper.ExternalSystemName(ifcSpatialStructureElement);
            target.Name = ifcSpatialStructureElement.Name;

            //Attributes
            target.Attributes = helper.GetAttributes(ifcSpatialStructureElement);
            if (target.Categories == null)
                target.Categories = new List<Category>();


            IEnumerable<IfcSpatialStructureElement> spaces = null;
            var site = ifcSpatialStructureElement as IfcSite;
            var building = ifcSpatialStructureElement as IfcBuilding;
            var storey = ifcSpatialStructureElement as IfcBuildingStorey;
            var spaceElement = ifcSpatialStructureElement as IfcSpace;
            if (site != null)
            {
                target.Categories.Add(new Category() { Code = "Site" });
                //upgrade code below to use extension method GetSpaces()

                if (site.IsDecomposedBy != null)
                {
                    var decomp = site.IsDecomposedBy;
                    var objs = decomp.SelectMany(s => s.RelatedObjects);
                    spaces = objs.OfType<IfcSpace>();
                }

            }
            else if (building != null)
            {
                target.Categories.Add(new Category() { Code = "Building" });
                spaces = building.GetSpaces();
            }
            else if (storey != null)
            {
                target.Categories.Add(new Category() { Code = "Floor" });
                if (storey.Elevation.HasValue)
                {
                    target.Elevation = storey.Elevation.Value;
                }
                spaces = storey.GetSpaces();
            }
            else if (spaceElement != null)
            {
                target.Categories.Add(new Category() { Code = "Space" });
                spaces = spaceElement.GetSpaces();
            }


            target.Description = ifcSpatialStructureElement.Description;
            target.CreatedBy = helper.GetCreatedBy(ifcSpatialStructureElement);
            target.CreatedOn = helper.GetCreatedOn(ifcSpatialStructureElement);
            //set the fall backs

           
            target.Height = helper.GetCoBieAttribute<DecimalAttributeValue>("FloorHeightValue", ifcSpatialStructureElement).Value;

            
            var ifcSpatialStructureElements = spaces as IList<IfcSpatialStructureElement> ?? spaces.ToList();
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
            //Space Issues
            //Space Documents
            return target;
        }
    }
}
