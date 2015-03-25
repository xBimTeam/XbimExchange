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
            IfcSpatialStructureElement space;
            if (helper.SpaceAssetLookup.TryGetValue(ifcElement, out space))
            {
                Space = new SpaceKey();
                if (space != null)
                {
                    Space.Name = space.Name;
                    if (space is IfcSpace)
                        Space.KeyType = EntityType.Space;
                    else if (space is IfcBuildingStorey)
                        Space.KeyType = EntityType.Floor;
                    else if (space is IfcBuilding)
                        Space.KeyType = EntityType.Facility;
                    else if (space is IfcSite)
                        Space.KeyType = EntityType.Space;
                }
                else //it is in nowhere land, assign it to a special space all Default External
                {
                    Space.Name = "Default External";
                    Space.KeyType = EntityType.Space;
                }
            }
           
            //Issues

            //Documents

        }

    }
}
