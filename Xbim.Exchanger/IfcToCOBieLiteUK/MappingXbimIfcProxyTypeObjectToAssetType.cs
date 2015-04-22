using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedFacilitiesElements;
using Xbim.IO;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// Maps a list of IfcTypeObject that are all the same
    /// </summary>
    internal class MappingXbimIfcProxyTypeObjectToAssetType :
        XbimMappings<XbimModel, List<Facility>, string, XbimIfcProxyTypeObject, AssetType>
    {
        protected override AssetType Mapping(XbimIfcProxyTypeObject proxyIfcTypeObject, AssetType target)
        {
           
            var helper = ((IfcToCOBieLiteUkExchanger) Exchanger).Helper;
            target.ExternalEntity = proxyIfcTypeObject.ExternalEntity;
            target.ExternalId = proxyIfcTypeObject.ExternalId;
            target.ExternalSystem = proxyIfcTypeObject.ExternalSystemName;
            target.Name = proxyIfcTypeObject.Name;
            target.Categories = proxyIfcTypeObject.Categories;
            target.AssetTypeEnum = proxyIfcTypeObject.AccountingCategory;
            target.CreatedBy = proxyIfcTypeObject.GetCreatedBy();
            target.CreatedOn = proxyIfcTypeObject.GetCreatedOn();
            target.Description = proxyIfcTypeObject.Description;
            var ifcTypeObject = proxyIfcTypeObject.IfcTypeObject;
            List<IfcElement> allAssetsofThisType;
            helper.DefiningTypeObjectMap.TryGetValue(proxyIfcTypeObject, out allAssetsofThisType);

            target.Warranty = new Warranty {GuarantorLabor = new ContactKey {Email = helper.XbimCreatedBy.Email}};
            target.Warranty.GuarantorParts = target.Warranty.GuarantorLabor;
            if (ifcTypeObject != null)
            {
                var manuf = helper.GetCoBieProperty("AssetTypeManufacturer", ifcTypeObject);
                if (string.IsNullOrWhiteSpace(manuf) && allAssetsofThisType!=null) //disagrrement between COBie and IFC where this value resides, look in assets
                {
                    manuf = allAssetsofThisType.Select(
                        a => helper.GetCoBieProperty("AssetTypeManufacturer", a))
                        .FirstOrDefault(a=>!string.IsNullOrWhiteSpace(a));

                }
                target.Manufacturer = helper.GetOrCreateContactKey(manuf);

                target.ModelNumber = helper.GetCoBieProperty("AssetTypeModelNumber", ifcTypeObject);
                target.ModelReference = helper.GetCoBieProperty("AssetTypeModelReference", ifcTypeObject);
                target.ReplacementCost =
                    helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeReplacementCostValue",
                        ifcTypeObject).Value;
                target.ExpectedLife =
                    helper.GetCoBieAttribute<IntegerAttributeValue>("AssetTypeExpectedLifeValue", ifcTypeObject)
                        .Value;
                target.NominalLength =
                    helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalLength", ifcTypeObject).Value;
                target.NominalWidth =
                    helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalWidth", ifcTypeObject).Value;
                target.NominalHeight =
                    helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalHeight", ifcTypeObject).Value;
                target.AccessibilityPerformance = helper.GetCoBieProperty("AssetTypeAccessibilityText",
                    ifcTypeObject);
                target.Color = helper.GetCoBieProperty("AssetTypeColorCode", ifcTypeObject);
                target.Constituents = helper.GetCoBieProperty("AssetTypeConstituentsDescription", ifcTypeObject);
                DurationUnit unit;
                if (Enum.TryParse(helper.GetCoBieProperty("AssetTypeDurationUnit", ifcTypeObject), true,
                    out unit))
                    target.DurationUnit = unit;
                target.Features = helper.GetCoBieProperty("AssetTypeFeaturesDescription", ifcTypeObject);
                target.Grade = helper.GetCoBieProperty("AssetTypeGradeDescription", ifcTypeObject);
                target.Material = helper.GetCoBieProperty("AssetTypeMaterialDescription", ifcTypeObject);
                target.Shape = helper.GetCoBieProperty("AssetTypeShapeDescription", ifcTypeObject);
                target.Size = helper.GetCoBieProperty("AssetTypeSizeDescription", ifcTypeObject);
                target.SustainabilityPerformance =
                    helper.GetCoBieProperty("AssetTypeSustainabilityPerformanceDescription", ifcTypeObject);
                target.CodePerformance = helper.GetCoBieProperty("AssetTypeCodePerformance", ifcTypeObject);
                target.Finish = helper.GetCoBieProperty("AssetTypeFinishDescription", ifcTypeObject);

                
                target.Warranty.Description = helper.GetCoBieProperty("AssetTypeWarrantyDescription", ifcTypeObject);
                target.Warranty.DurationLabor = helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeWarrantyDurationLabor", ifcTypeObject).Value;
                target.Warranty.DurationParts = helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeWarrantyDurationParts", ifcTypeObject).Value;

               
                if (Enum.TryParse(helper.GetCoBieProperty("AssetTypeWarrantyDurationUnit", ifcTypeObject), true,
                    out unit))
                    target.Warranty.DurationUnit = unit;
                var laborContact = helper.GetCoBieProperty("AssetTypeWarrantyGuarantorLabor", ifcTypeObject);
                if(!string.IsNullOrWhiteSpace(laborContact))
                    target.Warranty.GuarantorLabor = helper.GetOrCreateContactKey(laborContact);
                var partsContact = helper.GetCoBieProperty("AssetTypeWarrantyGuarantorParts", ifcTypeObject);
                if (!string.IsNullOrWhiteSpace(partsContact))
                    target.Warranty.GuarantorParts = helper.GetOrCreateContactKey(partsContact);
                //Attributes
                target.Attributes = helper.GetAttributes(ifcTypeObject);
            }
            //The Assets
            
            var assetMappings = Exchanger.GetOrCreateMappings<MappingIfcElementToAsset>();
            if (allAssetsofThisType != null && allAssetsofThisType.Any())
            {
                target.Assets = new List<Asset>();

                    foreach (IfcElement element in allAssetsofThisType)
                    {
                        var asset = new Asset();
                        asset = assetMappings.AddMapping(element, asset);
                        target.Assets.Add(asset);
                    }
            }

            return target;
        }

    }
}
