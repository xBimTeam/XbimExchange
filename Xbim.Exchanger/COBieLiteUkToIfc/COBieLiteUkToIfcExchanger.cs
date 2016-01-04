using System;
using System.IO;
using Xbim.Ifc2x3.ExternalReferenceResource;
using SystemConvert = System.Convert;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.GeometricConstraintResource;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.QuantityResource;
using Xbim.Ifc2x3.RepresentationResource;
using XbimExchanger.COBieLiteHelpers;
using XbimExchanger.IfcHelpers;
using Assembly = System.Reflection.Assembly;
using Attribute = Xbim.COBieLiteUK.Attribute;
using Xbim.FilterHelper;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.Ifc;

namespace XbimExchanger.COBieLiteUkToIfc
{
    /// <summary>
    /// Constructs the exchanger
    /// </summary>
    public class CoBieLiteUkToIfcExchanger : XbimExchanger<Facility, IfcStore>
    {
        #region Nested Structures
        /// <summary>
        /// A helper struct to handle property sets and their proper property names
        /// </summary>
        public struct NamedProperty
        {
            /// <summary>
            /// The name of the Property Set
            /// </summary>
            public string PropertySetName;
            /// <summary>
            /// The name of the Property Name
            /// </summary>
            public string PropertyName;

            /// <summary>
            /// Constructs the named property
            /// </summary>
            /// <param name="propertySetName"></param>
            /// <param name="propertyName"></param>
            public NamedProperty(string propertySetName, string propertyName)
                : this()
            {
                PropertyName = propertyName;
                PropertySetName = propertySetName;
            }
        }
        #endregion

        #region Static members and functions
        static readonly IDictionary<string, NamedProperty> CobieToIfcPropertyMap = new Dictionary<string, NamedProperty>();


        static CoBieLiteUkToIfcExchanger()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var configFile = Path.Combine(dir.FullName, "COBieAttributesCustom.config");

            var tmpFile = File.Exists(configFile) ? configFile : null;
            if (tmpFile == null)
            {

                tmpFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".csv";

                var asss = Assembly.GetExecutingAssembly();

                using (var input = asss.GetManifestResourceStream("XbimExchanger.IfcToCOBieLiteUK.COBieAttributes.config"))
                using (var output = File.Create(tmpFile))
                {
                    if (input != null)
                        input.CopyTo(output);
                }
            }

            if (!File.Exists(tmpFile))
            {
                var directory = new DirectoryInfo(".");
                throw new Exception(
                    string.Format(
                        @"Error loading configuration file ""{0}"". App folder is ""{1}"".", tmpFile,
                        directory.FullName)
                    );
            }
            COBiePropertyMapping propertyMaps = new COBiePropertyMapping(new FileInfo(tmpFile));
            var cobieFieldMap = propertyMaps.GetDictOfProperties().Where(d => d.Value.FirstOrDefault() != null).ToDictionary(d => d.Key, d => d.Value.First());
            foreach (var item in cobieFieldMap)
            {
                var names = item.Value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (names.Length == 2)
                    CobieToIfcPropertyMap.Add(item.Key, new NamedProperty(names[0], names[1]));
            }
        }
        #endregion

        #region Fields

        private readonly Dictionary<IfcObjectDefinition, List<IfcPropertySetDefinition>> _objectsToPropertySets =
            new Dictionary<IfcObjectDefinition, List<IfcPropertySetDefinition>>();
        private readonly Dictionary<string, IfcUnit> _units = new Dictionary<string, IfcUnit>();
        private readonly Dictionary<string, IfcSpatialStructureElement> _spaceLookup = new Dictionary<string, IfcSpatialStructureElement>();
        private readonly Dictionary<string, IfcClassification> _classificationSystems = new Dictionary<string, IfcClassification>();
        private readonly Dictionary<string, IfcClassificationReference> _classificationReferences = new Dictionary<string, IfcClassificationReference>();

        private Dictionary<IfcClassificationReference, IfcRelAssociatesClassification> _classificationRelationships = new Dictionary<IfcClassificationReference, IfcRelAssociatesClassification>();

        #endregion

        #region Properties

        /// <summary>
        /// The default units of length
        /// </summary>
        public IfcUnitConverter? DefaultLinearUnit;
        /// <summary>
        /// The default units of area
        /// </summary>
        public IfcUnitConverter? DefaultAreaUnit;
        /// <summary>
        /// The default units of volume
        /// </summary>
        public IfcUnitConverter? DefaultVolumeUnit;
        /// <summary>
        /// The default currency
        /// </summary>
        public CurrencyUnit? DefaultCurrencyUnit;
        private int _assetTypeInfoTypeNumber;
        private IfcCartesianPoint _origin3D;
        private IfcCartesianPoint _origin2D;
        private IfcDirection _downDirection;
        private IfcGeometricRepresentationContext _model3DContext;

        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs the exchanger
        /// </summary>
        /// <param name="facility"></param>
        /// <param name="repository"></param>
        public CoBieLiteUkToIfcExchanger(Facility facility, IfcStore repository)
            : base(facility, repository)
        {
            LoadPropertySetDefinitions();
            LoadUnits();
            //set up geometry
            InitialiseGeometry();
        }

        private void InitialiseGeometry()
        {
            var modelInstances = TargetRepository.Instances;
            _origin3D = modelInstances.New<IfcCartesianPoint>(c => c.SetXYZ(0, 0, 0));
            _origin2D = modelInstances.New<IfcCartesianPoint>(c => c.SetXY(0, 0));
            _downDirection = modelInstances.New<IfcDirection>(d => d.SetXYZ(0, 0, -1));
            var project = TargetRepository.Instances.FirstOrDefault<IfcProject>();
            if (project != null)
            {
                var mainContext = project.ModelContext;
                mainContext.ContextIdentifier = null; //balnk of the main context
                _model3DContext = modelInstances.New<IfcGeometricRepresentationSubContext>(c =>
                {
                    c.ContextType = "Model";
                    c.ContextIdentifier = "Body";
                    c.ParentContext = project.ModelContext;
                    c.TargetView = IfcGeometricProjectionEnum.MODEL_VIEW;
                }
                );
            }
        }

        #endregion
        #region Poperties
        /// <summary>
        /// Returns the origin for world coordinate system
        /// </summary>
        public IfcCartesianPoint Origin3D
        {
            get
            {
                return _origin3D;

            }
        }
        /// <summary>
        /// The 2D origin
        /// </summary>
        public IfcCartesianPoint Origin2D
        {
            get
            {
                return _origin2D;

            }
        }

        /// <summary>
        /// The Geometric Representation sub context for this exchange
        /// </summary>
        public IfcGeometricRepresentationContext Model3DContext
        {
            get { return _model3DContext; }
        }

        /// <summary>
        /// Returns the spaces successfully added to the model
        /// </summary>
        public IEnumerable<IfcSpatialStructureElement> Spaces
        {
            get { return _spaceLookup.Values; }
        }

        /// <summary>
        /// Mapping of OwnerHistory fields to IfcOwnerHistory Objects
        /// </summary>
        private Dictionary<OwnerHistoryKey, IfcOwnerHistory> OwnerHistories
        { get; set; }

        /// <summary>
        /// Contacts Lookup
        /// </summary>
        public Dictionary<string, IfcPersonAndOrganization> Contacts
        {
            get;
            set;
        }


        #endregion
        #region Converters
        /// <summary>
        /// Converts the facility to an IfcBuilding
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        public IfcBuilding Convert(Facility facility)
        {
            var mapping = GetOrCreateMappings<MappingFacilityToIfcBuilding>();
            var building = mapping.GetOrCreateTargetObject(facility.ExternalId);
            return mapping.AddMapping(facility, building);

        }

        /// <summary>
        /// Converts the facility to an IfcBuilding
        /// </summary>
        /// <returns></returns>
        public override IfcStore Convert()
        {
            Convert(SourceRepository);
            return TargetRepository;
        }
        #endregion

        #region Methods

        internal IfcPropertySetDefinition GetOrCreatePropertySetDefinition(IfcObjectDefinition ifcObjectDefinition, string propertySetName)
        {
            List<IfcPropertySetDefinition> propertySetDefinitionList;
            if (!_objectsToPropertySets.TryGetValue(ifcObjectDefinition, out propertySetDefinitionList))
            {
                propertySetDefinitionList = new List<IfcPropertySetDefinition>();
                _objectsToPropertySets.Add(ifcObjectDefinition, propertySetDefinitionList);
            }
            var propertySet = propertySetDefinitionList.Find(p => p.Name == propertySetName);
            if (propertySet == null)
            {
                //simplistic way to decide if this should be a quantity, IFC 4 specifies the name starts with QTO, under 2x3 most vendors have gone for BaseQuantities
                if (propertySetName != null && (propertySetName.StartsWith("qto_", true, CultureInfo.InvariantCulture) ||
                    propertySetName.StartsWith("basequantities", true,
                        CultureInfo.InvariantCulture)))
                {
                    var quantitySet = TargetRepository.Instances.New<IfcElementQuantity>();
                    propertySetDefinitionList.Add(quantitySet);
                    quantitySet.Name = propertySetName;
                    var ifcObject = ifcObjectDefinition as IfcObject;
                    var ifcTypeObject = ifcObjectDefinition as IfcTypeObject;
                    if (ifcObject != null)
                    {
                        var relDef = TargetRepository.Instances.New<IfcRelDefinesByProperties>();
                        relDef.RelatingPropertyDefinition = quantitySet;
                        relDef.RelatedObjects.Add(ifcObject);
                    }
                    else if (ifcTypeObject != null)
                    {                     
                        ifcTypeObject.HasPropertySets.Add(quantitySet);
                    }
                    else
                        throw new Exception("Invalid object type " + ifcObjectDefinition.GetType().Name);

                    propertySet = quantitySet;
                }
                else //it is a normal property set
                {
                    propertySet = TargetRepository.Instances.New<IfcPropertySet>();
                    propertySetDefinitionList.Add(propertySet);
                    propertySet.Name = propertySetName;
                    var ifcObject = ifcObjectDefinition as IfcObject;
                    var ifcTypeObject = ifcObjectDefinition as IfcTypeObject;
                    if (ifcObject != null)
                    {
                        var relDef = TargetRepository.Instances.New<IfcRelDefinesByProperties>();
                        relDef.RelatingPropertyDefinition = propertySet;
                        relDef.RelatedObjects.Add(ifcObject);
                    }
                    else if (ifcTypeObject != null)
                    {                      
                        ifcTypeObject.HasPropertySets.Add(propertySet);
                    }
                    else
                        throw new Exception("Invalid object type " + ifcObjectDefinition.GetType().Name);
                }
            }
            return propertySet;
        }
        private void LoadUnits()
        {
            //var units = TargetRepository.Instances.OfType<IfcUnit>().ToDictionary(k => k.GetName());
        }

        private void LoadPropertySetDefinitions()
        {
            var relProps = TargetRepository.Instances.OfType<IfcRelDefinesByProperties>().ToList();
            foreach (var relProp in relProps)
            {
                foreach (var ifcObject in relProp.RelatedObjects)
                {
                    List<IfcPropertySetDefinition> propDefinitionList;
                    if (!_objectsToPropertySets.TryGetValue(ifcObject, out propDefinitionList)) //if it hasn't got any, add an empty list
                    {
                        propDefinitionList = new List<IfcPropertySetDefinition>();
                        _objectsToPropertySets.Add(ifcObject, propDefinitionList);
                    }
                    propDefinitionList.Add(relProp.RelatingPropertyDefinition);
                }
            }
            var typeObjects = TargetRepository.Instances.OfType<IfcTypeObject>().ToList();
            foreach (var typeObject in typeObjects)
            {
                List<IfcPropertySetDefinition> propDefinitionList;
                if (!_objectsToPropertySets.TryGetValue(typeObject, out propDefinitionList)) //if it hasn't got any, add an empty list
                {
                    propDefinitionList = new List<IfcPropertySetDefinition>();
                    _objectsToPropertySets.Add(typeObject, propDefinitionList);
                }
                foreach (var propertySetDef in typeObject.HasPropertySets)
                {
                    propDefinitionList.Add(propertySetDef);
                }
            }
        }
        #endregion

        internal bool TryCreatePropertySingleValue(IfcObject ifcObject, string value, string cobiePropertyName)
        {
            try
            {
                NamedProperty namedProperty;
                if (CobieToIfcPropertyMap.TryGetValue(cobiePropertyName, out namedProperty))
                {
                    List<IfcPropertySetDefinition> propertySetDefinitionList;
                    if (!_objectsToPropertySets.TryGetValue(ifcObject, out propertySetDefinitionList))
                    {
                        propertySetDefinitionList = new List<IfcPropertySetDefinition>();
                        _objectsToPropertySets.Add(ifcObject, propertySetDefinitionList);
                    }
                    var propertySet = propertySetDefinitionList.Find(p => p.Name == namedProperty.PropertySetName) as IfcPropertySet;
                    if (propertySet == null)
                    {
                        propertySet = TargetRepository.Instances.New<IfcPropertySet>();
                        propertySetDefinitionList.Add(propertySet);
                        propertySet.Name = namedProperty.PropertySetName;
                        var relDef = TargetRepository.Instances.New<IfcRelDefinesByProperties>();
                        relDef.RelatingPropertyDefinition = propertySet;
                        relDef.RelatedObjects.Add(ifcObject);
                    }
                    AddProperty(ifcObject, new IfcText(value), cobiePropertyName, propertySet, namedProperty);
                    return true;
                }
                throw new ArgumentException("Incorrect property map", "cobiePropertyName");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Incorrect property map, " + e.Message);
                Debug.Assert(false);
            }
            return false;
        }

        /// <summary>
        /// Creates the property and if required the property set, populates them with the correct values and adds them to the IfcObject
        /// If the value is null or empty no property is created
        /// </summary>
        /// <param name="ifcObject">Object to associate the property with</param>
        /// <param name="valueBaseType">COBie value to populate the property with</param>
        /// <param name="cobiePropertyName">Name of the COBie property being mapped</param>
        /// <param name="defaultUnits">Units to use if the COBie property does not specify</param>
        internal bool TryCreatePropertySingleValue(IfcObject ifcObject, AttributeValue valueBaseType, string cobiePropertyName, IfcUnitConverter? defaultUnits)
        {
            if (valueBaseType == null) return false; //nothing to do
            try
            {
                NamedProperty namedProperty;
                if (CobieToIfcPropertyMap.TryGetValue(cobiePropertyName, out namedProperty))
                {
                    var actualUnits = new IfcUnitConverter(valueBaseType.Unit);
                    if (actualUnits.IsUndefined && defaultUnits.HasValue) actualUnits = defaultUnits.Value;
                    List<IfcPropertySetDefinition> propertySetDefinitionList;
                    if (!_objectsToPropertySets.TryGetValue(ifcObject, out propertySetDefinitionList))
                    {
                        propertySetDefinitionList = new List<IfcPropertySetDefinition>();
                        _objectsToPropertySets.Add(ifcObject, propertySetDefinitionList);
                    }
                    var propertySetDef = propertySetDefinitionList.Find(p => p.Name == namedProperty.PropertySetName);
                    //see what sets we have against this object
                    if (propertySetDef == null)
                    {

                        //simplistic way to decide if this should be a quantity, IFC 4 specifies the name starts with QTO, under 2x3 most vendors have gone for BaseQuantities
                        if (namedProperty.PropertySetName.StartsWith("qto_", true, CultureInfo.InvariantCulture) ||
                            namedProperty.PropertySetName.StartsWith("basequantities", true,
                                CultureInfo.InvariantCulture))
                        {
                            var quantitySet = TargetRepository.Instances.New<IfcElementQuantity>();
                            propertySetDefinitionList.Add(quantitySet);
                            quantitySet.Name = namedProperty.PropertySetName;
                            var relDef = TargetRepository.Instances.New<IfcRelDefinesByProperties>();
                            relDef.RelatingPropertyDefinition = quantitySet;
                            relDef.RelatedObjects.Add(ifcObject);
                            AddQuantity(valueBaseType, cobiePropertyName, actualUnits, quantitySet,
                                namedProperty);
                        }
                        else //it is a normal property set
                        {
                            var propertySet = TargetRepository.Instances.New<IfcPropertySet>();
                            propertySetDefinitionList.Add(propertySet);
                            propertySet.Name = namedProperty.PropertySetName;
                            var relDef = TargetRepository.Instances.New<IfcRelDefinesByProperties>();
                            relDef.RelatingPropertyDefinition = propertySet;
                            relDef.RelatedObjects.Add(ifcObject);
                            AddProperty(ifcObject, ConvertToIfcValue(valueBaseType), cobiePropertyName,
                                propertySet,
                                namedProperty);
                        }
                    }
                    else //need to use an existing PropertySet definition
                    {

                        //simplistic way to decide if this should be a quantity, IFC 4 specifies the name starts with QTO, under 2x3 most vendors have gone for BaseQuantities
                        if (namedProperty.PropertySetName.StartsWith("qto_", true, CultureInfo.InvariantCulture) ||
                            namedProperty.PropertySetName.StartsWith("basequantities", true,
                                CultureInfo.InvariantCulture))
                            AddQuantity(valueBaseType, cobiePropertyName, actualUnits,
                                (IfcElementQuantity)propertySetDef,
                                namedProperty);
                        else //it is a normal property set

                            AddProperty(ifcObject, ConvertToIfcValue(valueBaseType), cobiePropertyName, (IfcPropertySet)propertySetDef,
                                namedProperty);
                    }
                    return true;
                }
                throw new ArgumentException("Incorrect property map", "cobiePropertyName");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Incorrect property map, " + e.Message);
                Debug.Assert(false);
                return false;
            }
        }




        private void AddProperty(IfcObject ifcObject, IfcValue value, string cobiePropertyName, IfcPropertySet propertySet,
            NamedProperty namedProperty)
        {
            try
            {

                var property = TargetRepository.Instances.New<IfcPropertySingleValue>();
                property.NominalValue = value;
                property.Description = "Converted from COBie " + cobiePropertyName;
                property.Name = namedProperty.PropertyName;
                propertySet.HasProperties.Add(property);
                var relDef = TargetRepository.Instances.New<IfcRelDefinesByProperties>();
                relDef.RelatingPropertyDefinition = propertySet;
                relDef.RelatedObjects.Add(ifcObject);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to convert a COBie Value to and Ifc Single Value Property. " + e.Message);
            }
        }

        private void AddQuantity(AttributeValue valueBaseType, string cobiePropertyName,
            IfcUnitConverter actualUnits, IfcElementQuantity propertySetDefinition, NamedProperty namedProperty)
        {
            try
            {
                var cobieValue = ConvertTo<double>(valueBaseType); //quantities are always doubles
                if (actualUnits.IsUndefined)
                    throw new ArgumentException("Invalid unit type " + actualUnits.UserDefinedSiUnitName +
                                                " has been pass to CreatePropertySingleValue");

                IfcPhysicalQuantity quantity;

                switch (actualUnits.UnitName)
                //they are all here for future proofing, time, mass and count though are not really used by COBie
                {
                    case IfcUnitEnum.AREAUNIT:
                        quantity =
                            TargetRepository.Instances.New<IfcQuantityArea>(
                                q => q.AreaValue = new IfcAreaMeasure(cobieValue));
                        break;
                    case IfcUnitEnum.LENGTHUNIT:
                        quantity =
                            TargetRepository.Instances.New<IfcQuantityLength>(
                                q => q.LengthValue = new IfcLengthMeasure(cobieValue));
                        break;
                    case IfcUnitEnum.MASSUNIT:
                        quantity =
                            TargetRepository.Instances.New<IfcQuantityWeight>(
                                q => q.WeightValue = new IfcMassMeasure(cobieValue));
                        break;
                    case IfcUnitEnum.TIMEUNIT:
                        quantity =
                            TargetRepository.Instances.New<IfcQuantityTime>(
                                q => q.TimeValue = new IfcTimeMeasure(cobieValue));
                        break;
                    case IfcUnitEnum.VOLUMEUNIT:
                        quantity =
                            TargetRepository.Instances.New<IfcQuantityVolume>(
                                q => q.VolumeValue = new IfcVolumeMeasure(cobieValue));
                        break;
                    case IfcUnitEnum.USERDEFINED: //we will treat this as Item for now
                        quantity =
                            TargetRepository.Instances.New<IfcQuantityCount>(
                                q => q.CountValue = new IfcCountMeasure(cobieValue));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                quantity.Description = "Converted from COBie " + cobiePropertyName;
                quantity.Name = namedProperty.PropertyName;
                propertySetDefinition.Quantities.Add(quantity);

            }
            catch (Exception e)
            {
                throw new Exception("Failed to convert a COBie Value to and Ifc Quantity. " + e.Message);
            }
        }


        internal void ConvertAttributeTypeToIfcObjectProperty(IfcObjectDefinition ifcObjectDefinition, Attribute attributeType)
        {
            //need to add in consideration for quantities not just properties
            var ifcSimpleProperty = ConvertAttributeToIfcSimpleProperty(attributeType);
            var propertySet = GetOrCreatePropertySetDefinition(ifcObjectDefinition, attributeType.PropertySetName);
            propertySet.Add(ifcSimpleProperty);
        }

        /// <summary>
        /// Converts an attribute in to an Ifc Property, still needs support for units adding
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        internal IfcSimpleProperty ConvertAttributeToIfcSimpleProperty(Attribute attributeType)
        {
            var attributeValue = attributeType.Value;

            IfcSimpleProperty theProperty;

            var simplePropertyType = attributeValue.SimplePropertyType();
            switch (simplePropertyType)
            {
                case XbimSimplePropertyType.SimpleDecimal:
                case XbimSimplePropertyType.SimpleInteger:
                case XbimSimplePropertyType.SimpleBoolean:
                case XbimSimplePropertyType.SimpleMonetary:
                case XbimSimplePropertyType.SimpleString:
                case XbimSimplePropertyType.SimpleDateTime:
                case XbimSimplePropertyType.Null:
                    theProperty = TargetRepository.Instances.New<IfcPropertySingleValue>();
                    break;
                case XbimSimplePropertyType.BoundedDecimal:
                case XbimSimplePropertyType.BoundedInteger:
                    theProperty = TargetRepository.Instances.New<IfcPropertyBoundedValue>();
                    break;
                case XbimSimplePropertyType.EnumerationString:
                    theProperty = TargetRepository.Instances.New<IfcPropertyEnumeratedValue>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("attributeType", "Invalid attribute value type");
            }

            theProperty.Name = attributeType.Name;
            theProperty.Description = attributeType.Description;

            if (attributeValue != null)
            {
                var simpleProperty = theProperty as IfcPropertySingleValue;
                if (simpleProperty != null)
                {
                    var unitConverter = new IfcUnitConverter(attributeValue.Unit);

                    if (!unitConverter.IsUndefined)
                    {
                        simpleProperty.Unit = unitConverter.IfcUnit(_units, TargetRepository);
                    }
                }
                switch (simplePropertyType)
                {
                    case XbimSimplePropertyType.SimpleDecimal:
                        var decimalValue = attributeValue as DecimalAttributeValue;
                        var simpleDecimalProperty = (IfcPropertySingleValue)theProperty;
                        if (decimalValue != null)
                        {
                            if (decimalValue.Value.HasValue)
                                simpleDecimalProperty.NominalValue = new IfcReal(decimalValue.Value.Value);
                        }
                        break;
                    case XbimSimplePropertyType.BoundedDecimal:
                        var boundedDecimal = attributeValue as DecimalAttributeValue;
                        if (boundedDecimal != null)
                        {
                            var boundedProperty = (IfcPropertyBoundedValue)theProperty;
                            if (boundedDecimal.MaximalValue.HasValue)
                                boundedProperty.UpperBoundValue = new IfcReal(boundedDecimal.MaximalValue.Value);
                            if (boundedDecimal.MinimalValue.HasValue)
                                boundedProperty.LowerBoundValue = new IfcReal(boundedDecimal.MinimalValue.Value);
                        }
                        break;
                    case XbimSimplePropertyType.SimpleInteger:
                        var simpleInteger = attributeValue as IntegerAttributeValue;
                        if (simpleInteger != null && simpleInteger.Value.HasValue)
                        {
                            var simpleIntProperty = (IfcPropertySingleValue)theProperty;
                            simpleIntProperty.NominalValue = new IfcInteger(simpleInteger.Value.Value);
                        }
                        break;
                    case XbimSimplePropertyType.BoundedInteger:
                        var attributeBoundedIntegerValueType = attributeValue as IntegerAttributeValue;
                        if (attributeBoundedIntegerValueType != null)
                        {
                            var boundedIntegerProperty = (IfcPropertyBoundedValue)theProperty;
                            if (attributeBoundedIntegerValueType.MaximalValue.HasValue)
                                boundedIntegerProperty.UpperBoundValue =
                                    new IfcInteger(attributeBoundedIntegerValueType.MaximalValue.Value);
                            if (attributeBoundedIntegerValueType.MinimalValue.HasValue)
                                boundedIntegerProperty.LowerBoundValue =
                                    new IfcInteger(attributeBoundedIntegerValueType.MinimalValue.Value);
                        }
                        break;
                    case XbimSimplePropertyType.SimpleBoolean:
                        var attributeBooleanValueType = attributeValue as BooleanAttributeValue;
                        if (attributeBooleanValueType != null)
                        {
                            var simpleBooleanProperty = (IfcPropertySingleValue)theProperty;
                            if (attributeBooleanValueType.Value.HasValue)
                                simpleBooleanProperty.NominalValue = new IfcBoolean(attributeBooleanValueType.Value.Value);
                        }
                        break;
                    //case XbimSimplePropertyType.SimpleMonetary:
                    //    var attributeMonetaryValueType = attributeValueType as AttributeMonetaryValueType;              
                    //    if (attributeMonetaryValueType != null)
                    //    {
                    //        var simpleMonetaryProperty = (IfcPropertySingleValue) theProperty;
                    //        var monetaryValue = (double) attributeMonetaryValueType.MonetaryValue;
                    //        simpleMonetaryProperty.NominalValue = new IfcReal(monetaryValue);
                    //        IfcCurrencyEnum currencyEnum;
                    //        if (Enum.TryParse(attributeMonetaryValueType.MonetaryUnit.ToString(), true, out currencyEnum))
                    //            simpleMonetaryProperty.Unit = new IfcMonetaryUnit {Currency = currencyEnum};
                    //    }
                    //    break;
                    case XbimSimplePropertyType.EnumerationString:
                        var attributeEnumStringValueType = attributeValue as StringAttributeValue;
                        if (attributeEnumStringValueType != null)
                        {
                            var simpleEnumStringProperty = (IfcPropertyEnumeratedValue)theProperty;
                            simpleEnumStringProperty.EnumerationValues.Add(
                                new IfcLabel(attributeEnumStringValueType.Value));
                            if (attributeEnumStringValueType.AllowedValues != null &&
                                attributeEnumStringValueType.AllowedValues.Any())
                            {
                                simpleEnumStringProperty.EnumerationReference =
                                    TargetRepository.Instances.New<IfcPropertyEnumeration>();
                                foreach (var allowedValue in attributeEnumStringValueType.AllowedValues)
                                {
                                    simpleEnumStringProperty.EnumerationReference.Name = attributeType.Name;
                                    simpleEnumStringProperty.EnumerationReference.EnumerationValues.Add(
                                        new IfcLabel(allowedValue));
                                }
                            }
                        }
                        break;
                    case XbimSimplePropertyType.SimpleString:
                    case XbimSimplePropertyType.Null:
                        var attributeStringValueType = attributeValue as StringAttributeValue;
                        if (attributeStringValueType != null)
                        {
                            var simpleStringProperty = (IfcPropertySingleValue)theProperty;
                            simpleStringProperty.NominalValue = new IfcText(attributeStringValueType.Value);
                        }
                        break;
                    case XbimSimplePropertyType.SimpleDateTime:
                        var attributeDateTimeValueType = attributeValue as DateTimeAttributeValue;
                        if (attributeDateTimeValueType != null && attributeDateTimeValueType.Value.HasValue)
                        {
                            var simpleDateTimeProperty = (IfcPropertySingleValue)theProperty;
                            simpleDateTimeProperty.NominalValue = IfcTimeStamp.ToTimeStamp(attributeDateTimeValueType.Value.Value);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("attributeType", "Invalid attribute value type");
                }
            }
            return theProperty;
        }

        /// <summary>
        /// Creates a number in ifc geometry representing the element, the next number is incremented
        /// </summary>
        /// <param name="ifcElement"></param>
        /// <param name="index">The index of the element in the type collection</param>
        internal void CreateObjectGeometry(IfcElement ifcElement, int index)
        {
            var rectProfile = TargetRepository.Instances.New<IfcRectangleProfileDef>();
            var dims = TargetRepository.ModelFactors.OneMilliMeter * 100;
            rectProfile.Position = MakeAxis2Placement2D();
            rectProfile.XDim = dims;
            rectProfile.YDim = dims;
            rectProfile.ProfileType = IfcProfileTypeEnum.AREA;
            //extrude it
            var extrusion = TargetRepository.Instances.New<IfcExtrudedAreaSolid>();
            extrusion.Depth = dims;
            extrusion.ExtrudedDirection = _downDirection;
            extrusion.Position = MakeAxis2Placement3D();
            extrusion.SweptArea = rectProfile;
            //locate it
            ifcElement.ObjectPlacement = MakeLocalPlacement(_assetTypeInfoTypeNumber * dims * 1.2, 0, index * dims * 1.2);
            ifcElement.Representation = MakeSweptSolidRepresentation(extrusion);
        }

        private IfcProductRepresentation MakeSweptSolidRepresentation(IfcExtrudedAreaSolid extrusion)
        {
            //Create a Definition shape to hold the geometry
            var shape = TargetRepository.Instances.New<IfcShapeRepresentation>();
            shape.ContextOfItems = Model3DContext;
            shape.RepresentationType = "SweptSolid";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(extrusion);

            //Create a Product Definition and add the model geometry to the wall
            var rep = TargetRepository.Instances.New<IfcProductDefinitionShape>();
            rep.Representations.Add(shape);
            return rep;
        }

        private IfcLocalPlacement MakeLocalPlacement(double xDisplacement, double yDisplacement, double zDisplacement)
        {
            var localPlacement = TargetRepository.Instances.New<IfcLocalPlacement>();
            var axisPlacement3D = MakeAxis2Placement3D();
            localPlacement.RelativePlacement = axisPlacement3D;
            axisPlacement3D.Location = TargetRepository.Instances.New<IfcCartesianPoint>(c => c.SetXYZ(xDisplacement, yDisplacement, zDisplacement));
            return localPlacement;
        }

        private IfcAxis2Placement2D MakeAxis2Placement2D()
        {
            var p = TargetRepository.Instances.New<IfcAxis2Placement2D>();
            p.Location = _origin2D;
            return p;
        }

        private IfcAxis2Placement3D MakeAxis2Placement3D()
        {
            var p = TargetRepository.Instances.New<IfcAxis2Placement3D>();
            p.Location = _origin3D;
            return p;
        }

        /// <summary>
        /// Increments counters and state for processing an AssetTypeInfoType
        /// </summary>
        internal void BeginAssetTypeInfoType()
        {

        }

        internal void EndAssetTypeInfoType()
        {
            _assetTypeInfoTypeNumber++;
        }

        /// <summary>
        /// Adds the space to the space map if the storey name plus the space name is not unique returns false
        /// </summary>
        /// <param name="space"></param>
        /// <returns></returns>
        public bool AddToSpaceMap(IfcSpatialStructureElement space)
        {
            if (!space.Name.HasValue) return false;
            string key = space.Name;
            if (_spaceLookup.ContainsKey(key)) return false;
            _spaceLookup.Add(key, space);
            return true;
        }

        /// <summary>
        /// Returns the IfcSpace created for this key
        /// </summary>
        /// <param name="spacekey"></param>
        /// <returns></returns>
        public IfcSpatialStructureElement GetIfcSpace(SpaceKey spacekey)
        {
            IfcSpatialStructureElement ifcSpace;
            _spaceLookup.TryGetValue(spacekey.Name, out ifcSpace);
            return ifcSpace;
        }

        /// <summary>
        /// Converts a category to a classification
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ifcElement"></param>
        public void ConvertCategoryToClassification(Category category, IfcRoot ifcElement)
        {
            if (String.IsNullOrEmpty(category.Classification) && string.IsNullOrEmpty(category.Code))
                return;

            IfcClassification classificationSystem;
            if (String.IsNullOrEmpty(category.Classification) ||
                !_classificationSystems.TryGetValue(category.Classification, out classificationSystem))
            {
                classificationSystem = TargetRepository.Instances.New<IfcClassification>();
                classificationSystem.Name = category.Classification;
            }
            IfcClassificationReference classificationReference;
            if (String.IsNullOrEmpty(category.Code) ||
                !_classificationReferences.TryGetValue(category.Code, out classificationReference))
            {
                classificationReference = TargetRepository.Instances.New<IfcClassificationReference>();
                classificationReference.ItemReference = category.Code;
                classificationReference.Name = category.Description;
                classificationReference.ReferencedSource = classificationSystem;
            }

            IfcRelAssociatesClassification relationship;

            if (!_classificationRelationships.TryGetValue(classificationReference, out relationship))
            {
                relationship = TargetRepository.Instances.New<IfcRelAssociatesClassification>();
                relationship.RelatingClassification = classificationReference;
                relationship.Name = category.Code;
                relationship.Description = category.Description;
            }

            relationship.RelatedObjects.Add(ifcElement);
        }


        /// <summary>
        /// Converts an attribute to the required type if possible
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public TType ConvertTo<TType>(AttributeValue attributeValue)
        {
            var decimalType = attributeValue as DecimalAttributeValue;
            if (decimalType != null && decimalType.Value.HasValue)
                return (TType)SystemConvert.ChangeType(decimalType.Value.Value, typeof(TType));
            var stringType = attributeValue as StringAttributeValue;
            if (stringType != null && !string.IsNullOrWhiteSpace(stringType.Value))
                return (TType)SystemConvert.ChangeType(stringType.Value, typeof(TType));
            var integerType = attributeValue as IntegerAttributeValue;
            if (integerType != null)
                return (TType)SystemConvert.ChangeType(integerType.Value, typeof(TType));
            var booleanType = attributeValue as BooleanAttributeValue;
            if (booleanType != null)
                return (TType)SystemConvert.ChangeType(booleanType.Value, typeof(TType));
            return default(TType);
        }

        private IfcValue ConvertToIfcValue(AttributeValue valueBaseType)
        {
            var decimalType = valueBaseType as DecimalAttributeValue;
            if (decimalType != null && decimalType.Value.HasValue)
                return new IfcReal((double)SystemConvert.ChangeType(decimalType.Value.Value, typeof(double)));
            var stringType = valueBaseType as StringAttributeValue;
            if (stringType != null && !string.IsNullOrEmpty(stringType.Value))
                return new IfcText((string)SystemConvert.ChangeType(stringType.Value, typeof(string)));
            var integerType = valueBaseType as IntegerAttributeValue;
            if (integerType != null)
                return new IfcInteger((int)SystemConvert.ChangeType(integerType.Value, typeof(int)));
            var booleanType = valueBaseType as BooleanAttributeValue;
            if (booleanType != null)
                return new IfcBoolean((bool)SystemConvert.ChangeType(booleanType.Value, typeof(bool)));
            return default(IfcText);
        }


        internal void AddSpaceToZone(SpaceKey spaceKey, IfcZone ifcZone)
        {
            var relationship = TargetRepository.Instances.OfType<IfcRelAssignsToGroup>().FirstOrDefault(r => r.RelatingGroup == ifcZone);
            if (relationship == null)
            {
                relationship = TargetRepository.Instances.New<IfcRelAssignsToGroup>();
                relationship.RelatingGroup = ifcZone;
            }
            var ifcSpace = GetIfcSpace(spaceKey);
            relationship.RelatedObjects.Add(ifcSpace);
            relationship.RelatedObjectsType = IfcObjectTypeEnum.PRODUCT;
        }

        public bool StringHasValue(string createdOn)
        {
            return !(string.IsNullOrWhiteSpace(createdOn) || createdOn.Equals("unknown", StringComparison.OrdinalIgnoreCase) || createdOn.Equals("n/a", StringComparison.OrdinalIgnoreCase) || createdOn.Equals("n\a", StringComparison.OrdinalIgnoreCase));
        }


        #region OwnerHistory
        
        /// <summary>
        /// Set an existing or create a new owner history if no match is found
        /// </summary>
        /// <param name="ifcRoot">Object to add the owner history</param>
        /// <param name="externalSystem">Application used to modify/create</param>
        /// <param name="createdBy">IfcPersonAndOrganization object</param>
        /// <param name="createdOn">Date the object was created on</param>
        protected void SetOwnerHistory(IfcRoot ifcRoot, string externalSystem, string createdBy, DateTime createdOn)
        {
            try
            {
                IfcTimeStamp stamp = IfcTimeStamp.ToTimeStamp(createdOn);
            }
            catch (OverflowException)
            {
                IfcTimeStamp stamp = 0; //junk date passed, so set to 0 (1970) see http://www.buildingsmart-tech.org/ifc/IFC2x3/TC1/html/ifcmeasureresource/lexical/ifctimestamp.htm
            }
            catch(Exception)
            {
                throw;
            }
            if (OwnerHistories == null)
            {
                OwnerHistories = TargetRepository.Instances.OfType<IfcOwnerHistory>().Where(oh => (oh.CreationDate != null)
                                                                           && (
                                                                                (oh.OwningUser.ThePerson != null) && (oh.OwningUser.ThePerson.Addresses != null) && oh.OwningUser.ThePerson.Addresses.OfType<IfcTelecomAddress>().Any()
                                                                              )
                                                                           && (
                                                                                ((oh.LastModifyingApplication != null) && (oh.LastModifyingApplication.ApplicationFullName != null) )
                                                                                || ((oh.LastModifyingApplication == null) && (oh.OwningApplication != null) && (oh.OwningApplication.ApplicationFullName != null))
                                                                              )
                                                                           )
                                                                           .ToDictionary(oh => new OwnerHistoryKey { CreatedBy = oh.OwningUser.ThePerson.Addresses.OfType<IfcTelecomAddress>().First().ElectronicMailAddresses.First(),
                                                                            CreatedOn = (oh.CreationDate != null) ? IfcTimeStamp.ToDateTime(oh.CreationDate).ToString() : string.Empty,
                                                                            ExtSystem = (oh.LastModifyingApplication == null) ? oh.OwningApplication.ApplicationFullName : oh.LastModifyingApplication.ApplicationFullName
                                                                           }, oh => oh );
            }
           
            var key = new OwnerHistoryKey { ExtSystem = externalSystem, CreatedBy = createdBy, CreatedOn = (createdOn == null) ? string.Empty : createdOn.ToString() };
            if (OwnerHistories.ContainsKey(key))
            {
                ifcRoot.OwnerHistory = OwnerHistories[key];
            }
            else
            {
               ifcRoot.OwnerHistory = CreateOwnerHistory(externalSystem, SetEmailUser(createdBy), createdOn);
                OwnerHistories.Add(key, ifcRoot.OwnerHistory);
            }
        }
        /// <summary>
        /// Create the owner history
        /// </summary>
        /// <param name="ifcRoot">Object to add the owner history</param>
        /// <param name="externalSystem">Application used to modify/create</param>
        /// <param name="createdBy">IfcPersonAndOrganization object</param>
        /// <param name="createdOn">Date the object was created on</param>
        protected IfcOwnerHistory CreateOwnerHistory(string externalSystem, IfcPersonAndOrganization createdBy, DateTime createdOn)
        {
            IfcApplication ifcApplication = null;
            IfcOrganization ifcOrganization = null;
            string orgName       = (StringHasValue(externalSystem)) ? externalSystem.Split(' ').FirstOrDefault() : "Unknown";
            string appIdentifier = (StringHasValue(externalSystem)) ? externalSystem.Replace(' ', '_') : "Unknown";
            string appName       = (StringHasValue(externalSystem)) ? externalSystem : "Unknown";

            //create an organization for the external system
            ifcOrganization = TargetRepository.Instances.OfType<IfcOrganization>().Where(o => o.Name == orgName).FirstOrDefault();
            if (ifcOrganization == null)
            {
                ifcOrganization = TargetRepository.Instances.New<IfcOrganization>(org => { org.Name = orgName; });
            }
            //create application
            ifcApplication = TargetRepository.Instances.OfType<IfcApplication>().Where(app => app.ApplicationFullName == appName).FirstOrDefault();
            if (ifcApplication == null)
            {
                ifcApplication = TargetRepository.Instances.New<IfcApplication>(app =>
                {
                    app.ApplicationFullName   = appName;
                    app.ApplicationDeveloper  = ifcOrganization;
                    app.Version               = new IfcLabel("");
                    app.ApplicationIdentifier = new IfcIdentifier(appIdentifier);
                });
            }
            //return new owner history
            IfcTimeStamp stamp;
            try
            {
                stamp = IfcTimeStamp.ToTimeStamp(createdOn);
            }
            catch (OverflowException)
            {
                stamp = 0; //junk date passed, so set to 0 (1970) see http://www.buildingsmart-tech.org/ifc/IFC2x3/TC1/html/ifcmeasureresource/lexical/ifctimestamp.htm
            }
            catch (Exception)
            {
                throw;
            }
            return TargetRepository.Instances.New<IfcOwnerHistory>(oh =>
             {
                 oh.OwningUser               = createdBy;
                 oh.OwningApplication        = ifcApplication;
                 oh.LastModifyingApplication = ifcApplication;
                 oh.CreationDate             = stamp;
                 oh.ChangeAction             = IfcChangeActionEnum.NOCHANGE;
             });
        }

        /// <summary>
        /// Set the user history to a IfcRoot object
        /// </summary>
        /// <param name="ifcRoot">Object to set owner history</param>
        /// <param name="extSystem">External system string</param>
        /// <param name="createdBy">Email address of creator</param>
        /// <param name="createdOn">Created on date as string</param>
        public void SetUserHistory(IfcRoot ifcRoot, string extSystem, string createdBy, DateTime createdOn)
        {
            SetContacts();
            SetOwnerHistory(ifcRoot, extSystem, createdBy, createdOn);
        }

        private void SetContacts()
        {
            if (Contacts == null)
            {
                Contacts = TargetRepository.Instances.OfType<IfcPersonAndOrganization>()
                            .Where(po => po.ThePerson != null && po.ThePerson.Addresses != null
                                    && po.ThePerson.Addresses.OfType<IfcTelecomAddress>().Any()
                                    && po.ThePerson.Addresses.OfType<IfcTelecomAddress>().First().ElectronicMailAddresses.Any(e => !string.IsNullOrWhiteSpace(e)
                                    )
                                    )
                            .ToDictionary(po => po.ThePerson.Addresses.OfType<IfcTelecomAddress>().First().ElectronicMailAddresses, po => po)
                            .SelectMany(pair => pair.Key.Select(val => new { Key = val, Value = pair.Value }))
                            .ToDictionary(o => o.Key.ToString(), o => o.Value);
            }
        }

        /// <summary>
        /// Set the IfcPersonAndOrganization from using email only
        /// </summary>
        /// <param name="email">email</param>
        internal IfcPersonAndOrganization SetEmailUser(string email)
        {
            SetContacts();
            //check we have a valid string, if not set to default
            email = StringHasValue(email) ? email : "unknown@undefined.email";

            if (!Contacts.ContainsKey(email)) //should filter on merge also unless Contacts is reset
            {
                IfcPerson ifcPerson = TargetRepository.Instances.New<IfcPerson>();
                IfcOrganization ifcOrganization = TargetRepository.Instances.New<IfcOrganization>();
                IfcPersonAndOrganization ifcPersonAndOrganization = TargetRepository.Instances.New<IfcPersonAndOrganization>();

                Contacts.Add(email, ifcPersonAndOrganization); //build a list to reference for History objects

                //add Company
                ifcOrganization.Name = "Unknown"; //is not an optional field so fill with unknown value

                //create IfcTelecomAddress
                IfcTelecomAddress ifcTelecomAddress = TargetRepository.Instances.New<IfcTelecomAddress>();
                
                ifcTelecomAddress.ElectronicMailAddresses.Add(email); //add to existing collection
                
                //add Telecom Address
               
                ifcPerson.Addresses.Add(ifcTelecomAddress);//add to existing collection

                //add postal address
                //IfcPostalAddress ifcPostalAddress = TargetRepository.Instances.New<IfcPostalAddress>();                                               
                //if (ifcPerson.Addresses == null)
                //    ifcPerson.SetPostalAddresss(ifcPostalAddress);//create the AddressCollection and set to Addresses field
                //else
                //    ifcPerson.Addresses.Add(ifcPostalAddress);//add to existing collection

                //add the person and the organization objects 
                ifcPersonAndOrganization.ThePerson = ifcPerson;
                ifcPersonAndOrganization.TheOrganization = ifcOrganization;

                
                return ifcPersonAndOrganization;
            }
            else
            {
                return Contacts[email];
            }

        }
        #endregion

        #region Document Conversion

        /// <summary>
        /// Convert/Import a Document List into the model repository 
        /// </summary>
        /// <param name="ifcRootObj">IfcRoot object to link the document list too</param>
        /// <param name="docs">List of document to convert and import into the model</param>
        internal void ConvertDocumentsToDocumentSelect (IfcRoot ifcRootObj, List<Document> docs)
        {
            var documantMapping = GetOrCreateMappings<MappingDocumentToIfcDocumentSelect>();
            foreach (var doc in docs)
            {
                var ifcDocumentInformation = TargetRepository.Instances.New<IfcDocumentInformation>();
                ifcDocumentInformation = documantMapping.AddMapping(doc, ifcDocumentInformation);

                IfcRelAssociatesDocument ifcRelAssociatesDocument = TargetRepository.Instances.New<IfcRelAssociatesDocument>();
                SetUserHistory(ifcRelAssociatesDocument, doc.ExternalSystem, (doc.CreatedBy == null) ? null : doc.CreatedBy.Email, (doc.CreatedOn == null) ? DateTime.Now : (DateTime)doc.CreatedOn);

                //using statement will set the Model.OwnerHistoryAddObject to IfcConstructionProductResource.OwnerHistory as OwnerHistoryAddObject is used to reset user history on any property changes, 
                //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement to keep user history set in line above SetUserHistory
                using (OwnerHistoryEditScope context = new OwnerHistoryEditScope(TargetRepository, ifcRelAssociatesDocument.OwnerHistory))
                {
                    ifcRelAssociatesDocument.RelatedObjects.Add(ifcRootObj);
                    ifcRelAssociatesDocument.RelatingDocument = ifcDocumentInformation;
                    ifcRelAssociatesDocument.Name = doc.Name;
                    ifcRelAssociatesDocument.Description = doc.Description;
                }
            }
        }

        #endregion
    }
}
