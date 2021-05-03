using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common;
using Xbim.CobieLiteUk;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// Maps a list of IfcTypeObject that are all the same
    /// </summary>
    internal class MappingXbimIfcProxyTypeObjectToAssetType :
        XbimMappings<IModel, List<Facility>, string, XbimIfcProxyTypeObject, AssetType>
    {
        public bool HasCategory
        {
            get;
            private set;
        }

        protected override AssetType Mapping(XbimIfcProxyTypeObject proxyIfcTypeObject, AssetType target)
        {

            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            target.ExternalEntity = proxyIfcTypeObject.ExternalEntity;
            target.ExternalId = proxyIfcTypeObject.ExternalId;
            target.ExternalSystem = proxyIfcTypeObject.ExternalSystemName;
            target.Name = proxyIfcTypeObject.Name;
            target.Categories = proxyIfcTypeObject.Categories;
            var cat = target.Categories.FirstOrDefault();
            HasCategory = ((cat != null) && ((cat.Code != "unknown") || target.Categories.Count > 1)); //assume if more than 1 we have a category
            target.AssetTypeEnum = proxyIfcTypeObject.AccountingCategory;
            target.CreatedBy = proxyIfcTypeObject.GetCreatedBy();
            target.CreatedOn = proxyIfcTypeObject.GetCreatedOn();
            target.Description = proxyIfcTypeObject.Description;
            var ifcTypeObject = proxyIfcTypeObject.IfcTypeObject;
            List<IIfcElement> allAssetsofThisType;
            helper.DefiningTypeObjectMap.TryGetValue(proxyIfcTypeObject, out allAssetsofThisType);

            target.Warranty = new Warranty { GuarantorLabor = new ContactKey { Email = helper.XbimCreatedBy.Email } };
            target.Warranty.GuarantorParts = target.Warranty.GuarantorLabor;
            if (ifcTypeObject != null)
            {
                string manuf = helper.GetCoBieProperty("AssetTypeManufacturer", ifcTypeObject);
                if (string.IsNullOrWhiteSpace(manuf) && allAssetsofThisType != null) //disagrrement between COBie and IFC where this value resides, look in assets
                {
                    foreach (var element in allAssetsofThisType)
                    {
                        var prop = helper.GetCoBieProperty("AssetTypeManufacturer", element);
                        if(!string.IsNullOrWhiteSpace(prop))
                        {
                            manuf = prop;
                            break;
                        }
                    }
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
                if (!string.IsNullOrWhiteSpace(laborContact))
                    target.Warranty.GuarantorLabor = helper.GetOrCreateContactKey(laborContact);
                var partsContact = helper.GetCoBieProperty("AssetTypeWarrantyGuarantorParts", ifcTypeObject);
                if (!string.IsNullOrWhiteSpace(partsContact))
                    target.Warranty.GuarantorParts = helper.GetOrCreateContactKey(partsContact);
                //Attributes
                target.Attributes = helper.GetAttributes(ifcTypeObject);

                //Documents
                var docsMappings = Exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();
                helper.AddDocuments(docsMappings, target, ifcTypeObject);

                //Spare
                var spareMapping = Exchanger.GetOrCreateMappings<MappingIfcConstructionProductResourceToSpare>();
                spareMapping.ParentObject = ifcTypeObject; //set parent object 
                if (helper.SpareLookup.ContainsKey(ifcTypeObject))
                {
                    foreach (var item in helper.SpareLookup[ifcTypeObject])
                    {
                        if (target.Spares == null)
                            target.Spares = new List<Spare>();
                        target.Spares.Add(spareMapping.AddMapping(item, new Spare()));

                    }
                }

                // Assemblies
                var assemblyMapping = Exchanger.GetOrCreateMappings<MappingIfcRelAggregatesToAssembly>();
                assemblyMapping.EntityType = EntityType.AssetType;

                bool hasAttributes = helper.AssemblyLookup.ContainsKey(ifcTypeObject);
                if (hasAttributes)
                {
                    IIfcRelAggregates ifcRelAggregates = helper.AssemblyLookup[ifcTypeObject];
                    target.AssemblyOf = assemblyMapping.AddMapping(ifcRelAggregates, new Assembly());
                }
            }

            //The Assets

            var assetMappings = Exchanger.GetOrCreateMappings<MappingIfcElementToAsset>();
            if (allAssetsofThisType != null && allAssetsofThisType.Any())
            {
                target.Assets = new List<Asset>();

                foreach (IIfcElement element in allAssetsofThisType)
                {
                    var asset = new Asset();
                    asset = assetMappings.AddMapping(element, asset);
                    //pass categories over from Asset to AssetType, if none set
                    if (!HasCategory)
                    {
                        var assetcat = asset.Categories.FirstOrDefault();
                        if ((assetcat != null) && (assetcat.Code != "unknown"))
                        {
                            target.Categories = asset.Categories;
                            HasCategory = true;
                        }
                    }
                    target.Assets.Add(asset);
                }
            }

            return target;
        }


        public override AssetType CreateTargetObject()
        {
            return new AssetType();
        }
    }
}
