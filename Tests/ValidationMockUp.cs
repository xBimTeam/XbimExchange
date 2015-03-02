using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBieLite;
using System.Collections.Generic;
using Xbim.COBieLite.CollectionTypes;
using XbimExchanger.COBieLiteHelpers;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"DemoFiles\")]
    public class ValidationMockUp
    {
        [TestMethod]
        public void SudoValidate()
        {
            var requirements = FacilityType.ReadJson("LakesideRestaurant_stage6_Requirements.DPoW.json");
            var deliverables = FacilityType.ReadJson("LakesideRestaurant_Deliverables.DPoW.json");

            var convectorsRequirement =
                requirements.AssetTypes.FirstOrDefault(at => at.AssetTypeName == "Fan convectors");
            Assert.IsNotNull(convectorsRequirement);

            var lumniaRequirement =
                requirements.AssetTypes.FirstOrDefault(at => at.AssetTypeName == "Free standing luminaires");
            Assert.IsNotNull(lumniaRequirement);

            //validation of attributes
            ValidateAssets(deliverables, convectorsRequirement, "NBS_BiddleAirSystemsLtd");
            ValidateAssets(deliverables, lumniaRequirement, "NBS_iGuzziniIlluminazioneUKLtd");

            //save new validated file (requirements filled in with existing assets witch attributes stripped to the validated ones).
            requirements.WriteJson("..\\..\\Lakeside_Restaurant_stage6_Validation.DPoW.json");
        }

        private void ValidateAssets(FacilityType facility, AssetTypeInfoType required, string start)
        {
            //clear list of potentialy dummy assets
            required.Assets = new AssetCollectionType();
            var allValid = true;
            AssetTypeInfoType type;
            var assets = GetAssetsAndType(facility, start, out type);
            foreach (var asset in assets)
            {
                allValid = ValidateAttributes(required, type, asset) && allValid;
                if (asset != null)
                    required.Assets.Add(asset);
            }
            required.AssetTypeName = (allValid ? "[T]" : "[F]") + required.AssetTypeName;
        }

        private IEnumerable<AssetInfoType> GetAssetsAndType(FacilityType facility, string start, out AssetTypeInfoType type)
        {
            var result = new List<AssetInfoType>();
            type = null;
            foreach (var assetType in facility.AssetTypes)
            {
                foreach (var instance in assetType.Assets)
                {
                    if (instance.AssetName.StartsWith(start))
                    {
                        type = assetType;
                        result.Add(instance);
                    }
                }
            }
            return result;
        }

        private bool ValidateAttributes(AssetTypeInfoType typeRequirements, AssetTypeInfoType type, AssetInfoType asset)
        {
            //get requirements
            var requirements =
                typeRequirements.AssetTypeAttributes.Where(
                    a => a.propertySetName != null && a.propertySetName.StartsWith("[required]"));

            //merge instance and type attributes
            var attrs = asset.AssetAttributes;
            attrs.AddRange(type.AssetTypeAttributes);

            //clear and fill in only required ones
            asset.AssetAttributes = new AttributeCollectionType();
            var allValid = true;
            foreach (var requirement in requirements)
            {
                //create new instance
                var attr = new AttributeType
                {
                    AttributeName = requirement.AttributeName,
                    AttributeCategory = "Check",
                    AttributeDescription = requirement.AttributeDescription,
                    propertySetName = requirement.propertySetName,
                    propertySetExternalIdentifier = requirement.propertySetExternalIdentifier
                };

                //add value if defined
                //prefix [F] if no value is defined or [T] if it is defined
                //this will be used to report validation result
                var valAttr = attrs.FirstOrDefault(a => a.AttributeName == requirement.AttributeName);
                if (valAttr != null && valAttr.AttributeValue != null && valAttr.AttributeValue.Item != null)
                {
                    var prefix = "[T]";
                    var valType = valAttr.AttributeValue.Item as ValueBaseType;
                    if (valType != null && !valType.HasValue())
                    {
                        allValid = false;
                        prefix = "[F]";
                    }

                    attr.AttributeValue = valAttr.AttributeValue;
                    attr.AttributeName = prefix + attr.AttributeName;
                }
                else
                {
                    allValid = false;
                    attr.AttributeName = "[F]" + attr.AttributeName;
                }

                asset.AssetAttributes.Add(attr);
            }

            asset.AssetName = (allValid ? "[T]" : "[F]") + asset.AssetName;
            return allValid;
        }
    }
}