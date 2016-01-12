using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    /// <summary>
    /// Maps a list of IfcTypeObject that are all the same
    /// </summary>
    internal class MappingXbimIfcProxyTypeObjectToAssetType :
        XbimMappings<IfcStore, IModel, string, XbimIfcProxyTypeObject, CobieType>
    {
        public bool HasCategory
        {
            get;
            private set;
        }

        protected override CobieType Mapping(XbimIfcProxyTypeObject proxyIfcTypeObject, CobieType target)
        {

            var helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            target.ExternalObject = helper.GetExternalObject(proxyIfcTypeObject.ExternalEntity);
            target.ExternalId = proxyIfcTypeObject.ExternalId;
            target.ExternalSystem = proxyIfcTypeObject.ExternalSystem;
            target.Name = proxyIfcTypeObject.Name;
            target.Categories.AddRange(proxyIfcTypeObject.Categories);
            var cat = target.Categories.FirstOrDefault();
            HasCategory = ((cat != null) && ((cat.Value != "unknown") || target.Categories.Count > 1)); //assume if more than 1 we have a category
            target.AssetType = proxyIfcTypeObject.AccountingCategory;
            target.Created = proxyIfcTypeObject.GetCreatedInfo();
            target.Description = proxyIfcTypeObject.Description;
            var ifcTypeObject = proxyIfcTypeObject.IfcTypeObject;
            List<IIfcElement> allAssetsofThisType;
            helper.DefiningTypeObjectMap.TryGetValue(proxyIfcTypeObject, out allAssetsofThisType);

            target.Warranty = Exchanger.TargetRepository.Instances.New<CobieWarranty>();
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
                target.Manufacturer = helper.GetOrCreateContact(manuf);

                helper.TrySetSimpleValue<double?>("AssetTypeReplacementCostValue", ifcTypeObject, v => target.ReplacementCost = v);
                helper.TrySetSimpleValue<double?>("AssetTypeExpectedLifeValue", ifcTypeObject, v => target.ExpectedLife = v);
                helper.TrySetSimpleValue<double>("AssetTypeNominalLength", ifcTypeObject, v => target.NominalLength = v);
                helper.TrySetSimpleValue<double>("AssetTypeNominalWidth", ifcTypeObject, v => target.NominalWidth = v);
                helper.TrySetSimpleValue<double>("AssetTypeNominalHeight", ifcTypeObject, v => target.NominalHeight = v);
                
                target.ModelNumber = helper.GetCoBieProperty("AssetTypeModelNumber", ifcTypeObject);
                target.ModelReference = helper.GetCoBieProperty("AssetTypeModelReference", ifcTypeObject);
                target.AccessibilityPerformance = helper.GetCoBieProperty("AssetTypeAccessibilityText", ifcTypeObject);
                target.Color = helper.GetCoBieProperty("AssetTypeColorCode", ifcTypeObject);
                target.Constituents = helper.GetCoBieProperty("AssetTypeConstituentsDescription", ifcTypeObject);
                var durationUnit = helper.GetCoBieProperty("AssetTypeDurationUnit", ifcTypeObject);
                if (!string.IsNullOrWhiteSpace(durationUnit)) target.DurationUnit = helper.GetPickValue<CobieDurationUnit>(durationUnit);
                target.Features = helper.GetCoBieProperty("AssetTypeFeaturesDescription", ifcTypeObject);
                target.Grade = helper.GetCoBieProperty("AssetTypeGradeDescription", ifcTypeObject);
                target.Material = helper.GetCoBieProperty("AssetTypeMaterialDescription", ifcTypeObject);
                target.Shape = helper.GetCoBieProperty("AssetTypeShapeDescription", ifcTypeObject);
                target.Size = helper.GetCoBieProperty("AssetTypeSizeDescription", ifcTypeObject);
                target.SustainabilityPerformance = helper.GetCoBieProperty("AssetTypeSustainabilityPerformanceDescription", ifcTypeObject);
                target.CodePerformance = helper.GetCoBieProperty("AssetTypeCodePerformance", ifcTypeObject);
                target.Finish = helper.GetCoBieProperty("AssetTypeFinishDescription", ifcTypeObject);

                target.Warranty.Description = helper.GetCoBieProperty("AssetTypeWarrantyDescription", ifcTypeObject);
                helper.TrySetSimpleValue<double>("AssetTypeWarrantyDurationLabor", ifcTypeObject, v => target.Warranty.DurationLabor = v);
                helper.TrySetSimpleValue<double>("AssetTypeWarrantyDurationParts", ifcTypeObject, v => target.Warranty.DurationParts = v);

                var warrantyDurationUnit = helper.GetCoBieProperty("AssetTypeWarrantyDurationUnit", ifcTypeObject);
                if (!string.IsNullOrWhiteSpace(warrantyDurationUnit)) target.Warranty.DurationUnit = helper.GetPickValue<CobieDurationUnit>(warrantyDurationUnit);
                var laborContact = helper.GetCoBieProperty("AssetTypeWarrantyGuarantorLabor", ifcTypeObject);
                if (!string.IsNullOrWhiteSpace(laborContact))
                    target.Warranty.GuarantorLabor = helper.GetOrCreateContact(laborContact);
                var partsContact = helper.GetCoBieProperty("AssetTypeWarrantyGuarantorParts", ifcTypeObject);
                if (!string.IsNullOrWhiteSpace(partsContact))
                    target.Warranty.GuarantorParts = helper.GetOrCreateContact(partsContact);
                //Attributes
                target.Attributes.AddRange(helper.GetAttributes(ifcTypeObject));

                //Documents
                helper.AddDocuments(target, ifcTypeObject);

                //Spare
                var spareMapping = Exchanger.GetOrCreateMappings<MappingIfcConstructionProductResourceToSpare>();
                spareMapping.ParentObject = ifcTypeObject; //set parent object 
                if (helper.SpareLookup.ContainsKey(ifcTypeObject))
                {
                    foreach (var item in helper.SpareLookup[ifcTypeObject])
                    {
                        CobieSpare spare;
                        if (!spareMapping.GetOrCreateTargetObject(item.EntityLabel, out spare)) continue;
                        spareMapping.AddMapping(item, spare);
                        spare.Type = target;
                    }
                }
            }
            //The Assets

            var assetMappings = Exchanger.GetOrCreateMappings<MappingIfcElementToComponent>();
            if (allAssetsofThisType == null || !allAssetsofThisType.Any()) 
                return target;
            
                foreach (var element in allAssetsofThisType)
                {
                    CobieComponent component;
                    if(assetMappings.GetOrCreateTargetObject(element.EntityLabel, out component))
                        component = assetMappings.AddMapping(element, component);

                    //pass categories over from Asset to AssetType, if none set
                    if (!HasCategory)
                    {
                        var assetcat = component.Categories.FirstOrDefault();
                        if ((assetcat != null) && (assetcat.Value != "unknown"))
                        {
                            target.Categories.Clear();
                            target.Categories.AddRange(component.Categories);
                            HasCategory = true;
                        }
                    }
                    component.Type = target;
                }

            return target;
        }


        public override CobieType CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieType>();
        }
    }
}
