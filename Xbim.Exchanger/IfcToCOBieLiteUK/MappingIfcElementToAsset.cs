using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcElementToAsset : XbimMappings<XbimModel, List<Facility>, string, IfcElement, Asset>
    {
        protected override Asset Mapping(IfcElement ifcElement, Asset target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcElement);
            target.ExternalId = helper.ExternalEntityIdentity(ifcElement);
            target.ExternalSystem = helper.ExternalSystemName(ifcElement);
            target.Name = ifcElement.Name;
            target.CreatedBy = helper.GetCreatedBy(ifcElement);
            target.CreatedOn = helper.GetCreatedOn(ifcElement);

            target.AssetIdentifier = helper.GetCoBieProperty("AssetIdentifier", ifcElement);
            target.BarCode = helper.GetCoBieProperty("AssetBarCode", ifcElement);
            if(!string.IsNullOrWhiteSpace(ifcElement.Description))
                target.Description = ifcElement.Description;
            else
            {
                 target.Description = helper.GetCoBieProperty("AssetSerialNumber", ifcElement);
            }
            target.InstallationDate = helper.GetCoBieProperty<DateTime>("AssetInstallationDate", ifcElement);
            target.SerialNumber = helper.GetCoBieProperty("AssetSerialNumber", ifcElement);
            target.TagNumber = helper.GetCoBieProperty("AssetTagNumber", ifcElement);
            target.WarrantyStartDate = helper.GetCoBieProperty<DateTime>("AssetWarrantyStartDate", ifcElement);



            //Attributes
            target.Attributes = helper.GetAttributes(ifcElement);
            //System Assignments

            //Space Assignments
            var spatialElements = helper.GetSpaces(ifcElement);

            var ifcSpatialStructureElements = spatialElements as IList<IfcSpatialStructureElement> ?? spatialElements.ToList();
            target.Spaces = new List<SpaceKey>();
            if (ifcSpatialStructureElements.Any())
            {

                foreach (var spatialElement in ifcSpatialStructureElements)
                {
                    var space = new SpaceKey {Name = spatialElement.Name};
                    target.Spaces.Add(space);
                }
            }
            //else //it is in nowhere land, assign it to a special space all Default External
            //{
            //    var space = new SpaceKey();
            //    space.Name = "Default External";
            //    space.KeyType = EntityType.Space;
            //    target.Spaces.Add(space);
            //}


            //Issues

            //Documents
            return target;
        }
    }
}
