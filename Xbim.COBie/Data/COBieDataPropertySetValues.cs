using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.PropertyResource;
using System;
//using System;



namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to extract all the property sets and there associated properties for a list of either IfcObjects or IfcTypeObjects
    /// </summary>
    public class COBieDataPropertySetValues 
    {
        #region Fields
        private IfcObjectDefinition _currentObject;
        private Dictionary<IfcPropertySet, IEnumerable<IfcSimpleProperty>> _mapPsetToProps;
        #endregion

        #region Properties
        /// <summary>
        /// True if a Property Set name filter is set
        /// </summary>
        public bool PSetFilterOn { get; set; }
        
        /// <summary>
        /// IfcSimpleProperty value list of all properties associated to a IfcObject or IfcTypeObject currently set
        /// </summary>
        public IEnumerable<IfcSimpleProperty> ObjProperties
        {
            get
            {
                    return from dic in _mapPsetToProps
                       from psetval in dic.Value //list of IfcSimpleProperty
                       select psetval;
            }
        }

        /// <summary>
        /// Current object which set the properties
        /// </summary>
        public IfcObjectDefinition CurrentObject
        {
            get { return _currentObject; }
           
        }

        /// <summary>
        /// Property sets mapped to a list of properties for the current set object
        /// </summary>
        public Dictionary<IfcPropertySet, IEnumerable<IfcSimpleProperty>> MapPsetToProps
        {
            get { return _mapPsetToProps; }
        }
        #endregion


        #region Methods

        /// <summary>
        /// Default Constructor
        /// </summary>
        public COBieDataPropertySetValues()
        {
            _mapPsetToProps = new Dictionary<IfcPropertySet, IEnumerable<IfcSimpleProperty>>();
        }

         /// <summary>
        /// Get the related entity properties for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject"> IfcTypeObject </param>
        /// <returns>Dictionary of IfcPropertySet keyed to List of IfcPropertySingleValue</returns>
        public Dictionary<IfcPropertySet, IEnumerable<IfcSimpleProperty>> GetRelatedProperties(IfcTypeObject ifcTypeObject)
        {
            if ((ifcTypeObject != null) && (ifcTypeObject.ObjectTypeOf.Any()))
            {
                IfcObject IfcObj = ifcTypeObject.ObjectTypeOf.First().RelatedObjects.FirstOrDefault();
                if (IfcObj != null)
                {
                    return IfcObj.IsDefinedByProperties
                            .Select(def => def.RelatingPropertyDefinition).OfType<IfcPropertySet>()
                            .ToDictionary(ps => ps, ps => ps.HasProperties.OfType<IfcSimpleProperty>());
                }
                
            }
            return new Dictionary<IfcPropertySet, IEnumerable<IfcSimpleProperty>>();
            
            
        }
       
         
        /// <summary>
        /// Set the property sets mapped to list of simple property values held for the IfcObject
        /// </summary>
        /// <param name="ifcObject">IfcObject holding the property values</param>
        public void SetAllPropertyValues (IfcObject ifcObject)
        {
            _currentObject = ifcObject;
            PSetFilterOn = false;

            _mapPsetToProps = ifcObject.IsDefinedByProperties
                            .Select(def => def.RelatingPropertyDefinition).OfType<IfcPropertySet>()
                            .ToDictionary(ps => ps, ps => ps.HasProperties.OfType<IfcSimpleProperty>());
        }

        /// <summary>
        /// Set the property sets mapped to list of simple property values held for the IfcTypeObject
        /// </summary>
        /// <param name="ifcTypeObject">ifcTypeObject holding the property values</param>
        public void SetAllPropertyValues(IfcTypeObject ifcTypeObject)
        {
            _currentObject = ifcTypeObject;
            PSetFilterOn = false;


            if (ifcTypeObject.HasPropertySets != null) //we have properties to get
            {
                _mapPsetToProps = ifcTypeObject.HasPropertySets.OfType<IfcPropertySet>()
                                        .ToDictionary(ps => ps, ps => ps.HasProperties.OfType<IfcSimpleProperty>());
            }
            else
            {
                _mapPsetToProps.Clear(); //clear as we have no properties for this object
            }

            if (!_mapPsetToProps.Any()) //not sure we should do this, but we get values to fill from an object that is using the type object
            {
                if (ifcTypeObject.ObjectTypeOf.Any())
                {
                    IfcObject IfcObj = ifcTypeObject.ObjectTypeOf.First().RelatedObjects.FirstOrDefault();
                    if (IfcObj != null)
                    {
                        SetAllPropertyValues(IfcObj);//we do not filter on property set name here, just go for all of them
                    }
                    
                }
            }
        }

        
        /// <summary>
        /// Set the property sets mapped to list of simple property values held for the IfcTypeObject
        /// filtered by a IfcPropertySet name
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject holding the property values</param>
        /// <param name="propertySetName">IfcPropertySetName</param>
        public void SetAllPropertyValues(IfcTypeObject ifcTypeObject, string propertySetName)
        {
            _currentObject = ifcTypeObject;
            PSetFilterOn = true;

            if (ifcTypeObject.HasPropertySets != null) //we have properties to get
            {
                _mapPsetToProps = ifcTypeObject.HasPropertySets.OfType<IfcPropertySet>()
                                .Where(ps => (ps.Name == propertySetName))
                                .ToDictionary(ps => ps, ps => ps.HasProperties.OfType<IfcSimpleProperty>());
            }
            else
            {
                _mapPsetToProps.Clear(); //clear as we have no properties for this object
            }

            //fall back to related items to get the information from
            if (!_mapPsetToProps.Any())//not sure we should do this, but we get values to fill from an object that is using the type object
            {
                if (ifcTypeObject.ObjectTypeOf.Any())
                {
                    IfcObject IfcObj = ifcTypeObject.ObjectTypeOf.First().RelatedObjects.FirstOrDefault();
                    if (IfcObj != null)
                    {
                        SetAllPropertyValues(IfcObj); //we do not filter on property set name here, just go for all of them
                    }
                    
                }
               
            }
        }

        /// <summary>
        /// Set the property sets mapped to list of simple property values held for the IfcTypeObject
        /// filtered by a IfcPropertySet name
        /// </summary>
        /// <param name="ifcTypeObject">IfcTypeObject holding the property values</param>
        /// <param name="propertySetNames">List of IfcPropertySetName</param>
        public void SetAllPropertyValues(IfcTypeObject ifcTypeObject, List<string> propertySetNames)
        {
            _currentObject = ifcTypeObject;
            PSetFilterOn = true;

            if (ifcTypeObject.HasPropertySets != null) //we have properties to get
            {
                _mapPsetToProps = ifcTypeObject.HasPropertySets.OfType<IfcPropertySet>()
                                .Where(ps => (propertySetNames.Contains(ps.Name)))
                                .ToDictionary(ps => ps, ps => ps.HasProperties.OfType<IfcSimpleProperty>());
            }
            else
            {
                _mapPsetToProps.Clear(); //clear as we have no properties for this object
            }

            //fall back to related items to get the information from
            if (!_mapPsetToProps.Any())//not sure we should do this, but we get values to fill from an object that is using the type object
            {
                if (ifcTypeObject.ObjectTypeOf.Any())
                {
                    IfcObject IfcObj = ifcTypeObject.ObjectTypeOf.First().RelatedObjects.FirstOrDefault();
                    if (IfcObj != null)
                    {
                        SetAllPropertyValues(IfcObj); //we do not filter on property set name here, just go for all of them
                    }

                }

            }
        }

               
        /// <summary>
        /// Get the property value where the property name equals the passed in value. 
        /// Always use  SetAllPropertySingleValues before calling this method
        /// </summary>
        /// <param name="PropertyValueName">IfcPropertySingleValue name</param>
        /// <param name="containsString">Do Contains text match on PropertyValueName if true, exact match if false</param>
        /// <returns></returns>
        public string GetPropertySingleValueValue(string PropertyValueName, bool containsString)
        {
            
            IfcPropertySingleValue ifcPropertySingleValue = null;
            if (containsString)
                ifcPropertySingleValue = ObjProperties.OfType<IfcPropertySingleValue>().Where(p => p.Name.ToString().Contains(PropertyValueName)).FirstOrDefault();
            else
                ifcPropertySingleValue = ObjProperties.OfType<IfcPropertySingleValue>().Where(p => p.Name == PropertyValueName).FirstOrDefault();

            //return a string value
            if ((ifcPropertySingleValue != null) && 
                (ifcPropertySingleValue.NominalValue != null) && 
                (!string.IsNullOrEmpty(ifcPropertySingleValue.NominalValue.Value.ToString())) && 
                (ifcPropertySingleValue.Name.ToString() != ifcPropertySingleValue.NominalValue.Value.ToString()) //name and value are not the same
                ) 
            return ifcPropertySingleValue.NominalValue.Value.ToString();
            else
                return Constants.DEFAULT_STRING;
        }

        /// <summary>
        /// Get the property value where the property name equals the passed in value 
        /// Always use  SetAllPropertySingleValues before calling this method
        /// </summary>
        /// <param name="PropertyValueName"></param>
        /// <returns></returns>
        public IfcPropertySingleValue GetPropertySingleValue(string PropertyValueName)
        {
            return ObjProperties.OfType<IfcPropertySingleValue>().Where(p => p.Name == PropertyValueName).FirstOrDefault();
        }
        
        #endregion
        
    }

        
           
}
