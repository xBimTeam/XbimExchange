using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ProductExtension;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public class AssetInfoType: Asset
    {
    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcElement"></param>
        /// <param name="helper"></param>
        public AssetInfoType(IfcElement ifcElement, CoBieLiteUkHelper helper) 
        {
            ExternalEntity = helper.ExternalEntityName(ifcElement);
            ExternalId = helper.ExternalEntityIdentity(ifcElement);
            ExternalSystem = helper.ExternalSystemName(ifcElement);
            Name = ifcElement.Name;
            Description = ifcElement.Description;
       
            SerialNumber = helper.GetCoBieProperty("AssetSerialNumber", ifcElement);
            InstallationDate = helper.GetCoBieProperty<DateTime>("AssetInstallationDate", ifcElement);
            TagNumber = helper.GetCoBieProperty("AssetInstalledModelNumber", ifcElement);
            WarrantyStartDate = helper.GetCoBieProperty<DateTime>("AssetWarrantyStartDate", ifcElement);     
            
            TagNumber = helper.GetCoBieProperty("AssetTagNumber", ifcElement);
            BarCode = helper.GetCoBieProperty("AssetBarCode", ifcElement);
            AssetIdentifier = helper.GetCoBieProperty("AssetIdentifier", ifcElement);
            
            //Attributes
            Attributes = helper.GetAttributes(ifcElement);
            //System Assignments

             //Space Assignments
            var spatialElements = helper.GetSpaces(ifcElement);

            var ifcSpatialStructureElements = spatialElements as IList<IfcSpatialStructureElement> ?? spatialElements.ToList();
            Spaces = new List<SpaceKey>();
            if (ifcSpatialStructureElements.Any())
            {

                foreach (var spatialElement in ifcSpatialStructureElements)
                {
                    var space = new SpaceKey();

                    space.Name = spatialElement.Name;
                    if (spatialElement is IfcSpace)
                        space.KeyType = EntityType.Space;
                    else if (spatialElement is IfcBuildingStorey)
                        space.KeyType = EntityType.Floor;
                    else if (spatialElement is IfcBuilding)
                        space.KeyType = EntityType.Facility;
                    else if (spatialElement is IfcSite)
                        space.KeyType = EntityType.Space;
                    Spaces.Add(space);
                }
            }
            else //it is in nowhere land, assign it to a special space all Default External
            {
                var space = new SpaceKey();
                space.Name = "Default External";
                space.KeyType = EntityType.Space;
                Spaces.Add(space);
            }

           
            //Issues

            //Documents

        }

    }
}
