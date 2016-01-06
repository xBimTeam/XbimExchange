using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.COBieLiteUK;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    /// <summary>
    /// Proxy for an IfcTypeObject, allows proxy types where none are defined in the Ifc File
    /// </summary>
    public class XbimIfcProxyTypeObject
    {
        private readonly IIfcTypeObject _ifcTypeObject;
        private readonly COBieExpressHelper _helper;
        private readonly string _name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        public XbimIfcProxyTypeObject(COBieExpressHelper helper, string name)
        {
            _name = name;
            _helper = helper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="typeObject"></param>
        /// <param name="typeName"></param>
        public XbimIfcProxyTypeObject(COBieExpressHelper helper, IIfcTypeObject typeObject, string typeName)
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
                return COBieExpressHelper.UnknownCategory;
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
                    COBieExpressHelper.Logger.WarnFormat(
                        "AssetTypeAccountingCategory: An illegal value of [{0}] has been passed for the category of #{1}={2}.",
                        accCategoryString, _ifcTypeObject.EntityLabel, _ifcTypeObject.GetType().Name);
                    IIfcAsset ifcAsset;
                    if (_helper.AssetAsignments.TryGetValue(_ifcTypeObject, out ifcAsset))
                    {
                        string portability =
                            _helper.GetCoBieAttribute<StringValue>("AssetTypeAccountingCategory", ifcAsset);
                        if (Enum.TryParse(portability, true, out accCategoryEnum))
                            return accCategoryEnum;
                    }
                    //Not technically correct, but if all ifcElements all have the same AssetTypeAccountingCategory value , we can safely assume that the type is going to be the same
                    accCategoryString = GetObjPropByAssoc("AssetTypeAccountingCategory", _ifcTypeObject);
                    if (accCategoryString != null)
                    {
                        if (Enum.TryParse(accCategoryString, true, out accCategoryEnum))
                        return accCategoryEnum;
                    }
                    //Responsibility matrix, 'SpreadSheet Schema' tab, cell S81
                    if (_ifcTypeObject is IIfcFurnitureType) 
                    {
                        return AssetPortability.Moveable;
                    }
                }
                return AssetPortability.notdefined;
            }
        }

        /// <summary>
        /// Check all ifcElements associated with the pass ifcTypeObject for the passed property map key. 
        /// If all found elements of the property are the same then we assume that the property applies to the type as well as all the elements 
        /// </summary>
        /// <param name="valueName">property map key</param>
        /// <param name="ifcTypeObject">ifcTypeObject</param>
        /// <returns>property value</returns>
        private string GetObjPropByAssoc(string valueName, IIfcTypeObject ifcTypeObject)
        {
            string accCategoryString = string.Empty;
            var ObjDefByType = _helper.DefiningTypeObjectMap.Where(pair => (pair.Key.IfcTypeObject != null) && (pair.Key.IfcTypeObject == ifcTypeObject)).SelectMany(p => p.Value);
            var assetTypes = new List<string>();
            foreach (var item in ObjDefByType)
            {
                accCategoryString = _helper.GetCoBieProperty(valueName, item);
                assetTypes.Add(accCategoryString != null ? accCategoryString : string.Empty);
            }
            //assume every ifcElement hast to have the value and be set to the same value
            if (assetTypes.Count > 0)
            {
                var fat = assetTypes.First();
                if (assetTypes.All(at => at == fat))
                {
                    return accCategoryString;
                }
            }
            return null;
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
        public IIfcTypeObject IfcTypeObject { get { return _ifcTypeObject; }}

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

        internal ContactKey GetCreatedBy()
        {       
           return _helper.GetCreatedBy(_ifcTypeObject);
        }

        internal DateTime? GetCreatedOn()
        {
            if (_ifcTypeObject != null)
                return _helper.GetCreatedOn(_ifcTypeObject);
            return DateTime.Now;
        }
    }
}
