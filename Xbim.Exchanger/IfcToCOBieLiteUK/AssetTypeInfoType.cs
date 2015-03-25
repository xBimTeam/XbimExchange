using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedFacilitiesElements;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public class AssetTypeInfoType : AssetType
    {
       
     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcTypeObject"></param>
        /// <param name="helper"></param>
        public AssetTypeInfoType(IfcTypeObject ifcTypeObject, CoBieLiteUkHelper helper)    
        {
            ExternalEntity = helper.ExternalEntityName(ifcTypeObject);
            ExternalId = helper.ExternalEntityIdentity(ifcTypeObject);
            ExternalSystem = helper.ExternalSystemName(ifcTypeObject);
            Name = ifcTypeObject.Name;
            Categories = helper.GetCategories(ifcTypeObject);
            string accCategoryString =  helper.GetCoBieProperty("AssetTypeAccountingCategory", ifcTypeObject);
            AssetPortability accCategoryEnum;
            if (Enum.TryParse(accCategoryString, true, out accCategoryEnum))
                Categories = new List<Category>(1) {new Category {Code=accCategoryEnum.ToString()}};
            else
            {
                CoBieLiteUkHelper.Logger.WarnFormat("AssetTypeAccountingCategory: An illegal value of [{0}] has been passed for the category of #{1}={2}. It has been replaced with a value of 'Item'", accCategoryString, ifcTypeObject.EntityLabel, ifcTypeObject.GetType().Name);
                //Categories = new List<Category>(1) { new Category() { Code = "Item" } };
            }
            if (!Categories.Any()) //try the asset assignment
            {
                IfcAsset ifcAsset;
                if (helper.AssetAsignments.TryGetValue(ifcTypeObject, out ifcAsset))
                {
                    string portability = helper.GetCoBieAttribute<StringAttributeValue>("AssetTypeAccountingCategory", ifcAsset).Value;
                    if (!string.IsNullOrWhiteSpace(portability))
                        Categories = new List<Category>(1) { new Category { Code = "portability" } };
                }
            }

            Description = ifcTypeObject.Description;
            ModelNumber = helper.GetCoBieProperty("AssetTypeModelNumber", ifcTypeObject);
            ReplacementCost = helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeReplacementCostValue", ifcTypeObject).Value;
            ExpectedLife = helper.GetCoBieAttribute<IntegerAttributeValue>("AssetTypeExpectedLifeValue", ifcTypeObject).Value;
            NominalLength = helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalLength", ifcTypeObject).Value;
            NominalWidth = helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalWidth", ifcTypeObject).Value;
            NominalHeight = helper.GetCoBieAttribute<DecimalAttributeValue>("AssetTypeNominalHeight", ifcTypeObject).Value;
            AccessibilityPerformance = helper.GetCoBieProperty("AssetTypeAccessibilityText", ifcTypeObject);
            Color = helper.GetCoBieProperty("AssetTypeColorCode", ifcTypeObject);
            Constituents = helper.GetCoBieProperty("AssetTypeConstituentsDescription", ifcTypeObject);
            Features = helper.GetCoBieProperty("AssetTypeFeaturesDescription", ifcTypeObject);
            Grade = helper.GetCoBieProperty("AssetTypeGradeDescription", ifcTypeObject);
            Material = helper.GetCoBieProperty("AssetTypeMaterialDescription", ifcTypeObject);
            Shape = helper.GetCoBieProperty("AssetTypeShapeDescription", ifcTypeObject);
            Size = helper.GetCoBieProperty("AssetTypeSizeDescription", ifcTypeObject);
            SustainabilityPerformance = helper.GetCoBieProperty("AssetTypeSustainabilityPerformanceDescription", ifcTypeObject);

            //The Assets
            List<IfcElement> allAssetsofThisType;
            if (helper.DefiningTypeObjectMap.TryGetValue(ifcTypeObject, out allAssetsofThisType)) //should always work
            {
                Assets = new List<Asset>(allAssetsofThisType.Count);
                foreach (IfcElement t in allAssetsofThisType)
                {
                    Assets.Add(new AssetInfoType(t, helper));
                }
            }
            else
            {
                //just in case we have a problem
                CoBieLiteUkHelper.Logger.ErrorFormat("Asset Type: Failed to locate Asset Type #{0}={1}", ifcTypeObject.EntityLabel,ifcTypeObject.GetType().Name);
            }

            //Attributes
            Attributes = helper.GetAttributes(ifcTypeObject);
        }

    }
}
