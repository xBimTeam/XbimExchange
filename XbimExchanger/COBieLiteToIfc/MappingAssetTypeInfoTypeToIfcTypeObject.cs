using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.Ifc2x3.SharedBldgServiceElements;
using Xbim.IO;
using XbimExchanger.COBieLiteHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    class MappingAssetTypeInfoTypeToIfcTypeObject : CoBieLiteIfcMappings<string, AssetTypeInfoType, IfcTypeObject>
    {
        protected override IfcTypeObject Mapping(AssetTypeInfoType assetTypeInfoType, IfcTypeObject ifcTypeObject)
        {
            ifcTypeObject.Name = assetTypeInfoType.AssetTypeName;
            ifcTypeObject.Description = assetTypeInfoType.AssetTypeDescription;

            #region Attributes

            if (assetTypeInfoType.AssetTypeAttributes != null)
            {

                foreach (var attribute in assetTypeInfoType.AssetTypeAttributes)
                {
                    Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcTypeObject, attribute);
                }
            }
            #endregion
            
            
            if (assetTypeInfoType.Assets != null && assetTypeInfoType.Assets.Any())
            {
                var relDefinesType = Exchanger.TargetRepository.Instances.New<IfcRelDefinesByType>();
                relDefinesType.RelatingType = ifcTypeObject;
                relDefinesType.Name = assetTypeInfoType.AssetTypeName;
                relDefinesType.Description = assetTypeInfoType.AssetTypeDescription;
                var index = 0;
                foreach (var assetInfoType in assetTypeInfoType.Assets)
                {
                    IfcElement ifcElement;
                    if (assetInfoType.externalEntityName != null)     
                    {
                        switch (assetInfoType.externalEntityName.ToUpper())
                        {
                            case "IFCDOOR":
                                ifcElement = MapAsset<IfcDoor>(assetInfoType);
                                break; 
                            case "IFCENERGYCONVERSIONDEVICE":
                                ifcElement = MapAsset<IfcEnergyConversionDevice>(assetInfoType);
                                break; 
                            case "IFCDISTRIBUTIONCONTROLELEMENT":
                                ifcElement = MapAsset<IfcDistributionControlElement>(assetInfoType);
                                break; 
                            case "IFCFLOWCONTROLLER":
                                ifcElement = MapAsset<IfcFlowController>(assetInfoType);
                                break;
                            case "IFCFLOWMOVINGDEVICE":
                                ifcElement = MapAsset<IfcFlowMovingDevice>(assetInfoType);
                                break;
                            case "IFCFLOWTERMINAL":
                                ifcElement = MapAsset<IfcFlowTerminal>(assetInfoType);
                                break;
                            case "IFCFURNISHINGELEMENT":
                                ifcElement = MapAsset<IfcFurnishingElement>(assetInfoType);
                                break;
                            case "IFCWINDOW":
                                ifcElement = MapAsset<IfcWindow>(assetInfoType);
                                break;
                            default:
                                Console.WriteLine(assetInfoType.externalEntityName);
                                ifcElement = MapAsset<IfcBuildingElementProxy>(assetInfoType);
                                break;
                        }
                       
                        
                    }
                    else
                    {
                        var assetInfoTypeMapping = Exchanger.GetOrCreateMappings<MappingAssetInfoTypeToIfcElement<IfcBuildingElementProxy>>();
                        ifcElement = assetInfoTypeMapping.AddMapping(assetInfoType, 
                            assetInfoTypeMapping.GetOrCreateTargetObject(assetInfoType.externalID));
                    }
                    //add the relationship                 
                    relDefinesType.RelatedObjects.Add(ifcElement);
                    //create symbolic geometry
                    Exchanger.CreateObjectGeometry(ifcElement, index++);
                }
            }

            return ifcTypeObject;
        }

        TAsset MapAsset<TAsset>(AssetInfoType assetInfoType) where TAsset : IfcElement, new()
        {
            var assetInfoTypeMapping = Exchanger.GetOrCreateMappings<MappingAssetInfoTypeToIfcElement<TAsset>>();
            return assetInfoTypeMapping.AddMapping(assetInfoType, assetInfoTypeMapping.GetOrCreateTargetObject(assetInfoType.externalID));
        }
    }
}
