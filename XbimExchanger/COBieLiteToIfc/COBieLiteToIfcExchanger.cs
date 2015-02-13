using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xbim.COBieLite;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.QuantityResource;
using Xbim.IO;
using Xbim.XbimExtensions.SelectTypes;
using XbimExchanger.COBieLiteHelpers;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    public class CoBieLiteToIfcExchanger : XbimExchanger<FacilityType, XbimModel>
    {
        #region Nested Structures
        public struct NamedProperty
        {
            public string PropertySetName;
            public string PropertyName;

            public NamedProperty(string propertySetName, string propertyName )
                : this()
            {
                PropertyName = propertyName;
                PropertySetName = propertySetName;
            }
        }
        #endregion

        #region Static members and functions
        static readonly IDictionary<string, NamedProperty> CobieToIfcPropertyMap = new Dictionary<string, NamedProperty>();


        static CoBieLiteToIfcExchanger()
        {

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = "COBieAttributes.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var cobiePropertyMaps = (AppSettingsSection)config.GetSection("COBiePropertyMaps");

            foreach (KeyValueConfigurationElement keyVal in cobiePropertyMaps.Settings)
            {
                var values = keyVal.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var selected = values.FirstOrDefault();
                if (selected != null)
                {
                    var names = selected.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (names.Length == 2)
                        CobieToIfcPropertyMap.Add(keyVal.Key, new NamedProperty(names[0], names[1]));
                }
            }

        }
        #endregion

        #region Fields

        private readonly Dictionary<IfcObject, List<IfcPropertySetDefinition>> _objectsToPropertySets = new Dictionary<IfcObject, List<IfcPropertySetDefinition>>();

       // private readonly Dictionary<string, IfcUnit> _units = new Dictionary<string, IfcUnit>();

        #endregion

        #region Properties

        public IfcUnitConverter? DefaultLinearUnit;
        public IfcUnitConverter? DefaultAreaUnit;
        public IfcUnitConverter? DefaultVolumeUnit;
        public CurrencyUnitSimpleType? DefaultCurrencyUnit;

        #endregion

        #region Constructors
        public CoBieLiteToIfcExchanger(FacilityType facility, XbimModel repository)
            : base(facility, repository)
        {
            LoadPropertySetDefinitions();
            LoadUnits();
        }

       
        #endregion

        #region Converters
        public IfcBuilding Convert(FacilityType facility)
        {
            var mapping = GetOrCreateMappings<MappingFacilityTypeToIfcBuilding>();
            var building = mapping.GetOrCreateTargetObject(facility.externalID);
            return mapping.AddMapping(facility, building);

        }

        public override XbimModel Convert()
        {
            Convert(SourceRepository);
            return TargetRepository;
        }
        #endregion

        #region Methods

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
                return false;
            }
        }

        /// <summary>
        /// Creates the property and if required the property set, populates them with the correct values and adds them to the IfcObject
        /// If the value is null or empty no property is created
        /// </summary>
        /// <param name="ifcObject">Object to associate the property with</param>
        /// <param name="valueBaseType">COBie value to populate the property with</param>
        /// <param name="cobiePropertyName">Name of the COBie property being mapped</param>
        /// <param name="defaultUnits">Units to use if the COBie property does not specify</param>
        internal bool TryCreatePropertySingleValue(IfcObject ifcObject, ValueBaseType valueBaseType, string cobiePropertyName, IfcUnitConverter? defaultUnits)
        {
            if (!valueBaseType.HasValue()) return false; //nothing to do
            try
            {
                NamedProperty namedProperty;
                if (CobieToIfcPropertyMap.TryGetValue(cobiePropertyName, out namedProperty))
                {
                    var actualUnits = new IfcUnitConverter(valueBaseType.UnitName);
                    if (actualUnits.IsUndefined && defaultUnits.HasValue) actualUnits = defaultUnits.Value;
                    List<IfcPropertySetDefinition> propertySetDefinitionList;
                    if (!_objectsToPropertySets.TryGetValue(ifcObject, out propertySetDefinitionList))
                    {
                          propertySetDefinitionList = new List<IfcPropertySetDefinition>();
                        _objectsToPropertySets.Add(ifcObject, propertySetDefinitionList);
                    }
                    var propertySetDef = propertySetDefinitionList.Find(p => p.Name == namedProperty.PropertySetName);
                        //see what sets we have against this object
                    if (propertySetDef==null)
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
                            AddProperty(ifcObject, valueBaseType.ConvertToIfcValue(), cobiePropertyName,
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

                            AddProperty(ifcObject, valueBaseType.ConvertToIfcValue(), cobiePropertyName, (IfcPropertySet)propertySetDef,
                                namedProperty);
                    }
                    return true;
                }
                throw new ArgumentException("Incorrect property map","cobiePropertyName");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Incorrect property map, "+e.Message);
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

        private void AddQuantity(ValueBaseType valueBaseType, string cobiePropertyName,
            IfcUnitConverter actualUnits, IfcElementQuantity propertySetDefinition, NamedProperty namedProperty)
        {
            try
            {
                var cobieValue = valueBaseType.ConvertTo<double>(); //quantities are always doubles
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

        /// <summary>
        /// Converts an attribute in to an Ifc Property, still needs support for units adding
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        internal IfcSimpleProperty ConvertAttributeTypeToIfcSimpleProperty(AttributeType attributeType)
        {
            var attributeValueType = attributeType.AttributeValue;
            if (attributeValueType == null) throw new Exception("AttributeType has a null AttributeValue");

            switch (attributeValueType.SimplePropertyType())
            {
                case XbimSimplePropertyType.SimpleDecimal:
                    var attributeDecimalValueType = attributeValueType.Item as AttributeDecimalValueType;
                    double? decimalValue;
                    if (attributeDecimalValueType != null && attributeDecimalValueType.DecimalValueSpecified)
                        decimalValue = attributeDecimalValueType.DecimalValue;
                    else decimalValue = null;
                    var simpleProperty = TargetRepository.Instances.New<IfcPropertySingleValue>();
                    if (decimalValue.HasValue) simpleProperty.NominalValue = new IfcReal(decimalValue.Value);
                    simpleProperty.Name = attributeType.AttributeName;
                    simpleProperty.Description = attributeType.AttributeDescription;
                    //if (attributeDecimalValueType != null)
                    //{
                    //    var unitConverter = new IfcUnitConverter(attributeDecimalValueType.UnitName);
                    //    if (!unitConverter.IsUndefined)
                    //        simpleProperty.Unit = unitConverter.IfcUnit(TargetRepository);
                    //}
                    return simpleProperty;
                case XbimSimplePropertyType.BoundedDecimal:
                    var attributeBoundedDecimalValueType = attributeValueType.Item as AttributeDecimalValueType;
                    double? maxDecimalValue;
                    double? minDecimalValue;
                    if (attributeBoundedDecimalValueType != null &&
                        attributeBoundedDecimalValueType.MaxValueDecimalSpecified)
                        maxDecimalValue = attributeBoundedDecimalValueType.MaxValueDecimal;
                    else maxDecimalValue = null;
                    if (attributeBoundedDecimalValueType != null &&
                        attributeBoundedDecimalValueType.MinValueDecimalSpecified)
                        minDecimalValue = attributeBoundedDecimalValueType.MinValueDecimal;
                    else minDecimalValue = null;
                    var boundedProperty = TargetRepository.Instances.New<IfcPropertyBoundedValue>();
                    if (maxDecimalValue.HasValue)
                        boundedProperty.UpperBoundValue = new IfcReal(maxDecimalValue.Value);
                    if (minDecimalValue.HasValue)
                        boundedProperty.LowerBoundValue = new IfcReal(minDecimalValue.Value);
                    boundedProperty.Name = attributeType.AttributeName;
                    boundedProperty.Description = attributeType.AttributeDescription;
                    return boundedProperty;
                case XbimSimplePropertyType.SimpleInteger:
                    var attributeSimpleIntegerValueType = attributeValueType.Item as AttributeIntegerValueType;
                    var simpleIntProperty = TargetRepository.Instances.New<IfcPropertySingleValue>();
                    if (attributeSimpleIntegerValueType != null && attributeSimpleIntegerValueType.IntegerValue.HasValue)
                        simpleIntProperty.NominalValue = new IfcInteger(attributeSimpleIntegerValueType.IntegerValue.Value);
                    simpleIntProperty.Name = attributeType.AttributeName;
                    simpleIntProperty.Description = attributeType.AttributeDescription;
                    return simpleIntProperty;
                case XbimSimplePropertyType.BoundedInteger:
                    var attributeBoundedIntegerValueType = attributeValueType.Item as AttributeIntegerValueType;
                    var boundedIntegerProperty = TargetRepository.Instances.New<IfcPropertyBoundedValue>();
                    if (attributeBoundedIntegerValueType != null)
                    {
                        if (attributeBoundedIntegerValueType.MaxValueInteger.HasValue)
                        boundedIntegerProperty.UpperBoundValue =
                            new IfcInteger(attributeBoundedIntegerValueType.MaxValueInteger.Value);
                        if (attributeBoundedIntegerValueType.MinValueInteger.HasValue) 
                            boundedIntegerProperty.LowerBoundValue =
                            new IfcInteger(attributeBoundedIntegerValueType.MinValueInteger.Value);
                    }
                    boundedIntegerProperty.Name = attributeType.AttributeName;
                    boundedIntegerProperty.Description = attributeType.AttributeDescription;
                    return boundedIntegerProperty;
                case XbimSimplePropertyType.SimpleBoolean:
                    var attributeBooleanValueType = attributeValueType.Item as BooleanValueType;
                    var simpleBooleanProperty = TargetRepository.Instances.New<IfcPropertySingleValue>();
                    if (attributeBooleanValueType != null && attributeBooleanValueType.BooleanValueSpecified)
                        simpleBooleanProperty.NominalValue = new IfcBoolean(attributeBooleanValueType.BooleanValue);
                    simpleBooleanProperty.Name = attributeType.AttributeName;
                    simpleBooleanProperty.Description = attributeType.AttributeDescription;
                    return simpleBooleanProperty;
                case XbimSimplePropertyType.SimpleMonetary:
                    var attributeMonetaryValueType = attributeValueType.Item as AttributeMonetaryValueType;
                    var simpleMonetaryProperty = TargetRepository.Instances.New<IfcPropertySingleValue>();
                    if (attributeMonetaryValueType != null)
                    {
                        var monetaryValue = (double)attributeMonetaryValueType.MonetaryValue;
                        simpleMonetaryProperty.NominalValue = new IfcReal(monetaryValue);
                        IfcCurrencyEnum currencyEnum;
                        if (Enum.TryParse(attributeMonetaryValueType.MonetaryUnit.ToString(), true, out currencyEnum))
                            simpleMonetaryProperty.Unit = new IfcMonetaryUnit { Currency = currencyEnum };
                    }
                    simpleMonetaryProperty.Name = attributeType.AttributeName;
                    simpleMonetaryProperty.Description = attributeType.AttributeDescription;
                    return simpleMonetaryProperty;
                case XbimSimplePropertyType.EnumerationString:
                    var attributeEnumStringValueType = attributeValueType.Item as AttributeStringValueType;
                    var simpleEnumStringProperty = TargetRepository.Instances.New<IfcPropertyEnumeratedValue>();
                    if (attributeEnumStringValueType != null)
                    {
                        simpleEnumStringProperty.EnumerationValues.Add(
                            new IfcLabel(attributeEnumStringValueType.StringValue));
                        if (attributeEnumStringValueType.AllowedValues != null &&
                            attributeEnumStringValueType.AllowedValues.Any())
                        {
                            simpleEnumStringProperty.EnumerationReference =
                                TargetRepository.Instances.New<IfcPropertyEnumeration>();
                            foreach (var allowedValue in attributeEnumStringValueType.AllowedValues)
                            {
                                simpleEnumStringProperty.EnumerationReference.Name = attributeType.AttributeName;
                                simpleEnumStringProperty.EnumerationReference.EnumerationValues.Add(
                                    new IfcLabel(allowedValue));
                            }
                        }
                    }
                    simpleEnumStringProperty.Name = attributeType.AttributeName;
                    simpleEnumStringProperty.Description = attributeType.AttributeDescription;
                    return simpleEnumStringProperty;
                case XbimSimplePropertyType.SimpleString:
                    var attributeStringValueType = attributeValueType.Item as AttributeStringValueType;
                    var simpleStringProperty = TargetRepository.Instances.New<IfcPropertySingleValue>();
                    if (attributeStringValueType != null)
                        simpleStringProperty.NominalValue = new IfcText(attributeStringValueType.StringValue);
                    simpleStringProperty.Name = attributeType.AttributeName;
                    simpleStringProperty.Description = attributeType.AttributeDescription;
                    return simpleStringProperty;
                case XbimSimplePropertyType.SimpleDateTime:
                    var simpleDateTimeProperty = TargetRepository.Instances.New<IfcPropertySingleValue>();
                    if (attributeValueType.Item is DateTime)
                    {
                        var attributeDateTimeValueType = (DateTime)attributeValueType.Item;
                        simpleDateTimeProperty.NominalValue = IfcTimeStamp.ToTimeStamp(attributeDateTimeValueType);
                    }
                    simpleDateTimeProperty.Name = attributeType.AttributeName;
                    simpleDateTimeProperty.Description = attributeType.AttributeDescription;
                    return simpleDateTimeProperty;
                default:
                    throw new ArgumentOutOfRangeException("attributeType", "Invalid attribute value type");
            }
        }
    }
}
