using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace Xbim.COBie
{
    /// <summary>
    /// Class to work out the Precedence Rules on merging models
    /// </summary>
    public class FilterValuesOnMerge
    {
        /// <summary>
        /// List of Ifc objects we need to test for Merge Precedence Rules
        /// </summary>
        private List<Type> FilterTypes { get; set; }
        private const bool _mergeDefault = true; //if type not tested then assume merge

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterValuesOnMerge()
        {
            //set up the ifcTypes to filter on for merge
            FilterTypes = new List<Type>(){ typeof(IIfcBuildingElementPart),
                                            typeof(IIfcCovering),
                                            typeof(IIfcDistributionControlElement),
                                            typeof(IIfcDistributionFlowElement),
                                            typeof(IIfcDoor),
                                            typeof(IIfcEnergyConversionDevice),
                                            typeof(IIfcFlowController),
                                            //SRL DELTED IN IFC4
                                            //typeof(IIfcElectricDistributionPoint), //super type of IIfcFlowController
                                            typeof(IIfcFlowFitting),
                                            typeof(IIfcFlowMovingDevice),
                                            typeof(IIfcFlowStorageDevice),
                                            typeof(IIfcFlowTerminal),
                                            typeof(IIfcFlowTreatmentDevice),
                                            typeof(IIfcFurnishingElement),
                                            typeof(IIfcRoof),
                                            typeof(IIfcWindow),
                                            typeof(IIfcTransportElement),
                                        };
        }

        /// <summary>
        /// Test on workbook field holding type name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns>bool </returns>
        public bool Merge(string typeName)
        {
            typeName = typeName.ToUpper();
            switch (typeName)
            {
                case "IFCBUILDINGELEMENTPART":
                case "IFCCOVERING":
                case "IFCDISTRIBUTIONCONTROLELEMENT":
                case "IFCDISTRIBUTIONFLOWELEMENT":
                case "IFCDOOR":
                case "IFCENERGYCONVERSIONDEVICE":
                case "IFCFLOWCONTROLLER":
                case "IFCELECTRICDISTRIBUTIONPOINT": //specific flow controller;
                case "IFCFLOWFITTING":
                case "IFCFLOWMOVINGDEVICE":
                case "IFCFLOWSTORAGEDEVICE":
                case "IFCFLOWTERMINAL":
                case "IFCFLOWTREATMENTDEVICE":
                case "IFCFURNISHINGELEMENT":
                case "IFCROOF":
                case "IFCWINDOW":
                case "IFCTRANSPORTELEMENT":
                    return true;
                default:
                    return false;
            }
        }

        public bool Merge(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            
            Type ifcType = ifcElement.GetType();
            if (FilterTypes.Contains(ifcType)) //ok we have possible merge
            {
                switch (ifcType.Name)
                {
                    case "IfcBuildingElementPart":
                        return BuildingElementPart(ifcElement, fileRoles);
                    case "IfcCovering":
                        return Covering(ifcElement, fileRoles);
                    case "IfcDistributionControlElement":
                        return DistributionControlElement(ifcElement, fileRoles);
                    case "IfcDistributionFlowElement":
                        return DistributionFlowElement(ifcElement, fileRoles);
                    case "IfcDoor":
                        return Door(ifcElement, fileRoles);
                    case "IfcEnergyConversionDevice":
                        return EnergyConversionDevice(ifcElement, fileRoles);
                    case "IfcFlowController":
                    case "IfcElectricDistributionPoint": //specific flow controller;
                        return FlowController(ifcElement, fileRoles);
                    case "IfcFlowFitting":
                        return FlowFitting(ifcElement, fileRoles);
                    case "IfcFlowMovingDevice":
                        return FlowMovingDevice(ifcElement, fileRoles);
                    case "IfcFlowStorageDevice":
                        return FlowStorageDevice(ifcElement, fileRoles);
                    case "IfcFlowTerminal":
                        return FlowTerminal(ifcElement, fileRoles);
                    case "IfcFlowTreatmentDevice":
                        return FlowTreatmentDevice(ifcElement, fileRoles);
                    case "IfcFurnishingElement":
                        return FurnishingElement(ifcElement, fileRoles);
                    case "IfcRoof":
                        return Roof(ifcElement, fileRoles);
                    case "IfcWindow":
                        return Window(ifcElement, fileRoles);
                    case "IfcTransportElement":
                        return TransportElement(ifcElement, fileRoles);
                    default:
                        return false;
                        
                }
                
            }
            return true; //not a filter type so ok to add
        }

        private bool TransportElement(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcTransportElement ifcTransportElement = ifcElement as IIfcTransportElement;
            if (ifcTransportElement != null)
            {
                var ifcTransportElementType = ifcTransportElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcTransportElementType>();  //Excel cell ref A1280
                //Excel A716 to A722 id full range of the IfcTransportElementEnum  which is a property of IfcTransportElement, so if we have ifcTransportElement.Any() then we have covered this requirement
                if (ifcTransportElementType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
            }
            return _mergeDefault; 
        }

        

        private bool Window(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx

            //IfcWindow is IIfc 2x4 with IfcWindowTypeEnum property .PredefinedType  , not supported at this time. A823
            IIfcWindow ifcWindow = ifcElement as IIfcWindow;
            if (ifcWindow != null)//A1389
            {
                //assume any window will want to be in Architectural
                return MatchRoles(fileRoles, COBieMergeRoles.Architectural);
            }
            return _mergeDefault;

        }

        private bool Roof(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcRoof ifcRoof = ifcElement as IIfcRoof;
            if (ifcRoof != null)//A1045
            {
                //no type Ifc entity, here the IfcRoofTypeEnum is the ShapeType property of the ifcRoof
                //Excel A1030 to A1044 id full range of the IfcRoofTypeEnum then we have covered this requirement
                return MatchRoles(fileRoles, COBieMergeRoles.Architectural);
            }
            return _mergeDefault;
        }

        private bool FurnishingElement(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {

            IIfcFurnishingElement ifcFurnishingElement = ifcElement as IIfcFurnishingElement;
            if (ifcFurnishingElement != null)
            {
                //IfcFurniture is IIfc 2x4, not supported at this time. A823
                //as stored in Ifc 2x3
                var ifcFurnitureType = ifcFurnishingElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcFurnitureType>();  //Excel cell ref A723
                if (ifcFurnitureType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Architectural);

                //IfcSystemFurnitureElement is IIfc 2x4, not supported at this time. A828
                //as stored in Ifc 2x3
                var ifcSystemFurnitureElementType = ifcFurnishingElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcSystemFurnitureElementType>();  //Excel cell ref A723
                if (ifcSystemFurnitureElementType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Architectural);
            }
            return _mergeDefault;
        }

        private bool FlowTreatmentDevice(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            IIfcFlowTreatmentDevice ifcFlowTreatmentDevice = ifcElement as IIfcFlowTreatmentDevice;
            if (ifcFlowTreatmentDevice != null)
            {
                //IfcFilter is IIfc 2x4, not supported at this time. A723
                //as stored in Ifc 2x3
                var ifcFilterType = ifcFlowTreatmentDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcFilterType>();  //Excel cell ref A723
                //Excel A716 to A722 id full range of the IfcFilterTypeEnum  which is a property of IfcFilterType, so if we have ifcFilterType.Any() then we have covered this requirement
                if (ifcFilterType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                //IfcFireSuppressionTerminal is IIfc 2x4, not supported at this time. A733
                //Try search on type
                var ifcFireSuppressionTerminalType = ifcFlowTreatmentDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcFireSuppressionTerminalType>();  //Excel cell ref A733
                //Excel A716 to A722 id full range of the IfcFireSuppressionTerminalTypeEnum  which is a property of IfcFireSuppressionTerminalType, so if we have ifcFireSuppressionTerminalType.Any() then we have covered this requirement
                if (ifcFireSuppressionTerminalType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.FireProtection);
                
                //IfcHumidifier is IIfc 2x4, not supported at this time. A850
                //Try search on type
                var ifcHumidifierType = ifcFlowTreatmentDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcHumidifierType>();  //Excel cell ref A850
                //Excel A935 to A849 id full range of the IffcHumidifierTypeEnum  which is a property of IfcHumidifierType, so if we have ifcFireSuppressionTerminalType.Any() then we have covered this requirement
                if (ifcHumidifierType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
            }
            return _mergeDefault; 
        }

        private bool FlowTerminal(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcFlowTerminal ifcFlowTerminal = ifcElement as IIfcFlowTerminal;
            if (ifcFlowTerminal != null)
            {
                var ifcAirTerminalType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcAirTerminalType>();  //Excel cell ref E26
                //Excel E21 to E25 part range of the IfcAirTerminalTypeEnum  which is a property of IfcAirTerminalType, so if we have ifcAirTerminalType.Any() then we need to test the requirement
                //Note: IfcAirTerminalTypeEnum.LOUVRE is IIfc 2x4, not supported
                if (ifcAirTerminalType.Any())
                {
                    //if (ifcAirTerminalType.Where(att => att.PredefinedType == IfcAirTerminalTypeEnum.DIFFUSER).Any())
                    //    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                    //if (ifcAirTerminalType.Where(att => att.PredefinedType == IfcAirTerminalTypeEnum.GRILLE).Any())
                    //    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                    //if (ifcAirTerminalType.Where(att => att.PredefinedType == IfcAirTerminalTypeEnum.REGISTER).Any())
                    //    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                    //if (ifcAirTerminalType.Where(att => att.PredefinedType == IfcAirTerminalTypeEnum.NOTDEFINED).Any())
                    //    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical); //as IIfc 2x4 covers all enumeration, assume Ifc 2x3 covers all enumeration
                }
                
                var ifcElectricApplianceType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcElectricApplianceType>();  //Excel cell ref E618
                //Excel E594 to E617 part range of the ifcElectricApplianceTypeEnum  which is a property of ifcElectricApplianceType, so if we have ifcElectricApplianceType.Any() then we need to test the requirement
                if (ifcElectricApplianceType.Any()) //although not all are in Excel sheet, I will assume that all should be based on types!!
                {
                    //assume any user defined is lightly to be in the same role as generic type will be electrical 
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical); //as IIfc 2x4 covers all enumeration, assume IIfc 2x3 covers all enumeration
                }

                //IfcAirTerminalBox is IIfc 2x4, not supported at this time. A33
                //Try search on type
                var ifcAirTerminalBoxType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcAirTerminalBoxType>();  //Excel cell ref A33
                //Excel A23 to A32 part range of the IfcAirTerminalBoxTypeEnum  which is a property of IfcAirTerminalBoxType, so if we have ifcAirTerminalBoxType.Any() then we have covered this requirement
                if (ifcAirTerminalBoxType.Any()) 
                {
                    //assume any user defined is lightly to be in the same role as generic type will be.
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical); 
                }

                //IfcAudioVisualAppliance and IfcAudioVisualApplianceType is IIfc 2x4, not supported at this time. A80
                //IfcCommunicationsAppliance and IfcCommunicationsApplianceType is IIfc 2x4, not supported at this time. A183
                //IfcMedicalDevice is IIfc 2x4, not supported at this time. A303

                //IfcLamp is IIfc 2x4, not supported at this time. A876
                //Try search on type
                var ifcLampType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcLampType>();  //Excel cell ref A876
                //Excel A866 to A875 part range of the IfcLampTypeEnum  which is a property of IfcLampType, so if we have ifcLampType.Any() then we have covered this requirement
                if (ifcLampType.Any())
                {
                    //assume any user defined is lightly to be in the same role as generic type will be.
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                }

                //IfcLightFixture is IIfc 2x4, not supported at this time. A883
                //Try search on type
                var ifcLightFixtureType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcLightFixtureType>();  //Excel cell ref A876
                //Excel A866 to A875 part range of the IfcLightFixtureTypeEnum  which is a property of IfcLightFixtureType, so if we have ifcLightFixtureType.Any() then we have covered this requirement
                if (ifcLightFixtureType.Any())
                {
                    //assume any user defined is lightly to be in the same role as generic type will be.
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                }
                
                //IfcOutlet is IIfc 2x4, not supported at this time. A941
                //Try search on type
                var ifcOutletType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcOutletType>();  //Excel cell ref A941
                //Excel A935 to A940 part range of the IfcOutletTypeEnum  which is a property of IfcOutletType, so if we have ifcOutletType.Any() then we have covered this requirement
                if (ifcOutletType.Any())
                {
                    //assume any user defined is lightly to be in the same role as generic type will be.
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                }

                //IfcSanitaryTerminal is IIfc 2x4, not supported at this time. A1058
                //Try search on type
                var ifcSanitaryTerminalType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcSanitaryTerminalType>();  //Excel cell ref A1058
                //Excel A1047 to A1057 part range of the IfcSanitaryTerminalTypeEnum  which is a property of IfcSanitaryTerminalType, so if we have ifcSanitaryTerminalType.Any() then we have covered this requirement
                if (ifcSanitaryTerminalType.Any())
                {
                    //assume any user defined is lightly to be in the same role as generic type will be.
                    return MatchRoles(fileRoles, COBieMergeRoles.Plumbing);
                }

                //IfcStackTerminal is IIfc 2x4, not supported at this time. A1119
                //Try search on type
                var ifcStackTerminalType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcStackTerminalType>();  //Excel cell ref A1119
                //Excel A1115 to A1118 part range of the IfcStackTerminalTypeEnum  which is a property of IfcStackTerminalType, so if we have ifcStackTerminalType.Any() then we have covered this requirement
                if (ifcStackTerminalType.Any())
                {
                    //assume any user defined is lightly to be in the same role as generic type will be.
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                }

                //IfcWasteTerminal is IIfc 2x4, not supported at this time. A1369
                //Try search on type
                var ifcWasteTerminalType = ifcFlowTerminal.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcWasteTerminalType>();  //Excel cell ref A1119
                //Excel A1361 to A1368 part range of the IfcWasteTerminalTypeEnum  which is a property of IfcWasteTerminalType, so if we have ifcWasteTerminalType.Any() then we have covered this requirement
                if (ifcWasteTerminalType.Any())
                {
                    //assume any user defined is lightly to be in the same role as generic type will be.
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Plumbing);
                }
            }

            return _mergeDefault;
        }

        private bool FlowStorageDevice(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            
            IIfcFlowStorageDevice ifcFlowStorageDevice = ifcElement as IIfcFlowStorageDevice;
            if (ifcFlowStorageDevice != null)
            {
                //IfcElectricFlowStorageDevice is IIfc 2x4, not supported at this time. A644,
                //as stored in Ifc2x3
                var ifcElectricFlowStorageDeviceType = ifcFlowStorageDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcElectricFlowStorageDeviceType>();  //Excel cell ref A644
                //Excel A639 to A643 id full range of the IfcElectricFlowStorageDeviceType  which is a property of IfcElectricFlowStorageDeviceType, so if we have ifcElectricFlowStorageDeviceType.Any() then we have covered this requirement
                if (ifcElectricFlowStorageDeviceType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
            }
            return _mergeDefault;
        }

        private bool FlowMovingDevice(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcFlowMovingDevice ifcFlowMovingDevice = ifcElement as IIfcFlowMovingDevice;
            if (ifcFlowMovingDevice != null)
            {
                var ifcCompressorType = ifcFlowMovingDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcCompressorType>();  //Excel cell ref A219
                //Excel A187 to A202 id full range of the IfcCompressorTypeEnum  which is a property of IfcCompressorType, so if we have ifcCompressorType.Any() then we have covered this requirement
                if (ifcCompressorType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcFanType = ifcFlowMovingDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcFanType>();  //Excel cell ref A710
                //Excel A702 to A709 id full range of the IfcFanTypeEnum  which is a property of IfcFanType, so if we have ifcFanType.Any() then we have covered this requirement
                if (ifcFanType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical );

                var ifcPumpType = ifcFlowMovingDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcPumpType>();  //Excel cell ref A999
                //Excel A991 to A998 id full range of the IfcPumpTypeEnum  which is a property of IfcPumpType, so if we have ifcPumpType.Any() then we have covered this requirement
                if (ifcPumpType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Plumbing | COBieMergeRoles.FireProtection);
            }
            return _mergeDefault;
        }

        private bool FlowFitting(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcFlowFitting ifcFlowFitting = ifcElement as IIfcFlowFitting;
            if (ifcFlowFitting != null)
            {
                //IfcInterceptor/IfcInterceptorType is IIfc 2x4, not supported at this time.A859

                var ifcJunctionBoxType = ifcFlowFitting.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcJunctionBoxType>();  //Excel cell ref A864
                //Excel A851 to A863 id full range of the IfcJunctionBoxType  which is a property of IfcJunctionBoxType.
                //ifcJunctionBoxTypeEnum for Ifc2x3 does not hold "POWER" or "DATA", but lets assume that if we have the type we have the merge requirement
                if (ifcJunctionBoxType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
            }
            return _mergeDefault;
        }

        private bool FlowController(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcFlowController ifcFlowController = ifcElement as IIfcFlowController;
            if (ifcFlowController != null)
            {
                List<string> testStrings = new List<string>(){"Facility Lightning Protection", "Circuit", "Switch"};
                var ifcDamperType = ifcFlowController.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcDamperType>();  //Excel cell ref E463
                //Excel A437 to A449 id full range of the IfcDamperTypeEnum  which is a property of IfcDamperType, so if we have ifcDamperType.Any() then we have covered this requirement
                if (ifcDamperType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                //Excel A622 to A631 id full range of the IfcElectricDistributionPointEnum  which is a property of IfcElectricDistributionPoint, so if we have a IfcElectricDistributionPoint then we have covered this requirement
                //SRL DELETED IN ifc4
                //if (ifcFlowController is IIfcElectricDistributionPoint)//Excel cell ref E637
                //{
                //    IIfcElectricDistributionPoint ifcElectricDistributionPoint = (IIfcElectricDistributionPoint)ifcFlowController;
                //    //Excel cell ref E738
                //    if (ifcElectricDistributionPoint.DistributionPointFunction == IfcElectricDistributionPointFunctionEnum.USERDEFINED)
                //    {
                //        if ((!string.IsNullOrEmpty(ifcElectricDistributionPoint.UserDefinedFunction)) &&
                //            testStrings.Contains(ifcElectricDistributionPoint.UserDefinedFunction)
                //           )
                //        {
                //            return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                //        }
                //        else
                //            return false;

                //    }
                //    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                //}

                var ifcElectricTimeControlType = ifcFlowController.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcElectricTimeControlType>();  //Excel cell ref E665
                //Excel A660 to A664 id full range of the IfcElectricTimeControlTypeEnum  which is a property of IfcElectricTimeControlType, so if we have ifcElectricTimeControlType.Any() then we have covered this requirement
                if (ifcElectricTimeControlType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);

                var ifcProtectiveDeviceType = ifcFlowController.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcProtectiveDeviceType>();  //Excel cell ref E982
                //Excel A973 to A981 id full range of the IfcProtectiveDeviceTypeEnum  which is a property of IfcProtectiveDeviceType, so if we have ifcProtectiveDeviceType.Any() then we have covered this requirement
                //note Ifc 2x4 has EARTHLEAKAGECIRCUITBREAKER, EARTHINGSWITCH but Ifc 2x3 has EARTHFAILUREDEVICE, assume this equals the Ifc 2x4 requirement
                if (ifcProtectiveDeviceType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                
                //IfcProtectiveDeviceTrippingUnitType is IIfc 2x4, not supported at this time. E989

                var ifcSwitchingDeviceType = ifcFlowController.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcSwitchingDeviceType>();  //Excel cell ref E1153
                //Excel A1148 to A1158 id full range of the IfcSwitchingDeviceTypeEnum  which is a property of IfcSwitchingDeviceType, so if we have ifcSwitchingDeviceType.Any() then we have covered this requirement
                //note Ifc 2x4 has DIMMERSWITCH, KEYPAD, MOMENTARYSWITCH, SELECTORSWITCH not supported
                if (ifcSwitchingDeviceType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);

                //IfcUnitaryControlElement/IfcUnitaryControlElementType is IIfc 2x4, not supported at this time.A1235

                //IfcValve is IIfc 2x4, not supported at this time. A1327
                //try Ifc2x3 type
                var ifcValveType = ifcFlowController.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcValveType>();  //Excel cell ref A1327
                //Excel A1305 to A1326 id full range of the IfcValveTypeEnum  which is a property of IfcValveType, so if we have ifcValveType.Any() then we have covered this requirement
                if (ifcValveType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Plumbing | COBieMergeRoles.FireProtection);
            }
            return _mergeDefault;
        }

        private bool EnergyConversionDevice(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcEnergyConversionDevice ifcEnergyConversionDevice = ifcElement as IIfcEnergyConversionDevice;
            if (ifcEnergyConversionDevice != null)
            {
                List<string> testStrings = new List<string>() { "Furnaces", "Fuel-Fired Heaters", "Heat Exchangers for HVAC", "Refrigerant Condensers", "Packaged Water Chillers", "Cooling Towers", "Evaporative Air-Cooling Equipment" }; //A667 to A673
                var ifcEnergyConversionDeviceType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcEnergyConversionDeviceType>();  //Excel cell ref A674
                if (ifcEnergyConversionDeviceType.Where(ecdt => testStrings.Contains(ecdt.ElementType)).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcAirToAirHeatRecoveryType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcAirToAirHeatRecoveryType>();  //Excel cell ref A46
                //Excel A36 to A45 id full range of the IfcAirToAirHeatRecoveryTypeEnum  which is a property of IfcAirToAirHeatRecoveryType, so if we have ifcAirToAirHeatRecoveryType.Any() then we have this requirement
                if (ifcAirToAirHeatRecoveryType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcBoilerType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcBoilerType>();  //Excel cell ref A88
                //Excel A91 to A34 id full range of the IfcBoilerTypeEnum  which is a property of IfcBoilerType, so if we have ifcBoilerType.Any() then we have this requirement
                if (ifcBoilerType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                //IfcBurnerType is IIfc 2x4, not supported at this time. A107

                var ifcChillerType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcChillerType>();  //Excel cell ref A144
                //Excel A135 to A139 id full range of the IfcChillerTypeEnum  which is a property of IfcChillerType, so if we have IfcChillerType.Any() then we have this requirement
                if (ifcChillerType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcCoilType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcCoilType>();  //Excel cell ref A165
                //Excel A143 to A156 id full range of the IfcCoilTypeEnum  which is a property of IfcCoilType, so if we have IfcCoilType.Any() then we have this requirement
                if (ifcCoilType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcCondenserType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcCondenserType>();  //Excel cell ref A238
                //Excel A222 to A223 id full range of the IfcCondenserTypeEnum  which is a property of IfcCondenserType, so if we have ifcCondenserType.Any() then we have this requirement
                if (ifcCondenserType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcElectricGeneratorType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcElectricGeneratorType>();  //Excel cell ref A650
                //Excel A646 to A649 id full range of the IfcElectricGeneratorTypeEnum  which is a property of IfcElectricGeneratorType, so if we have ifcElectricGeneratorType.Any() then we have this requirement
                if (ifcElectricGeneratorType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);

                var ifcElectricMotorType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcElectricMotorType>();  //Excel cell ref A650
                //Excel A652 to A657 id full range of the IfcElectricMotorTypeEnum  which is a property of IfcElectricMotorType, so if we have ifcElectricMotorType.Any() then we have this requirement
                if (ifcElectricMotorType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);

                //IfcEngineType is IIfc 2x4, not supported at this time. A673

                var ifcEvaporativeCoolerType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcEvaporativeCoolerType>();  //Excel cell ref A691
                //Excel A681 to A690 id full range of the  IfcEvaporativeCoolerTypeEnum  which is a property of  IfcEvaporativeCoolerType, so if we have  ifcEvaporativeCoolerType.Any() then we have this requirement
                if (ifcEvaporativeCoolerType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcEvaporatorType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcEvaporatorType>();  //Excel cell ref A700
                //Excel A693 to A699 id full range of the  IfcEvaporatorTypeEnum  which is a property of  IfcEvaporatorType, so if we have  ifcEvaporatorType.Any() then we have this requirement
                if (ifcEvaporatorType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcMotorConnectionType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcMotorConnectionType>();  //Excel cell ref A933
                //Excel A929 to A932 id full range of the  IfcMotorConnectionTypeEnum  which is a property of  IfcMotorConnectionType, so if we have  ifcMotorConnectionType.Any() then we have this requirement
                if (ifcMotorConnectionType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                //IfcSolarDeviceType is IIfc 2x4, not supported at this time. A1108
                //IfcSpaceHeater is IIfc 2x4, not supported at this time. A1113

                var ifcTransformerType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcTransformerType>();  //Excel cell ref A1260
                //Excel A1254 to A1259 id full range of the  IfcTransformerTypeEnum  which is a property of  IfcTransformerType, so if we have  ifcTransformerType.Any() then we have this requirement
                if (ifcTransformerType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);

                var ifcUnitaryEquipmentType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcUnitaryEquipmentType>();  //Excel cell ref A1303
                //Excel A1297 to A1302 id full range of the  IfcUnitaryEquipmentTypeEnum  which is a property of  IfcUnitaryEquipmentType, so if we have  ifcUnitaryEquipmentType.Any() then we have this requirement
                if (ifcUnitaryEquipmentType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);

                var ifcHeatExchangerType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcHeatExchangerType>();  //Excel cell ref A833
                //Excel A830 to A832 id full range of the  IfcHeatExchangerTypeEnum  which is a property ofIfcHeatExchangerType so if we have ifcHeatExchangerType.Any() then we have this requirement
                if (ifcHeatExchangerType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                var ifcCoolingTowerType = ifcEnergyConversionDevice.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcCoolingTowerType>();  //Excel cell ref A272
                //Excel A263 to A267 id full range of the  IfcCoolingTowerTypeEnum  which is a property of  IfcCoolingTowerType, so if we have  ifcCoolingTowerType.Any() then we have this requirement
                if (ifcCoolingTowerType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                 
            }

            return _mergeDefault;
        }

        private bool Door(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
           //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcDoor ifcDoor = ifcElement as IIfcDoor;
            if (ifcDoor != null)
            {
                var ifcDoorStyle = ifcDoor.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcDoorStyle>();  //Excel cell ref A543
                //Excel A480 to A507 id full range of the IfcDoorStyleOperationEnum/IfcDoorStyleConstructionEnum  which are properties of IfcDoorStyle, so if we have ifcDoorStyle.Any() then we have covered this requirement
                if (ifcDoorStyle.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Architectural);
            }

            return _mergeDefault;
        }

        private bool DistributionFlowElement(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcDistributionFlowElement ifcDistributionFlowElement = ifcElement as IIfcDistributionFlowElement;
            if (ifcDistributionFlowElement != null)
            {
                //IfcTubeBundle is IIfc 2x4, not supported at this time. A1284
                //as stored in Ifc 2x3
                var ifcTubeBundleType = ifcDistributionFlowElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcTubeBundleType>();  //Excel cell ref A1284
                //Excel A1282 to A1283 id full range of the IfcTubeBundleTypeEnum  which is a property of IfcTubeBundleType, so if we have ifcTubeBundleType.Any() then we have covered this requirement
                if (ifcTubeBundleType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);

                //IfcTankType is IIfc 2x4, IfcTankTypeEnum has more values, not supported at this time. A1252
                //as stored in Ifc 2x3
                var ifcTankType = ifcDistributionFlowElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcTankType>();  //Excel cell ref A1252
                //Excel A1244 to A1251 id full range of the IfcTankTypeEnum(2x4) assume that 2x3 full values OK which is a property of IfcTankType, so if we have ifcTankType.Any() then we have covered this requirement
                if (ifcTankType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);

                //IfcFlowMeter is IIfc 2x4, IfcFlowMeterTypeEnum has less values, not supported at this time. A758
                //as stored in Ifc 2x3
                var ifcFlowMeterType = ifcDistributionFlowElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcFlowMeterType>();  //Excel cell ref A758
                //Excel A753 to A757 id full range of the IfcFlowMeterTypeEnum(2x4) assume that 2x3 full values OK which is a property of IfcFlowMeterType, so if we have IfcFlowMeterType.Any() then we have covered this requirement
                if (ifcFlowMeterType.Any())
                {
                    switch (ifcFlowMeterType.First().PredefinedType)
                    {
                            //SRL DELETED IN IFC4
                        //case IfcFlowMeterTypeEnum.ELECTRICMETER:
                        //    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                        //case IfcFlowMeterTypeEnum.FLOWMETER:
                        //    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical); //assume this from name
                        case IfcFlowMeterTypeEnum.ENERGYMETER:
                            return MatchRoles(fileRoles, COBieMergeRoles.Electrical);                 
                        case IfcFlowMeterTypeEnum.GASMETER:
                            return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                        case IfcFlowMeterTypeEnum.OILMETER:
                            return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                        case IfcFlowMeterTypeEnum.WATERMETER:
                            return MatchRoles(fileRoles, COBieMergeRoles.Plumbing);
                        case IfcFlowMeterTypeEnum.USERDEFINED:
                            return _mergeDefault;
                        case IfcFlowMeterTypeEnum.NOTDEFINED:
                            return false;
                        default:
                            return _mergeDefault;
                    }
                }
                   

            }

            return _mergeDefault;
        }

        private bool Covering(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcCovering ifcCovering = ifcElement as IIfcCovering;
            if (ifcCovering != null)
            {
                var ifcCoveringType = ifcCovering.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcCoveringType>();  //Excel cell ref A426
                //Excel A276 to A285 id full range of the IfcCoveringTypeEnum  which is a property of IfcCoveringType, so if we have ifcCoveringType.Any() then we have covered this requirement
                if (ifcCoveringType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Architectural);
                
                
                //assume any covering is Architectural
                return MatchRoles(fileRoles, COBieMergeRoles.Architectural);
            }
            return _mergeDefault;
        }

        private bool BuildingElementPart(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
           //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcBuildingElementPart ifcBuildingElementPart = ifcElement as IIfcBuildingElementPart;
            if (ifcBuildingElementPart != null) 
            {
                //IfcCoolingTowerType is not a class of IfcBuildingElementType, but IfcEnergyConversionDeviceType so moved to EnergyConversionDevice() method
                //IfcShadingDevice/IfcShadingDeviceType is IIfc 2x4, not supported at this time. A1093
            }
            return _mergeDefault;
        }

        private bool DistributionControlElement(IIfcElement ifcElement, COBieMergeRoles fileRoles)
        {
            //using merge rules defined in 2012-03-27-IFCObjects-DRAFT-CDMergeRules-v04.xlsx
            IIfcDistributionControlElement ifcDistributionControlElement = ifcElement as IIfcDistributionControlElement;
            if (ifcDistributionControlElement != null) //Excel row 3 to 11, 12 to 13 is IIfc2x4 which is not implemented in Xbim at this time
            {
               
                var ifcActuatorType = ifcDistributionControlElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcActuatorType>();  //Excel cell ref E4
                //Excel A6 to A11 id full range of the IfcActuatorTypeEnum  which is a property of IfcActuatorType, so if we have ifcActuatorType.Any() then we have covered this requirement
                if (ifcActuatorType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Electrical | COBieMergeRoles.Plumbing | COBieMergeRoles.FireProtection);

                //IfcDistributionControlElementType is ignored as this is the parent class of the ifcAlarmType, IfcActuatorType, IfcControllerType, IfcSensorType and IfcFlowInstrumentType (IIfcFlowInstrumentType, which has no reference in excel sheet)
                var ifcAlarmType = ifcDistributionControlElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcAlarmType>();  //Excel cell ref E43
                //Excel A50 to A57 id full range of the IfcAlarmTypeEnum  which is a property of IfcAlarmType, so if we have ifcAlarmType.Any() then we have covered this requirement
                if (ifcAlarmType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Electrical | COBieMergeRoles.FireProtection);

                //IfcDistributionControlElementType see note above
                var ifcControllerType = ifcDistributionControlElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcControllerType>();  //Excel cell ref E241
                //Excel A242 to A249 id full range of the IfcControllerTypeEnum  which is a property of IfcControllerType, so if we have ifcControllerType.Any() then we have covered this requirement
                if (ifcControllerType.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Electrical | COBieMergeRoles.Plumbing | COBieMergeRoles.FireProtection);
                
                //down as ? in excel sheet, but assume this i think A751
                var ifcFlowInstrumentType = ifcDistributionControlElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcFlowInstrumentType>();  //Excel cell ref E751
                //Excel A742 to A750 id full range of the IfcFlowInstrumentTypeEnum  which is a property of IfcFlowInstrumentType, so if we have ifcFlowInstrumentType.Any() then we have covered this requirement
                if (ifcFlowInstrumentType.Any())
                {
                    switch (ifcFlowInstrumentType.First().PredefinedType)
                    {
                        case IfcFlowInstrumentTypeEnum.PRESSUREGAUGE:
                            return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                        case IfcFlowInstrumentTypeEnum.THERMOMETER:
                            return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                        case IfcFlowInstrumentTypeEnum.AMMETER:
                        case IfcFlowInstrumentTypeEnum.FREQUENCYMETER:
                        case IfcFlowInstrumentTypeEnum.POWERFACTORMETER:
                        case IfcFlowInstrumentTypeEnum.PHASEANGLEMETER:
                        case IfcFlowInstrumentTypeEnum.VOLTMETER_PEAK:
                        case IfcFlowInstrumentTypeEnum.VOLTMETER_RMS:
                            return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                        case IfcFlowInstrumentTypeEnum.NOTDEFINED:
                            return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                        case IfcFlowInstrumentTypeEnum.USERDEFINED:
                            return _mergeDefault;
                        default:
                            return _mergeDefault;
                    }
                }

                //IfcDistributionControlElementType see note above
                var ifcSensorType = ifcDistributionControlElement.IsDefinedBy.OfType<IIfcRelDefinesByType>().Select(rdbt => rdbt.RelatingType).OfType<IIfcSensorType>();  //Excel cell ref E241
                //Excel E1060 to E1080 , CONDUCTANCESENSOR, CONTACTSENSOR, IONCONCENTRATIONSENSOR, LEVELSENSOR, PHSENSOR, RADIATIONSENSOR, RADIOACTIVITYSENSOR, WINDSENSOR -  IFC2x4(not supported)
                //sheet shows nothing for IFC2x3, assumed that if the IfcSensorTypeEnum (ifc2x3) contains any of the ifc2x4 items that the rule would hold.
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.FIRESENSOR).Any())
                    return MatchRoles(fileRoles,  COBieMergeRoles.FireProtection);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.FLOWSENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.GASSENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.HEATSENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.FireProtection);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.HUMIDITYSENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.LIGHTSENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.MOISTURESENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.MOVEMENTSENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.PRESSURESENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Electrical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.SMOKESENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Electrical);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.SOUNDSENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Electrical | COBieMergeRoles.FireProtection);
                if (ifcSensorType.Where(st => st.PredefinedType == IfcSensorTypeEnum.TEMPERATURESENSOR).Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Electrical );

                
                //assume if any flow controller is attached then set roles below
                var ifcFlowControllers = ifcDistributionControlElement.AssignedToFlowElement.Select(rfcl => rfcl.RelatingFlowElement).OfType<IIfcFlowController>();          //Excel cell ref E3
                if (ifcFlowControllers.Any())
                    return MatchRoles(fileRoles, COBieMergeRoles.Mechanical | COBieMergeRoles.Electrical | COBieMergeRoles.Plumbing | COBieMergeRoles.FireProtection);
               
            }
            return _mergeDefault;
        }

        /// <summary>
        /// See if we need to merge comparing the file roles to the merge ifcType roles
        /// </summary>
        /// <param name="fileRoles">Roles associated with the file, COBieMergeRoles</param>
        /// <param name="mergeRoles">Roles associated with the ifcType, COBieMergeRoles</param>
        /// <returns></returns>
        private bool MatchRoles (COBieMergeRoles fileRoles, COBieMergeRoles mergeRoles)
        {
            foreach (COBieMergeRoles role in Enum.GetValues(typeof(COBieMergeRoles)))
            {
                if ((fileRoles.HasFlag(role)) && (mergeRoles.HasFlag(role)))
                    return true;
            }
            return false;
        }

        
    }
}
