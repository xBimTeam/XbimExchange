using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Common;
using Xbim.Ifc4.DateTimeResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace XbimExchanger.IfcToCOBieLiteUK
{
   
    /// <summary>
    /// 
    /// </summary>
    public class XbimAttributedObject
    {
        private readonly IIfcObjectDefinition _ifcObject;
        private Dictionary<string, IIfcProperty> _properties = new Dictionary<string, IIfcProperty>();
        private Dictionary<string, IIfcPhysicalQuantity> _quantities = new Dictionary<string, IIfcPhysicalQuantity>();
        private Dictionary<string, IIfcPropertySetDefinition> _propertySets=new Dictionary<string, IIfcPropertySetDefinition>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public XbimAttributedObject(IIfcObjectDefinition obj)
        {
            _ifcObject = obj;
        }

        /// <summary />
        public IIfcObjectDefinition IfcObject
        {
            get { return _ifcObject; }
        }

        public void AddPropertySetDefinition(IIfcPropertySetDefinitionSelect pSetDefSelect)
        {
            foreach (var pSetDef in pSetDefSelect.PropertySetDefinitions)
            {
                AddPropertySetDefinition(pSetDef);
            }

        }
        /// <summary />
        /// <param name="pSetDef"></param>
        public void AddPropertySetDefinition(IIfcPropertySetDefinition pSetDef)
        {
            if (!pSetDef.Name.HasValue || string.IsNullOrWhiteSpace(pSetDef.Name))
            {
                CoBieLiteUkHelper.Logger.WarnFormat("Property Set Definition: #{0}, has no defined name. It has been ignored", pSetDef.EntityLabel);
                return;
            }
            if ( _propertySets.ContainsKey(pSetDef.Name))
            {
                CoBieLiteUkHelper.Logger.WarnFormat("Property Set Definition: #{0}={1}, is duplicated in Entity #{2}={3}. Duplicate ignored", pSetDef.EntityLabel, pSetDef.Name, _ifcObject.EntityLabel, _ifcObject.GetType().Name);
                return;
            }
            _propertySets.Add(pSetDef.Name,pSetDef);
            var propertySet = pSetDef as IIfcPropertySet;
            var quantitySet = pSetDef as IIfcElementQuantity;
            if (propertySet != null)
            {
                foreach (var prop in propertySet.HasProperties)
                {
                    var uniquePropertyName = pSetDef.Name + "." + prop.Name;
                    if (_properties.ContainsKey(uniquePropertyName))
                    {
                        CoBieLiteUkHelper.Logger.WarnFormat("Property: #{0}={1}.{2}, is duplicated in Entity #{3}={4}. Duplicate ignored", prop.EntityLabel, pSetDef.Name, prop.Name, _ifcObject.EntityLabel, _ifcObject.GetType().Name);
                        continue;
                    }
                    _properties[uniquePropertyName] = prop;
                }
            }
            else if (quantitySet != null)
            {
                foreach (var quantity in quantitySet.Quantities)
                {
                    if (_quantities.ContainsKey(pSetDef.Name + "." + quantity.Name))
                    {
                        CoBieLiteUkHelper.Logger.WarnFormat("Quantity: #{0}={1}.{2}, is duplicated in Entity #{3}={4}. Duplicate ignored", quantity.EntityLabel, pSetDef.Name, quantity.Name, _ifcObject.EntityLabel, _ifcObject.GetType().Name);
                        continue;
                    }
                    _quantities[pSetDef.Name + "." + quantity.Name]= quantity;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, IIfcPhysicalQuantity> Quantities
        {
            get { return _quantities; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, IIfcProperty> Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSetName"></param>
        /// <returns></returns>
        public IIfcPropertySetDefinition GetPropertySetDefinition(string pSetName)
        {
            IIfcPropertySetDefinition pSetDef;
            if (_propertySets.TryGetValue(pSetName, out pSetDef))
                return pSetDef;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public bool GetSimplePropertyValue<TValue>(string propertyName, out TValue val) where TValue : struct
        {
            IIfcProperty ifcProperty;
            val = default(TValue);
            if (_properties.TryGetValue(propertyName, out ifcProperty))
            {
                val = ConvertToSimpleType<TValue>(ifcProperty);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool GetSimplePropertyValue(string propertyName, out string val)
        {
            IIfcProperty ifcProperty;
            val = null;
            if (_properties.TryGetValue(propertyName, out ifcProperty))
            {
                val = ConvertToString(ifcProperty);
                return true;
            }
            return false;

        }

        private string ConvertToString(IIfcPropertySingleValue ifcProperty)
        {
            IIfcValue ifcValue = ifcProperty.NominalValue;
            if (ifcValue == null) return null;
            if (ifcValue is IfcTimeStamp)
            {
                var timeStamp = (IfcTimeStamp)ifcValue;
                return WriteDateTime(timeStamp.ToDateTime());
            }
            var expressVal = (IExpressValueType)ifcValue ;
            if (expressVal.UnderlyingSystemType == typeof(bool) || expressVal.UnderlyingSystemType == typeof(bool?))
            {
                if (expressVal.Value != null && (bool)expressVal.Value)
                    return "yes";
                return "no";
            }
            // all other cases will convert to a string
            if (expressVal.Value != null)
            {
                return expressVal.Value.ToString();
            }
            else
            {
                return null;
            }
            
        }

        private string WriteDateTime(DateTime dateTime)
        {
            if (dateTime.TimeOfDay == new TimeSpan(0)) //it is a date only
                return string.Format("{0:0000}-{1:00}-{2:00}", dateTime.Year, dateTime.Month, dateTime.Day);
            return string.Format("{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}{6}", dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind == DateTimeKind.Utc ? "Z" : "");
        }

        private string ConvertToString(IIfcProperty ifcProperty)
        {
            var ifcPropertySingleValue = ifcProperty as IIfcPropertySingleValue;
            var ifcPropertyEnumeratedValue = ifcProperty as IIfcPropertyEnumeratedValue;
           
            if (ifcPropertySingleValue != null)
            {
                return ConvertToString(ifcPropertySingleValue);
            }
            if (ifcPropertyEnumeratedValue != null)
            {

                if (ifcPropertyEnumeratedValue.EnumerationValues.Count() == 1)
                    return ifcPropertyEnumeratedValue.EnumerationValues.FirstOrDefault().ToString();
                var result = "";
                foreach (var enumValue in ifcPropertyEnumeratedValue.EnumerationValues)
                {
                    result += enumValue + ";";
                }
                return result.TrimEnd(new[] { ';', ' ' });
            }

            CoBieLiteUkHelper.Logger.WarnFormat("Conversion Error: #{0}={1} [{2}] cannot be converted to s simple value type", ifcProperty.EntityLabel, ifcProperty.Name, ifcProperty.GetType().Name);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        /// <typeparam name="TCoBieValueBaseType"></typeparam>
        /// <returns></returns>
        public bool GetAttributeValue<TCoBieValueBaseType>(string propertyName, ref TCoBieValueBaseType val) where TCoBieValueBaseType : AttributeValue, new()
        {
            IIfcProperty ifcProperty;
            IIfcPhysicalQuantity ifcQuantity;
            if (_properties.TryGetValue(propertyName, out ifcProperty))
            {
                val = ConvertAttribute<TCoBieValueBaseType>(ifcProperty);
                return true;
            }
            if (_quantities.TryGetValue(propertyName, out ifcQuantity))
            {
                val = ConvertAttribute<TCoBieValueBaseType>(ifcQuantity);
                return true;
            }
           
            return false;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcProperty"></param>
        /// <typeparam name="TCoBieValueBaseType"></typeparam>
        /// <returns></returns>
        public static TCoBieValueBaseType ConvertAttribute<TCoBieValueBaseType>(IIfcProperty ifcProperty) where TCoBieValueBaseType : AttributeValue, new()
        {
            var ifcPropertySingleValue = ifcProperty as IIfcPropertySingleValue;
            var ifcPropertyEnumeratedValue = ifcProperty as IIfcPropertyEnumeratedValue;
            var ifcPropertyBoundedValue = ifcProperty as IIfcPropertyBoundedValue;
            var ifcPropertyTableValue = ifcProperty as IIfcPropertyTableValue;
            var ifcPropertyReferenceValue = ifcProperty as IIfcPropertyReferenceValue;
            var ifcPropertyListValue = ifcProperty as IIfcPropertyListValue;

            var result = new TCoBieValueBaseType();
            
            if (ifcPropertySingleValue != null)
            {
                SetCoBieAttributeValue(result, ifcPropertySingleValue.NominalValue);
                if(ifcPropertySingleValue.Unit!=null)
                    result.Unit = ((IIfcUnit)ifcPropertySingleValue).FullName;
            }
            else if (ifcPropertyEnumeratedValue != null)
            {
                if (ifcPropertyEnumeratedValue.EnumerationReference != null)
                    result.Unit = ((IIfcUnit)ifcPropertyEnumeratedValue.EnumerationReference).FullName;
                if (ifcPropertyEnumeratedValue.EnumerationValues.Count()==1)
                    SetCoBieAttributeValue(result, ifcPropertyEnumeratedValue.EnumerationValues.FirstOrDefault());
                else if (result is StringAttributeValue) //if it is a string we can add all the  values in a list
                {
                    var stringValueType = result as StringAttributeValue;
                    foreach (var enumValue in ifcPropertyEnumeratedValue.EnumerationValues)
                    {
                        stringValueType.Value += enumValue + ";";
                    }
                    stringValueType.Value = stringValueType.Value.TrimEnd(new[] { ';', ' ' });
                }
                else
                {
                    CoBieLiteUkHelper.Logger.WarnFormat("IfcPropertyEnumeratedValue Conversion: Multiple Enumerated values can only be stored in a string type");
                }
                
            }
            else if (ifcPropertyBoundedValue != null)
            {
                if (result is StringAttributeValue) //if it is a string we can add  the bounded values in a statement
                {
                    if (ifcPropertyBoundedValue.Unit != null)
                        result.Unit = ((IIfcUnit)ifcPropertyBoundedValue).FullName;
                    var stringValueType = result as StringAttributeValue;
                    stringValueType.Value = ifcPropertyBoundedValue.LowerBoundValue + " to " +
                                                  ifcPropertyBoundedValue.UpperBoundValue;
                }
                else
                {
                    CoBieLiteUkHelper.Logger.WarnFormat("IfcPropertyBoundedValue Conversion: Bounded values can only be stored in a string type");
                }
                
            }
            else if (ifcPropertyTableValue != null)
            {
                //Logger.WarnFormat("Table values are not supported in COBie");
            }
            else if (ifcPropertyReferenceValue != null)
            {
                //Logger.WarnFormat("Reference property values are not supported in COBie");
            }
            else if (ifcPropertyListValue != null)
            {
                if (ifcPropertyListValue.Unit!=null)
                    result.Unit = ((IIfcUnit)ifcPropertyListValue).FullName;
                if (ifcPropertyListValue.ListValues.Count() == 1)
                    SetCoBieAttributeValue(result, ifcPropertyListValue.ListValues.FirstOrDefault());
                else if (result is StringAttributeValue) //if it is a string we can add all the  values in a list
                {
                    var stringValueType = result as StringAttributeValue;
                    foreach (var listValue in ifcPropertyListValue.ListValues)
                    {
                        stringValueType.Value += listValue + ";";
                    }
                    stringValueType.Value = stringValueType.Value.TrimEnd(new[] {';', ' '});
                }
                else
                {
                    CoBieLiteUkHelper.Logger.WarnFormat("IfcPropertyList Conversion: ValueMultiple List values can only be stored in a string type");
                }
            }
            
            return result;

        }

        //private static void SetCoBieAttributeValue<TCoBieValueBaseType>(TCoBieValueBaseType result, IIfcValue ifcValue) where TCoBieValueBaseType : AttributeValue
        //{
        //    SetCoBieAttributeValue(result, (IExpressValueType)ifcValue);
        //}


        private static void SetCoBieAttributeValue<TCoBieValueBaseType>(TCoBieValueBaseType result, IExpressValueType ifcValue) where TCoBieValueBaseType : AttributeValue
        {
            var stringValueType = result as StringAttributeValue;
            var decimalValueType = result as DecimalAttributeValue;
            var booleanValueType = result as BooleanAttributeValue;
            var integerValueType = result as IntegerAttributeValue;

            if (stringValueType != null)
            {
                stringValueType.Value = ifcValue.ToString();
            }
            else if (decimalValueType != null)
            {
                try
                {
                    decimalValueType.Value = Convert.ToDouble(ifcValue.Value);
                }
                catch(NullReferenceException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("ifcValue is null.");
                }
                catch (ArgumentNullException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Decimal Conversion: String is null.");
                }
                catch (FormatException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Decimal Conversion: String does not consist of an " + "optional sign followed by a series of digits.");
                }
                catch (OverflowException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Decimal Conversion: Overflow in string to int conversion.");
                }

            }
            else if (booleanValueType != null)
            {
                try
                {
                    booleanValueType.Value = Convert.ToBoolean(ifcValue.Value);
                }
                catch (NullReferenceException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("ifcValue is null.");
                }
                catch (ArgumentNullException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Boolean Conversion: String is null.");
                }
                catch (FormatException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Boolean Conversion: String does not consist of an " + "optional sign followed by a series of digits.");
                }
                catch (OverflowException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Boolean Conversion: Overflow in string to int conversion.");
                }
            }
            else if (integerValueType != null )
            {
                try
                {
                    //this looks like an error in COBieLite, suggest should be same as Decimal and Boolean
                    integerValueType.Value = Convert.ToInt32(ifcValue.Value);
                }
                catch (NullReferenceException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("ifcValue is null.");
                }
                catch (ArgumentNullException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Integer Conversion: String is null.");
                }
                catch (FormatException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Integer Conversion: String does not consist of an " +
                    //                                  "optional sign followed by a series of digits.");
                }
                catch (OverflowException)
                {
                    //CoBieLiteUkHelper.Logger.WarnFormat("Integer Conversion: Overflow in string to int conversion.");
                }
            }
            else
                CoBieLiteUkHelper.Logger.Warn("Unexpected ValueBaseType");
        }
        internal static TCoBieValueBaseType ConvertAttribute<TCoBieValueBaseType>(IIfcPhysicalQuantity ifcQuantity) where TCoBieValueBaseType : AttributeValue, new()
        {
            var ifcQuantityLength = ifcQuantity as IIfcQuantityLength;
            var ifcQuantityArea = ifcQuantity as IIfcQuantityArea;
            var ifcQuantityVolume = ifcQuantity as IIfcQuantityVolume;
            var ifcQuantityCount = ifcQuantity as IIfcQuantityCount;
            var ifcQuantityWeight = ifcQuantity as IIfcQuantityWeight;
            var ifcQuantityTime = ifcQuantity as IIfcQuantityTime;
            var ifcPhysicalComplexQuantity = ifcQuantity as IIfcPhysicalComplexQuantity;
            var ifcPhysicalSimpleQuantity = ifcQuantity as IIfcPhysicalSimpleQuantity;
            var result = new TCoBieValueBaseType();
            if (ifcPhysicalSimpleQuantity != null && ifcPhysicalSimpleQuantity.Unit != null)
                result.Unit = ((IIfcUnit)ifcPhysicalSimpleQuantity).FullName;
            if (ifcQuantityLength != null)
            {
                SetCoBieAttributeValue(result, ifcQuantityLength.LengthValue);
            }
            else if (ifcQuantityArea != null)
            {
                SetCoBieAttributeValue(result, ifcQuantityArea.AreaValue);
            }
            else if (ifcQuantityVolume  != null)
            {
                SetCoBieAttributeValue(result, ifcQuantityVolume.VolumeValue);
            }
            else if (ifcQuantityCount != null)
            {
                SetCoBieAttributeValue(result, ifcQuantityCount.CountValue);
            }
            else if (ifcQuantityWeight != null)
            {
                SetCoBieAttributeValue(result, ifcQuantityWeight.WeightValue);
            }
            else if (ifcQuantityTime != null)
            {
                SetCoBieAttributeValue(result, ifcQuantityTime.TimeValue);
            }
            else if (ifcPhysicalComplexQuantity != null)
            {
                //Logger.WarnFormat("Ifc Physical Complex Quantities  values are not supported in COBie");
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Converts the property to a simple type, date, string
        /// </summary>
        /// <param name="ifcProperty"></param>
        /// <returns></returns>
        static public TValue ConvertToSimpleType<TValue>(IIfcProperty ifcProperty) where TValue:struct
        {

            var ifcPropertySingleValue = ifcProperty as IIfcPropertySingleValue;
            var ifcPropertyEnumeratedValue = ifcProperty as IIfcPropertyEnumeratedValue;
            var ifcPropertyBoundedValue = ifcProperty as IIfcPropertyBoundedValue;
            var ifcPropertyTableValue = ifcProperty as IIfcPropertyTableValue;
            var ifcPropertyReferenceValue = ifcProperty as IIfcPropertyReferenceValue;
            var ifcPropertyListValue = ifcProperty as IIfcPropertyListValue;

            if (ifcPropertySingleValue != null)
            {
                return ConvertToSimpleType<TValue>(ifcPropertySingleValue);
            }
            if (ifcPropertyEnumeratedValue != null)
            {
               
                if (ifcPropertyEnumeratedValue.EnumerationValues.Count() == 1)
                    return (TValue) Convert.ChangeType(ifcPropertyEnumeratedValue.EnumerationValues.FirstOrDefault().ToString(), typeof(TValue));
                var result = "";
                foreach (var enumValue in ifcPropertyEnumeratedValue.EnumerationValues)
                {
                    result += enumValue + ";";
                }
                result = result.TrimEnd(new[] { ';', ' ' });
                return (TValue)Convert.ChangeType(result, typeof(TValue));
            }
            if (ifcPropertyBoundedValue != null)
            {
                CoBieLiteUkHelper.Logger.WarnFormat("Conversion Error: PropertyBoundedValue #{0}={1} cannot be converted to s simple value type", ifcProperty.EntityLabel, ifcProperty.Name);
            }
            else if (ifcPropertyTableValue != null)
            {
                CoBieLiteUkHelper.Logger.WarnFormat("Conversion Error: PropertyTableValue #{0}={1} cannot be converted to s simple value type", ifcProperty.EntityLabel, ifcProperty.Name);
            }
            else if (ifcPropertyReferenceValue != null)
            {
                CoBieLiteUkHelper.Logger.WarnFormat("Conversion Error: PropertyReferenceValue #{0}={1} cannot be converted to s simple value type", ifcProperty.EntityLabel, ifcProperty.Name);
            }
            else if (ifcPropertyListValue != null)
            {
                CoBieLiteUkHelper.Logger.WarnFormat("Conversion Error: PropertyListValue #{0}={1} cannot be converted to s simple value type", ifcProperty.EntityLabel, ifcProperty.Name);
            }
            return default(TValue);
        }


        /// <summary>
        /// Converts an IfcProperty to a COBie Attribute
        /// </summary>
        /// <param name="ifcProperty"></param>
        /// <returns></returns>
        static public Attribute ConvertToAttributeType(IIfcProperty ifcProperty)
        {
            
             var attributeType = new Attribute
             {
                 Description = ifcProperty.Description,
                 Name = ifcProperty.Name,
                 Categories = new List<Category>
                 {
                     new Category { Classification = "DPoW Status", Code = "Submitted" },
                 }
                
                 //srl we need to define categories, the schema proposes "As Built|Submitted|Approved|Exact Requirement|Maximum Requirement|Minimum Requirement|Requirement", should DPoW set requirements?
             };

           
            var ifcPropertySingleValue = ifcProperty as IIfcPropertySingleValue;
            var ifcPropertyEnumeratedValue = ifcProperty as IIfcPropertyEnumeratedValue;
            var ifcPropertyBoundedValue = ifcProperty as IIfcPropertyBoundedValue;
            var ifcPropertyTableValue = ifcProperty as IIfcPropertyTableValue;
            var ifcPropertyReferenceValue = ifcProperty as IIfcPropertyReferenceValue;
            var ifcPropertyListValue = ifcProperty as IIfcPropertyListValue;
            
            if (ifcPropertySingleValue != null)
            {
                attributeType.Value = GetAttributeValueType(ifcPropertySingleValue);
            }
            else if (ifcPropertyEnumeratedValue != null)
            {
                
                var valueItem = new StringAttributeValue();
                attributeType.Value = valueItem;
               
                if (ifcPropertyEnumeratedValue.EnumerationReference != null && ifcPropertyEnumeratedValue.EnumerationReference.Unit!= null)
                    valueItem.Unit = ((IIfcUnit)ifcPropertyEnumeratedValue.EnumerationReference).FullName;
                if (ifcPropertyEnumeratedValue.EnumerationValues.Count()==1)
                    valueItem.Value =  ifcPropertyEnumeratedValue.EnumerationValues.FirstOrDefault().ToString();
                else
                {
                    valueItem.Value = "";
                    foreach (var enumValue in ifcPropertyEnumeratedValue.EnumerationValues)
                    {
                        valueItem.Value += enumValue + ";";
                    }
                    valueItem.Value = valueItem.Value.TrimEnd(new[] { ';', ' ' });
                }
                //add in the allowed values
               
                if (ifcPropertyEnumeratedValue.EnumerationReference != null && ifcPropertyEnumeratedValue.EnumerationReference.EnumerationValues.Any())
                {
                    var allowedValues = new List<string>();
                   
                    foreach (var enumValue in ifcPropertyEnumeratedValue.EnumerationReference.EnumerationValues)
                    {
                        allowedValues.Add(enumValue.ToString());
                       
                    }
                }
            }
            else if (ifcPropertyBoundedValue != null)
            {
                attributeType.Value = GetAttributeValue(ifcPropertyBoundedValue);
            }
            else if (ifcPropertyTableValue != null)
            {
                //Logger.WarnFormat("Table values are not supported in COBie");
            }
            else if (ifcPropertyReferenceValue != null)
            {
                //Logger.WarnFormat("Reference property values are not supported in COBie");
            }
            else if (ifcPropertyListValue != null)
            {
                //Logger.WarnFormat("Multiple List values are not supported in COBie");
            }
            return attributeType;
        }

        static private AttributeValue GetAttributeValue(IIfcPropertyBoundedValue ifcPropertyBoundedValue)
        {            
            var ifcValue = (IExpressValueType) ifcPropertyBoundedValue.LowerBoundValue ;
            //only upper and lowwer bounds are supported by COBie on integer and decimal values
            if (ifcValue.UnderlyingSystemType == typeof(int) || ifcValue.UnderlyingSystemType == typeof(long) || ifcValue.UnderlyingSystemType == typeof(short) || ifcValue.UnderlyingSystemType == typeof(byte)
                || ifcValue.UnderlyingSystemType == typeof(int?) || ifcValue.UnderlyingSystemType == typeof(long?) || ifcValue.UnderlyingSystemType == typeof(short?) || ifcValue.UnderlyingSystemType == typeof(byte?))
            {
                var integerValue = new IntegerAttributeValue();
                if (ifcPropertyBoundedValue.UpperBoundValue != null)
                {
                    integerValue.MaximalValue = Convert.ToInt32(ifcPropertyBoundedValue.UpperBoundValue);
                }
                if (ifcPropertyBoundedValue.LowerBoundValue != null)
                {
                    integerValue.MinimalValue = Convert.ToInt32(ifcPropertyBoundedValue.LowerBoundValue);
                }
                return integerValue;
            }
            if (ifcValue.UnderlyingSystemType == typeof(double) || ifcValue.UnderlyingSystemType == typeof(float) 
                || ifcValue.UnderlyingSystemType == typeof(double?) || ifcValue.UnderlyingSystemType == typeof(float?))
            {
                var decimalValue = new DecimalAttributeValue();
                if (ifcPropertyBoundedValue.UpperBoundValue != null)
                    decimalValue.MaximalValue = (double) ((IExpressValueType)ifcPropertyBoundedValue.UpperBoundValue).Value;
                if (ifcPropertyBoundedValue.LowerBoundValue != null)
                    decimalValue.MinimalValue = (double) ((IExpressValueType)ifcPropertyBoundedValue.LowerBoundValue).Value;
                return decimalValue;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcProperty"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        static public TValue ConvertToSimpleType<TValue>(IIfcPropertySingleValue ifcProperty) where TValue : struct
        {
            var ifcValue = (IExpressValueType)ifcProperty.NominalValue;
            var value = new TValue();
            if (ifcValue is IfcMonetaryMeasure)
            {
                 value = (TValue)Convert.ChangeType(ifcValue.Value, typeof(TValue));
            }
            else if (ifcValue is IfcTimeStamp)
            {    
                var timeStamp = (IfcTimeStamp)ifcValue;
                value = (TValue)Convert.ChangeType(timeStamp.ToDateTime(), typeof(TValue)); 
            }
            else if (value is DateTime) //sometimes these are written as strings in the ifc file
            {
                value = (TValue)Convert.ChangeType(ReadDateTime(ifcValue.Value.ToString()), typeof(TValue));
            }
            else if (ifcValue.UnderlyingSystemType == typeof(int) || ifcValue.UnderlyingSystemType == typeof(long) || ifcValue.UnderlyingSystemType == typeof(short) || ifcValue.UnderlyingSystemType == typeof(byte))
            {
                value = (TValue)Convert.ChangeType(ifcValue.Value, typeof(TValue));             
            }
            else if (ifcValue.UnderlyingSystemType == typeof(double) || ifcValue.UnderlyingSystemType == typeof(float))
            {
                value = (TValue)Convert.ChangeType(ifcValue.Value, typeof(TValue)); 
            }
            else if (ifcValue.UnderlyingSystemType == typeof(string))
            {
                value = (TValue)Convert.ChangeType(ifcValue.Value, typeof(TValue)); 
            }
            else if (ifcValue.UnderlyingSystemType == typeof(bool) || ifcValue.UnderlyingSystemType == typeof(bool?))
            {
                
                if (ifcValue != null)
                {
                    value = (TValue)Convert.ChangeType(ifcValue.Value, typeof(TValue)); 
                }
            }
            return value;
        }

        private static DateTime ReadDateTime(string str)
        {
            var parts = str.Split(new[] {':', '-', 'T', 'Z'},StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length==6) //it is a date time
            {
                int year = Convert.ToInt32(parts[0]);
                int month = Convert.ToInt32(parts[1]);
                int day = Convert.ToInt32(parts[2]);
                int hours = Convert.ToInt32(parts[3]);
                int minutes = Convert.ToInt32(parts[4]);
                int seconds = Convert.ToInt32(parts[5]);
                return new DateTime(year, month, day, hours, minutes, seconds, str.Last() == 'Z' ? DateTimeKind.Utc : DateTimeKind.Unspecified);
            }
            if (parts.Length == 3) //it is a date
            {
                int year = Convert.ToInt32(parts[0]);
                int month = Convert.ToInt32(parts[1]);
                int day = Convert.ToInt32(parts[2]);
                return new DateTime(year, month, day);
            }
            CoBieLiteUkHelper.Logger.WarnFormat("Date Time Conversion: An illegal date time string has been found [{0}]", str);
            return default(DateTime);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcProperty"></param>
        /// <returns></returns>
        static public AttributeValue GetAttributeValueType(IIfcPropertySingleValue ifcProperty)
        {
            var ifcValue = (IExpressValueType)ifcProperty.NominalValue;
            
            if (ifcValue == null) 
                return null;
            if (ifcValue is IfcMonetaryMeasure)
            {
                var moneyAttribute = new DecimalAttributeValue {Value= (double) ifcValue.Value};
                if (ifcProperty.Unit is IfcMonetaryUnit)
                {
                    var mu = ifcProperty.Unit as IfcMonetaryUnit;
                    moneyAttribute.Unit = mu.ToString();
                }
            }
            else if (ifcValue is IfcTimeStamp)
            {
                var timeStamp = (IfcTimeStamp)ifcValue;
                return new DateTimeAttributeValue { Value = timeStamp.ToDateTime() };
            }
            else if (ifcValue.UnderlyingSystemType == typeof(int) || ifcValue.UnderlyingSystemType == typeof(long) || ifcValue.UnderlyingSystemType == typeof(short) || ifcValue.UnderlyingSystemType == typeof(byte))
            {
                return new IntegerAttributeValue {Value=Convert.ToInt32(ifcValue.Value) };
            }
            else if (ifcValue.UnderlyingSystemType == typeof(double) || ifcValue.UnderlyingSystemType == typeof(float) )
            {            
                 return new DecimalAttributeValue {Value=(double)ifcValue.Value};
            }
            else if (ifcValue.UnderlyingSystemType == typeof(string))
            {
               return new StringAttributeValue {Value=ifcValue.ToString()};
            }
            else if (ifcValue.UnderlyingSystemType == typeof(bool) || ifcValue.UnderlyingSystemType == typeof(bool?))
            {               
                if (ifcValue.Value != null && (bool) ifcValue.Value)
                {
                    return new BooleanAttributeValue {Value = (bool) ifcValue.Value};
                }
               return new BooleanAttributeValue();//return an undefined
            }
            
            return null;
        }
    }

    
}
