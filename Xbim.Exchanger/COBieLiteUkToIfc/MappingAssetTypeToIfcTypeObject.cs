using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.Ifc2x3.SharedBldgServiceElements;
using Xbim.Ifc2x3.SharedComponentElements;
using Xbim.Ifc2x3.StructuralElementsDomain;

namespace XbimExchanger.COBieLiteUkToIfc
{
    class MappingAssetTypeToIfcTypeObject : CoBieLiteUkIfcMappings<string, AssetType, IfcTypeObject>
    {
        protected override IfcTypeObject Mapping(AssetType assetType, IfcTypeObject ifcTypeObject)
        {
            ifcTypeObject.Name = assetType.Name;
            ifcTypeObject.Description = assetType.Description;

            #region Attributes

            if (assetType.Attributes != null)
            {

                foreach (var attribute in assetType.Attributes)
                {
                    Exchanger.ConvertAttributeTypeToIfcObjectProperty(ifcTypeObject, attribute);
                }
            }
            #endregion
            #region Categories
            if (assetType.Categories != null)
                foreach (var category in assetType.Categories)
                {
                    Exchanger.ConvertCategoryToClassification(category, ifcTypeObject);
                }

            #endregion
            #region Spare
            if (assetType.Spares != null)
            {
                foreach (var spare in assetType.Spares)
                {
                    //create the resource
                    var resourceMapping = Exchanger.GetOrCreateMappings<MappingSpareToIfcConstructionProductResource>();
                    IfcConstructionProductResource ifcConstructionProductResource = resourceMapping.AddMapping(spare, resourceMapping.GetOrCreateTargetObject(spare.ExternalId));
                    //connect relationship up to the ifcTypeObject
                    var relResource = Exchanger.TargetRepository.Instances.New<IfcRelAssignsToResource>();
                    relResource.RelatingResource = ifcConstructionProductResource;
                    relResource.RelatedObjects.Add(ifcTypeObject);
                    relResource.Name = assetType.Name;
                    relResource.Description = assetType.Description;
                    relResource.RelatedObjectsType = IfcObjectTypeEnum.PRODUCT;
                }
            }
            #endregion

            if (assetType.Assets != null && assetType.Assets.Any())
            {
                var relDefinesType = Exchanger.TargetRepository.Instances.New<IfcRelDefinesByType>();
                relDefinesType.RelatingType = ifcTypeObject;
                relDefinesType.Name = assetType.Name;
                relDefinesType.Description = assetType.Description;
                var index = 0;
                foreach (var assetInfoType in assetType.Assets)
                {
                    IfcElement ifcElement;
                    if (!string.IsNullOrWhiteSpace(assetInfoType.ExternalEntity))
                    {
                        switch (assetInfoType.ExternalEntity.ToUpper())
                        {
                            case "IFCBEAM":
                                ifcElement = MapAsset<IfcBeam>(assetInfoType);
                                break;
                            case "IFCREINFORCINGBAR":
                                ifcElement = MapAsset<IfcReinforcingBar>(assetInfoType);
                                break;
                            case "IFCREINFORCINGMESH":
                                ifcElement = MapAsset<IfcReinforcingMesh>(assetInfoType);
                                break;
                            case "IFCBUILDINGELEMENTPROXY":
                                ifcElement = MapAsset<IfcBuildingElementProxy>(assetInfoType);
                                break;
                            case "IFCCOLUMN":
                                ifcElement = MapAsset<IfcColumn>(assetInfoType);
                                break;
                            case "IFCCOVERING":
                                ifcElement = MapAsset<IfcCovering>(assetInfoType);
                                break;
                            case "IFCCURTAINWALL":
                                ifcElement = MapAsset<IfcCurtainWall>(assetInfoType);
                                break;
                            case "IFCDOOR":
                                ifcElement = MapAsset<IfcDoor>(assetInfoType);
                                break;
                            case "IFCFOOTING":
                                ifcElement = MapAsset<IfcFooting>(assetInfoType);
                                break;
                            case "IFCMEMBER":
                                ifcElement = MapAsset<IfcMember>(assetInfoType);
                                break;
                            case "IFCPILE":
                                ifcElement = MapAsset<IfcPile>(assetInfoType);
                                break;
                            case "IFCPLATE":
                                ifcElement = MapAsset<IfcPlate>(assetInfoType);
                                break;
                            case "IFCRAILING":
                                ifcElement = MapAsset<IfcRailing>(assetInfoType);
                                break;
                            case "IFCRAMP":
                                ifcElement = MapAsset<IfcRamp>(assetInfoType);
                                break;
                            case "IFCRAMPFLIGHT":
                                ifcElement = MapAsset<IfcRampFlight>(assetInfoType);
                                break;
                            case "IFCROOF":
                                ifcElement = MapAsset<IfcRoof>(assetInfoType);
                                break;
                            case "IFCSLAB":
                                ifcElement = MapAsset<IfcSlab>(assetInfoType);
                                break;
                            case "IFCSTAIR":
                                ifcElement = MapAsset<IfcStair>(assetInfoType);
                                break;
                            case "IFCSTAIRFLIGHT":
                                ifcElement = MapAsset<IfcStairFlight>(assetInfoType);
                                break;
                            case "IFCWALL":
                                ifcElement = MapAsset<IfcWall>(assetInfoType);
                                break;
                            case "IFCWALLSTANDARDCASE":
                                ifcElement = MapAsset<IfcWallStandardCase>(assetInfoType);
                                break;
                            case "IFCWINDOW":
                                ifcElement = MapAsset<IfcWindow>(assetInfoType);
                                break;
                            case "IFCDISTRIBUTIONELEMENT":
                                ifcElement = MapAsset<IfcDistributionElement>(assetInfoType);
                                break;
                            case "IFCDISTRIBUTIONCONTROLELEMENT":
                                ifcElement = MapAsset<IfcDistributionControlElement>(assetInfoType);
                                break;
                            case "IFCDISTRIBUTIONFLOWELEMENT":
                                ifcElement = MapAsset<IfcDistributionFlowElement>(assetInfoType);
                                break;
                            case "IFCDISTRIBUTIONCHAMBERELEMENT":
                                ifcElement = MapAsset<IfcDistributionChamberElement>(assetInfoType);
                                break;
                            case "IFCENERGYCONVERSIONDEVICE":
                                ifcElement = MapAsset<IfcEnergyConversionDevice>(assetInfoType);
                                break;
                            case "IFCFLOWCONTROLLER":
                                ifcElement = MapAsset<IfcFlowController>(assetInfoType);
                                break;
                            case "IFCFLOWFITTING":
                                ifcElement = MapAsset<IfcFlowFitting>(assetInfoType);
                                break;
                            case "IFCFLOWMOVINGDEVICE":
                                ifcElement = MapAsset<IfcFlowMovingDevice>(assetInfoType);
                                break;
                            case "IFCFLOWSEGMENT":
                                ifcElement = MapAsset<IfcFlowSegment>(assetInfoType);
                                break;
                            case "IFCFLOWSTORAGEDEVICE":
                                ifcElement = MapAsset<IfcFlowStorageDevice>(assetInfoType);
                                break;
                            case "IFCFLOWTERMINAL":
                                ifcElement = MapAsset<IfcFlowTerminal>(assetInfoType);
                                break;
                            case "IFCFLOWTREATMENTDEVICE":
                                ifcElement = MapAsset<IfcFlowTreatmentDevice>(assetInfoType);
                                break;
                            case "IFCELEMENTASSEMBLY":
                                ifcElement = MapAsset<IfcElementAssembly>(assetInfoType);
                                break;
                            case "IFCBUILDINGELEMENTPART":
                                ifcElement = MapAsset<IfcBuildingElementPart>(assetInfoType);
                                break;
                            case "IFCDISCRETEACCESSORY":
                                ifcElement = MapAsset<IfcDiscreteAccessory>(assetInfoType);
                                break;
                            case "IFCFASTENER":
                                ifcElement = MapAsset<IfcFastener>(assetInfoType);
                                break;
                            case "IFCMECHANICALFASTENER":
                                ifcElement = MapAsset<IfcMechanicalFastener>(assetInfoType);
                                break;
                            case "IFCTENDON":
                                ifcElement = MapAsset<IfcTendon>(assetInfoType);
                                break;
                            case "IFCTENDONANCHOR":
                                ifcElement = MapAsset<IfcTendonAnchor>(assetInfoType);
                                break;
                            case "IFCPROJECTIONELEMENT":
                                ifcElement = MapAsset<IfcProjectionElement>(assetInfoType);
                                break;
                            case "IFCOPENINGELEMENT":
                                ifcElement = MapAsset<IfcOpeningElement>(assetInfoType);
                                break;
                            case "IFCFURNISHINGELEMENT":
                                ifcElement = MapAsset<IfcFurnishingElement>(assetInfoType);
                                break;
                            case "IFCTRANSPORTELEMENT":
                                ifcElement = MapAsset<IfcTransportElement>(assetInfoType);
                                break;
                            case "IFCVIRTUALELEMENT":
                                ifcElement = MapAsset<IfcVirtualElement>(assetInfoType);
                                break;
                            // The following are not inherited from IFCElement
                            //case "IFCDISTRIBUTIONPORT":
                            //ifcElement = MapAsset<IfcDistributionPort>(assetInfoType);
                            //break;  
                            //
                            //      case "IFCSTRUCTURALACTIVITY":
                            //ifcElement = MapAsset<IfcStructuralActivity>(assetInfoType);
                            //break;
                            //        case "IFCSTRUCTURALACTION":
                            //ifcElement = MapAsset<IfcStructuralAction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALCURVEACTION":
                            //ifcElement = MapAsset<IfcStructuralCurveAction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALLINEARACTION":
                            //ifcElement = MapAsset<IfcStructuralLinearAction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALPOINTACTION":
                            //ifcElement = MapAsset<IfcStructuralPointAction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALSURFACEACTION":
                            //ifcElement = MapAsset<IfcStructuralSurfaceAction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALPLANARACTION":
                            //ifcElement = MapAsset<IfcStructuralPlanarAction>(assetInfoType);
                            //break;
                            //        case "IFCSTRUCTURALREACTION":
                            //ifcElement = MapAsset<IfcStructuralReaction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALCURVEREACTION":
                            //ifcElement = MapAsset<IfcStructuralCurveReaction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALPOINTREACTION":
                            //ifcElement = MapAsset<IfcStructuralPointReaction>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALSURFACEREACTION":
                            //ifcElement = MapAsset<IfcStructuralSurfaceReaction>(assetInfoType);
                            //break;
                            //      case "IFCSTRUCTURALITEM":
                            //ifcElement = MapAsset<IfcStructuralItem>(assetInfoType);
                            //break;
                            //        case "IFCSTRUCTURALCONNECTION":
                            //ifcElement = MapAsset<IfcStructuralConnection>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALCURVECONNECTION":
                            //ifcElement = MapAsset<IfcStructuralCurveConnection>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALPOINTCONNECTION":
                            //ifcElement = MapAsset<IfcStructuralPointConnection>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALSURFACECONNECTION":
                            //ifcElement = MapAsset<IfcStructuralSurfaceConnection>(assetInfoType);
                            //break;
                            //        case "IFCSTRUCTURALMEMBER":
                            //ifcElement = MapAsset<IfcStructuralMember>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALCURVEMEMBER":
                            //ifcElement = MapAsset<IfcStructuralCurveMember>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALCURVEMEMBERVARYING":
                            //ifcElement = MapAsset<IfcStructuralCurveMemberVarying>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALSURFACEMEMBER":
                            //ifcElement = MapAsset<IfcStructuralSurfaceMember>(assetInfoType);
                            //break;
                            //          case "IFCSTRUCTURALSURFACEMEMBERVARYING":
                            //ifcElement = MapAsset<IfcStructuralSurfaceMemberVarying>(assetInfoType);
                            //break;

                            #region IFC 4 support


                            //                                          case "IFCCHIMNEY":
                            //ifcElement = MapAsset<IfcChimney>(assetInfoType);
                            //break;
                            //                                     case "IFCDOORSTANDARDCASE":
                            //ifcElement = MapAsset<IfcDoorStandardCase>(assetInfoType);
                            //break;        
                            //                                case "IFCMEMBERSTANDARDCASE":
                            //ifcElement = MapAsset<IfcMemberStandardCase>(assetInfoType);
                            //break;                       
                            //case "IFCPLATESTANDARDCASE":
                            //ifcElement = MapAsset<IfcPlateStandardCase>(assetInfoType);
                            //break;  
                            //                                 case "IFCSHADINGDEVICE":
                            //ifcElement = MapAsset<IfcShadingDevice>(assetInfoType);
                            //break;     
                            //                                          case "IFCSLABELEMENTEDCASE":
                            //ifcElement = MapAsset<IfcSlabElementedCase>(assetInfoType);
                            //break; 
                            //          case "IFCSLABSTANDARDCASE":
                            //ifcElement = MapAsset<IfcSlabStandardCase>(assetInfoType);
                            //break; 
                            //                                          case "IFCWALLELEMENTEDCASE":
                            //ifcElement = MapAsset<IfcWallElementedCase>(assetInfoType);
                            //break;
                            //                                          case "IFCWINDOWSTANDARDCASE":
                            //ifcElement = MapAsset<IfcWindowStandardCase>(assetInfoType);
                            //break;
                            //                                          case "IFCACTUATOR":
                            //ifcElement = MapAsset<IfcActuator>(assetInfoType);
                            //break;
                            //                                          case "IFCALARM":
                            //ifcElement = MapAsset<IfcAlarm>(assetInfoType);
                            //break;
                            //                                          case "IFCCONTROLLER":
                            //ifcElement = MapAsset<IfcController>(assetInfoType);
                            //break;
                            //                                          case "IFCFLOWINSTRUMENT":
                            //ifcElement = MapAsset<IfcFlowInstrument>(assetInfoType);
                            //break;
                            //                                          case "IFCPROTECTIVEDEVICETRIPPINGUNIT":
                            //ifcElement = MapAsset<IfcProtectiveDeviceTrippingUnit>(assetInfoType);
                            //break;
                            //                                          case "IFCSENSOR":
                            //ifcElement = MapAsset<IfcSensor>(assetInfoType);
                            //break;
                            //                                          case "IFCUNITARYCONTROLELEMENT":
                            //ifcElement = MapAsset<IfcUnitaryControlElement>(assetInfoType);
                            //break;
                            //                                          case "IFCAIRTOAIRHEATRECOVERY":
                            //ifcElement = MapAsset<IfcAirToAirHeatRecovery>(assetInfoType);
                            //break;
                            //                                          case "IFCBOILER":
                            //ifcElement = MapAsset<IfcBoiler>(assetInfoType);
                            //break;
                            //                                          case "IFCBURNER":
                            //ifcElement = MapAsset<IfcBurner>(assetInfoType);
                            //break;
                            //                                          case "IFCCHILLER":
                            //ifcElement = MapAsset<IfcChiller>(assetInfoType);
                            //break;
                            //                                          case "IFCCOIL":
                            //ifcElement = MapAsset<IfcCoil>(assetInfoType);
                            //break;
                            //                                          case "IFCCONDENSER":
                            //ifcElement = MapAsset<IfcCondenser>(assetInfoType);
                            //break;
                            //                                          case "IFCCOOLEDBEAM":
                            //ifcElement = MapAsset<IfcCooledBeam>(assetInfoType);
                            //break;
                            //                                          case "IFCCOOLINGTOWER":
                            //ifcElement = MapAsset<IfcCoolingTower>(assetInfoType);
                            //break;
                            //                                          case "IFCELECTRICGENERATOR":
                            //ifcElement = MapAsset<IfcElectricGenerator>(assetInfoType);
                            //break;
                            //                                          case "IFCELECTRICMOTOR":
                            //ifcElement = MapAsset<IfcElectricMotor>(assetInfoType);
                            //break;
                            //                                          case "IFCENGINE":
                            //ifcElement = MapAsset<IfcEngine>(assetInfoType);
                            //break;
                            //                                          case "IFCEVAPORATIVECOOLER":
                            //ifcElement = MapAsset<IfcEvaporativeCooler>(assetInfoType);
                            //break;
                            //                                          case "IFCEVAPORATOR":
                            //ifcElement = MapAsset<IfcEvaporator>(assetInfoType);
                            //break;
                            //                                          case "IFCHEATEXCHANGER":
                            //ifcElement = MapAsset<IfcHeatExchanger>(assetInfoType);
                            //break;
                            //                                          case "IFCHUMIDIFIER":
                            //ifcElement = MapAsset<IfcHumidifier>(assetInfoType);
                            //break;
                            //                                          case "IFCMOTORCONNECTION":
                            //ifcElement = MapAsset<IfcMotorConnection>(assetInfoType);
                            //break;
                            //                                          case "IFCSOLARDEVICE":
                            //ifcElement = MapAsset<IfcSolarDevice>(assetInfoType);
                            //break;
                            //                                          case "IFCTRANSFORMER":
                            //ifcElement = MapAsset<IfcTransformer>(assetInfoType);
                            //break;
                            //                                          case "IFCTUBEBUNDLE":
                            //ifcElement = MapAsset<IfcTubeBundle>(assetInfoType);
                            //break;
                            //                                          case "IFCUNITARYEQUIPMENT":
                            //ifcElement = MapAsset<IfcUnitaryEquipment>(assetInfoType);
                            //break;
                            //                                          case "IFCAIRTERMINALBOX":
                            //ifcElement = MapAsset<IfcAirTerminalBox>(assetInfoType);
                            //break;
                            //                                          case "IFCDAMPER":
                            //ifcElement = MapAsset<IfcDamper>(assetInfoType);
                            //break;
                            //                                          case "IFCELECTRICDISTRIBUTIONBOARD":
                            //ifcElement = MapAsset<IfcElectricDistributionBoard>(assetInfoType);
                            //break;
                            //                                          case "IFCELECTRICTIMECONTROL":
                            //ifcElement = MapAsset<IfcElectricTimeControl>(assetInfoType);
                            //break;
                            //                                          case "IFCFLOWMETER":
                            //ifcElement = MapAsset<IfcFlowMeter>(assetInfoType);
                            //break;
                            //                                          case "IFCPROTECTIVEDEVICE":
                            //ifcElement = MapAsset<IfcProtectiveDevice>(assetInfoType);
                            //break;
                            //                                          case "IFCSWITCHINGDEVICE":
                            //ifcElement = MapAsset<IfcSwitchingDevice>(assetInfoType);
                            //break;
                            //                                          case "IFCVALVE":
                            //ifcElement = MapAsset<IfcValve>(assetInfoType);
                            //break;
                            //                                          case "IFCCABLECARRIERFITTING":
                            //ifcElement = MapAsset<IfcCableCarrierFitting>(assetInfoType);
                            //break;
                            //                                          case "IFCCABLEFITTING":
                            //ifcElement = MapAsset<IfcCableFitting>(assetInfoType);
                            //break;
                            //                                          case "IFCDUCTFITTING":
                            //ifcElement = MapAsset<IfcDuctFitting>(assetInfoType);
                            //break;
                            //                                          case "IFCJUNCTIONBOX":
                            //ifcElement = MapAsset<IfcJunctionBox>(assetInfoType);
                            //break;
                            //                                          case "IFCPIPEFITTING":
                            //ifcElement = MapAsset<IfcPipeFitting>(assetInfoType);
                            //break;
                            //                                          case "IFCCOMPRESSOR":
                            //ifcElement = MapAsset<IfcCompressor>(assetInfoType);
                            //break;
                            //                                          case "IFCFAN":
                            //ifcElement = MapAsset<IfcFan>(assetInfoType);
                            //break;
                            //                                          case "IFCPUMP":
                            //ifcElement = MapAsset<IfcPump>(assetInfoType);
                            //break;
                            //                                          case "IFCCABLECARRIERSEGMENT":
                            //ifcElement = MapAsset<IfcCableCarrierSegment>(assetInfoType);
                            //break;
                            //                                          case "IFCCABLESEGMENT":
                            //ifcElement = MapAsset<IfcCableSegment>(assetInfoType);
                            //break;
                            //                                          case "IFCDUCTSEGMENT":
                            //ifcElement = MapAsset<IfcDuctSegment>(assetInfoType);
                            //break;
                            //                                          case "IFCPIPESEGMENT":
                            //ifcElement = MapAsset<IfcPipeSegment>(assetInfoType);
                            //break;
                            //                                          case "IFCELECTRICFLOWSTORAGEDEVICE":
                            //ifcElement = MapAsset<IfcElectricFlowStorageDevice>(assetInfoType);
                            //break;
                            //                                          case "IFCTANK":
                            //ifcElement = MapAsset<IfcTank>(assetInfoType);
                            //break;
                            //                                          case "IFCVIBRATIONISOLATOR":
                            //ifcElement = MapAsset<IfcVibrationIsolator>(assetInfoType);
                            //break;
                            //                                          case "IFCAIRTERMINAL":
                            //ifcElement = MapAsset<IfcAirTerminal>(assetInfoType);
                            //break;
                            //                                          case "IFCAUDIOVISUALAPPLIANCE":
                            //ifcElement = MapAsset<IfcAudioVisualAppliance>(assetInfoType);
                            //break;
                            //                                          case "IFCCOMMUNICATIONSAPPLIANCE":
                            //ifcElement = MapAsset<IfcCommunicationsAppliance>(assetInfoType);
                            //break;
                            //                                          case "IFCELECTRICAPPLIANCE":
                            //ifcElement = MapAsset<IfcElectricAppliance>(assetInfoType);
                            //break;
                            //                                          case "IFCFIRESUPPRESSIONTERMINAL":
                            //ifcElement = MapAsset<IfcFireSuppressionTerminal>(assetInfoType);
                            //break;
                            //                                          case "IFCLAMP":
                            //ifcElement = MapAsset<IfcLamp>(assetInfoType);
                            //break;
                            //                                          case "IFCLIGHTFIXTURE":
                            //ifcElement = MapAsset<IfcLightFixture>(assetInfoType);
                            //break;
                            //                                          case "IFCMEDICALDEVICE":
                            //ifcElement = MapAsset<IfcMedicalDevice>(assetInfoType);
                            //break;
                            //                                          case "IFCOUTLET":
                            //ifcElement = MapAsset<IfcOutlet>(assetInfoType);
                            //break;
                            //                                          case "IFCSANITARYTERMINAL":
                            //ifcElement = MapAsset<IfcSanitaryTerminal>(assetInfoType);
                            //break;
                            //                                          case "IFCSPACEHEATER":
                            //ifcElement = MapAsset<IfcSpaceHeater>(assetInfoType);
                            //break;
                            //                                          case "IFCSTACKTERMINAL":
                            //ifcElement = MapAsset<IfcStackTerminal>(assetInfoType);
                            //break;
                            //                                          case "IFCWASTETERMINAL":
                            //ifcElement = MapAsset<IfcWasteTerminal>(assetInfoType);
                            //break;
                            //                                          case "IFCDUCTSILENCER":
                            //ifcElement = MapAsset<IfcDuctSilencer>(assetInfoType);
                            //break;
                            //                                          case "IFCFILTER":
                            //ifcElement = MapAsset<IfcFilter>(assetInfoType);
                            //break;
                            //                                          case "IFCINTERCEPTOR":
                            //ifcElement = MapAsset<IfcInterceptor>(assetInfoType);
                            //break;
                            //                                          case "IFCOPENINGSTANDARDCASE":
                            //ifcElement = MapAsset<IfcOpeningStandardCase>(assetInfoType);
                            //break;
                            //                                          case "IFCVOIDINGFEATURE":
                            //ifcElement = MapAsset<IfcVoidingFeature>(assetInfoType);
                            //break;
                            //                                          case "IFCSURFACEFEATURE":
                            //ifcElement = MapAsset<IfcSurfaceFeature>(assetInfoType);
                            //break;
                            //                                          case "IFCSPATIALELEMENT":
                            //ifcElement = MapAsset<IfcSpatialElement>(assetInfoType);
                            //break;
                            //                                          case "IFCEXTERNALSPATIALSTRUCTUREELEMENT":
                            //ifcElement = MapAsset<IfcExternalSpatialStructureElement>(assetInfoType);
                            //break;
                            //                                          case "IFCEXTERNALSPATIALELEMENT":
                            //ifcElement = MapAsset<IfcExternalSpatialElement>(assetInfoType);
                            //break;
                            //                                          case "IFCFURNITURE":
                            //ifcElement = MapAsset<IfcFurniture>(assetInfoType);
                            //break;
                            //                                          case "IFCSYSTEMFURNITUREELEMENT":
                            //ifcElement = MapAsset<IfcSystemFurnitureElement>(assetInfoType);
                            //break;
                            //                                          case "IFCGEOGRAPHICELEMENT":
                            //ifcElement = MapAsset<IfcGeographicElement>(assetInfoType);
                            //break;
                            //                                          case "IFCSPATIALZONE":
                            //ifcElement = MapAsset<IfcSpatialZone>(assetInfoType);
                            //break;
                            #endregion

                            default:
#if DEBUG
                                System.Console.WriteLine(assetInfoType.ExternalEntity + " has been made IfcBuildingElementProxy");
#endif
                                ifcElement = MapAsset<IfcBuildingElementProxy>(assetInfoType);
                                break;
                        }
                    }
                    else
                    {
                        var assetInfoTypeMapping = Exchanger.GetOrCreateMappings<MappingAssetToIfcElement<IfcBuildingElementProxy>>();
                        ifcElement = assetInfoTypeMapping.AddMapping(assetInfoType,
                            assetInfoTypeMapping.GetOrCreateTargetObject(assetInfoType.ExternalId));
                    }
                    //add the relationship                 
                    relDefinesType.RelatedObjects.Add(ifcElement);
                    //create symbolic geometry
                    Exchanger.CreateObjectGeometry(ifcElement, index++);
                }
            }

            #region Documents
            if (assetType.Documents != null && assetType.Documents.Any())
            {
                Exchanger.ConvertDocumentsToDocumentSelect(ifcTypeObject, assetType.Documents);
            }
            #endregion

            return ifcTypeObject;
        }

        TAsset MapAsset<TAsset>(Asset assetInfoType) where TAsset : IfcElement
        {
            var assetInfoTypeMapping = Exchanger.GetOrCreateMappings<MappingAssetToIfcElement<TAsset>>();
            return assetInfoTypeMapping.AddMapping(assetInfoType, assetInfoTypeMapping.GetOrCreateTargetObject(assetInfoType.ExternalId));
        }
    }
}
