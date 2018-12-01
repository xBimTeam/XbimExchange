using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using System.Xml;
using Xbim.Common.Metadata;
using Formatting = System.Xml.Formatting;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Microsoft.Extensions.Logging;
using Xbim.Common;

namespace Xbim.COBieLite
{
    public enum EntityIdentifierMode
    {
        /// <summary>
        /// Use the Entity Label in the IIfc file (e.g. #23)
        /// </summary>
        IIfcEntityLabels = 0,
        /// <summary>
        /// Use the GlobalId of the Entity (e.g. "10mjSDZJj9gPS2PrQaxa3z")
        /// </summary>
        GloballyUniqueIds = 1,
        /// <summary>
        /// Does not write any External Identifier for Entities
        /// </summary>
        None = 2
    }

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
    public class CoBieLiteHelper
    {

        internal static readonly ILogger Logger = XbimLogging.CreateLogger<CoBieLiteHelper>();
       
        private readonly IfcStore _model;
        private IIfcClassification _classificationSystem; 
        private readonly string _creatingApplication;

        #region Model measurement units

        private LinearUnitSimpleType _modelLinearUnit;
        private AreaUnitSimpleType _modelAreaUnit;
        private VolumeUnitSimpleType _modelVolumeUnit;
        private CurrencyUnitSimpleType _modelCurrencyUnit;

        private bool _hasLinearUnit;
        private bool _hasAreaUnit;
        private bool _hasVolumeUnit;
        private bool _hasCurrencyUnit;

        #endregion

        #region Settings

        public EntityIdentifierMode EntityIdentifierMode { get; set; }
        public ExternalReferenceMode ExternalReferenceMode { get; set; }
        #endregion

        #region Lookups
        //Classification for any root object
        private Dictionary<IIfcDefinitionSelect, IIfcClassificationReference> _classifiedObjects;

        private Dictionary<IIfcZone, HashSet<IIfcSpace>>  _zoneSpaces;
        private Dictionary<IIfcSpace, HashSet<IIfcZone>> _spaceZones;

        private Dictionary<IIfcObjectDefinition,XbimAttributedObject> _attributedObjects;

        private Dictionary<string, string[]> _cobieFieldMap;


        private readonly HashSet<ExpressType> _includedTypes = new HashSet<ExpressType>();

        private Dictionary<IIfcObject,IIfcTypeObject> _objectToTypeObjectMap;
        private Dictionary<IIfcTypeObject, List<IIfcElement>> _definingTypeObjectMap;
/*
        private Lookup<string, IIfcElement> _elementTypeToElementObjectMap;
*/
        private Dictionary<IIfcTypeObject, IIfcAsset> _assetAsignments;
        private Dictionary<IIfcSystem, IEnumerable<IIfcObjectDefinition>> _systemAssignment;
        private Dictionary<IIfcObjectDefinition, List<IIfcSystem>> _systemLookup;
        private readonly HashSet<string> _cobieProperties = new HashSet<string>();
        private Dictionary<IIfcElement, List<IIfcSpace>> _spaceAssetLookup;
        private Dictionary<IIfcSpace, IIfcBuildingStorey> _spaceFloorLookup;
        private Dictionary<IIfcSpatialStructureElement, List<IIfcSpatialStructureElement>> _spatialDecomposition;

        #endregion

        private readonly string _configFileName;

        public CoBieLiteHelper(IfcStore model, string classificationSystemName = null, string configurationFile = null)
        {
            _configFileName = configurationFile;

            _model = model;
            _creatingApplication = model.Header.CreatingApplication;
            LoadCobieMaps();
            GetClassificationDictionary(classificationSystemName);
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
                _model.FederatedInstances.OfType<IIfcRelAssignsToGroup>().Where(r => r.RelatingGroup is IIfcSystem)
                .ToDictionary(k => (IIfcSystem)k.RelatingGroup, v => v.RelatedObjects.Cast<IIfcObjectDefinition>());
            _systemLookup = new Dictionary<IIfcObjectDefinition, List<IIfcSystem>>();
            foreach (var systemAssignment in SystemAssignment)
                foreach (var objectDefinition in systemAssignment.Value)
                {
                    if (_systemLookup.ContainsKey(objectDefinition))
                        _systemLookup[objectDefinition].Add(systemAssignment.Key);
                    else
                        _systemLookup.Add(objectDefinition, new List<IIfcSystem>(new[] {systemAssignment.Key} ));
                }
        }

        public IDictionary<IIfcTypeObject, List<IIfcElement>> DefiningTypeObjectMap
        {
            get { return _definingTypeObjectMap; }
        }
        private void GetTypeMaps()
        {
            _definingTypeObjectMap = _model.FederatedInstances.OfType<IIfcRelDefinesByType>().ToDictionary(k => k.RelatingType, kv => kv.RelatedObjects.OfType<IIfcElement>().ToList());
            _objectToTypeObjectMap = new Dictionary<IIfcObject, IIfcTypeObject>();
            foreach (var typeObjectToObjects in _definingTypeObjectMap)
            {
                foreach (var ifcObject in typeObjectToObjects.Value)
                {
                    _objectToTypeObjectMap.Add(ifcObject, typeObjectToObjects.Key);
                }
            }

            //Get asset assignments

            var assetRels = _model.FederatedInstances.OfType<IIfcRelAssignsToGroup>()
                .Where(r => r.RelatingGroup is IIfcAsset &&
                            r.RelatedObjects.Any(
                                o => o is IIfcTypeObject && _definingTypeObjectMap.ContainsKey((IIfcTypeObject) o)));

            _assetAsignments = new Dictionary<IIfcTypeObject, IIfcAsset>();
            foreach (var assetRel in assetRels)
            {
                foreach (var assetType in assetRel.RelatedObjects)
                    AssetAsignments[(IIfcTypeObject)assetType] = (IIfcAsset) assetRel.RelatingGroup; 
            }
            //////get all Assets that don't belong to an IIfc Type or are not classified
            //////get all IIfcElements that aren't classified or have a type
            ////var existingAssets =  _classifiedObjects.Keys.OfType<IIfcElement>()
            ////    .Concat(_objectToTypeObjectMap.Keys.OfType<IIfcElement>()).Distinct();
            //////retrieve all the IIfcElements from the model and exclude them if they are already classified or are a member of an IIfcType
            ////var unCategorizedAssets = _model.FederatedInstances.OfType<IIfcElement>().Except(existingAssets);
            //////convert to a Lookup with the key the type of the IIfcElement and the value a list of IIfcElements
            //////if the object has a classification we use this to distinguish types
            ////_elementTypeToElementObjectMap = (Lookup<string,IIfcElement>) unCategorizedAssets.ToLookup
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

                var asss = Assembly.GetExecutingAssembly();

                using (var input = asss.GetManifestResourceStream("Xbim.COBieLite.COBieAttributes.config"))
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


            var ifcElementInclusion = (AppSettingsSection) config.GetSection("IIfcElementInclusion");

            if (ifcElementInclusion != null)
            {
                foreach (KeyValueConfigurationElement keyVal in ifcElementInclusion.Settings)
                {
                    if (string.Compare(keyVal.Value, "YES", StringComparison.OrdinalIgnoreCase) != 0) continue;
                    var includedType = Model.Metadata.ExpressType(keyVal.Key.ToUpper());
                    if (includedType != null) _includedTypes.Add(includedType);
                }
            }

            if (_configFileName == null)
            {
                File.Delete(tmpFile);
            }
        }


        private void GetPropertySets()
        {
            _attributedObjects = new Dictionary<IIfcObjectDefinition, XbimAttributedObject>();
            var relProps = _model.FederatedInstances.OfType<IIfcRelDefinesByProperties>().ToList();
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
                    foreach (var pset in relProp.RelatingPropertyDefinition.PropertySetDefinitions)
                    {
                        attributedObject.AddPropertySetDefinition(pset);  
                    }
                    
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
            _spatialDecomposition = _model.FederatedInstances.OfType<IIfcRelAggregates>().Where(r=>r.RelatingObject is IIfcSpatialStructureElement)
                .ToDictionary(ifcRelAggregate => (IIfcSpatialStructureElement) ifcRelAggregate.RelatingObject, ifcRelAggregate => ifcRelAggregate.RelatedObjects.OfType<IIfcSpatialStructureElement>().ToList());

            //get the relationship between spaces and storeys
            _spaceFloorLookup = new Dictionary<IIfcSpace, IIfcBuildingStorey>();
            foreach (var spatialElement in _spatialDecomposition)
            {
                if (spatialElement.Key is IIfcBuildingStorey) //only care if the space is on a floor (COBie rule)
                {
                    foreach (var ifcSpace in spatialElement.Value.OfType<IIfcSpace>())
                        _spaceFloorLookup[ifcSpace] = (IIfcBuildingStorey)spatialElement.Key;
                }

            }

            var relZones = _model.FederatedInstances.OfType<IIfcRelAssignsToGroup>().Where(r=>r.RelatingGroup is IIfcZone).ToList();
            _zoneSpaces = new Dictionary<IIfcZone, HashSet<IIfcSpace>>();
            _spaceZones = new Dictionary<IIfcSpace, HashSet<IIfcZone>>();
            foreach (var relZone in relZones)
            {
                var spaces = relZone.RelatedObjects.OfType<IIfcSpace>().ToList();
                if (spaces.Any())
                {
                    //add the spaces to each zone lookup
                    var zone = (IIfcZone) relZone.RelatingGroup;
                    HashSet<IIfcSpace> zoneSpaces;
                    if (!_zoneSpaces.TryGetValue(zone, out zoneSpaces))
                    {
                        zoneSpaces = new HashSet<IIfcSpace>();
                        _zoneSpaces.Add(zone,zoneSpaces);
                    }
                    foreach (var space in spaces) zoneSpaces.Add(space);
                    
                    //now add the zones to the space lookup         
                    foreach (var ifcSpace in spaces)
                    {
                        HashSet<IIfcZone> spaceZones;
                        if (!_spaceZones.TryGetValue(ifcSpace, out spaceZones))
                        {
                            spaceZones = new HashSet<IIfcZone>();
                            _spaceZones.Add(ifcSpace,spaceZones);
                        }
                        spaceZones.Add(zone);
                    }
                }
            }         
        }

        public IEnumerable<IIfcZone> GetZones(IIfcSpace space)
        {
            HashSet<IIfcZone> zones;
            if (_spaceZones.TryGetValue(space, out zones))
                return zones;
            return Enumerable.Empty<IIfcZone>();
        }
        public LinearUnitSimpleType ModelLinearUnit
        {
            get { return _modelLinearUnit; }
        }

        public AreaUnitSimpleType ModelAreaUnit
        {
            get { return _modelAreaUnit; }
        }

        public VolumeUnitSimpleType ModelVolumeUnit
        {
            get { return _modelVolumeUnit; }
        }

        public CurrencyUnitSimpleType ModelCurrencyUnit
        {
            get { return _modelCurrencyUnit; }
        }

        public bool HasLinearUnit
        {
            get { return _hasLinearUnit; }
        }

        public bool HasAreaUnit
        {
            get { return _hasAreaUnit; }
        }

        public bool HasVolumeUnit
        {
            get { return _hasVolumeUnit; }
        }

        public bool HasCurrencyUnit
        {
            get { return _hasCurrencyUnit; }
        }

        public IfcStore Model
        {
            get { return _model; }
        }

        public Dictionary<IIfcTypeObject, IIfcAsset> AssetAsignments
        {
            get { return _assetAsignments; }
        }
        public IDictionary<IIfcObjectDefinition, List<IIfcSystem>> SystemLookup
        {
            get { return _systemLookup; }
        }
        public IDictionary<IIfcSystem, IEnumerable<IIfcObjectDefinition>> SystemAssignment
        {
            get { return _systemAssignment; }
        }

        public IDictionary<IIfcElement, List<IIfcSpace>> SpaceAssetLookup
        {
            get { return _spaceAssetLookup; }
        }

        public IDictionary<IIfcSpace, IIfcBuildingStorey> SpaceFloorLookup
        {
            get { return _spaceFloorLookup; }
        }

        private void GetUnits()
        {
            var ifcProject = Model.Instances.FirstOrDefault<IIfcProject>();
            if (ifcProject == null) return;
            foreach (var unit in ifcProject.UnitsInContext.Units)
            {
                var namedUnit = unit as IIfcNamedUnit;
                if (namedUnit != null)
                {
                    var unitType = namedUnit.UnitType;
                    switch (unitType)
                    {
                        case IfcUnitEnum.AREAUNIT:
                            _hasAreaUnit = Enum.TryParse(AdjustUnitName(namedUnit.FullName), true,
                                out _modelAreaUnit);
                            break;
                        case IfcUnitEnum.LENGTHUNIT:
                            _hasLinearUnit = Enum.TryParse(AdjustUnitName(namedUnit.FullName), true,
                                out _modelLinearUnit);
                            break;
                        case IfcUnitEnum.VOLUMEUNIT:
                            _hasVolumeUnit = Enum.TryParse(AdjustUnitName(namedUnit.FullName), true,
                                out _modelVolumeUnit);
                            break;
                    }
                }
                else if (unit is IIfcMonetaryUnit)
                {
                    _hasCurrencyUnit = Enum.TryParse(unit.ToString(), true,
                        out _modelCurrencyUnit);
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

        private void GetClassificationDictionary(string classificationSystemName = null)
        {
            if (string.IsNullOrWhiteSpace(classificationSystemName))
                _classificationSystem = null;
            else
            {
                _classificationSystem =
                    Model.Instances.OfType<IIfcClassification>(true).FirstOrDefault(rel => rel.Name == classificationSystemName);
            }

            //get the relationships
            var associations =
                Model.Instances.OfType<IIfcRelAssociatesClassification>()
                    .Where<IIfcRelAssociatesClassification>(rel => rel.RelatingClassification is IIfcClassificationReference);
            //filter them for our chosen classification system
            associations =
                associations.Where(
                    assoc =>
                        ((IIfcClassificationReference)assoc.RelatingClassification).ReferencedSource ==
                        _classificationSystem);
            _classifiedObjects = new Dictionary<IIfcDefinitionSelect, IIfcClassificationReference>();
            //create a dictionary of classified objects
            foreach (var ifcRelAssociatesClassification in associations)
            {
                foreach (var relatedObject in ifcRelAssociatesClassification.RelatedObjects)
                {
                    _classifiedObjects.Add(relatedObject,
                        ((IIfcClassificationReference)ifcRelAssociatesClassification.RelatingClassification));
                }
            }

        }

        public string GetClassification(IIfcDefinitionSelect classifiedObject)
        {
            

            IIfcClassificationReference classification;
            if (_classifiedObjects.TryGetValue(classifiedObject, out classification))
            {
                if (classification.Identification == classification.Name)
                    return classification.Identification;
                return classification.Identification + ": " + classification.Name;
            }
            //if the object is an IIfcObject we might be able to get a classification from its aggregating type
            var ifcObject = classifiedObject as IIfcObject;
            if (ifcObject != null)
            {
                var definingTypeObject = GetDefiningTypeObject(ifcObject); //do we have a defining type
                if (definingTypeObject != null)
                {
                    if (_classifiedObjects.TryGetValue(definingTypeObject, out classification))
                    {
                        if (classification.Identification == classification.Name)
                            return classification.Identification;
                        return classification.Identification + ": " + classification.Name;
                    }
                }
            }
            
            if (classifiedObject is IIfcSpace)
            {
                var val = _attributedObjects[(IIfcSpace)classifiedObject];
                string classificationName;
                val.GetSimplePropertyValue("PSet_Revit_Identity Data.OmniClass Table 13 Category", out classificationName);

                if (classificationName != null)
                {
                    return classificationName;
                }
            }
            else
            {
                var classifiedIIfcTypeObject = classifiedObject as IIfcTypeObject;
                if (classifiedIIfcTypeObject == null) 
                    return null;
                if (!_definingTypeObjectMap.ContainsKey(classifiedIIfcTypeObject)) 
                    return null;
                var obj = _definingTypeObjectMap[classifiedIIfcTypeObject].FirstOrDefault();

                if (obj == null) 
                    return null;
                // PSet_Revit_Type_Other.Classification Code
                var cfg =
                    new[]
                    {
                        "SimpleProp(PSet_Revit_Type_Other.Classification Code): SimpleProp(PSet_Revit_Type_Other.Classification Description)",
                        "SimpleProp(PSet_Revit_Type_Identity Data.OmniClass Number): SimpleProp(PSet_Revit_Type_Identity Data.OmniClass Title)"
                    };
                try
                {
                    if (!_attributedObjects.ContainsKey(obj))
                        return null;
                    var val = _attributedObjects[obj];
                    const string pattern = @"SimpleProp\(([^\)]*)\)";
                    var regex = new Regex(pattern);

                    foreach (var classificationRule in cfg)
                    {
                        var ok = true;
                        var result = classificationRule;
                        var mts = regex.Matches(classificationRule);
                        foreach (Match mt in mts)
                        {
                            var propName = mt.Groups[1].Value;
                            string propVal;
                            val.GetSimplePropertyValue(propName, out propVal);
                            if (propVal == null)
                            {
                                ok = false;
                                break;
                            }
                            result = result.Replace("SimpleProp(" + propName + ")", propVal);
                        }
                        if (ok)
                            return result;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public string GetCreatingApplication(IIfcRoot ifcRootObject)
        {
            if (ifcRootObject.OwnerHistory.LastModifyingApplication != null)
                return ifcRootObject.OwnerHistory.LastModifyingApplication.ApplicationFullName;
            if (ifcRootObject.OwnerHistory.OwningApplication != null)
                return ifcRootObject.OwnerHistory.OwningApplication.ApplicationFullName;
            return _creatingApplication;
        }

        

        #region Model unit accessors

        public string GetAreaUnit(IIfcQuantityArea areaUnit)
        {
            return areaUnit.Unit != null ? areaUnit.Unit.FullName : ModelAreaUnit.ToString();
        }

        public string GetLinearUnit(IIfcQuantityLength lengthUnit)
        {
            return lengthUnit.Unit != null ? lengthUnit.Unit.FullName : ModelLinearUnit.ToString();
        }

        public string GetVolumeUnit(IIfcQuantityVolume volumeUnit)
        {
            return volumeUnit.Unit != null ? volumeUnit.Unit.FullName : ModelVolumeUnit.ToString();
        }

        #endregion


        public IEnumerable<FacilityType> GetFacilities()
        {
            var buildings = Model.Instances.OfType<IIfcBuilding>();
            foreach (IIfcBuilding ifcBuilding in buildings)
                yield return new FacilityType(ifcBuilding, this);
        }

        public TCoBieValueBaseType GetCoBieAttribute<TCoBieValueBaseType>(string valueName, IIfcObjectDefinition ifcObjectDefinition)
            where TCoBieValueBaseType : ValueBaseType, new()
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

        public TValue GetCoBieProperty<TValue>(string valueName, IIfcObject ifcObject) where TValue:struct
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

        

        private IIfcTypeObject GetDefiningTypeObject(IIfcObject ifcObject)
        {
            IIfcTypeObject definingType;
            _objectToTypeObjectMap.TryGetValue(ifcObject, out definingType);
            return definingType;
        }

        public List<AttributeType> GetAttributes(IIfcObjectDefinition ifcObjectDefinition)
        {
            XbimAttributedObject attributedObject;

            if (_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject))
            {
                //var properties = attributedObject.Properties.Where(kv=>!_cobieProperties.Contains(kv.Key)); //exclude the properties we have written as COBie value
                //var keyValuePairs = properties as KeyValuePair<string, IIfcProperty>[] ?? properties.ToArray();
                //if (keyValuePairs.Length>0)
                //{
                //    var attributeCollection = new List<AttributeType>(keyValuePairs.Length);
                //    for (int i = 0; i < keyValuePairs.Length; i++)
                //    {
                        
                //        var property = keyValuePairs[i].Value;
                //        var splitName = keyValuePairs[i].Key.Split('.');
                //        var pSetName = splitName[0];
                //        var attributeType = XbimAttributedObject.ConvertToAttributeType(property);
                //        attributeType.propertySetName = pSetName;
                //        //var pSetDef = attributedObject.GetPropertySetDefinition(pSetName);
                //        //if (pSetDef != null)
                //        //    attributeType.externalID = ExternalEntityIdentity(pSetDef);
                //        attributeCollection.Add(attributeType);                   
                //    }
                //    return attributeCollection;
                //}
                var properties = attributedObject.Properties;
                var keyValuePairs = properties.ToArray();
                if (keyValuePairs.Length > 0)
                {
                    var attributeCollection = new List<AttributeType>(keyValuePairs.Length);
                    for (int i = 0; i < keyValuePairs.Length; i++)
                    {

                        var property = keyValuePairs[i].Value;
                        var splitName = keyValuePairs[i].Key.Split('.');
                        var pSetName = splitName[0];
                        var attributeType = XbimAttributedObject.ConvertToAttributeType(property);
                        attributeType.propertySetName = pSetName;
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

        #region Exporters

        public static void WriteBson(BinaryWriter binaryWriter, FacilityType theFacility)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
            var serialiser = JsonSerializer.Create(serializerSettings);
            // serialize product to BSON
            
            var writer = new BsonDataWriter(binaryWriter);
            serialiser.Serialize(writer, theFacility);
        }

        public static void WriteJson(TextWriter textWriter, FacilityType theFacility)
        {
            var serialiser = FacilityType.GetJsonSerializer();
            serialiser.Serialize(textWriter, theFacility);

        }

        public static FacilityType ReadJson(TextReader textReader)
        {
            var serialiser = FacilityType.GetJsonSerializer();
            return (FacilityType)serialiser.Deserialize(textReader, typeof(FacilityType));
        }

        public static FacilityType ReadJson(string path)
        {
            using (var textReader = File.OpenText(path))
            {
                var serialiser = FacilityType.GetJsonSerializer();
                var facility = (FacilityType)serialiser.Deserialize(textReader, typeof(FacilityType));    
                textReader.Close();
                return facility;
            }
            
        }

        public static FacilityType ReadXml(string cobieModelFileName)
        {
            var x = FacilityType.GetSerializer();
            var reader = new XmlTextReader(cobieModelFileName);
            var reqFacility = (FacilityType)x.Deserialize(reader);
            reader.Close();
            return reqFacility;
        }
       

        public static void WriteXml(TextWriter textWriter, FacilityType theFacility)
        {
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName("cobielite", "http://docs.buildingsmartalliance.org/nbims03/cobie/cobielite"),
                new XmlQualifiedName("core", "http://docs.buildingsmartalliance.org/nbims03/cobie/core"),
                new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance")
            });

            var x = FacilityType.GetSerializer();

            using (var xtw = new XbimCoBieLiteXmlWriter(textWriter))
            {
                xtw.Formatting = Formatting.Indented;
                // Now serialize our object.
                x.Serialize(xtw, theFacility, namespaces);
            }

        }

        #endregion


        public string ExternalEntityIdentity(IIfcRoot ifcObject)
        {
            switch (EntityIdentifierMode)
            {
                case EntityIdentifierMode.IIfcEntityLabels:
                    
                    return ifcObject.EntityLabel.ToString(CultureInfo.InvariantCulture);
                case EntityIdentifierMode.GloballyUniqueIds:
                    return ifcObject.GlobalId;
                default:
                    return null;
            }          
        }

        public string ExternalEntityName(IIfcRoot ifcObject)
        {
            if (
                ExternalReferenceMode == ExternalReferenceMode.IgnoreEntityName ||
                    ExternalReferenceMode == ExternalReferenceMode.IgnoreSystemAndEntityName)
                return null;
            return ifcObject.GetType().Name;
        }

        internal string ExternalSystemName(IIfcRoot ifcObject)
        {
            if (ExternalReferenceMode == ExternalReferenceMode.IgnoreSystem ||
                ExternalReferenceMode == ExternalReferenceMode.IgnoreSystemAndEntityName)
                return null;
            return GetCreatingApplication(ifcObject);
        }


 
        private void GetSpaceAssetLookup()
        {
          
            //get all elements that are contained in any spatial structure of this building
            _spaceAssetLookup = new Dictionary<IIfcElement, List<IIfcSpace>>(); 
           
            var ifcRelContainedInSpaces = _model.FederatedInstances.OfType<IIfcRelContainedInSpatialStructure>().Where(r=>r.RelatingStructure is IIfcSpace).ToList();
            foreach (var ifcRelContainedInSpace in ifcRelContainedInSpaces)
            {
                foreach (var element in ifcRelContainedInSpace.RelatedElements.OfType<IIfcElement>())
                {
                    if (SpaceAssetLookup.ContainsKey(element))
                        SpaceAssetLookup[element].Add((IIfcSpace)ifcRelContainedInSpace.RelatingStructure);
                    else
                        SpaceAssetLookup.Add(element, new List<IIfcSpace>(new[] { (IIfcSpace)ifcRelContainedInSpace.RelatingStructure }));
                }
            }
           
        }
        public IEnumerable<IIfcElement> GetAllAssets(IIfcBuilding ifcBuilding)
        {
            var spatialStructureOfBuilding = new HashSet<IIfcSpatialStructureElement>(); // all the spatial decomposition of the building
           
            //get all the spatial structural elements which may contain assets
            DecomposeSpatialStructure(ifcBuilding, spatialStructureOfBuilding);
            //get all elements that are contained in the spatial structure of this building
            var elementsInBuilding = _model.FederatedInstances.OfType<IIfcRelContainedInSpatialStructure>()
                .Where(r => spatialStructureOfBuilding.Contains(r.RelatingStructure))
                .SelectMany(s=>s.RelatedElements.OfType<IIfcElement>()).Distinct();

            return elementsInBuilding;
        }

        private void DecomposeSpatialStructure(IIfcSpatialStructureElement ifcSpatialStructuralElement,
            HashSet<IIfcSpatialStructureElement> allSpatialStructuralElements)
        {
            List<IIfcSpatialStructureElement> spatialElements;
            if (_spatialDecomposition.TryGetValue(ifcSpatialStructuralElement, out spatialElements))
            {
                foreach (var spatialElement in spatialElements)
                {
                    allSpatialStructuralElements.Add(spatialElement);
                    DecomposeSpatialStructure(spatialElement, allSpatialStructuralElements);
                }
            }
        }

       

        public string GetCoBieProperty(string valueName, IIfcObjectDefinition ifcObjectDefinition)
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

        public IEnumerable<IIfcActorSelect> GetContacts()
        {
            //get any actors and select their
            var actors = new HashSet<IIfcActorSelect>(_model.FederatedInstances.OfType<IIfcActor>().Select(a=>a.TheActor)); //unique actors
            var personOrgs =  new HashSet<IIfcActorSelect>(_model.FederatedInstances.OfType<IIfcPersonAndOrganization>().Where(p => !actors.Contains(p)));
            actors = new HashSet<IIfcActorSelect>(actors.Concat(personOrgs));
            var personsAlreadyIn = new HashSet<IIfcActorSelect>(actors.Where(a => a is IIfcPerson));

            var persons = new HashSet<IIfcActorSelect>(_model.FederatedInstances.OfType<IIfcPerson>().Where(p => !personsAlreadyIn.Contains(p)));
            actors = new HashSet<IIfcActorSelect>(actors.Concat(persons));
           return actors;
        }

        public void WriteIIfc(TextWriter textWriter, FacilityType facilityType)
        {
            
        }
    }
}
