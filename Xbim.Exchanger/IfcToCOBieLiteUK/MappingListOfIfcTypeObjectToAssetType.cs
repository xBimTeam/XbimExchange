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
    internal class MappingListOfIfcTypeObjectToAssetType :
        XbimMappings<XbimModel, List<Facility>, string, IEnumerable<IfcTypeObject>, AssetType>
    {
        protected override AssetType Mapping(IEnumerable<IfcTypeObject> ifcTypeObjectList, AssetType target)
        {
            var ifcTypeObject = ifcTypeObjectList.First(); //all the same so first is as good as any
            var helper = ((IfcToCOBieLiteUkExchanger) Exchanger).Helper;
            target.ExternalEntity = helper.ExternalEntityName(ifcTypeObject);
            target.ExternalId = helper.ExternalEntityIdentity(ifcTypeObject);
            target.ExternalSystem = helper.ExternalSystemName(ifcTypeObject);
            target.Name = ifcTypeObject.Name;
            target.Categories = helper.GetCategories(ifcTypeObject);
            string accCategoryString = helper.GetCoBieProperty("AssetTypeAccountingCategory", ifcTypeObject);
            AssetPortability accCategoryEnum;
            if (Enum.TryParse(accCategoryString, true, out accCategoryEnum))
                target.Categories = new List<Category>(1) {new Category {Code = accCategoryEnum.ToString()}};
            else
            {
                CoBieLiteUkHelper.Logger.WarnFormat(
                    "AssetTypeAccountingCategory: An illegal value of [{0}] has been passed for the category of #{1}={2}. It has been replaced with a value of 'Item'",
                    accCategoryString, ifcTypeObject.EntityLabel, ifcTypeObject.GetType().Name);
                //Categories = new List<Category>(1) { new Category() { Code = "Item" } };
            }
            if (target.Categories != null && !target.Categories.Any()) //try the asset assignment
            {
                IfcAsset ifcAsset;
                if (helper.AssetAsignments.TryGetValue(ifcTypeObject, out ifcAsset))
                {
                    string portability =
                        helper.GetCoBieAttribute<StringAttributeValue>("AssetTypeAccountingCategory", ifcAsset).Value;
                    if (!string.IsNullOrWhiteSpace(portability))
                        target.Categories = new List<Category>(1) {new Category {Code = "portability"}};
                }
            }

            target.Description = ifcTypeObject.Description;
            target.ModelNumber = helper.GetCoBieProperty("AssetTypeModelNumber", ifcTypeObject);
            target.ReplacementCost =
                helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeReplacementCostValue", ifcTypeObject).Value;
            target.ExpectedLife =
                helper.GetCoBieAttribute<IntegerAttributeValue>("AssetTypeExpectedLifeValue", ifcTypeObject).Value;
            target.NominalLength =
                helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalLength", ifcTypeObject).Value;
            target.NominalWidth =
                helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalWidth", ifcTypeObject).Value;
            target.NominalHeight =
                helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalHeight", ifcTypeObject).Value;
            target.AccessibilityPerformance = helper.GetCoBieProperty("AssetTypeAccessibilityText", ifcTypeObject);
            target.Color = helper.GetCoBieProperty("AssetTypeColorCode", ifcTypeObject);
            target.Constituents = helper.GetCoBieProperty("AssetTypeConstituentsDescription", ifcTypeObject);
            target.Features = helper.GetCoBieProperty("AssetTypeFeaturesDescription", ifcTypeObject);
            target.Grade = helper.GetCoBieProperty("AssetTypeGradeDescription", ifcTypeObject);
            target.Material = helper.GetCoBieProperty("AssetTypeMaterialDescription", ifcTypeObject);
            target.Shape = helper.GetCoBieProperty("AssetTypeShapeDescription", ifcTypeObject);
            target.Size = helper.GetCoBieProperty("AssetTypeSizeDescription", ifcTypeObject);
            target.SustainabilityPerformance = helper.GetCoBieProperty("AssetTypeSustainabilityPerformanceDescription",
                ifcTypeObject);

            //The Assets
            var assetMappings = Exchanger.GetOrCreateMappings<MappingIfcElementToAsset>();
            target.Assets = new List<Asset>();
            foreach (var typeObject in ifcTypeObjectList)
            {
                List<IfcElement> allAssetsofThisType;
                if (helper.DefiningTypeObjectMap.TryGetValue(typeObject, out allAssetsofThisType))
                    //should always work
                {
           
                    foreach (IfcElement element in allAssetsofThisType)
                    {
                        var asset = new Asset();
                        asset = assetMappings.AddMapping(element, asset);
                        target.Assets.Add(asset);
                    }
                }
                else
                {
                    //just in case we have a problem
                    CoBieLiteUkHelper.Logger.ErrorFormat("Asset Type: Failed to locate Asset Type #{0}={1}",
                        ifcTypeObject.EntityLabel, ifcTypeObject.GetType().Name);
                }
            }
            //Attributes
            target.Attributes = helper.GetAttributes(ifcTypeObject);
            return target;
        }

    }
}
