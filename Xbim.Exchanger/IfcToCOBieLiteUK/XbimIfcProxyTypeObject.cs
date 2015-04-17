using System;
using System.Collections.Generic;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.SharedFacilitiesElements;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// Proxy for an IfcTypeObject, allows proxy types where none are defined in the Ifc File
    /// </summary>
    public class XbimIfcProxyTypeObject
    {
        private readonly IfcTypeObject _ifcTypeObject;
        private readonly CoBieLiteUkHelper _helper;
        private readonly string _name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public XbimIfcProxyTypeObject(string name)
        {
            _name = name;
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="typeObject"></param>
        /// <param name="typeName"></param>
        public XbimIfcProxyTypeObject(CoBieLiteUkHelper helper, IfcTypeObject typeObject, string typeName)
        {
            _ifcTypeObject = typeObject;
            _helper = helper;
            _name = typeName;
        }
        /// <summary>
        /// Returns the external name of the IfcTypeObject, if this is a generated type returns IfcTypeObject
        /// </summary>
        public string ExternalEntity
        {
            get
            {
                if(_ifcTypeObject!=null) 
                    return _helper.ExternalEntityName(_ifcTypeObject);
                return "IfcTypeObject";
            }
        }
        /// <summary>
        /// returns the external id of the type or null if this is generated
        /// </summary>
        public string ExternalId 
        {
            get
            {
                if(_ifcTypeObject!=null) 
                    return _helper.ExternalEntityIdentity(_ifcTypeObject);
                return null;
            }
        }

        /// <summary>
        /// Returns the external name of the IfcTypeObject, if this is a generated type returns xBIM
        /// </summary>
        public string ExternalSystemName 
        {
            get
            {
                if(_ifcTypeObject!=null) 
                    return _helper.ExternalSystemName(_ifcTypeObject);
                return "xBIM";
            }
        }
        /// <summary>
        /// returns the name of the type or the generated name if this is a generated type
        /// </summary>
        public string Name 
        {
            get
            {
                return _name;
            }
        }
         
        /// <summary>
        /// Returns the categories for the type null if no Ifc Type exists
        /// </summary>
        public List<Category> Categories 
        {
            get
            {
                if(_ifcTypeObject!=null) 
                    return  _helper.GetCategories(_ifcTypeObject);
                return null;
            }
        }
        /// <summary>
        /// Returns the Accounting category, undefined if no type exists
        /// </summary>
        public AssetPortability AccountingCategory
        {
            get
            {
                if (_ifcTypeObject != null)
                {
                    var accCategoryString = _helper.GetCoBieProperty("AssetTypeAccountingCategory", _ifcTypeObject);
                    AssetPortability accCategoryEnum;
                    if (Enum.TryParse(accCategoryString, true, out accCategoryEnum))
                        return accCategoryEnum;
                    CoBieLiteUkHelper.Logger.WarnFormat(
                        "AssetTypeAccountingCategory: An illegal value of [{0}] has been passed for the category of #{1}={2}. It has been replaced with a value of 'Item'",
                        accCategoryString, _ifcTypeObject.EntityLabel, _ifcTypeObject.GetType().Name);
                    IfcAsset ifcAsset;
                    if (_helper.AssetAsignments.TryGetValue(_ifcTypeObject, out ifcAsset))
                    {
                        string portability =
                            _helper.GetCoBieAttribute<StringAttributeValue>("AssetTypeAccountingCategory", ifcAsset).Value;
                        if (Enum.TryParse(portability, true, out accCategoryEnum))
                            return accCategoryEnum;
                    }
                }
                return AssetPortability.notdefined;
            }
        }

        /// <summary>
        /// Returns the description of the type, null if no type exists
        /// </summary>
        public string Description
        {
            get
            {
                if (_ifcTypeObject != null)
                    return _ifcTypeObject.Description;
                return null;
            }
        }
        /// <summary>
        /// returns the type object null if this is a generated type
        /// </summary>
        public IfcTypeObject IfcTypeObject { get { return _ifcTypeObject; }}

        /// <summary>
        /// Returns the entity label , -1 if this is a generated type </summary>
        public int EntityLabel
        {
            get
            {
                if (_ifcTypeObject != null)
                    return _ifcTypeObject.EntityLabel; 
                return -1;
            }
        }

        /// <summary>
        /// Returns the type name of the object, null if this is a generated type
        /// </summary>
        public string TypeName
        {
            get
            {
                if (_ifcTypeObject != null)
                    return _ifcTypeObject.GetType().Name;
                return null;
            }
        }
    }
}
