using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public partial class AssetInfoType
    {
        public AssetInfoType()
        {
            
        }

        public AssetInfoType(IfcElement ifcElement, CoBieLiteHelper helper)
            : this()
        {
            externalEntityName = helper.ExternalEntityName(ifcElement);
            externalID = helper.ExternalEntityIdentity(ifcElement);
            externalSystemName = helper.ExternalSystemName(ifcElement);
            AssetName = ifcElement.Name;
            AssetDescription = ifcElement.Description;
            
            AssetSerialNumber = helper.GetCoBieProperty("AssetSerialNumber", ifcElement);
            AssetInstallationDate = helper.GetCoBieProperty<DateTime>("AssetInstallationDate", ifcElement);
            AssetInstallationDateSpecified = AssetInstallationDate != default(DateTime);
            AssetInstalledModelNumber = helper.GetCoBieProperty("AssetInstalledModelNumber", ifcElement);
            AssetWarrantyStartDate = helper.GetCoBieProperty<DateTime>("AssetWarrantyStartDate", ifcElement);
            AssetWarrantyStartDateSpecified = AssetWarrantyStartDate != default(DateTime);
            AssetStartDate = helper.GetCoBieProperty("AssetStartDate", ifcElement); //why isn't this a date in the schema?
            AssetTagNumber = helper.GetCoBieProperty("AssetTagNumber", ifcElement);
            AssetBarCode = helper.GetCoBieProperty("AssetBarCode", ifcElement);
            AssetIdentifier = helper.GetCoBieProperty("AssetIdentifier", ifcElement);
            AssetLocationDescription = helper.GetCoBieProperty("AssetLocationDescription", ifcElement);
            
            //Attributes
            AttributeType[] ifcAttributes = helper.GetAttributes(ifcElement);
            if (ifcAttributes != null && ifcAttributes.Length > 0)
                AssetAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
            
            //System Assignments
            List<IfcSystem> systems;
            if (helper.SystemLookup.TryGetValue(ifcElement, out systems))
            {
                AssetSystemAssignments = new SystemAssignmentCollectionType { SystemAssignment = new SystemKeyType[systems.Count] };
                for (int i = 0; i < systems.Count; i++)
                {
                    AssetSystemAssignments.SystemAssignment[i] = new SystemKeyType(systems[i], helper);
                }
            }

             //Space Assignments
            List<IfcSpace> spaces;
            if (helper.SpaceAssetLookup.TryGetValue(ifcElement, out spaces))
            {
                AssetSpaceAssignments = new SpaceAssignmentCollectionType { SpaceAssignment = new SpaceKeyType[spaces.Count] };
                for (int i = 0; i < spaces.Count; i++)
                {
                    AssetSpaceAssignments.SpaceAssignment[i] = new SpaceKeyType(spaces[i], helper);
                }
            }
            
            //Issues

            //Documents
        }
    }
}
