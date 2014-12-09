using System;
using System.Collections.Generic;
using Xbim.Ifc2x3.ElectricalDomain;
using Xbim.Ifc2x3.HVACDomain;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.Ifc2x3.SharedBldgServiceElements;
using Xbim.Ifc2x3.SharedComponentElements;
using Xbim.Ifc2x3.StructuralElementsDomain;
using Xbim.Ifc2x3.StructuralAnalysisDomain;

namespace Xbim.COBie
{
    public class FilterValues
    {
        public ComponentValues Component { get; set; }
        public CommonValues Common { get; set; }
        public ObjectTypeValues ObjectType { get; set;}
        public FacilityValues Facility { get; set; }
        public FloorValues Floor { get; set; }
        public SpaceValues Space { get; set; }
        public TypeValues Types { get; set; }
        public ZoneValues Zone { get; set; }
        public SpareValues Spare { get; set; }
        

        public FilterValues()
        {
            Component = new ComponentValues();
            Common = new CommonValues();
            ObjectType = new ObjectTypeValues();
            Facility = new FacilityValues();
            Floor = new FloorValues();
            Space = new SpaceValues();
            Types = new TypeValues();
            Zone = new ZoneValues();
            Spare = new SpareValues();
        }

        public void Clear()
        {
            ObjectType.Component.Clear();
            ObjectType.Types.Clear();
            ObjectType.Assembly.Clear();
            Space.PropertySetsEqualTo.Clear();
            Space.AttributesEqualTo.Clear();
            Space.AttributesContain.Clear();
            Types.PropertySetsEqualTo.Clear();
            Types.AttributesEqualTo.Clear();
            Types.AttributesContain.Clear();
            Zone.AttributesContain.Clear();
            Spare.AttributesEqualTo.Clear();
            Spare.AttributesContain.Clear();
            Floor.AttributesContain.Clear();
            Floor.AttributesEqualTo.Clear();
            Facility.AttributesEqualTo.Clear();
            Facility.AttributesContain.Clear();
            Component.AttributesEqualTo.Clear();
            Component.AttributesContain.Clear();
            Common.AttributesStartWith.Clear();
            Common.AttributesEqualTo.Clear();
            Common.AttributesContain.Clear();
        }
    }

    
    public class ObjectTypeValues
    {
        /// <summary>
        /// List of component class types to exclude from selection
        /// </summary>
        public List<Type> Component {get; private set;}

        /// <summary>
        /// List of type object class types to exclude from selection
        /// </summary>
        public List<Type> Types {get; private set;}

        //list or classes to exclude if the related object of the IfcRelAggregates is one of these types
        private List<Type> _assemblyExcludeTypes = new List<Type>(){  typeof(IfcSite),
                                                        typeof(IfcProject),
                                                        typeof(IfcBuilding),
                                                        typeof(IfcBuildingStorey)
                                                        };

        /// <summary>
        /// List of type object class types to exclude from selection
        /// </summary>
        public List<Type> Assembly {get; private set;}
        
        public ObjectTypeValues()
        {
            // List of component class types to exclude from selection
            Component = new List<Type>(){ typeof(IfcAnnotation),
                                        typeof(IfcBeam),
                                        typeof(IfcColumn),
                                        typeof(IfcCurtainWall),
                                        typeof(IfcMember),
                                        typeof(IfcPile),
                                        typeof(IfcPlate),
                                        typeof(IfcRailing),
                                        typeof(IfcRamp),
                                        typeof(IfcRampFlight),
                                        typeof(IfcRoof),
                                        typeof(IfcSlab),
                                        typeof(IfcStair),
                                        typeof(IfcStairFlight),
                                        typeof(IfcWall),
                                        typeof(IfcWallStandardCase),
                                        typeof(IfcFlowFitting),
                                        typeof(IfcFlowSegment),
                                        typeof(IfcElementAssembly),
                                        typeof(IfcBuildingElementPart),
                                        typeof(IfcFastener),
                                        typeof(IfcMechanicalFastener),
                                        typeof(IfcReinforcingBar),
                                        typeof(IfcReinforcingMesh),
                                        typeof(IfcFeatureElement),
                                        typeof(IfcFeatureElementAddition), 
                                        typeof(IfcProjectionElement), 
                                        typeof(IfcVirtualElement),
                                        typeof(IfcDistributionPort), 
                                        typeof(IfcBuilding),
                                        typeof(IfcBuildingStorey),
                                        typeof(IfcSite),
                                        typeof(IfcSpace),
                                        typeof(IfcProject),
                                        //typeof(IfcTendon),
                                        //typeof(IfcTendonAnchor),
                                        typeof(IfcFooting),
                                        typeof(IfcStructuralActivity),
                                        typeof(IfcStructuralItem)
                                        //typeof(ifcCovering),
                                        ///typeof(IfcColumnStandardCase), //IFC2x Edition 4.
                                        //typeof(IfcMemberStandardCase), //IFC2x Edition 4.
                                        //typeof(IfcPlateStandardCase), //IFC2x Edition 4.
                                        //typeof(IfcSlabElementedCase), //IFC2x Edition 4.
                                        //typeof(IfcSlabStandardCase), //IFC2x Edition 4.
                                        //typeof(IfcWallElementedCase), //IFC2x Edition 4.
                                        //typeof(IfcSpatialElement),//IFC2x Edition 4.
                                        //typeof(IfcCableCarrierSegment), //IFC2x Edition 4.
                                        //typeof(IfcCableSegment), //IFC2x Edition 4.
                                        //typeof(IfcCableCarrierFitting),
                                        //typeof(IfcDuctSegment), //IFC2x Edition 4.
                                        //typeof(IfcPipeSegment), //IFC2x Edition 4.
                                        //typeof(IfcPipeFitting), //IFC2x Edition 4.
                                        //typeof(IfcJunctionBox)
                                        };

            //TODO: after IfcRampType and IfcStairType are implemented then add to excludedTypes list
            // List of Type object class types to exclude from selection
            Types = new List<Type>(){  typeof(IfcTypeProduct),
                                                            typeof(IfcElementType),
                                                            typeof(IfcBeamType),
                                                            typeof(IfcColumnType),
                                                            typeof(IfcCurtainWallType),
                                                            typeof(IfcMemberType),
                                                            typeof(IfcPlateType),
                                                            typeof(IfcRailingType),
                                                            typeof(IfcRampFlightType),
                                                            typeof(IfcSlabType),
                                                            typeof(IfcStairFlightType),
                                                            typeof(IfcWallType),
                                                            typeof(IfcDuctFittingType ),
                                                            typeof(IfcJunctionBoxType ),
                                                            typeof(IfcPipeFittingType),
                                                            typeof(IfcCableCarrierSegmentType),
                                                            typeof(IfcCableSegmentType),
                                                            typeof(IfcDuctSegmentType),
                                                            typeof(IfcPipeSegmentType),
                                                            typeof(IfcFastenerType),
                                                            typeof(IfcSpaceType),
                                                            typeof(IfcBuildingElementProxyType),
                                                            typeof(IfcFlowFittingType),
                                                            typeof(IfcFlowSegmentType)
                                                            //typeof(IfcTypeProcess), //IFC2x Edition 4.
                                                            //typeof(Xbim.Ifc.SharedBldgElements.IfcRampType), //IFC2x Edition 4.
                                                            //typeof(IfcStairType), //IFC2x Edition 4.
                                                             };

            Assembly = new List<Type>(){  typeof(IfcSite),
                                                typeof(IfcProject),
                                                typeof(IfcBuilding),
                                                typeof(IfcBuildingStorey)
                                                };
        }
    }

    /// <summary>
    /// Attribute exclude strings class, for Zone sheet
    /// </summary>
    public class ZoneValues
    {
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Zone sheet with contains compare
        /// </summary>
        public List<string> AttributesContain { get; private set; }
        public ZoneValues()
        {
            AttributesContain = new List<string> { "Roomtag", "RoomTag", "Tag", "GSA BIM Area", "Length", "Width", "Height" };

        }

    }

    /// <summary>
    /// Attribute exclude strings class, for Type sheet
    /// </summary>
    public class TypeValues
    {
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Type sheet with equal compare
        /// </summary>
        public List<string> AttributesEqualTo { get; private set; }
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Type sheet with contains compare
        /// </summary>
        public List<string> AttributesContain { get; private set; }
        /// <summary>
        /// List of property set names that are to be excluded from the Attributes generated from the Type sheet with equal compare
        /// </summary>
        public List<string> PropertySetsEqualTo { get; private set; }

        public TypeValues()
        {
            AttributesEqualTo = new List<string>() 
                                {   "SustainabilityPerformanceCodePerformance",     "AccessibilityPerformance",     "Features",     "Constituents",     "Material",     "Grade", 
                                    "Finish",   "Color",    "Size",     "Shape",    "ModelReference",   "NominalHeight",    "NominalWidth", "NominalLength",    "WarrantyName",
                                    "WarrantyDescription",  "DurationUnit",         "ServiceLifeType",  "ServiceLifeDuration",  "ExpectedLife",     "LifeCyclePhase",   "Cost",
                                    "ReplacementCost",  "WarrantyDurationUnit", "WarrantyDurationLabor",    "WarrantyGuarantorLabor",   "WarrantyDurationParts",    
                                    "WarrantyGuarantorParts",   "ModelLabel",   "ModelNumber",  "Manufacturer", "IsFixed",  "AssetType", "CodePerformance", "SustainabilityPerformance",
                                    "PointOfContact", "Colour", "Regulation", "Environmental"
                                };

            AttributesContain = new List<string>() { "Roomtag", "RoomTag", "GSA BIM Area" }; //"Tag",

            PropertySetsEqualTo = new List<string>() { "BaseQuantities" }; 
        }

    }

    /// <summary>
    /// Attribute exclude strings class, for Space sheet
    /// </summary>
    public class SpaceValues
    {
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Space sheet with equal compare
        /// </summary>
        public List<string> AttributesEqualTo { get; private set; }
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Space sheet with contains compare
        /// </summary>
        public List<string> AttributesContain { get; private set; }
        /// <summary>
        /// List of property set names that are to be excluded from the Attributes generated from the Space sheet with equal compare
        /// </summary>
        public List<string> PropertySetsEqualTo{ get; private set; }
        
        public SpaceValues()
        {
            AttributesEqualTo = new List<string> { "Area", "Number", "UsableHeight", "RoomTag", "Room Tag", "Tag", "Room_Tag", "FinishCeilingHeight" }; 

            AttributesContain = new List<string> { "ZoneName", "Category", "Length", "Width", "GrossFloorArea", "GSA", "NetFloorArea" };

            PropertySetsEqualTo = new List<string>() { "BaseQuantities" }; 
        }

    }

    /// <summary>
    /// Attribute exclude strings class, for Floor sheet
    /// </summary>
    public class FloorValues
    {
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Floor sheet with equal compare
        /// </summary>
        public List<string> AttributesEqualTo { get; private set; }
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Floor sheet with contains compare
        /// </summary>
        public List<string> AttributesContain { get; private set; }
        public FloorValues()
        {
            AttributesEqualTo = new List<string> { "Name", "Line Weight", "Color", 
                                                          "Colour",   "Symbol at End 1 Default", 
                                                          "Symbol at End 2 Default", "Automatic Room Computation Height", "Elevation", "Storey Height" }; 

            AttributesContain = new List<string> { "Roomtag", "RoomTag", "Tag", "GSA BIM Area", "Length", "Width" }; 

        }

    }

    /// <summary>
    /// Attribute exclude strings class, for Facility sheet
    /// </summary>
    public class FacilityValues
    {
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Facility sheet with equal compare
        /// </summary>
        public List<string> AttributesEqualTo { get; private set; }
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Facility sheet with contains compare
        /// </summary>
        public List<string> AttributesContain { get; private set; }
        public FacilityValues()
        {
            AttributesEqualTo = new List<string> { "Phase" }; //excludePropertyValueNames
            AttributesContain = new List<string> { "Roomtag", "RoomTag", "Tag", "GSA BIM Area", "Length", "Width", "Height" }; 

        }
        
    }

    /// <summary>
    /// Attribute exclude strings class, for Spare sheet
    /// </summary>
    public class SpareValues
    {
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Spare sheet with equal compare
        /// </summary>
        public List<string> AttributesEqualTo { get; private set; }
        /// <summary>
        /// List of property names that are to be excluded from the Attributes generated from the Spare sheet with contains compare
        /// </summary>
        public List<string> AttributesContain { get; private set; }
        public SpareValues()
        {
            AttributesEqualTo = new List<string> { "Suppliers"}; //excludePropertyValueNames
            AttributesContain = new List<string> { "Suppliers" };

        }

    }

    /// <summary>
    /// Attribute exclude strings class, for component sheet
    /// </summary>
    public class ComponentValues
    {
        /// <summary>
        /// List of property names that are to be excluded from Attributes generated from the Component sheet with equal compare
        /// </summary>
        public List<string> AttributesEqualTo { get; private set; }
        /// <summary>
        ///  List of property names that are to be excluded from the Attributes generated from the Component sheet with contains compare
        /// </summary>
        public List<string> AttributesContain { get; private set; }
        
        public ComponentValues()
        {
            // List of property names that are to be excluded from the Attributes generated from the Component sheet with equal compare
            AttributesEqualTo = new List<string>() 
            {   "Circuit NumberSystem Type", "System Name",  "AssetIdentifier", "BarCode", "TagNumber", "WarrantyStartDate", "InstallationDate", "SerialNumber"
            };

            //List of property names that are to be excluded from the Attributes generated from the Component sheet with contains compare
            AttributesContain = new List<string>() { "Roomtag", "RoomTag", "GSA BIM Area", "Length", "Height", "Render Appearance", "Arrow at End" }; //"Tag",
        
        

        }

    }

    /// <summary>
    /// Attribute exclude strings class, common to all sheets
    /// </summary>
    public class CommonValues
    {
        /// <summary>
        /// List of property names that are to be excluded from Attributes sheet with equal compare
        /// </summary>
        public List<string> AttributesEqualTo {get; private set;}
        /// <summary>
        /// List of property names that are to be excluded from Attributes sheet with start with compare
        /// </summary>
        public List<string> AttributesStartWith {get; private set;}
        /// <summary>
        ///  List of property names that are to be excluded from Attributes sheet with contains with compare
        /// </summary>
        public List<string> AttributesContain {get; private set;}
        
        public CommonValues()
        {
            //List of field names that are to be excluded from Attributes sheet with equal compare
            AttributesEqualTo = new List<string>()
            {   "MethodOfMeasurement",  "Omniclass Number",     "Assembly Code",                "Assembly Description",     "Uniclass Description",     "Uniclass Code", 
                "Category Code",    "Category Description",     "Classification Description",   "Classification Code",      "Name",                     "Description", 
                "Hot Water Radius", "Host",                     "Limit Offset",                 "Recepticles",              "Mark",     "Workset",  "Keynote",  "VisibleOnPlan",
                "Edited by", "Elevation Base", "Phase", "Phase Created", "Window Inset", "Symbol" , "Line Pattern",
                "Roomtag", "Upper Limit", "Base Offset"

                //"Zone Base Offset", "Upper Limit",   "Line Pattern", "Symbol","Window Inset", "Radius", "Phase Created","Phase", //old ones might need to put back in
            };

            //List of field names that are to be excluded from Attributes sheet with start with compare
            AttributesStartWith = new List<string>()
            {   "Omniclass Title",  "Half Oval",    "Level",    "Outside Diameter", "Outside Radius", 
                "Moves With", "Width"
            };

            // List of property names that are to be excluded from Attributes sheet with contains with compare
            AttributesContain = new List<string>()
            {   "AssetAccountingType",  "GSA BIM Area",     "Height",   "Length",   "Size",     "Lighting Calculation Workplan",    
                "Offset",   "Omniclass", "Radius", "Zone"
            };
            
        }
    }
}
