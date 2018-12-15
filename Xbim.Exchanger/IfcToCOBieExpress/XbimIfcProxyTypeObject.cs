using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
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
                return _ifcTypeObject!=null ? 
                    _helper.ExternalEntityName(_ifcTypeObject) : 
                    "IfcTypeObject";
            }
        }
        /// <summary>
        /// returns the external id of the type or null if this is generated
        /// </summary>
        public string ExternalId 
        {
            get
            {
                return _ifcTypeObject!=null ? 
                    _helper.ExternalEntityIdentity(_ifcTypeObject) : 
                    null;
            }
        }

        /// <summary>
        /// Returns the external name of the IfcTypeObject, if this is a generated type returns xBIM
        /// </summary>
        public CobieExternalSystem ExternalSystem
        {
            get
            {
                return _ifcTypeObject!=null ? 
                    _helper.GetExternalSystem(_ifcTypeObject) : 
                    _helper.XbimSystem;
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
        public HashSet<CobieCategory> Categories 
        {
            get
            {
                return _ifcTypeObject!=null ? 
                    _helper.GetCategories(_ifcTypeObject) : 
                    new HashSet<CobieCategory> { _helper.UnknownCategory };
            }
        }
        /// <summary>
        /// Returns the Accounting category, undefined if no type exists
        /// </summary>
        public CobieAssetType AccountingCategory
        {
            get
            {
                if (_ifcTypeObject != null)
                {
                    var accCategoryString = _helper.GetCoBieProperty("AssetTypeAccountingCategory", _ifcTypeObject);
                    if (!string.IsNullOrWhiteSpace(accCategoryString))
                        return _helper.GetPickValue<CobieAssetType>(accCategoryString);

                    IIfcAsset ifcAsset;
                    if (_helper.AssetAsignments.TryGetValue(_ifcTypeObject, out ifcAsset))
                    {
                        string portability = null;
                        _helper.TrySetSimpleValue<string>("AssetTypeAccountingCategory", ifcAsset, v => portability = v);
                        if (!string.IsNullOrWhiteSpace(portability))
                            return _helper.GetPickValue<CobieAssetType>(portability);
                    }
                    //Not technically correct, but if all ifcElements all have the same AssetTypeAccountingCategory value , we can safely assume that the type is going to be the same
                    accCategoryString = GetObjPropByAssoc("AssetTypeAccountingCategory", _ifcTypeObject);
                    if (!string.IsNullOrWhiteSpace(accCategoryString))
                        return _helper.GetPickValue<CobieAssetType>(accCategoryString);
                    
                    //Responsibility matrix, 'SpreadSheet Schema' tab, cell S81
                    if (_ifcTypeObject is IIfcFurnitureType) 
                    {
                        return _helper.GetPickValue<CobieAssetType>("Moveable");
                    }
                }
                return _helper.GetPickValue<CobieAssetType>("notdefined");
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
            var accCategoryString = string.Empty;
            var objDefByType = _helper.DefiningTypeObjectMap.Where(pair => (pair.Key.IfcTypeObject != null) && Equals(pair.Key.IfcTypeObject, ifcTypeObject)).SelectMany(p => p.Value);
            var assetTypes = new List<string>();
            foreach (var item in objDefByType)
            {
                accCategoryString = _helper.GetCoBieProperty(valueName, item);
                assetTypes.Add(accCategoryString ?? string.Empty);
            }
            //assume every ifcElement hast to have the value and be set to the same value
            if (assetTypes.Count <= 0) return null;

            var fat = assetTypes.First();
            return assetTypes.All(at => at == fat) ? 
                accCategoryString : 
                null;
        }

        /// <summary>
        /// Returns the description of the type, null if no type exists
        /// </summary>
        public string Description
        {
            get
            {
                return _ifcTypeObject != null ? 
                    _ifcTypeObject.Description : 
                    null;
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
            get { 
                return _ifcTypeObject != null ? 
                _ifcTypeObject.EntityLabel : 
                -1; 
            }
        }

        /// <summary>
        /// Returns the type name of the object, null if this is a generated type
        /// </summary>
        public string TypeName
        {
            get
            {
                return _ifcTypeObject != null ? 
                    _ifcTypeObject.GetType().Name : 
                    null;
            }
        }

        internal CobieCreatedInfo GetCreatedInfo()
        {       
           return _helper.GetCreatedInfo(_ifcTypeObject);
        }
    }
}
