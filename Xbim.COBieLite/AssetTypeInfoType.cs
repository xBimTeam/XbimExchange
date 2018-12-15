using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xbim.COBieLite.CollectionTypes;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;


namespace Xbim.COBieLite
{
    public partial class AssetTypeInfoType : ICOBieObject
    {

        static readonly ILogger logger = XbimLogging.CreateLogger<AssetTypeInfoType>();

        public AssetTypeInfoType(IIfcTypeObject ifcTypeObject, CoBieLiteHelper helper)
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
                logger.LogWarning("AssetTypeAccountingCategory: An illegal value of [{category}] has been passed for the category of #{entityId}={entityType}. It has been replaced with a value of 'Item'", 
                    accCategoryString, ifcTypeObject.EntityLabel, ifcTypeObject.GetType().Name);
                AssetTypeAccountingCategory = AssetPortabilitySimpleType.Item;
            }
            if (string.IsNullOrWhiteSpace(AssetTypeCategory)) //try the asset assignment
            {
                IIfcAsset ifcAsset;
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
            List<IIfcElement> allAssetsofThisType;
            if (helper.DefiningTypeObjectMap.TryGetValue(ifcTypeObject, out allAssetsofThisType)) //should always work
            {
                Assets = new AssetCollectionType { Asset =  new List<AssetInfoType>(allAssetsofThisType.Count)};
                foreach (IIfcElement t in allAssetsofThisType)
                {
                    Assets.Add(new AssetInfoType(t, helper));
                }
            }
            else
            {
                //just in case we have a problem
                logger.LogWarning("Asset Type: Failed to locate Asset Type #{entityLabel}={entityType}", 
                    ifcTypeObject.EntityLabel,
                    ifcTypeObject.GetType().Name);
            }

            //Attributes
            var ifcAttributes = helper.GetAttributes(ifcTypeObject);
            if (ifcAttributes != null && ifcAttributes.Any())
                AssetTypeAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
           
        }

        [XmlIgnore, JsonIgnore]
        DocumentCollectionType ICOBieObject.Documents
        {
            get { return AssetTypeDocuments; }
            set { AssetTypeDocuments = value; }
        }

        [XmlIgnore, JsonIgnore]
        IssueCollectionType ICOBieObject.Issues
        {
            get { return AssetTypeIssues; }
            set { AssetTypeIssues = value; }
        }

        [XmlIgnore, JsonIgnore]
        AttributeCollectionType ICOBieObject.Attributes
        {
            get { return AssetTypeAttributes; }
            set { AssetTypeAttributes = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Name
        {
            get { return AssetTypeName; }
            set { AssetTypeName = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Description
        {
            get { return AssetTypeDescription; }
            set { AssetTypeDescription = value; }
        }

        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Category
        {
            get { return AssetTypeCategory; }
            set { AssetTypeCategory = value; }
        }


        [XmlIgnore, JsonIgnore]
        string ICOBieObject.Id
        {
            get { return externalID; }
            set { externalID = value; }
        }
    }
}
