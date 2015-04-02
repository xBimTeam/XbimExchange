
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Common.Logging;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.QuantityResource;
using Xbim.Ifc2x3.SharedFacilitiesElements;
using Xbim.IO;
using Xbim.XbimExtensions.SelectTypes;
using Attribute = Xbim.COBieLiteUK.Attribute;
using SystemAssembly = System.Reflection.Assembly;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public enum EntityIdentifierMode
    {
        /// <summary>
        /// Use the Entity Label in the Ifc file (e.g. #23)
        /// </summary>
        IfcEntityLabels = 0,
        /// <summary>
        /// Use the GlobalId of the Entity (e.g. "10mjSDZJj9gPS2PrQaxa3z")
        /// </summary>
        GloballyUniqueIds = 1,
        /// <summary>
        /// Does not write any External Identifier for Entities
        /// </summary>
        None = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ExternalReferenceMode
    {
        /// <summary>
        /// Does not write out the External Entity Type Name or the External System Name
        /// </summary>
        IgnoreSystemAndEntityName = 3,
        /// <summary>
        /// Does not write out the External System Name but does write out the External Entity Type Name
        /// </summary>
        IgnoreSystem = 1,
        /// <summary>
        /// Does not write out the External Entity Type Name but does write the External System Name
        /// </summary>
        IgnoreEntityName = 2

    }
    /// <summary>
    /// 
    /// </summary>
    public class CoBieLiteUkHelper
    {

        internal static readonly ILogger Logger = LoggerFactory.GetLogger();
       
        private readonly XbimModel _model;
        private readonly string _creatingApplication;

        #region Model measurement units

        private LinearUnit? _modelLinearUnit;
        private AreaUnit? _modelAreaUnit;
        private VolumeUnit? _modelVolumeUnit;
        private CurrencyUnit? _modelCurrencyUnit;

        private bool _hasLinearUnit;
        private bool _hasAreaUnit;
        private bool _hasVolumeUnit;
        private bool _hasCurrencyUnit;

        #endregion

        #region Settings

        /// <summary>
        /// 
        /// </summary>
        public EntityIdentifierMode EntityIdentifierMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ExternalReferenceMode ExternalReferenceMode { get; set; }
        #endregion

        #region Lookups
        //Classification for any root object
        private Dictionary<IfcRoot, List<IfcClassificationReference>> _classifiedObjects;

        private Dictionary<IfcZone, HashSet<IfcSpace>>  _zoneSpaces;
        private Dictionary<IfcSpace, HashSet<IfcZone>> _spaceZones;

        private Dictionary<IfcObjectDefinition,XbimAttributedObject> _attributedObjects;

        private Dictionary<string, string[]> _cobieFieldMap;


        private readonly HashSet<IfcType> _includedTypes = new HashSet<IfcType>();

        private Dictionary<IfcObject,IfcTypeObject> _objectToTypeObjectMap;
        private Dictionary<IfcTypeObject, List<IfcElement>> _definingTypeObjectMap;
/*
        private Lookup<string, IfcElement> _elementTypeToElementObjectMap;
*/
        private Dictionary<IfcTypeObject, IfcAsset> _assetAsignments;
        private Dictionary<IfcSystem, ObjectDefinitionSet> _systemAssignment;
        private Dictionary<IfcObjectDefinition, List<IfcSystem>> _systemLookup;
        private readonly HashSet<string> _cobieProperties = new HashSet<string>();
        private Dictionary<IfcElement, List<IfcSpatialStructureElement>> _spaceAssetLookup;
        private Dictionary<IfcSpace, IfcBuildingStorey> _spaceFloorLookup;
        private Dictionary<IfcSpatialStructureElement, List<IfcSpatialStructureElement>> _spatialDecomposition;

        #endregion

        private readonly string _configFileName;
        private Dictionary<String, List<IfcTypeObject>> _assetTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="configurationFile"></param>
        public CoBieLiteUkHelper(XbimModel model, string configurationFile = null)
        {
            _configFileName = configurationFile;

            _model = model;
            _creatingApplication = model.Header.CreatingApplication;
            LoadCobieMaps();
            GetClassificationDictionary();
            GetSpacesAndZones();
            GetUnits();
            GetTypeMaps();
            GetPropertySets();
            GetSystems();
            GetSpaceAssetLookup();
        }
        
        private void GetSystems()
        {
            _systemAssignment = 
                _model.Instances.OfType<IfcRelAssignsToGroup>().Where(r => r.RelatingGroup is IfcSystem)
                .ToDictionary(k => (IfcSystem)k.RelatingGroup, v => v.RelatedObjects);
            _systemLookup = new Dictionary<IfcObjectDefinition, List<IfcSystem>>();
            foreach (var systemAssignment in SystemAssignment)
                foreach (var objectDefinition in systemAssignment.Value)
                {
                    if (_systemLookup.ContainsKey(objectDefinition))
                        _systemLookup[objectDefinition].Add(systemAssignment.Key);
                    else
                        _systemLookup.Add(objectDefinition, new List<IfcSystem>(new[] {systemAssignment.Key} ));
                }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IfcTypeObject, List<IfcElement>> DefiningTypeObjectMap
        {
            get { return _definingTypeObjectMap; }
        }
        private void GetTypeMaps()
        {
            _assetTypes = _model.Instances.OfType<IfcRelDefinesByType>().GroupBy(k => k.RelatingType.Name.HasValue ? k.RelatingType.Name.ToString() : "Undefined", v=>v.RelatingType).ToDictionary(group => group.Key, group => group.ToList());

         

            _definingTypeObjectMap = _model.Instances.OfType<IfcRelDefinesByType>().ToDictionary(k => k.RelatingType, kv => kv.RelatedObjects.OfType<IfcElement>().ToList());
            _objectToTypeObjectMap = new Dictionary<IfcObject, IfcTypeObject>();
            foreach (var typeObjectToObjects in _definingTypeObjectMap)
            {
                foreach (var ifcObject in typeObjectToObjects.Value)
                {
                    _objectToTypeObjectMap.Add(ifcObject, typeObjectToObjects.Key);
                }
            }

            //Get asset assignments

            var assetRels = _model.Instances.OfType<IfcRelAssignsToGroup>()
                .Where(r => r.RelatingGroup is IfcAsset &&
                            r.RelatedObjects.Any(
                                o => o is IfcTypeObject && _definingTypeObjectMap.ContainsKey((IfcTypeObject) o)));

            _assetAsignments = new Dictionary<IfcTypeObject, IfcAsset>();
            foreach (var assetRel in assetRels)
            {
                foreach (var assetType in assetRel.RelatedObjects)
                    AssetAsignments[(IfcTypeObject)assetType] = (IfcAsset) assetRel.RelatingGroup; 
            }
            //////get all Assets that don't belong to an Ifc Type or are not classified
            //////get all IfcElements that aren't classified or have a type
            ////var existingAssets =  _classifiedObjects.Keys.OfType<IfcElement>()
            ////    .Concat(_objectToTypeObjectMap.Keys.OfType<IfcElement>()).Distinct();
            //////retrieve all the IfcElements from the model and exclude them if they are already classified or are a member of an IfcType
            ////var unCategorizedAssets = _model.Instances.OfType<IfcElement>().Except(existingAssets);
            //////convert to a Lookup with the key the type of the IfcElement and the value a list of IfcElements
            //////if the object has a classification we use this to distinguish types
            ////_elementTypeToElementObjectMap = (Lookup<string,IfcElement>) unCategorizedAssets.ToLookup
            ////    (k =>
            ////    {
            ////        var category = GetClassification(k); return category == null ? k.GetType().Name : k.GetType().Name + " [" + category + "]";
            ////    }
            ////    , v => v);

        }


        private void LoadCobieMaps()
        {
            var tmpFile = _configFileName;
            if (_configFileName == null)
            {
                tmpFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".csv";

                var asss = SystemAssembly.GetExecutingAssembly();

                using (var input = asss.GetManifestResourceStream("XbimExchanger.IfcToCOBieLiteUK.COBieAttributes.config"))
                using (var output = File.Create(tmpFile))
                {
                    if (input != null) input.CopyTo(output);
                }
            }


            Configuration config;
            AppSettingsSection cobiePropertyMaps;
            _cobieFieldMap = new Dictionary<string, string[]>();

            if (!File.Exists(tmpFile))
            {
                var directory = new DirectoryInfo(".");
                throw new Exception(
                    string.Format(
                        @"Error loading configuration file ""{0}"". App folder is ""{1}"".", tmpFile,
                        directory.FullName)
                    );
            }

            try
            {
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = tmpFile };
                config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                cobiePropertyMaps = (AppSettingsSection) config.GetSection("COBiePropertyMaps");
            }
            catch (Exception ex)
            {
                var directory = new DirectoryInfo(".");
                throw new Exception(
                    @"Error loading configuration file ""COBieAttributes.config"". App folder is " + directory.FullName,
                    ex);
            }

            if (cobiePropertyMaps != null)
            {
                foreach (KeyValueConfigurationElement keyVal in cobiePropertyMaps.Settings)
                {
                    var values = keyVal.Value.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    _cobieFieldMap.Add(keyVal.Key, values);
                    foreach (var cobieProperty in values)
                        //used to keep a list of properties that are reserved for COBie and not written out as attributes
                        _cobieProperties.Add(cobieProperty);
                }
            }


            var ifcElementInclusion = (AppSettingsSection) config.GetSection("IfcElementInclusion");

            if (ifcElementInclusion != null)
            {
                foreach (KeyValueConfigurationElement keyVal in ifcElementInclusion.Settings)
                {
                    if (String.Compare(keyVal.Value, "YES", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var includedType = IfcMetaData.IfcType(keyVal.Key.ToUpper());
                        if (includedType != null) _includedTypes.Add(includedType);
                    }
                }
            }

            if (_configFileName == null)
            {
                File.Delete(tmpFile);
            }
        }


        private void GetPropertySets()
        {
            _attributedObjects = new Dictionary<IfcObjectDefinition, XbimAttributedObject>();
            var relProps = _model.Instances.OfType<IfcRelDefinesByProperties>().ToList();
            foreach (var relProp in relProps)
            {
                foreach (var ifcObject in relProp.RelatedObjects)
                {
                    XbimAttributedObject attributedObject;
                    if (!_attributedObjects.TryGetValue(ifcObject, out attributedObject))
                    {
                        attributedObject = new XbimAttributedObject(ifcObject);
                        _attributedObjects.Add(ifcObject, attributedObject);
                    }
                    attributedObject.AddPropertySetDefinition(relProp.RelatingPropertyDefinition);  
                }
            }
            foreach (var typeObject in _definingTypeObjectMap.Keys)
            {
                XbimAttributedObject attributedObject;
                if (!_attributedObjects.TryGetValue(typeObject, out attributedObject))
                    {
                        attributedObject = new XbimAttributedObject(typeObject);
                        _attributedObjects.Add(typeObject, attributedObject);
                    }
                if (typeObject.HasPropertySets != null)
                {
                    foreach (var pset in typeObject.HasPropertySets)
                    {
                        attributedObject.AddPropertySetDefinition(pset);
                    }
                }
            }
            
        }

        private void GetSpacesAndZones()
        {
            _spatialDecomposition = _model.Instances.OfType<IfcRelAggregates>().Where(r=>r.RelatingObject is IfcSpatialStructureElement)
                .ToDictionary(ifcRelAggregate => (IfcSpatialStructureElement) ifcRelAggregate.RelatingObject, ifcRelAggregate => ifcRelAggregate.RelatedObjects.OfType<IfcSpatialStructureElement>().ToList());

            //get the relationship between spaces and storeys
            _spaceFloorLookup = new Dictionary<IfcSpace, IfcBuildingStorey>();
            foreach (var spatialElement in _spatialDecomposition)
            {
                var key = spatialElement.Key as IfcBuildingStorey;
                if (key != null) //only care if the space is on a floor (COBie rule)
                {
                    foreach (var ifcSpace in spatialElement.Value.OfType<IfcSpace>())
                        _spaceFloorLookup[ifcSpace] = key;
                }
            }

            var relZones = _model.Instances.OfType<IfcRelAssignsToGroup>().Where(r=>r.RelatingGroup is IfcZone).ToList();
            _zoneSpaces = new Dictionary<IfcZone, HashSet<IfcSpace>>();
            _spaceZones = new Dictionary<IfcSpace, HashSet<IfcZone>>();
            foreach (var relZone in relZones)
            {
                var spaces = relZone.RelatedObjects.OfType<IfcSpace>().ToList();
                if (spaces.Any())
                {
                    //add the spaces to each zone lookup
                    var zone = (IfcZone) relZone.RelatingGroup;
                    HashSet<IfcSpace> zoneSpaces;
                    if (!ZoneSpaces.TryGetValue(zone, out zoneSpaces))
                    {
                        zoneSpaces = new HashSet<IfcSpace>();
                        ZoneSpaces.Add(zone,zoneSpaces);
                    }
                    foreach (var space in spaces) zoneSpaces.Add(space);
                    
                    //now add the zones to the space lookup         
                    foreach (var ifcSpace in spaces)
                    {
                        HashSet<IfcZone> spaceZones;
                        if (!_spaceZones.TryGetValue(ifcSpace, out spaceZones))
                        {
                            spaceZones = new HashSet<IfcZone>();
                            _spaceZones.Add(ifcSpace,spaceZones);
                        }
                        spaceZones.Add(zone);
                    }
                }
            }         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="space"></param>
        /// <returns></returns>
        public IEnumerable<IfcZone> GetZones(IfcSpace space)
        {
            HashSet<IfcZone> zones;
            if (_spaceZones.TryGetValue(space, out zones))
                return zones;
            return Enumerable.Empty<IfcZone>();
        }
        /// <summary>
        /// 
        /// </summary>
        public LinearUnit? ModelLinearUnit
        {
            get { return _modelLinearUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public AreaUnit? ModelAreaUnit
        {
            get { return _modelAreaUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public VolumeUnit? ModelVolumeUnit
        {
            get { return _modelVolumeUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CurrencyUnit? ModelCurrencyUnit
        {
            get { return _modelCurrencyUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasLinearUnit
        {
            get { return _hasLinearUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasAreaUnit
        {
            get { return _hasAreaUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasVolumeUnit
        {
            get { return _hasVolumeUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasCurrencyUnit
        {
            get { return _hasCurrencyUnit; }
        }

        /// <summary>
        /// 
        /// </summary>
        public XbimModel Model
        {
            get { return _model; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<IfcTypeObject, IfcAsset> AssetAsignments
        {
            get { return _assetAsignments; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IfcObjectDefinition, List<IfcSystem>> SystemLookup
        {
            get { return _systemLookup; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IfcSystem, ObjectDefinitionSet> SystemAssignment
        {
            get { return _systemAssignment; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IfcElement, List<IfcSpatialStructureElement>> SpaceAssetLookup
        {
            get { return _spaceAssetLookup; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IfcSpace, IfcBuildingStorey> SpaceFloorLookup
        {
            get { return _spaceFloorLookup; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<IfcZone, HashSet<IfcSpace>> ZoneSpaces
        {
            get { return _zoneSpaces; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, List<IfcTypeObject>> AssetTypes
        {
            get { return _assetTypes; }
        }

        private void GetUnits()
        {
            var ifcProject = Model.IfcProject;
            foreach (var unit in ifcProject.UnitsInContext.Units)
            {
                if (unit is IfcNamedUnit)
                {
                    var unitType = (unit as IfcNamedUnit).UnitType;
                    switch (unitType)
                    {
                        case IfcUnitEnum.AREAUNIT:
                            AreaUnit au;
                            _hasAreaUnit = Enum.TryParse(AdjustUnitName(unit.GetName()), true,
                                out au);
                            if (_hasAreaUnit) _modelAreaUnit = au;
                            break;
                        case IfcUnitEnum.LENGTHUNIT:
                            LinearUnit lu;
                            _hasLinearUnit = Enum.TryParse(AdjustUnitName(unit.GetName()), true,
                                out lu);
                            if (_hasLinearUnit) _modelLinearUnit = lu;
                            break;
                        case IfcUnitEnum.VOLUMEUNIT:
                            VolumeUnit vu;
                            _hasVolumeUnit = Enum.TryParse(AdjustUnitName(unit.GetName()), true,
                                out vu);
                            if (_hasVolumeUnit) _modelVolumeUnit = vu;
                            break;
                    }
                }
                else if (unit is IfcMonetaryUnit)
                {
                    CurrencyUnit cu;
                    _hasCurrencyUnit = Enum.TryParse(unit.GetName(), true,
                        out cu);
                    if (_hasCurrencyUnit) _modelCurrencyUnit = cu;
                }

                //this.FacilityDefaultMeasurementStandard needs to be resolved
            }
        }
        /// <summary>
        /// Xbim uses the ifc schema names for units, but these are british english, this corrects to international english and removes unwanted separators
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        private string AdjustUnitName(string unitName)
        {
            var ret = unitName.Replace("METRE", "METERS");
            return ret.Replace("_", "");
        }

        private void GetClassificationDictionary()
        {


            _classifiedObjects = new Dictionary<IfcRoot, List<IfcClassificationReference>>();
            //create a dictionary of classified objects
            foreach (var ifcRelAssociatesClassification in Model.Instances.OfType<IfcRelAssociatesClassification>())
            {
                foreach (var relatedObject in ifcRelAssociatesClassification.RelatedObjects)
                {
                    List<IfcClassificationReference> classificationList;
                    if (!_classifiedObjects.TryGetValue(relatedObject, out classificationList))
                    {
                        classificationList = new List<IfcClassificationReference>();
                        _classifiedObjects.Add(relatedObject, classificationList);
                    }
                    classificationList.Add(((IfcClassificationReference)ifcRelAssociatesClassification.RelatingClassification));
                }
            }
        }

        private List<Category> ConvertToCategories(IEnumerable<IfcClassificationReference> classifications)
        {
            var categories = new List<Category>();
            foreach (var classification in classifications)
            {
                var category = new Category();
                if (classification.ReferencedSource != null)
                    category.Classification = classification.ReferencedSource.Name;
                else
                    category.Classification = "Unspecified";
                if (classification.ItemReference.HasValue && classification.Name.HasValue &&
                    string.CompareOrdinal(classification.ItemReference, classification.Name) == 0)
                {
                    var strRef = classification.ItemReference.Value.ToString();
                    var parts = strRef.Split(new[] {':', ';', '/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                        category.Description = parts[1];
                    category.Code = parts[0];
                }
                else
                {
                    category.Code = classification.ItemReference;
                    category.Description = classification.Name;
                }
                categories.Add(category);
            }
            return categories;
        }
        /// <summary>
        /// Returns the COBie Category for this object, based on the Ifc Classification
        /// </summary>
        /// <param name="classifiedObject"></param>
        /// <returns></returns>
        public List<Category> GetCategories(IfcRoot classifiedObject)
        {
            List<IfcClassificationReference> classifications;
            if (_classifiedObjects.TryGetValue(classifiedObject, out classifications))
                return  ConvertToCategories(classifications);
            //if the object is an IfcObject we might be able to get a classification from its aggregating type
            var ifcObject = classifiedObject as IfcObject;
            if (ifcObject != null)
            {
                var definingTypeObject = GetDefiningTypeObject(ifcObject); //do we have a defining type
                if (definingTypeObject != null)
                {
                    if (_classifiedObjects.TryGetValue(definingTypeObject, out classifications))
                        return ConvertToCategories(classifications);
                }
            }
          
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcRootObject"></param>
        /// <returns></returns>
        public string GetCreatingApplication(IfcRoot ifcRootObject)
        {
            if (ifcRootObject.OwnerHistory.LastModifyingApplication != null)
                return "xBIM from " + ifcRootObject.OwnerHistory.LastModifyingApplication.ApplicationFullName;
            if (ifcRootObject.OwnerHistory.OwningApplication != null)
                return  "xBIM from " +ifcRootObject.OwnerHistory.OwningApplication.ApplicationFullName;
            return "xBIM from " + _creatingApplication;
        }

        

        #region Model unit accessors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaUnit"></param>
        /// <returns></returns>
        public string GetAreaUnit(IfcQuantityArea areaUnit)
        {
            return areaUnit.Unit != null ? areaUnit.Unit.GetName() : ModelAreaUnit.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lengthUnit"></param>
        /// <returns></returns>
        public string GetLinearUnit(IfcQuantityLength lengthUnit)
        {
            return lengthUnit.Unit != null ? lengthUnit.Unit.GetName() : ModelLinearUnit.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeUnit"></param>
        /// <returns></returns>
        public string GetVolumeUnit(IfcQuantityVolume volumeUnit)
        {
            return volumeUnit.Unit != null ? volumeUnit.Unit.GetName() : ModelVolumeUnit.ToString();
        }

        #endregion




        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="ifcObjectDefinition"></param>
        /// <typeparam name="TCoBieValueBaseType"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public TCoBieValueBaseType GetCoBieAttribute<TCoBieValueBaseType>(string valueName, IfcObjectDefinition ifcObjectDefinition)
            where TCoBieValueBaseType : AttributeValue, new()
        {
            XbimAttributedObject attributedObject;
            var result = new TCoBieValueBaseType();
            if (_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject))
            {
                string[] propertyNames;
                if (_cobieFieldMap.TryGetValue(valueName, out propertyNames))
                {
                    foreach (var propertyName in propertyNames)
                    {
                        if (attributedObject.GetAttributeValue(propertyName, ref result))
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Illegal COBie Attribute name:", valueName);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="ifcObject"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public TValue GetCoBieProperty<TValue>(string valueName, IfcObject ifcObject) where TValue:struct
        {
            XbimAttributedObject attributedObject;
            if (_attributedObjects.TryGetValue(ifcObject, out attributedObject))
            {
                string[] propertyNames;
                if (_cobieFieldMap.TryGetValue(valueName, out propertyNames))
                {
                    foreach (var propertyName in propertyNames)
                    {
                        TValue value;
                        if (attributedObject.GetSimplePropertyValue(propertyName, out value))
                            return value;
                    }
                }
                else
                {
                    throw new ArgumentException("Illegal COBie Attribute name:", valueName);
                }
            }
            return default(TValue);
        }

        

        private IfcTypeObject GetDefiningTypeObject(IfcObject ifcObject)
        {
            IfcTypeObject definingType;
            _objectToTypeObjectMap.TryGetValue(ifcObject, out definingType);
            return definingType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObjectDefinition"></param>
        /// <returns></returns>
        public List<Attribute> GetAttributes(IfcObjectDefinition ifcObjectDefinition)
        {
            XbimAttributedObject attributedObject;
            if (_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject))
            {
                var properties = attributedObject.Properties;
                var keyValuePairs = properties.ToArray();
                if (keyValuePairs.Length > 0)
                {
                    var attributeCollection = new List<Attribute>(keyValuePairs.Length);
                    for (int i = 0; i < keyValuePairs.Length; i++)
                    {

                        var property = keyValuePairs[i].Value;
                        var splitName = keyValuePairs[i].Key.Split('.');
                        var pSetName = splitName[0];
                        var attributeType = XbimAttributedObject.ConvertToAttributeType(property);
                        attributeType.ExternalEntity = pSetName;
                        //var pSetDef = attributedObject.GetPropertySetDefinition(pSetName);
                        //if (pSetDef != null)
                        //    attributeType.externalID = ExternalEntityIdentity(pSetDef);
                        attributeCollection.Add(attributeType);
                    }
                    return attributeCollection;
                }
            }
            return null;
        }

       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObject"></param>
        /// <returns></returns>
        public string ExternalEntityIdentity(IfcRoot ifcObject)
        {
            switch (EntityIdentifierMode)
            {
                case EntityIdentifierMode.IfcEntityLabels:
                    
                    return ifcObject.EntityLabel.ToString(CultureInfo.InvariantCulture);
                case EntityIdentifierMode.GloballyUniqueIds:
                    return ifcObject.GlobalId;
                default:
                    return null;
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObject"></param>
        /// <returns></returns>
        public string ExternalEntityName(IfcRoot ifcObject)
        {
            if (
                ExternalReferenceMode == ExternalReferenceMode.IgnoreEntityName ||
                    ExternalReferenceMode == ExternalReferenceMode.IgnoreSystemAndEntityName)
                return null;
            return ifcObject.GetType().Name;
        }

        internal string ExternalSystemName(IfcRoot ifcObject)
        {
            if (ExternalReferenceMode == ExternalReferenceMode.IgnoreSystem ||
                ExternalReferenceMode == ExternalReferenceMode.IgnoreSystemAndEntityName)
                return null;
            return GetCreatingApplication(ifcObject);
        }


 
        private void GetSpaceAssetLookup()
        {
          
            //get all elements that are contained in any spatial structure of this building
            _spaceAssetLookup = new Dictionary<IfcElement, List<IfcSpatialStructureElement>>(); 
           
            var ifcRelContainedInSpaces = _model.Instances.OfType<IfcRelContainedInSpatialStructure>().ToList();
            foreach (var ifcRelContainedInSpace in ifcRelContainedInSpaces)
            {
                foreach (var element in ifcRelContainedInSpace.RelatedElements.OfType<IfcElement>())
                { 
                    List<IfcSpatialStructureElement> spaceList;
                    if (!SpaceAssetLookup.TryGetValue(element, out spaceList))
                    {
                        spaceList = new List<IfcSpatialStructureElement>();
                        SpaceAssetLookup[element] = spaceList;

                    }
                    spaceList.Add(ifcRelContainedInSpace.RelatingStructure);
                }
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcBuilding"></param>
        /// <returns></returns>
        public IEnumerable<IfcElement> GetAllAssets(IfcBuilding ifcBuilding)
        {
            var spatialStructureOfBuilding = new HashSet<IfcSpatialStructureElement>(); // all the spatial decomposition of the building
           
            //get all the spatial structural elements which may contain assets
            DecomposeSpatialStructure(ifcBuilding, spatialStructureOfBuilding);
            //get all elements that are contained in the spatial structure of this building
            var elementsInBuilding = _model.Instances.OfType<IfcRelContainedInSpatialStructure>()
                .Where(r => spatialStructureOfBuilding.Contains(r.RelatingStructure))
                .SelectMany(s=>s.RelatedElements.OfType<IfcElement>()).Distinct();

            return elementsInBuilding;
        }

        private void DecomposeSpatialStructure(IfcSpatialStructureElement ifcSpatialStructuralElement,
            HashSet<IfcSpatialStructureElement> allSpatialStructuralElements)
        {
            List<IfcSpatialStructureElement> spatialElements;
            if (_spatialDecomposition.TryGetValue(ifcSpatialStructuralElement, out spatialElements))
            {
                foreach (var spatialElement in spatialElements)
                {
                    allSpatialStructuralElements.Add(spatialElement);
                    DecomposeSpatialStructure(spatialElement, allSpatialStructuralElements);
                }
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="ifcObjectDefinition"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetCoBieProperty(string valueName, IfcObjectDefinition ifcObjectDefinition)
        {
            XbimAttributedObject attributedObject;
            if (_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject))
            {
                string[] propertyNames;
                if (_cobieFieldMap.TryGetValue(valueName, out propertyNames))
                {
                    foreach (var propertyName in propertyNames)
                    {
                        string value;
                        if (attributedObject.GetSimplePropertyValue(propertyName, out value))
                            return value;
                    }
                }
                else
                {
                    throw new ArgumentException("Illegal COBie Attribute name:", valueName);
                }
            }
            return null;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IfcActorSelect> GetContacts()
        {
            //get any actors and select their
            var actors = new HashSet<IfcActorSelect>(_model.Instances.OfType<IfcActor>().Select(a=>a.TheActor)); //unique actors
            var personOrgs =  new HashSet<IfcActorSelect>(_model.Instances.OfType<IfcPersonAndOrganization>().Where(p => !actors.Contains(p)));
            actors = new HashSet<IfcActorSelect>(actors.Concat(personOrgs));
            var personsAlreadyIn = new HashSet<IfcActorSelect>(actors.Where(a => a is IfcPerson));

            var persons = new HashSet<IfcActorSelect>(_model.Instances.OfType<IfcPerson>().Where(p => !personsAlreadyIn.Contains(p)));
            actors = new HashSet<IfcActorSelect>(actors.Concat(persons));
            return actors;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcZone"></param>
        /// <returns></returns>
        public IEnumerable<IfcSpace> GetSpaces(IfcZone ifcZone)
        {
            HashSet<IfcSpace> spaces;
            if (_zoneSpaces.TryGetValue(ifcZone, out spaces))
                return spaces;
            return Enumerable.Empty<IfcSpace>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcSystem"></param>
        /// <returns></returns>
        public IEnumerable<IfcObjectDefinition> GetSystemAssignments(IfcSystem ifcSystem)
        {
            ObjectDefinitionSet assignments;
            if (SystemAssignment.TryGetValue(ifcSystem, out assignments))
                return assignments;
            return Enumerable.Empty<IfcObjectDefinition>();
            
        }

        /// <summary>
        /// Returns a list of spaces the element is in
        /// </summary>
        /// <param name="ifcElement"></param>
        /// <returns></returns>
        public IEnumerable<IfcSpatialStructureElement> GetSpaces(IfcElement ifcElement)
        {
            List<IfcSpatialStructureElement> spaceList;
            if (_spaceAssetLookup.TryGetValue(ifcElement, out spaceList))
                return spaceList;
            return Enumerable.Empty<IfcSpatialStructureElement>();
        }
    }
}
