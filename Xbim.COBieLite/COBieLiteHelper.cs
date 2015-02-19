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
using Xbim.Common.Logging;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.QuantityResource;
using Xbim.Ifc2x3.SharedFacilitiesElements;
using Xbim.IO;
using System.Xml;
using Xbim.XbimExtensions.SelectTypes;
using Formatting = System.Xml.Formatting;

namespace Xbim.COBieLite
{
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

        internal static readonly ILogger Logger = LoggerFactory.GetLogger();
       
        private readonly XbimModel _model;
        private IfcClassification _classificationSystem; 
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
        private Dictionary<IfcRoot, IfcClassificationReference> _classifiedObjects;

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
        private HashSet<string> _cobieProperties = new HashSet<string>();
        private Dictionary<IfcElement, List<IfcSpace>> _spaceAssetLookup;
        private Dictionary<IfcSpace, IfcBuildingStorey> _spaceFloorLookup;
        private Dictionary<IfcSpatialStructureElement, List<IfcSpatialStructureElement>> _spatialDecomposition;

        #endregion

        private readonly string _configFileName;

        public CoBieLiteHelper(XbimModel model, string classificationSystemName = null, string configurationFile = null)
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

        public IDictionary<IfcTypeObject, List<IfcElement>> DefiningTypeObjectMap
        {
            get { return _definingTypeObjectMap; }
        }
        private void GetTypeMaps()
        {
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
                if (spatialElement.Key is IfcBuildingStorey) //only care if the space is on a floor (COBie rule)
                {
                    foreach (var ifcSpace in spatialElement.Value.OfType<IfcSpace>())
                        _spaceFloorLookup[ifcSpace] = (IfcBuildingStorey)spatialElement.Key;
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
                    if (!_zoneSpaces.TryGetValue(zone, out zoneSpaces))
                    {
                        zoneSpaces = new HashSet<IfcSpace>();
                        _zoneSpaces.Add(zone,zoneSpaces);
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

        public IEnumerable<IfcZone> GetZones(IfcSpace space)
        {
            HashSet<IfcZone> zones;
            if (_spaceZones.TryGetValue(space, out zones))
                return zones;
            return Enumerable.Empty<IfcZone>();
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

        public XbimModel Model
        {
            get { return _model; }
        }

        public Dictionary<IfcTypeObject, IfcAsset> AssetAsignments
        {
            get { return _assetAsignments; }
        }
        public IDictionary<IfcObjectDefinition, List<IfcSystem>> SystemLookup
        {
            get { return _systemLookup; }
        }
        public IDictionary<IfcSystem, ObjectDefinitionSet> SystemAssignment
        {
            get { return _systemAssignment; }
        }

        public IDictionary<IfcElement, List<IfcSpace>> SpaceAssetLookup
        {
            get { return _spaceAssetLookup; }
        }

        public IDictionary<IfcSpace, IfcBuildingStorey> SpaceFloorLookup
        {
            get { return _spaceFloorLookup; }
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
                            _hasAreaUnit = Enum.TryParse(AdjustUnitName(unit.GetName()), true,
                                out _modelAreaUnit);
                            break;
                        case IfcUnitEnum.LENGTHUNIT:
                            _hasLinearUnit = Enum.TryParse(AdjustUnitName(unit.GetName()), true,
                                out _modelLinearUnit);
                            break;
                        case IfcUnitEnum.VOLUMEUNIT:
                            _hasVolumeUnit = Enum.TryParse(AdjustUnitName(unit.GetName()), true,
                                out _modelVolumeUnit);
                            break;
                    }
                }
                else if (unit is IfcMonetaryUnit)
                {
                    _hasCurrencyUnit = Enum.TryParse(unit.GetName(), true,
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
                    Model.Instances.OfType<IfcClassification>(true).FirstOrDefault(rel => rel.Name == classificationSystemName);
            }

            //get the relationships
            var associations =
                Model.Instances.OfType<IfcRelAssociatesClassification>()
                    .Where<IfcRelAssociatesClassification>(rel => rel.RelatingClassification is IfcClassificationReference);
            //filter them for our chosen classification system
            associations =
                associations.Where(
                    assoc =>
                        ((IfcClassificationReference)assoc.RelatingClassification).ReferencedSource ==
                        _classificationSystem);
            _classifiedObjects = new Dictionary<IfcRoot, IfcClassificationReference>();
            //create a dictionary of classified objects
            foreach (var ifcRelAssociatesClassification in associations)
            {
                foreach (var relatedObject in ifcRelAssociatesClassification.RelatedObjects)
                {
                    _classifiedObjects.Add(relatedObject,
                        ((IfcClassificationReference)ifcRelAssociatesClassification.RelatingClassification));
                }
            }

        }

        public string GetClassification(IfcRoot classifiedObject)
        {
            

            IfcClassificationReference classification;
            if (_classifiedObjects.TryGetValue(classifiedObject, out classification))
            {
                if (classification.ItemReference == classification.Name)
                    return classification.ItemReference;
                return classification.ItemReference + ": " + classification.Name;
            }
            //if the object is an IfcObject we might be able to get a classification from its aggregating type
            var ifcObject = classifiedObject as IfcObject;
            if (ifcObject != null)
            {
                var definingTypeObject = GetDefiningTypeObject(ifcObject); //do we have a defining type
                if (definingTypeObject != null)
                {
                    if (_classifiedObjects.TryGetValue(definingTypeObject, out classification))
                    {
                        if (classification.ItemReference == classification.Name)
                            return classification.ItemReference;
                        return classification.ItemReference + ": " + classification.Name;
                    }
                }
            }
            if (classifiedObject.EntityLabel == 11640)
            {

            }
            if (classifiedObject is IfcSpace)
            {
                var val = _attributedObjects[(IfcSpace)classifiedObject];
                string classificationName;
                val.GetSimplePropertyValue("PSet_Revit_Identity Data.OmniClass Table 13 Category", out classificationName);

                if (classificationName != null)
                {
                    return classificationName;
                }
            }
            else if (classifiedObject is IfcTypeObject)
            {
                if (_definingTypeObjectMap.ContainsKey((IfcTypeObject) classifiedObject))
                {
                    var obj = _definingTypeObjectMap[(IfcTypeObject) classifiedObject].FirstOrDefault();

                    if (obj != null)
                    {
                        // PSet_Revit_Type_Other.Classification Code
                        var cfg =
                            new[]
                            {
                                "SimpleProp(PSet_Revit_Type_Other.Classification Code): SimpleProp(PSet_Revit_Type_Other.Classification Description)",
                                "SimpleProp(PSet_Revit_Type_Identity Data.OmniClass Number): SimpleProp(PSet_Revit_Type_Identity Data.OmniClass Title)"
                            };

                        var val = _attributedObjects[obj];
                        string pattern = @"SimpleProp\(([^\)]*)\)";
                        Regex regex = new Regex(pattern);

                        foreach (var classificationRule in cfg)
                        {
                            bool ok = true;
                            string result = classificationRule;
                            var mts = regex.Matches(classificationRule);
                            foreach (Match mt in mts)
                            {
                                string propName = mt.Groups[1].Value;
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
                }
            }

            return null;
        }

        public string GetCreatingApplication(IfcRoot ifcRootObject)
        {
            if (ifcRootObject.OwnerHistory.LastModifyingApplication != null)
                return ifcRootObject.OwnerHistory.LastModifyingApplication.ApplicationFullName;
            if (ifcRootObject.OwnerHistory.OwningApplication != null)
                return ifcRootObject.OwnerHistory.OwningApplication.ApplicationFullName;
            return _creatingApplication;
        }

        

        #region Model unit accessors

        public string GetAreaUnit(IfcQuantityArea areaUnit)
        {
            return areaUnit.Unit != null ? areaUnit.Unit.GetName() : ModelAreaUnit.ToString();
        }

        public string GetLinearUnit(IfcQuantityLength lengthUnit)
        {
            return lengthUnit.Unit != null ? lengthUnit.Unit.GetName() : ModelLinearUnit.ToString();
        }

        public string GetVolumeUnit(IfcQuantityVolume volumeUnit)
        {
            return volumeUnit.Unit != null ? volumeUnit.Unit.GetName() : ModelVolumeUnit.ToString();
        }

        #endregion


        public IEnumerable<FacilityType> GetFacilities()
        {
            var buildings = Model.Instances.OfType<IfcBuilding>();
            foreach (IfcBuilding ifcBuilding in buildings)
                yield return new FacilityType(ifcBuilding, this);
        }

        public TCoBieValueBaseType GetCoBieAttribute<TCoBieValueBaseType>(string valueName, IfcObjectDefinition ifcObjectDefinition)
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

        public List<AttributeType> GetAttributes(IfcObjectDefinition ifcObjectDefinition)
        {
            XbimAttributedObject attributedObject;

            if (_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject))
            {
                var properties = attributedObject.Properties.Where(kv=>!_cobieProperties.Contains(kv.Key)); //exclude the properties we have written as COBie value
                var keyValuePairs = properties as KeyValuePair<string, IfcProperty>[] ?? properties.ToArray();
                if (keyValuePairs.Length>0)
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

        static public void WriteBson(BinaryWriter binaryWriter, FacilityType theFacility)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
            var serialiser = JsonSerializer.Create(serializerSettings);
            // serialize product to BSON
            var writer = new BsonWriter(binaryWriter);
            serialiser.Serialize(writer, theFacility);
        }

        static public void WriteJson(TextWriter textWriter, FacilityType theFacility)
        {
            var serialiser = FacilityType.GetJsonSerializer();
            serialiser.Serialize(textWriter, theFacility);

        }

        static public FacilityType ReadJson(TextReader textReader)
        {
            var serialiser = FacilityType.GetJsonSerializer();
            return (FacilityType)serialiser.Deserialize(textReader, typeof(FacilityType));
        }

        static public FacilityType ReadJson(string path)
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
       

        static public void WriteXml(TextWriter textWriter, FacilityType theFacility)
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
            _spaceAssetLookup = new Dictionary<IfcElement, List<IfcSpace>>(); 
           
            var ifcRelContainedInSpaces = _model.Instances.OfType<IfcRelContainedInSpatialStructure>().Where(r=>r.RelatingStructure is IfcSpace).ToList();
            foreach (var ifcRelContainedInSpace in ifcRelContainedInSpaces)
            {
                foreach (var element in ifcRelContainedInSpace.RelatedElements.OfType<IfcElement>())
                {
                    if (SpaceAssetLookup.ContainsKey(element))
                        SpaceAssetLookup[element].Add((IfcSpace)ifcRelContainedInSpace.RelatingStructure);
                    else
                        SpaceAssetLookup.Add(element, new List<IfcSpace>(new[] { (IfcSpace)ifcRelContainedInSpace.RelatingStructure }));
                }
            }
           
        }
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

        public void WriteIfc(TextWriter textWriter, FacilityType facilityType)
        {
            
        }
    }
}
