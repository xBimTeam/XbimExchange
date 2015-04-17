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

            target.Description = proxyIfcTypeObject.Description;
            var ifcTypeObject = proxyIfcTypeObject.IfcTypeObject;
            if (ifcTypeObject != null)
            {
                target.ModelNumber = helper.GetCoBieProperty("AssetTypeModelNumber", ifcTypeObject);
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
                target.Features = helper.GetCoBieProperty("AssetTypeFeaturesDescription", ifcTypeObject);
                target.Grade = helper.GetCoBieProperty("AssetTypeGradeDescription", ifcTypeObject);
                target.Material = helper.GetCoBieProperty("AssetTypeMaterialDescription", ifcTypeObject);
                target.Shape = helper.GetCoBieProperty("AssetTypeShapeDescription", ifcTypeObject);
                target.Size = helper.GetCoBieProperty("AssetTypeSizeDescription", ifcTypeObject);
                target.SustainabilityPerformance =
                    helper.GetCoBieProperty("AssetTypeSustainabilityPerformanceDescription",
                        ifcTypeObject);
                //Attributes
                target.Attributes = helper.GetAttributes(ifcTypeObject);
            }
            //The Assets
            
                var assetMappings = Exchanger.GetOrCreateMappings<MappingIfcElementToAsset>();
                target.Assets = new List<Asset>();
               
                    List<IfcElement> allAssetsofThisType;
                    if (helper.DefiningTypeObjectMap.TryGetValue(proxyIfcTypeObject, out allAssetsofThisType))
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
                            proxyIfcTypeObject.EntityLabel, proxyIfcTypeObject.TypeName);
                    }
                
            
           
            return target;
        }

    }
}
