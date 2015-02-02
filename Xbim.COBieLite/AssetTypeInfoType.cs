using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedFacilitiesElements;

namespace Xbim.COBieLite
{
    public partial class AssetTypeInfoType
    {
       
        public AssetTypeInfoType()
        {
            
        }

        public AssetTypeInfoType(IfcTypeObject ifcTypeObject, CoBieLiteHelper helper)
            : this()
        {
            externalEntityName = helper.ExternalEntityName(ifcTypeObject);
            externalID = helper.ExternalEntityIdentity(ifcTypeObject);
            externalSystemName = helper.ExternalSystemName(ifcTypeObject);
            AssetTypeName = ifcTypeObject.Name;
            AssetTypeCategory = helper.GetClassification(ifcTypeObject);
            string accCategoryString =  helper.GetCoBieProperty("AssetTypeAccountingCategory", ifcTypeObject);
            AssetPortabilitySimpleType accCategoryEnum;
            if (Enum.TryParse(accCategoryString, true, out accCategoryEnum))
                AssetTypeAccountingCategory = accCategoryEnum;
            else
            {
                CoBieLiteHelper.Logger.WarnFormat("AssetTypeAccountingCategory: An illegal value of [{0}] has been passed for the category of #{1}={2}. It has been replaced with a value of 'Item'", accCategoryString, ifcTypeObject.EntityLabel, ifcTypeObject.GetType().Name);
                AssetTypeAccountingCategory = AssetPortabilitySimpleType.Item;
            }
            if (string.IsNullOrWhiteSpace(AssetTypeCategory)) //try the asset assignment
            {
                IfcAsset ifcAsset;
                if(helper.AssetAsignments.TryGetValue(ifcTypeObject, out ifcAsset))
                    AssetTypeCategory = helper.GetCoBieAttribute<StringValueType>("AssetTypeAccountingCategory", ifcAsset).StringValue;
            }
            AssetTypeDescription = ifcTypeObject.Description;
            AssetTypeModelNumber = helper.GetCoBieProperty("AssetTypeModelNumber", ifcTypeObject);
            AssetTypeReplacementCostValue = helper.GetCoBieAttribute<DecimalValueType>("AssetTypeReplacementCostValue", ifcTypeObject);
            AssetTypeExpectedLifeValue = helper.GetCoBieAttribute<IntegerValueType>("AssetTypeExpectedLifeValue", ifcTypeObject);
            AssetTypeNominalLength = helper.GetCoBieAttribute<DecimalValueType>("AssetTypeNominalLength", ifcTypeObject);
            AssetTypeNominalWidth = helper.GetCoBieAttribute<DecimalValueType>("AssetTypeNominalWidth", ifcTypeObject);
            AssetTypeNominalHeight = helper.GetCoBieAttribute<DecimalValueType>("AssetTypeNominalHeight", ifcTypeObject);
            AssetTypeAccessibilityText = helper.GetCoBieProperty("AssetTypeAccessibilityText", ifcTypeObject);
            AssetTypeColorCode = helper.GetCoBieProperty("AssetTypeColorCode", ifcTypeObject);
            AssetTypeConstituentsDescription = helper.GetCoBieProperty("AssetTypeConstituentsDescription", ifcTypeObject);
            AssetTypeFeaturesDescription = helper.GetCoBieProperty("AssetTypeFeaturesDescription", ifcTypeObject);
            AssetTypeGradeDescription = helper.GetCoBieProperty("AssetTypeGradeDescription", ifcTypeObject);
            AssetTypeMaterialDescription = helper.GetCoBieProperty("AssetTypeMaterialDescription", ifcTypeObject);
            AssetTypeShapeDescription = helper.GetCoBieProperty("AssetTypeShapeDescription", ifcTypeObject);
            AssetTypeSizeDescription = helper.GetCoBieProperty("AssetTypeSizeDescription", ifcTypeObject);
            AssetTypeSustainabilityPerformanceDescription = helper.GetCoBieProperty("AssetTypeSustainabilityPerformanceDescription", ifcTypeObject);

            //The Assets
            List<IfcElement> allAssetsofThisType;
            if (helper.DefiningTypeObjectMap.TryGetValue(ifcTypeObject, out allAssetsofThisType)) //should always work
            {
                Assets = new AssetCollectionType { Asset =  new List<AssetInfoType>(allAssetsofThisType.Count)};
                foreach (IfcElement t in allAssetsofThisType)
                {
                    Assets.Add(new AssetInfoType(t, helper));
                }
            }
            else
            {
                //just in case we have a problem
                CoBieLiteHelper.Logger.ErrorFormat("Asset Type: Failed to locate Asset Type #{0}={1}", ifcTypeObject.EntityLabel,ifcTypeObject.GetType().Name);
            }

            //Attributes
            var ifcAttributes = helper.GetAttributes(ifcTypeObject);
            if (ifcAttributes != null && ifcAttributes.Any())
                AssetTypeAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
           
        }
    }
}
