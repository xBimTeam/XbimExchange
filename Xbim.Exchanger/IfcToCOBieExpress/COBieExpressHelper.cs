using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Common.Logging;
using Xbim.FilterHelper;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using XbimExchanger.IfcToCOBieExpress.EqCompare;

namespace XbimExchanger.IfcToCOBieExpress
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
    /// Control what we extract from IFC as systems
    /// </summary>
    [Flags]
    public enum SystemExtractionMode
    {
        System = 0x1, //default and should always be set
        PropertyMaps = 0x2, //include properties as set by GetPropMap("SystemMaps")
        Types = 0x4, //include types as system listing all defined objects in componentnsnames
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
    // ReSharper disable once InconsistentNaming
    public class COBieExpressHelper
    {

        internal static readonly ILogger Logger = LoggerFactory.GetLogger();
        /// <summary>
        /// Object to use to report progress on Exchangers
        /// </summary>
        public Xbim.COBieLiteUK.ProgressReporter ReportProgress
        { get; set; }

        private readonly IfcStore _model;
        private readonly string _creatingApplication;

        #region Model measurement units

        #endregion

        #region Settings

        public IModel Target { get; set; }

        public IfcToCoBieExpressExchanger Exchanger { get; set; }

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
        private Dictionary<IIfcDefinitionSelect, List<IIfcClassificationReference>> _classifiedObjects;

        private Dictionary<IIfcSpace, HashSet<IIfcZone>> _spaceZones;

        private Dictionary<IIfcObjectDefinition, XbimAttributedObject> _attributedObjects;

        private Dictionary<string, string[]> _cobieFieldMap;
        
        private Dictionary<IIfcObject, XbimIfcProxyTypeObject> _objectToTypeObjectMap;

        private readonly Dictionary<XbimIfcProxyTypeObject, List<IIfcElement>> _definingTypeObjectMap =new Dictionary<XbimIfcProxyTypeObject, List<IIfcElement>>();

/*
        private Lookup<string, IfcElement> _elementTypeToElementObjectMap;
*/
        private Dictionary<IIfcTypeObject, IIfcAsset> _assetAsignments;
        private Dictionary<IIfcSystem, IEnumerable<IIfcObjectDefinition>> _systemAssignment;
        private Dictionary<IIfcObjectDefinition, List<IIfcSystem>> _systemLookup;
        private Dictionary<IIfcElement, List<IIfcSpatialElement>> _spaceAssetLookup;
        private Dictionary<IIfcSpace, IIfcBuildingStorey> _spaceFloorLookup;
        private Dictionary<IIfcSpatialStructureElement, List<IIfcSpatialStructureElement>> _spatialDecomposition;
        private readonly Dictionary<string, int> _typeNames = new Dictionary<string, int>();

        #region Document Lookups
        /// <summary>
        /// Document to Object mapping
        /// </summary>
        public Dictionary<IIfcDefinitionSelect, IEnumerable<IIfcDocumentSelect>> DocumentLookup
        { get; private set; }

        /// <summary>
        /// Documents not attached to ant IIfcRoot object
        /// </summary>
        public IEnumerable<IIfcDocumentSelect> OrphanDocs
        { get; private set; }

        /// <summary>
        /// Document to IIfcRelAssociatesDocument mapping, fall back info from IIfcRelAssociatesDocument history, if nothing set on IIfcDocumentInformation dates
        /// </summary>
        public Dictionary<IIfcDocumentSelect, IIfcRelAssociatesDocument> DocumentOwnerLookup
        { get; private set; }

        #endregion

        #region Spare 

        public Dictionary<IIfcRoot, IEnumerable<IIfcConstructionProductResource>> SpareLookup
        { get; private set; }
        #endregion

        /// <summary>
        /// Property Sets used to establish systems as per responsibility matrix 
        /// </summary>
        public Dictionary<IIfcPropertySet, IEnumerable<IIfcObjectDefinition>> SystemViaPropAssignment { get; private set; }
        #endregion

        #region Filters

        private OutPutFilters Filter  { get; set; }

        #endregion

        #region Unknown pick values

        public static CobieRole UnknownRole;
        public static CobieCategory UnknownCategory;

        #endregion

        private readonly string _configFileName;
        private readonly Dictionary<IIfcActorSelect, CobieContact> _contacts = new Dictionary<IIfcActorSelect, CobieContact>();
        private Dictionary<IIfcActorSelect, IIfcActor> _actors;
        private readonly CobieContact _xbimCreatedBy;
        private readonly CobieExternalSystem _externalSystemXbim;
        private readonly DateTime _now;
        private readonly List<CobieCreatedInfo> _createdInfoCache = new List<CobieCreatedInfo>();
        private CobieZone _xbimDefaultZone;
        private CobieSystem _xbimDefaultSystem;

        private readonly MappingIfcClassificationReferenceToCategory _categoryMapping;
        private readonly MappingStringToExternalObject _externalObjectMapping;
        private readonly MappingStringToExternalSystem _externalSystemMapping;
        private readonly MappingIfcActorToContact _contactMapping;
        private readonly MappingIfcDocumentSelectToDocument _documentMapping;

        /// <summary>
        /// Creates a default contact and adds it to the SundryContacts
        /// </summary>

        public CobieExternalSystem XbimSystem
        {
            get { return _externalSystemXbim; }
        }

        public CobieContact XbimContact
        {
            get
            {
                if (!SundryContacts.ContainsKey(_xbimCreatedBy.Email))
                    SundryContacts.Add(_xbimCreatedBy.Email, _xbimCreatedBy);
                return _xbimCreatedBy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="configurationFile"></param>
        /// <param name="exchanger"></param>
        /// <param name="reportProgress"></param>
        /// <param name="extId"></param>
        /// <param name="sysMode"></param>
        public COBieExpressHelper(IfcToCoBieExpressExchanger exchanger, Xbim.COBieLiteUK.ProgressReporter reportProgress, OutPutFilters filter = null, string configurationFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types)
        {
            //set props
            _configFileName = configurationFile;
            Filter = filter 
                ?? new OutPutFilters();
            _model = exchanger.SourceRepository;
            Target = exchanger.TargetRepository;
            Exchanger = exchanger;
            EntityIdentifierMode = extId;
            SystemMode = sysMode;
            _creatingApplication = _model.Header.CreatingApplication;
            //pass the exchanger progress reporter over to helper
            ReportProgress = reportProgress; 

            //init
            ReportProgress.Reset(1, 1, "Creating Facility - Extracting Objects");
            LoadCobieMaps(); //1%
            ReportProgress.IncrementAndUpdate();
            GetContacts(); //5%
            GetClassificationDictionary();//8%
            GetSpacesAndZones();//13%
            GetUnits();
            GetSpare();
            GetTypeMaps();//25%
            GetDocumentSelects();
            GetPropertySets();//33%
            GetSystems();//38%
            GetSpaceAssetLookup();//40%

            UnknownRole = Target.Instances.New<CobieRole>(r => r.Value = "unknown");
            UnknownCategory = Target.Instances.New<CobieCategory>(r => r.Value = "unknown");

            _xbimCreatedBy = Target.Instances.New<CobieContact>(c =>
            {
                c.Email = "unknown@OpenBIM.org";
                c.GivenName = "XbimTeam";
                c.Category = UnknownRole;
                c.Created = Target.Instances.New<CobieCreatedInfo>(ci => ci.CreatedOn = DateTime.Now);
            });
            _xbimCreatedBy.Created.CreatedBy = _xbimCreatedBy;
            _now = DateTime.Now;

            _categoryMapping = exchanger.GetOrCreateMappings<MappingIfcClassificationReferenceToCategory>();
            _externalObjectMapping = exchanger.GetOrCreateMappings<MappingStringToExternalObject>();
            _externalSystemMapping = exchanger.GetOrCreateMappings<MappingStringToExternalSystem>();
            _contactMapping = exchanger.GetOrCreateMappings<MappingIfcActorToContact>();
            _documentMapping = exchanger.GetOrCreateMappings<MappingIfcDocumentSelectToDocument>();

            _externalSystemXbim = _externalSystemMapping.GetOrCreateTargetObject("xBIM Toolkit");
            _externalSystemXbim.Name = "xBIM Toolkit";

            var creatingApp = Model.Header.CreatingApplication;
            var externalSystem = _externalSystemMapping.GetOrCreateTargetObject(creatingApp);
            externalSystem.Name = creatingApp;
        }

        /// <summary>
        /// Get Spare lookup and set SpareLookup property
        /// </summary>
        public void GetSpare()
        {
            var spareToObjs = GetSpareResource();

            //reverse lookup to entity to list of documents
            SpareLookup = spareToObjs
                            .SelectMany(pair => pair.Value
                            .Select(val => new { Key = val, Value = pair.Key }))
                            .GroupBy(item => item.Key)
                            .ToDictionary(gr => gr.Key, gr => gr.Select(item => item.Value));
        }

        /// <summary>
        /// Convert all IIfcRelAssignsToResource to a dictionary of IIfcConstructionProductResource, List of IIfcRoot
        /// </summary>
        /// <returns>Dictionary of IIfcConstructionProductResource, List of IIfcRoot</returns>
        private Dictionary<IIfcConstructionProductResource, List<IIfcRoot>> GetSpareResource()
        {
            Dictionary<IIfcResourceSelect, List<IIfcObjectDefinition>> resourceToObjs;

            var ifcRelAssignsToResource = _model.Instances.OfType<IIfcRelAssignsToResource>().Where(
                r => r.RelatingResource is IIfcConstructionProductResource && 
                    (
                        r.RelatedObjectsType == null 
                        || r.RelatedObjectsType == IfcObjectTypeEnum.PRODUCT
                        || r.RelatedObjectsType == IfcObjectTypeEnum.NOTDEFINED)
                    ).ToList(); //linked to IIfcRoot objects

            var dups = ifcRelAssignsToResource.GroupBy(d => d.RelatingResource).SelectMany(grp => grp.Skip(1)).ToList(); //get any duplicate related resource objects
            if (dups.Any())
            {
                //remove the duplicates related resource objects and convert to dictionary
                resourceToObjs = ifcRelAssignsToResource.Except(dups).ToDictionary(p => p.RelatingResource, p => p.RelatedObjects.ToList());
                //merge any duplicate related resource objects resource into list of single link of IfcConstructionProductResource to IfcRoot list, as duplicate related object could hold different IfcConstructionProductResource so lets not lose them
                var dupsMerge = dups.GroupBy(d => d.RelatingResource).Select(p => new { x = p.Key, y = p.SelectMany(c => c.RelatedObjects) });

                //add the duplicate lists to the resourceToObjs list
                foreach (var item in dupsMerge)
                {
                    resourceToObjs[item.x] = resourceToObjs[item.x].Union(item.y).ToList(); //union will exclude any duplicates
                }
            }
            else
            {
                //no duplicates, so just convert to dictionary
                resourceToObjs = ifcRelAssignsToResource.ToDictionary(p => p.RelatingResource, p => p.RelatedObjects.ToList());
            }
            //finally convert to correct types in Dictionary
            return resourceToObjs.ToDictionary(r => r.Key as IIfcConstructionProductResource, r => r.Value.ConvertAll(x => (IIfcRoot)x));
        }



        /// <summary>
        /// Add document to List of Documents
        /// </summary>
        /// <param name="target">Target object holding the document list - CobieObject</param>
        /// <param name="ifcRoot">Object holding the documents</param>
        internal void AddDocuments(CobieAsset target, IIfcDefinitionSelect ifcRoot)
        {
            if (ifcRoot == null) 
                return;
            var documents = GetDocuments(ifcRoot);

            foreach (var document in documents)
            {
                List<CobieDocument> cDocs;
                if (_documentMapping.GetOrCreateTargetObject(document.EntityLabel, out cDocs))
                    _documentMapping.AddMapping(document, cDocs);
                target.Documents.AddRange(cDocs);
            }
        }

        /// <summary>
        /// Return the documents associated with the object
        /// </summary>
        /// <param name="ifcSelect">Object to get associated documents</param>
        /// <returns></returns>
        public IEnumerable<IIfcDocumentSelect> GetDocuments(IIfcDefinitionSelect ifcSelect)
        {
            return DocumentLookup.ContainsKey(ifcSelect) 
                ? DocumentLookup[ifcSelect] 
                : Enumerable.Empty<IIfcDocumentSelect>();
        }

        /// <summary>
        /// Extract Document information
        /// </summary>
        private void GetDocumentSelects()
        {
            var docToObjs = GetAssociatedDocuments();

            //get orphan docs, not attached to IfcRoot objects
            OrphanDocs = GetOrphanDocuments(docToObjs);

            //reverse lookup to entity to list of documents
            DocumentLookup = docToObjs
                            .SelectMany(pair => pair.Value
                            .Select(val => new { Key = val, Value = pair.Key }))
                            .GroupBy(item => item.Key)
                            .ToDictionary(gr => gr.Key, gr => gr.Select(item => item.Value));

        }
       
        /// <summary>
        /// Get Orphan documents
        /// </summary>
        /// <param name="docToObjs">Document linked to objects</param>
        /// <returns>List of IIfcDocumentSelect</returns>
        private IEnumerable<IIfcDocumentSelect> GetOrphanDocuments(Dictionary<IIfcDocumentSelect, List<IIfcDefinitionSelect>> docToObjs)
        {
            //------GET ORPHAN DOCUMENTINFOS------
            //Get all documents information objects held in model
            var docAllInfos = _model.Instances.OfType<IIfcDocumentInformation>().ToList();
            //Get the child document relationships
            var childDocRels = _model.Instances.OfType<IIfcDocumentInformationRelationship>().ToList();

            //see if we have any documents not attached to IIfcRoot objects, but could be attached as children documents to a parent document...

            //get the already attached to entity documents 
            var docInfosAttached = docToObjs.Select(dic => dic.Key).OfType<IIfcDocumentInformation>().ToList();
            var docInfosNotAttached = docAllInfos.Except(docInfosAttached);
           
            //get document infos attached to the IIfcDocumentReference, which are directly linked to IIfcRoot objects
            var docRefsAttached = docToObjs.Select(dictionary => dictionary.Key).OfType<IIfcDocumentReference>().ToList();//attached to IIfcRoot docRefs
            if (docRefsAttached.Any())
            {
                var docRefsInfos = docAllInfos.Where(info => info.HasDocumentReferences.Any(doc => docRefsAttached.Contains(doc))).ToList();
                docInfosNotAttached = docAllInfos.Except(docRefsInfos); //remove the DocInfos attached to the DocRefs that are attached to IIfcRoot Objects
                docInfosAttached = docInfosAttached.Union(docRefsInfos).ToList(); //add the DocInfos attached to the DocRefs that are attached to IIfcRoot Objects
            }
           
            var docChildren = docInfosAttached.ToList(); //first check on docs attached to IIfcRoot Objects, and attached to IIfcDocumentReference(which are attached to IIfcRoot)
            var idx = 0;
            do
            {
                //get the relationships that are attached to the docs already associated with an IIfcRoot object on first pass, then associated with all children, drilling down until nothing found
                docChildren = childDocRels.Where(docRel => docChildren.Contains(docRel.RelatingDocument)).SelectMany(docRel => docRel.RelatedDocuments).ToList(); //docs that are children to attached entity docs, drilling down
                docInfosNotAttached = docInfosNotAttached.Except(docChildren); //attached by association to the root parent document, so remove from none attached document list


            } while (docChildren.Any() && (++idx < 100)); //assume that docs are not embedded deeper than 100

            //------GET ORPHAN DOCUMENTREFERENCES------
            //get all the doc reference objects held in the model
            var docAllRefs = _model.Instances.OfType<IIfcDocumentReference>();
           
            //checked on direct attached to object document references
            var docRefsNotAttached = docAllRefs.Except(docRefsAttached).ToList();

            //Check for document references held in the IIfcDocumentInformation objects
            var docRefsAttachedDocInfo = docAllInfos.SelectMany(docInfo => docInfo.HasDocumentReferences);
            //remove from Not Attached list
            docRefsNotAttached = docRefsNotAttached.Except(docRefsAttachedDocInfo).ToList();

            return docInfosNotAttached.Cast<IIfcDocumentSelect>().Concat(docRefsNotAttached);
        }


        /// <summary>
        /// Document linked to objects
        /// </summary>
        /// <returns>IIfcDocumentSelect attached to IIfcRoot objects,</returns>
        private Dictionary<IIfcDocumentSelect, List<IIfcDefinitionSelect>> GetAssociatedDocuments()
        {
            var ifcRelAssociatesDocuments = _model.Instances.OfType<IIfcRelAssociatesDocument>().ToList(); //linked to IIfcRoot objects

            //get fall back owner history
            DocumentOwnerLookup = ifcRelAssociatesDocuments.ToDictionary(p => p.RelatingDocument, p => p);

            var dups = ifcRelAssociatesDocuments.GroupBy(d => d.RelatingDocument).SelectMany(grp => grp.Skip(1)).ToList(); //get any duplicate related documents objects

            //merge any duplicate IIfcDocumentSelect IIfcRoot objects to a single link of IIfcDocumentSelect to IIfcRoot list
            Dictionary<IIfcDocumentSelect, List<IIfcDefinitionSelect>> docToObjs;
            if (dups.Any())
            {
                //remove the duplicates related documents objects and convert to dictionary
                docToObjs = ifcRelAssociatesDocuments.Except(dups).ToDictionary(p => p.RelatingDocument, p => p.RelatedObjects.ToList());
                //merge any duplicate related documents objects documents into list of single link of IIfcDocumentSelect to IfcRoot list, as duplicate related object could hold different documents so lets not lose them
                var dupsMerge = dups.GroupBy(d => d.RelatingDocument).Select(p => new { x = p.Key, y = p.SelectMany(c => c.RelatedObjects) });

                //add the duplicate lists to the DocToObjs list
                foreach (var item in dupsMerge)
                {
                    docToObjs[item.x] = docToObjs[item.x].Union(item.y).ToList(); //union will exclude any duplicates
                }
            }
            else
            {
                //no duplicates, so just convert to dictionary
                docToObjs = ifcRelAssociatesDocuments.ToDictionary(p => p.RelatingDocument, p => p.RelatedObjects.ToList());
            }
            return docToObjs;
        }

        /// <summary>
        /// Get next name for duplicates
        /// </summary>
        /// <param name="name">name to check</param>
        /// <param name="usedNames">List of names already used</param>
        /// <returns>name to use</returns>
        public string GetNextName(string name, List<string> usedNames)
        {
            //do we have any matching names
            if (usedNames == null || !usedNames.Any()) return name;

            var found = usedNames.Where(d => d.StartsWith(name, StringComparison.OrdinalIgnoreCase)).Select(n => n).ToList();
            if (!found.Any()) return name;

            if ((found.Count == 1) && (found.First().Length == name.Length)) //we match the whole name
                return name + "(1)"; //first duplicate
            
            var srch = name + "(";
            //we have duplicates so get names that are in correct format
            var correctFormat = found.Where(s => s.StartsWith(srch, StringComparison.OrdinalIgnoreCase) && s.EndsWith(")")).ToList();
            if (!correctFormat.Any()) return name;

            var number = correctFormat.Max(s => GetNextNo(srch, s));//.OrderBy(s => s).LastOrDefault();
            if (number > 0)
                return srch + number + ")";
            
            //string is not found or we failed to add next number return input argument string
            return name;
        }

        /// <summary>
        /// Get next number from string in a format Name(#), so "This Document(10)" should return 11
        /// </summary>
        /// <param name="prefix">string up to  and including'(', such as "Name(" </param>
        /// <param name="number">string formated "Name(#)", such as "Name(10)" </param>
        /// <returns>int</returns>
        private int GetNextNo(string prefix, string number)
        {
            var start = prefix.Length;
            var lgth = number.Length - start - 1;
            number = number.Substring(start, lgth); //get the string between brackets
            var strNo = Regex.Match(number, @"\d+").Value;
            if (!string.IsNullOrEmpty(strNo))
            {
                int no;
                if (int.TryParse(strNo, out no))
                {
                    return ++no;
                }
            }
            return 0;
        }

        private void GetSystems()
        {
            _systemAssignment = new Dictionary<IIfcSystem, IEnumerable<IIfcObjectDefinition>>();
            if (SystemMode.HasFlag(SystemExtractionMode.System))
            {
                _systemAssignment =
                        _model.Instances.OfType<IIfcRelAssignsToGroup>().Where(r => r.RelatingGroup is IIfcSystem)
                        .Distinct(new IfcRelAssignsToGroupRelatedGroupObjCompare()) //make sure we do not have duplicate keys, or ToDictionary will throw ex. could lose RelatedObjects though. 
                        .ToDictionary(k => (IIfcSystem)k.RelatingGroup, v => v.RelatedObjects);
                _systemLookup = new Dictionary<IIfcObjectDefinition, List<IIfcSystem>>();
                ReportProgress.NextStage(SystemAssignment.Count, 35);
                foreach (var systemAssignment in SystemAssignment)
                {
                    foreach (var objectDefinition in systemAssignment.Value)
                    {
                        if (_systemLookup.ContainsKey(objectDefinition))
                            _systemLookup[objectDefinition].Add(systemAssignment.Key);
                        else
                            _systemLookup.Add(objectDefinition, new List<IIfcSystem>(new[] { systemAssignment.Key }));
                    }
                    ReportProgress.IncrementAndUpdate();
                }
            }

            //Use PropertySet Property with names matching config values on section name = SystemPropertyMaps with key=SystemMaps
            SystemViaPropAssignment = new Dictionary<IIfcPropertySet, IEnumerable<IIfcObjectDefinition>>();
            if (!SystemMode.HasFlag(SystemExtractionMode.PropertyMaps)) return;

            var props = GetPropMap("SystemMaps");
            ReportProgress.NextStage(props.Length, 35);
            foreach (var propmap in props.Select(propertyName => propertyName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)))
            {
                if (propmap.Length == 2)
                {
                    var sets = Model.Instances.OfType<IIfcPropertySet>()
                        .Where(ps => ps.Name != null && propmap[0].Equals(ps.Name, StringComparison.OrdinalIgnoreCase)
                                     && ps.DefinesOccurrence.Any()
                                     && ps.HasProperties.OfType<IIfcPropertySingleValue>().Any(psv => psv.Name == propmap[1])
                                     && !SystemViaPropAssignment.ContainsKey(ps)
                        )
                        .SelectMany(ps => ps.DefinesOccurrence)
                        .Where(dbp => dbp.RelatedObjects.Any(e => _objectToTypeObjectMap.Keys.Contains(e))) //only none filtered objects
                        .ToDictionary(dbp => dbp.RelatingPropertyDefinition as IIfcPropertySet, dbp => dbp.RelatedObjects.AsEnumerable());

                    SystemViaPropAssignment = SystemViaPropAssignment.Concat(sets).ToDictionary(p => p.Key, p => p.Value);
                }
                ReportProgress.IncrementAndUpdate();
            }
        }

        /// <summary>
        /// Get the property mappings for a given field name
        /// </summary>
        /// <param name="filedKey">Field name</param>
        /// <returns>string[]</returns>
        public string[] GetPropMap(string filedKey)
        {
            string[] propertyNames;
            return _cobieFieldMap.TryGetValue("SystemMaps", out propertyNames) ? 
                propertyNames : 
                new string[] { };
        }
        

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<XbimIfcProxyTypeObject, List<IIfcElement>> DefiningTypeObjectMap
        {
            get { return _definingTypeObjectMap; }
        }
        private void GetTypeMaps()
        {

            var relDefinesByType = _model.Instances.OfType<IIfcRelDefinesByType>().Where(r => !Filter.ObjFilter(r.RelatingType)).ToList();
            //creates a dictionary of uniqueness for type objects
            var propertySetHashes = new Dictionary<string,string>();
            var proxyTypesByKey = new Dictionary<string, XbimIfcProxyTypeObject>();
            var relDefinesByRelType = relDefinesByType.Select(r => r.RelatingType).ToList();
            ReportProgress.NextStage(relDefinesByRelType.Count, 17);
            foreach (var typeObject in relDefinesByRelType)
            {
                var hash = GetTypeObjectHashString(typeObject);
                if (!propertySetHashes.ContainsKey(hash))
                {
                    var typeName = BuildTypeName(typeObject);
                    propertySetHashes.Add(hash, typeName);
                    proxyTypesByKey.Add(hash, new XbimIfcProxyTypeObject(this, typeObject, typeName));
                }
                ReportProgress.IncrementAndUpdate();

            }

            var assemblyParts = new HashSet<IIfcObjectDefinition>(_model.Instances.OfType<IIfcRelAggregates>().SelectMany(a => a.RelatedObjects));
            var grouping = relDefinesByType.GroupBy(k => proxyTypesByKey[GetTypeObjectHashString(k.RelatingType)],
                kv => kv.RelatedObjects).ToList();
            ReportProgress.NextStage(grouping.Count, 19);
            foreach (var group in grouping)
            {
                //filter on in assembly, and ifcElement if filtered in ProductFilter even if the ifcTypeObject is not filtered (passed filter in relDefinesByType assignment above)
                var allObjects = group.SelectMany(o => o.ToList()).OfType<IIfcElement>().Where(e => !assemblyParts.Contains(e) && !Filter.ObjFilter(e, false)).ToList();  
                _definingTypeObjectMap.Add(group.Key,allObjects);
                ReportProgress.IncrementAndUpdate();
            }
            
            _objectToTypeObjectMap = new Dictionary<IIfcObject, XbimIfcProxyTypeObject>();


            ReportProgress.NextStage(_definingTypeObjectMap.Count, 21);
            foreach (var typeObjectToObjects in _definingTypeObjectMap)
            {
                foreach (var ifcObject in typeObjectToObjects.Value.Where(t => !(t is IIfcFeatureElement) && !assemblyParts.Contains(t)))
                {
                    _objectToTypeObjectMap.Add(ifcObject, typeObjectToObjects.Key);
                }
                ReportProgress.IncrementAndUpdate();
            }

            //**NOTE**: removed _classifiedObjects from existingAssets as some elements do not have a type but have a classification, this excludes them from the _objectToTypeObjectMap, not what we want I think
            
            //get all Assets that don't belong to an Ifc Type or are not classified
            //get all IfcElements that aren't classified or have a type
            //var existingAssets = _classifiedObjects.Keys.OfType<IIfcElement>()
            //    .Concat(_objectToTypeObjectMap.Keys.OfType<IIfcElement>()).Distinct();
            var existingAssets = _objectToTypeObjectMap.Keys.OfType<IIfcElement>();

            //retrieve all the IfcElements from the model and exclude them if they are a member of an IIfcType, 
            var unCategorizedAssets = _model.Instances.OfType<IIfcElement>()
                .Where(t => !(t is IIfcFeatureElement) && !assemblyParts.Contains(t) && !Filter.ObjFilter(t)) //filter IIfcElement it IIfcTypeObject it is defined by is in excluded list of IIfcTypeobjects
                .Except(existingAssets);
            //convert to a Lookup with the key the type of the IIfcElement and the value a list of IIfcElements
            //if the object has a classification we use this to distinguish types

            var unCategorizedAssetsWithTypes = unCategorizedAssets.GroupBy
                (t=>GetProxyTypeObject(t).Name, v => v).ToDictionary(k=>k.Key,v=>v.ToList());
            ReportProgress.NextStage(unCategorizedAssetsWithTypes.Count, 23);
            foreach (var unCategorizedAssetsWithType in unCategorizedAssetsWithTypes)
            {
                XbimIfcProxyTypeObject proxyType;
                if (proxyTypesByKey.ContainsKey(unCategorizedAssetsWithType.Key))
                {
                    proxyType = proxyTypesByKey[unCategorizedAssetsWithType.Key];
                    _definingTypeObjectMap[proxyType].AddRange(
                        unCategorizedAssetsWithType.Value);
                }
                else
                {
                    proxyType = new XbimIfcProxyTypeObject(this,unCategorizedAssetsWithType.Key);
                    proxyTypesByKey.Add(unCategorizedAssetsWithType.Key, proxyType);
                    _definingTypeObjectMap.Add(proxyType, unCategorizedAssetsWithType.Value);
                }
                foreach (var ifcObject in unCategorizedAssetsWithType.Value)
                {
                    _objectToTypeObjectMap.Add(ifcObject, proxyType);
                }
                ReportProgress.IncrementAndUpdate();
            }
            
           

            //Get asset assignments

            var assetRels = _model.Instances.OfType<IIfcRelAssignsToGroup>()
                .Where(r => r.RelatingGroup is IIfcAsset).ToList();

            _assetAsignments = new Dictionary<IIfcTypeObject, IIfcAsset>();
            ReportProgress.NextStage(assetRels.Count, 25);
            foreach (var assetRel in assetRels)
            {
                foreach (var typeObject in assetRel.RelatedObjects.OfType<IIfcTypeObject>().Where(typeObject => !Filter.ObjFilter(typeObject)))
                {
                    AssetAsignments[typeObject] = (IIfcAsset)assetRel.RelatingGroup;
                }
                ReportProgress.IncrementAndUpdate();
            }
        }

        private static string GetTypeObjectHashString(IIfcTypeObject typeObject)
        {
            var hashString = "";
            if (typeObject.HasPropertySets != null && typeObject.HasPropertySets.Any())
            {
                var labels = typeObject.HasPropertySets.Select(t => t.EntityLabel).OrderBy(e => e);

                hashString = labels.Aggregate(hashString, (current, label) => current + (label + ":"));
            }
            //might be good to add classification
            hashString += typeObject.Name+":";
            hashString += typeObject.GetType().Name;
            return hashString;
        }

        private string ChangeNameFromStyleToType(IIfcTypeObject ifcTypeObject)
        {
            if (ifcTypeObject is IIfcDoorStyle )
                return "DoorType";
            if (ifcTypeObject is IIfcWindowStyle)
                return "WindowType";
            return ifcTypeObject.GetType().Name.Substring(3);
            
        }

        private string BuildTypeName(IIfcTypeObject ifcTypeObject)
        {
            var typeName = AllocateTypeName(ChangeNameFromStyleToType(ifcTypeObject));
            //remove names
            return string.Format("{0} {1}", typeName, ifcTypeObject.Name);
        }

        private string AllocateTypeName(string typeName)
        {
            
            if (_typeNames.ContainsKey(typeName))
                _typeNames[typeName]++;
            else
                _typeNames.Add(typeName, 1);
            return string.Format("{0}.{1}", typeName, _typeNames[typeName]);
        }

        /// <summary>
        /// For an element gets a XbimIfcProxyTypeObject for the asset
       /// </summary>
       /// <param name="element"></param>
       /// <returns></returns>
        public XbimIfcProxyTypeObject GetProxyTypeObject(IIfcElement element)
       {
           XbimIfcProxyTypeObject ifcTypeObject;
            //If there is a formal IfcTypeObject then use that name
           if (_objectToTypeObjectMap.TryGetValue(element, out ifcTypeObject))
           {
                return ifcTypeObject;
           }

           //get element name
           string name = element.Name;
           //look to see if it has been classified
           var categories = GetCategories(element, false);
           
            //its unclassified
           if (categories == null || !categories.Any())
                return !string.IsNullOrWhiteSpace(name)
                    ? new XbimIfcProxyTypeObject(this,
                        string.Format("{0}Type {1}", element.GetType().Name.Substring(3), name))
                    : new XbimIfcProxyTypeObject(this, AllocateTypeName(element.GetType().Name.Substring(3) + "Type"));

            //prefer the Uniclass2015 code
            foreach (var category in categories)
            {
                if (category.Classification != null && category.Classification.Name != null &&
                    category.Classification.Name.ToUpperInvariant().Contains("UNICLASS2015"))
                    return new XbimIfcProxyTypeObject(this, string.Format("{0}Type {1}", element.GetType().Name.Substring(3), category.Value));
            }
            //otherwise take the first
            return new XbimIfcProxyTypeObject(this,string.Format("{0}Type {1}", element.GetType().Name.Substring(3), categories.First().Value));

       }

        

        private void LoadCobieMaps()
        {
            var tmpFile = _configFileName;
            if (_configFileName == null)
            {
                tmpFile = Path.GetTempPath() + Guid.NewGuid().ToString() + ".csv";

                var asss = Assembly.GetExecutingAssembly();

                using (var input = asss.GetManifestResourceStream("XbimExchanger.IfcToCOBieLiteUK.COBieAttributes.config"))
                using (var output = File.Create(tmpFile))
                {
                    if (input != null) input.CopyTo(output);
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

            //using COBiePropertyMapping to set properties, might pass this into function, but for now read file passed file name, or default
            var propertyMaps = new COBiePropertyMapping(new FileInfo(tmpFile));
            _cobieFieldMap = propertyMaps.GetDictOfProperties();
            
            if (_configFileName == null)
                File.Delete(tmpFile);
        }

        private void GetPropertySets()
        {
            _attributedObjects = new Dictionary<IIfcObjectDefinition, XbimAttributedObject>();
            var relProps = _model.Instances.OfType<IIfcRelDefinesByProperties>().ToList();
            ReportProgress.NextStage(relProps.Count, 29);
            foreach (var relProp in relProps)
            {
                //get objects left after the IfcElement filters, plus none IfcElement (floors, spaces...)
                var filteredObjects = relProp.RelatedObjects.Where(obj => _objectToTypeObjectMap.Keys.Contains(obj) || !(obj is IIfcElement));
                foreach (var ifcObject in filteredObjects)
                {
                    XbimAttributedObject attributedObject;
                    if (!_attributedObjects.TryGetValue(ifcObject, out attributedObject))
                    {
                        attributedObject = new XbimAttributedObject(ifcObject, Target);
                        _attributedObjects.Add(ifcObject, attributedObject);
                    }
                    attributedObject.AddPropertySetDefinition(relProp.RelatingPropertyDefinition);  
                }
                ReportProgress.IncrementAndUpdate();
            }
            //process type objects ignoring pure proxies
            var defTypeToProxy = _definingTypeObjectMap.Keys.Where(t => t.IfcTypeObject != null).ToList();
            ReportProgress.NextStage(defTypeToProxy.Count, 33);
            foreach (var typeObject in defTypeToProxy)
            {
                XbimAttributedObject attributedObject;
                if (!_attributedObjects.TryGetValue(typeObject.IfcTypeObject, out attributedObject))
                    {
                        attributedObject = new XbimAttributedObject(typeObject.IfcTypeObject, Target);
                        _attributedObjects.Add(typeObject.IfcTypeObject, attributedObject);
                    }
                if (typeObject.IfcTypeObject.HasPropertySets != null)
                {
                    foreach (var pset in typeObject.IfcTypeObject.HasPropertySets)
                    {
                        attributedObject.AddPropertySetDefinition(pset);
                    }
                }
                ReportProgress.IncrementAndUpdate();
            }
            
        }

        private void GetSpacesAndZones()
        {
            _spatialDecomposition = _model.Instances.OfType<IIfcRelAggregates>().Where(r=>r.RelatingObject is IIfcSpatialStructureElement)
                .ToDictionary(ifcRelAggregate => (IIfcSpatialStructureElement) ifcRelAggregate.RelatingObject, ifcRelAggregate => ifcRelAggregate.RelatedObjects.OfType<IIfcSpatialStructureElement>().ToList());
            ReportProgress.NextStage(_spatialDecomposition.Count, 10);
            //get the relationship between spaces and storeys
            _spaceFloorLookup = new Dictionary<IIfcSpace, IIfcBuildingStorey>();
            foreach (var spatialElement in _spatialDecomposition)
            {
                var key = spatialElement.Key as IIfcBuildingStorey;
                if (key != null) //only care if the space is on a floor (COBie rule)
                {
                    foreach (var ifcSpace in spatialElement.Value.OfType<IIfcSpace>())
                        _spaceFloorLookup[ifcSpace] = key;
                }
                ReportProgress.IncrementAndUpdate();
            }

            var relZones = _model.Instances.OfType<IIfcRelAssignsToGroup>().Where(r=>r.RelatingGroup is IIfcZone).ToList();
            ReportProgress.NextStage(relZones.Count, 13);
            ZoneSpaces = new Dictionary<IIfcZone, HashSet<IIfcSpace>>();
            _spaceZones = new Dictionary<IIfcSpace, HashSet<IIfcZone>>();
            foreach (var relZone in relZones)
            {
                var spaces = relZone.RelatedObjects.OfType<IIfcSpace>().ToList();
                if (spaces.Any())
                {
                    //add the spaces to each zone lookup
                    var zone = (IIfcZone) relZone.RelatingGroup;
                    HashSet<IIfcSpace> zoneSpaces;
                    if (!ZoneSpaces.TryGetValue(zone, out zoneSpaces))
                    {
                        zoneSpaces = new HashSet<IIfcSpace>();
                        ZoneSpaces.Add(zone,zoneSpaces);
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
                ReportProgress.IncrementAndUpdate();
            }         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="space"></param>
        /// <returns></returns>
        public IEnumerable<IIfcZone> GetZones(IIfcSpace space)
        {
            HashSet<IIfcZone> zones;
            return _spaceZones.TryGetValue(space, out zones) ? 
                zones : 
                Enumerable.Empty<IIfcZone>();
        }
        /// <summary>
        /// 
        /// </summary>
        public CobieLinearUnit ModelLinearUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public CobieAreaUnit ModelAreaUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public CobieVolumeUnit ModelVolumeUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public CobieCurrencyUnit ModelCurrencyUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasLinearUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasAreaUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasVolumeUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasCurrencyUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IfcStore Model
        {
            get { return _model; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<IIfcTypeObject, IIfcAsset> AssetAsignments
        {
            get { return _assetAsignments; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IIfcObjectDefinition, List<IIfcSystem>> SystemLookup
        {
            get { return _systemLookup; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IIfcSystem, IEnumerable<IIfcObjectDefinition>> SystemAssignment
        {
            get { return _systemAssignment; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IIfcElement, List<IIfcSpatialElement>> SpaceAssetLookup
        {
            get { return _spaceAssetLookup; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<IIfcSpace, IIfcBuildingStorey> SpaceFloorLookup
        {
            get { return _spaceFloorLookup; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<IIfcZone, HashSet<IIfcSpace>> ZoneSpaces { get; private set; }

        public Dictionary<string, CobieContact> SundryContacts { get; private set; }

        public SystemExtractionMode SystemMode { get; internal set; }

        public Dictionary<IIfcActorSelect, CobieContact> Contacts
        {
            get { return _contacts; }
        }

        private void GetUnits()
        {
            var ifcProject = Model.Instances.FirstOrDefault<IIfcProject>();
            foreach (var unit in ifcProject.UnitsInContext.Units)
            {
                if (unit is IIfcNamedUnit)
                {
                    var unitType = (unit as IIfcNamedUnit).UnitType;
                    switch (unitType)
                    {
                        case IfcUnitEnum.AREAUNIT:
                            var areaUnitName = AdjustUnitName(unit.FullName);
                            HasAreaUnit = !string.IsNullOrWhiteSpace(areaUnitName);
                            if (HasAreaUnit) ModelAreaUnit = Target.Instances.New<CobieAreaUnit>(au => au.Value = areaUnitName);
                            break;
                        case IfcUnitEnum.LENGTHUNIT:
                            var lengthUnitName = AdjustUnitName(unit.FullName);
                            HasLinearUnit = !string.IsNullOrWhiteSpace(lengthUnitName);
                            if (HasLinearUnit) ModelLinearUnit = Target.Instances.New<CobieLinearUnit>(au => au.Value = lengthUnitName);
                            break;
                        case IfcUnitEnum.VOLUMEUNIT:
                            var volumeUnitName = AdjustUnitName(unit.FullName);
                            HasVolumeUnit = !string.IsNullOrWhiteSpace(volumeUnitName);
                            if (HasVolumeUnit) ModelVolumeUnit = Target.Instances.New<CobieVolumeUnit>(vu => vu.Value = volumeUnitName);
                            break;
                    }
                }
                else if (unit is IIfcMonetaryUnit)
                {
                    var currencyUnitName = unit.FullName;
                    HasCurrencyUnit = !string.IsNullOrWhiteSpace(currencyUnitName);
                    if (HasCurrencyUnit) ModelCurrencyUnit = Target.Instances.New<CobieCurrencyUnit>(cu => cu.Value = currencyUnitName);
                }

                //this.FacilityDefaultMeasurementStandard needs to be resolved
            }
        }
        /// <summary>
        /// Xbim uses the ifc schema names for units, but these are british english, this corrects to international english and removes unwanted separators
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        private static string AdjustUnitName(string unitName)
        {
            var ret = unitName.Replace("METRE", "METERS");
            return ret.Replace("_", "");
        }

        private void GetClassificationDictionary()
        {
            _classifiedObjects = new Dictionary<IIfcDefinitionSelect, List<IIfcClassificationReference>>();
            //create a dictionary of classified objects
            ReportProgress.NextStage(_classifiedObjects.Count, 8);
            foreach (var ifcRelAssociatesClassification in Model.Instances.OfType<IIfcRelAssociatesClassification>())
            {
                foreach (var relatedObject in ifcRelAssociatesClassification.RelatedObjects)
                {
                    List<IIfcClassificationReference> classificationList;
                    if (!_classifiedObjects.TryGetValue(relatedObject, out classificationList))
                    {
                        classificationList = new List<IIfcClassificationReference>();
                        _classifiedObjects.Add(relatedObject, classificationList);
                    }
                    classificationList.Add((IIfcClassificationReference)ifcRelAssociatesClassification.RelatingClassification);
                    ReportProgress.IncrementAndUpdate();
                }
            }
        }


        private List<CobieCategory> ConvertToCategories(IEnumerable<IIfcClassificationReference> classifications)
        {
            var categories = new List<CobieCategory>();
            foreach (var classification in classifications)
            { 
                CobieCategory category;
                if (
                    _categoryMapping.GetOrCreateTargetObject(
                        classification.Identification.HasValue
                            ? classification.Identification.ToString()
                            : classification.Name.ToString(), out category))
                    _categoryMapping.AddMapping(classification, category);

                if (category.Value != null && string.CompareOrdinal(category.Value.ToLower(),"n/a")!=0 )
                    categories.Add(category);
            }
            return categories;
        }

        /// <summary>
        /// Set Category with code and description as single delimited string
        /// </summary>
        /// <param name="strRef">Uniclass string</param>
        /// <returns>List of Category Objects</returns>
        private List<CobieCategory> ConvertToCategories(string strRef)
        {
            var code = strRef;
            string description = null;
            var parts = strRef.Split(new[] { ':', ';', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
                description = parts[1];
            if (parts.Length > 0)
                code = parts[0];

            CobieCategory category;
            if (!_categoryMapping.GetOrCreateTargetObject(code, out category))
                return new List<CobieCategory> {category};

            category.Value = code;
            category.Description = description;
            return new List<CobieCategory>{category};
        }

        /// <summary>
        /// Set Category with code and description
        /// </summary>
        /// <param name="code">Uniclass code</param>
        /// <param name="desc">Uniclass description</param>
        /// <returns>List of Category Objects</returns>
        private List<CobieCategory> ConvertToCategories(string code, string desc)
        {
            CobieCategory category;
            if (!_categoryMapping.GetOrCreateTargetObject(code ?? desc ?? "unknown", out category))
                return new List<CobieCategory> {category};

            if (!string.IsNullOrEmpty(code))
                category.Value= code;
            if (!string.IsNullOrEmpty(desc))
                category.Description= desc;
            return new List<CobieCategory>{category};
        }

        /// <summary>
        /// Returns the COBie Category for this object, based on the Ifc Classification
        /// </summary>
        /// <param name="classifiedObject"></param>
        /// <param name="useProp"></param>
        /// <returns></returns>
        public List<CobieCategory> GetCategories(IIfcDefinitionSelect classifiedObject, bool useProp = true)
        {
            List<IIfcClassificationReference> classifications;
            if (_classifiedObjects.TryGetValue(classifiedObject, out classifications))
                return  ConvertToCategories(classifications);
            //if the object is an IfcObject we might be able to get a classification from its aggregating type
            var ifcObject = classifiedObject as IIfcObject;
            if (ifcObject != null)
            {
                var definingTypeObject = GetDefiningTypeObject(ifcObject); //do we have a defining type
                if (definingTypeObject != null)
                {
                    if (_classifiedObjects.TryGetValue(definingTypeObject, out classifications))
                        return ConvertToCategories(classifications);
                }
            }
            //get category from properties
            if (!useProp || !(classifiedObject is IIfcObjectDefinition)) return new List<CobieCategory>{ UnknownCategory };

            var code = GetCoBieProperty("CommonCategoryCode", (IIfcObjectDefinition) classifiedObject);
            var desc = GetCoBieProperty("CommonCategoryDescription", (IIfcObjectDefinition) classifiedObject);
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(desc))
            {
                return ConvertToCategories(code, desc);
            }

            var cat = GetCoBieProperty("CommonCategoryCode", (IIfcObjectDefinition) classifiedObject);
            if (!string.IsNullOrEmpty(cat))
            {
                return ConvertToCategories(cat);
            }

            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(desc))
            {
                return new List<CobieCategory> { UnknownCategory };
            }

            return ConvertToCategories(cat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcRootObject"></param>
        /// <returns></returns>
        public string GetCreatingApplication(IIfcRoot ifcRootObject)
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
        public string GetAreaUnit(IIfcQuantityArea areaUnit)
        {
            return areaUnit.Unit != null ? areaUnit.Unit.FullName : ModelAreaUnit.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lengthUnit"></param>
        /// <returns></returns>
        public string GetLinearUnit(IIfcQuantityLength lengthUnit)
        {
            return lengthUnit.Unit != null ? lengthUnit.Unit.FullName : ModelLinearUnit.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeUnit"></param>
        /// <returns></returns>
        public string GetVolumeUnit(IIfcQuantityVolume volumeUnit)
        {
            return volumeUnit.Unit != null ? volumeUnit.Unit.FullName : ModelVolumeUnit.ToString();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="ifcObjectDefinition"></param>
        /// <param name="setter"></param>
        /// <typeparam name="TSimpleType"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public void TrySetSimpleValue<TSimpleType>(string valueName, IIfcObjectDefinition ifcObjectDefinition, Action<TSimpleType> setter)
        {
            XbimAttributedObject attributedObject;
            var result = default(TSimpleType);
            if (!_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject)) 
                return;
            
            string[] propertyNames;
            if (_cobieFieldMap.TryGetValue(valueName, out propertyNames))
            {
                if (propertyNames.Any(propertyName => attributedObject.TryGetAttributeValue(propertyName, out result)))
                    setter(result);
            }
            else
                throw new ArgumentException("Illegal COBie Attribute name:", valueName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="ifcObject"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public TValue? GetCoBieProperty<TValue>(string valueName, IIfcObject ifcObject) where TValue:struct
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
            return null;
        }

        

        private IIfcTypeObject GetDefiningTypeObject(IIfcObject ifcObject)
        {
            XbimIfcProxyTypeObject definingType;
            _objectToTypeObjectMap.TryGetValue(ifcObject, out definingType);
            return definingType != null ? definingType.IfcTypeObject : null;
        }

        /// <summary>
        /// Get the XbimAttributedObject object associated with the passed ifcObjectDefinition
        /// </summary>
        /// <param name="ifcObjectDefinition">ifcObjectDefinition, IfcTypeObject, IfcObject</param>
        /// <returns>XbimAttributedObject</returns>
        public XbimAttributedObject GetAttributesObj(IIfcObjectDefinition ifcObjectDefinition)
        {
            XbimAttributedObject attributedObject;
            return _attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject) ? 
                attributedObject : 
                null;
        }

        private CobieExternalObject GetCobiePset(string name)
        {
            var pset = _externalObjectMapping.GetOrCreateTargetObject(name);
            pset.Name = name;
            return pset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObjectDefinition"></param>
        /// <returns></returns>
        public List<CobieAttribute> GetAttributes(IIfcObjectDefinition ifcObjectDefinition)
        {
            var uniqueAttributes = new Dictionary<string, CobieAttribute>();
            XbimAttributedObject attributedObject;
            if (!_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject)) return null;

            var properties = attributedObject.Properties;
            var keyValuePairs = properties.ToArray();
            if (keyValuePairs.Length <= 0) return null;

            var attributeCollection = new List<CobieAttribute>(keyValuePairs.Length);
            foreach (var kvp in keyValuePairs)
            {
                var property = kvp.Value;
                var splitName = kvp.Key.Split('.');
                var pSetName = splitName[0];
                var newAttribute = attributedObject.ConvertToAttributeType(property);
                var pSetDef = attributedObject.GetPropertySetDefinition(pSetName);
                        
                if (pSetDef != null)
                {
                    newAttribute.Created = GetCreatedInfo(pSetDef);
                    newAttribute.ExternalId = ExternalEntityIdentity(pSetDef);
                    newAttribute.ExternalSystem = GetExternalSystem(pSetDef);
                }
                else
                {
                    newAttribute.Created = GetCreatedInfo();
                    newAttribute.ExternalSystem = GetExternalSystem();
                }

                newAttribute.ExternalObject = GetCobiePset(pSetName);
                CobieAttribute existingAttribute;
                if (uniqueAttributes.TryGetValue(newAttribute.Name, out existingAttribute))
                    //it is a duplicate so append the pset name
                {
                            
                    var keyName = string.Format("{0}.{1}", existingAttribute.Name, pSetName);
                    if(!uniqueAttributes.ContainsKey(keyName))
                    {
                        uniqueAttributes.Remove(existingAttribute.Name);
                        existingAttribute.Name = keyName;
                        uniqueAttributes.Add(keyName, existingAttribute); //update existing key
                    }
                    newAttribute.Name = string.Format("{0}.{1}", newAttribute.Name, pSetName);
                    if (!uniqueAttributes.ContainsKey(newAttribute.Name))
                    {
                        uniqueAttributes.Add(newAttribute.Name, newAttribute); //update existing key
                    }
                }
                else
                    uniqueAttributes.Add(newAttribute.Name, newAttribute);
            }
                   
            attributeCollection.AddRange(uniqueAttributes.Values);
            return attributeCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObjectDefinition"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public CobieAttribute MakeAttribute(IIfcObjectDefinition ifcObjectDefinition, string attributeName, object attributeValue)
        {
            var newAttribute = Target.Instances.New<CobieAttribute>(a =>
            {
                a.Created = GetCreatedInfo(ifcObjectDefinition);
                a.ExternalId = ExternalEntityIdentity(ifcObjectDefinition);
                a.ExternalSystem = GetExternalSystem(ifcObjectDefinition);
                a.Name = attributeName;
            });
            newAttribute.Set(attributeValue);
            return newAttribute;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObject"></param>
        /// <returns></returns>
        public string ExternalEntityIdentity(IIfcRoot ifcObject)
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
        public string ExternalEntityName(IIfcRoot ifcObject)
        {
            if (
                ExternalReferenceMode == ExternalReferenceMode.IgnoreEntityName ||
                    ExternalReferenceMode == ExternalReferenceMode.IgnoreSystemAndEntityName)
                return null;
            return ifcObject.GetType().Name;
        }

        internal CobieExternalSystem GetExternalSystem(IIfcRoot ifcObject = null, bool usePropFirst = false)
        {
            if (ExternalReferenceMode == ExternalReferenceMode.IgnoreSystem ||
                ExternalReferenceMode == ExternalReferenceMode.IgnoreSystemAndEntityName)
                return null;
            if (usePropFirst && ifcObject is IIfcObjectDefinition)//support for COBie Toolkit for Autodesk Revit(had this in on old code, not sure if still relevant. this note date 8/10/2015)
            {
                var extSystem = GetCoBieProperty("CommonExtSystem", (IIfcObjectDefinition) ifcObject);
                if (!string.IsNullOrEmpty(extSystem))
                {
                    var result = _externalSystemMapping.GetOrCreateTargetObject(extSystem);
                    result.Name = extSystem;
                    return result;
                }
            }
            return _externalSystemXbim; 
        }


 
        private void GetSpaceAssetLookup()
        {
          
            //get all elements that are contained in any spatial structure of this building
            _spaceAssetLookup = new Dictionary<IIfcElement, List<IIfcSpatialElement>>(); 
           
            var ifcRelContainedInSpaces = _model.Instances.OfType<IIfcRelContainedInSpatialStructure>().ToList();
            ReportProgress.NextStage(ifcRelContainedInSpaces.Count, 40);
            foreach (var ifcRelContainedInSpace in ifcRelContainedInSpaces)
            {
                foreach (var element in ifcRelContainedInSpace.RelatedElements.OfType<IIfcElement>())
                { 
                    List<IIfcSpatialElement> spaceList;
                    if (!SpaceAssetLookup.TryGetValue(element, out spaceList))
                    {
                        spaceList = new List<IIfcSpatialElement>();
                        SpaceAssetLookup[element] = spaceList;

                    }
                    var container = ifcRelContainedInSpace.RelatingStructure;
                    spaceList.Add(container);
                }
                ReportProgress.IncrementAndUpdate();
            }
           
        }
        /// <summary>
        /// Returns all assets in the building but removes
        /// </summary>
        /// <param name="ifcBuilding"></param>
        /// <returns></returns>
        public IEnumerable<IIfcElement> GetAllAssets(IIfcBuilding ifcBuilding)
        {
            var spatialStructureOfBuilding = new HashSet<IIfcSpatialStructureElement>(); // all the spatial decomposition of the building
           
            //get all the spatial structural elements which may contain assets
            DecomposeSpatialStructure(ifcBuilding, spatialStructureOfBuilding);
            //get all elements that are contained in the spatial structure of this building
            var elementsInBuilding = _model.Instances.OfType<IIfcRelContainedInSpatialStructure>()
                .Where(r => spatialStructureOfBuilding.Contains(r.RelatingStructure))
                .SelectMany(s=>s.RelatedElements.OfType<IIfcElement>()).Distinct();
            //remove
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

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="ifcObjectDefinition"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetCoBieProperty(string valueName, IIfcObjectDefinition ifcObjectDefinition)
        {
            XbimAttributedObject attributedObject;
            if (!_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject)) return null;
            string[] propertyNames;
            if (!_cobieFieldMap.TryGetValue(valueName, out propertyNames))
                throw new ArgumentException("Illegal COBie Attribute name:", valueName);
            foreach (var propertyName in propertyNames)
            {
                string value;
                if (attributedObject.GetSimplePropertyValue(propertyName, out value))
                    return value;
            }
            return null;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void GetContacts()
        {
            
            //get any actors and select their
            var ifcActors = _model.Instances.OfType<IIfcActor>().ToList();
            var actors = new HashSet<IIfcActorSelect>(ifcActors.Select(a => a.TheActor)); //unique actors

            var personOrgs =  new HashSet<IIfcActorSelect>(_model.Instances.OfType<IIfcPersonAndOrganization>().Where(p => !actors.Contains(p)));
            actors = new HashSet<IIfcActorSelect>(actors.Concat(personOrgs));

            var orgAlreadyIn = actors.OfType<IIfcPersonAndOrganization>().Select(po => po.TheOrganization);
            var orgs = _model.Instances.OfType<IIfcOrganization>().Where(p => !orgAlreadyIn.Contains(p) && p.Addresses != null); //lets only see ones with any address info
            actors = new HashSet<IIfcActorSelect>(actors.Union(orgs)); //union will exclude duplicates

            _actors = new Dictionary<IIfcActorSelect, IIfcActor>();
            //set progress report
            ReportProgress.NextStage(ifcActors.Count + actors.OfType<IIfcPersonAndOrganization>().Count(), 5);
            foreach (var actor in ifcActors)
            {
                if(!_actors.ContainsKey(actor.TheActor))
                    _actors.Add(actor.TheActor,actor);
                ReportProgress.IncrementAndUpdate();
            }

            //sort out createdByKeys, these will always be IIfcPersonAndOrganization which are held in IfcOwnerHistory fields
            foreach (var actor in actors.OfType<IIfcPersonAndOrganization>())
            {
                var email = EmailAddressOf(actor);
                CobieContact contact;
                if (_contactMapping.GetOrCreateTargetObject(email, out contact))
                {
                    contact = _contactMapping.AddMapping(actor, contact);
                    contact.Email = email;
                }
                _contacts.Add(actor, contact);
                ReportProgress.IncrementAndUpdate();
            }
            SundryContacts = new Dictionary<string, CobieContact>();
        }

        public string EmailAddressOf(IIfcActorSelect personOrg)
        {
            IIfcPerson person = null;
            IIfcOrganization organisation= null;
            var ifcPerson = personOrg as IIfcPerson;
            if (ifcPerson != null)
            {
                person = ifcPerson;
            }
            var organization = personOrg as IIfcOrganization;
            if (organization != null)
            {
                organisation = organization;
            }
            var personAndOrganization = personOrg as IIfcPersonAndOrganization;
            if (personAndOrganization != null)
            {
                person = personAndOrganization.ThePerson;
                organisation = personAndOrganization.TheOrganization;
            }
            
            //get a default that will be unique
            var email = string.Format("unknown{0}@undefined.email", personOrg.EntityLabel);
            if ((organisation != null) && (organisation.Addresses != null))
            {
                var telecom = organisation.Addresses.OfType<IIfcTelecomAddress>().FirstOrDefault(a=>a.ElectronicMailAddresses.Any(e=>!string.IsNullOrWhiteSpace(e)));
                if (telecom != null)
                    email = telecom.ElectronicMailAddresses.FirstOrDefault(e => !string.IsNullOrWhiteSpace(e));
            }
            //overwrite if we have it at person level
            if ((person != null) && (person.Addresses != null))
            {
                var telecom = person.Addresses.OfType<IIfcTelecomAddress>().FirstOrDefault(a => a.ElectronicMailAddresses.Any(e => !string.IsNullOrWhiteSpace(e)));
                if (telecom != null)
                    email = telecom.ElectronicMailAddresses.FirstOrDefault(e => !string.IsNullOrWhiteSpace(e));
            }
            return email;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcZone"></param>
        /// <returns></returns>
        public IEnumerable<IIfcSpace> GetSpaces(IIfcZone ifcZone)
        {
            HashSet<IIfcSpace> spaces;
            if (ZoneSpaces.TryGetValue(ifcZone, out spaces))
                return spaces;
            return Enumerable.Empty<IIfcSpace>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcSystem"></param>
        /// <returns></returns>
        public IEnumerable<IIfcObjectDefinition> GetSystemAssignments(IIfcSystem ifcSystem)
        {
            IEnumerable<IIfcObjectDefinition> assignments;
            return SystemAssignment.TryGetValue(ifcSystem, out assignments) ? 
                assignments : 
                Enumerable.Empty<IIfcObjectDefinition>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcPropertySet"></param>
        /// <returns></returns>
        public IEnumerable<IIfcObjectDefinition> GetSystemAssignments(IIfcPropertySet ifcPropertySet)
        {
            IEnumerable<IIfcObjectDefinition> assignments;
            return SystemViaPropAssignment.TryGetValue(ifcPropertySet, out assignments) ? 
                assignments : 
                Enumerable.Empty<IIfcObjectDefinition>();
        }
        /// <summary>
        /// Returns a list of spaces the element is in
        /// </summary>
        /// <param name="ifcElement"></param>
        /// <returns></returns>
        public IEnumerable<IIfcSpatialElement> GetSpaces(IIfcElement ifcElement)
        {
            List<IIfcSpatialElement> spaceList;
            if (_spaceAssetLookup.TryGetValue(ifcElement, out spaceList))
                return spaceList;
            return Enumerable.Empty<IIfcSpatialElement>();
        }

        internal string GetFacilityName(IIfcBuilding building)
        {
            if (!string.IsNullOrWhiteSpace(building.Name))
                return building.Name;
            var project = _model.Instances.FirstOrDefault<IIfcProject>();
            if (project != null)
            {
                if (!string.IsNullOrWhiteSpace(project.Name))
                    return project.Name;
            }
            return "Unknown";
        }


        private CobieCreatedInfo GetCreatedInfo(CobieContact contact, DateTimeValue dateTime)
        {
            var info = _createdInfoCache.FirstOrDefault(ci => ci.CreatedBy == contact && ci.CreatedOn == dateTime);
            if (info != null)
                return info;

            info = Target.Instances.New<CobieCreatedInfo>(ci =>
            {
                ci.CreatedBy = contact;
                ci.CreatedOn = dateTime;
            });
            _createdInfoCache.Add(info);
            return info;
        }

        internal CobieCreatedInfo GetCreatedInfo(IIfcRoot ifcRoot = null, bool usePropFirst = false)
        {
            var contact = GetCreatedBy(ifcRoot, usePropFirst);
            var dateTime = GetCreatedOn(ifcRoot, usePropFirst) ?? _now;
            return GetCreatedInfo(contact, dateTime);
        }

        internal CobieContact GetCreatedBy(IIfcRoot ifcRoot = null, bool usePropFirst = false)
        {
            if (ifcRoot == null) return XbimContact;

            CobieContact contact;
            if (usePropFirst && ifcRoot is IIfcObjectDefinition)//support for COBie Toolkit for Autodesk Revit(had this in on old code, not sure if still relevant. this note date 8/10/2015)
            {
                var email = GetCoBieProperty("CommonCreatedBy", (IIfcObjectDefinition) ifcRoot);
                if (string.IsNullOrEmpty(email)) return XbimContact;

                if (!_contactMapping.GetOrCreateTargetObject(email, out contact)) 
                    return contact;

                contact.Email = email;
                contact.Created = GetCreatedInfo(XbimContact, _now);
                contact.Category = UnknownRole;
                return contact;
            }
            if (ifcRoot.OwnerHistory.LastModifyingUser != null)
            {
                if (_contacts.TryGetValue(ifcRoot.OwnerHistory.LastModifyingUser, out contact))
                    return contact;
            }
            else if (ifcRoot.OwnerHistory.OwningUser != null)
            {
                if (_contacts.TryGetValue(ifcRoot.OwnerHistory.OwningUser, out contact))
                    return contact;
            }
            return XbimContact;
        }

        private DateTime? GetCreatedOn(IIfcRoot ifcRoot = null, bool usePropFirst = false)
        {
            //use the same time for all null root objects
            if (ifcRoot == null) return _now;
            
            if (usePropFirst)
            {
                DateTime? propDate;
                if (GetCreatedOnFromProp(ifcRoot, out propDate))
                    return propDate;
            }

            //use last modified date if we have one
            var dateTime = ifcRoot.OwnerHistory.LastModifiedDate ?? ifcRoot.OwnerHistory.CreationDate;
            return dateTime.ToDateTime();
        }

        /// <summary>
        /// Get CreateDate from properties
        /// </summary>
        /// <param name="ifcRoot">object to get properties on</param>
        /// <param name="date">out Date</param>
        /// <returns>bool</returns>
        private bool GetCreatedOnFromProp(IIfcRoot ifcRoot, out DateTime? date)
        {
            var objectDefinition = ifcRoot as IIfcObjectDefinition;
            if (objectDefinition != null)//support for COBie Toolkit for Autodesk Revit(had this in on old code, not sure if still relevant. this note date 8/10/2015)
            {
                var createdOn = GetCoBieProperty("CommonCreatedOn", objectDefinition);
                if (!string.IsNullOrEmpty(createdOn))
                {
                    DateTime foundDate;
                    if (DateTime.TryParse(createdOn, out foundDate))
                    {
                        date = foundDate;
                        return true;
                    }
                    //try and get just the date part of the date time, as a conversion above failed, so assume time might be corrupt
                    var idx = createdOn.IndexOf("T", StringComparison.Ordinal);
                    if (idx > -1)
                    {
                        var datestr = createdOn.Substring(0, idx);
                        if (DateTime.TryParse(datestr, out foundDate))
                        {
                            date = foundDate;
                            return true;
                        }
                    }
                }
            }
            date = null;
            return false;
        }

        internal CobieCreatedInfo GetCreatedInfo(IIfcActorSelect actorSelect, bool useOwnerHistory = false)
        {
            var contact = GetCreatedBy(actorSelect, useOwnerHistory);
            var dateTime = GetCreatedOn(actorSelect) ?? _now;
            return GetCreatedInfo(contact, dateTime);
        }

        /// <summary>
        /// Get ContactKey for CreatedBy, first from IfcActor OwnerHistory if useOwnerHistory = true, then IfcActorSelect returning the ContactKey for the IfcActorSelect
        /// </summary>
        /// <param name="actorSelect">IfcActorSelect Object</param>
        /// <param name="useOwnerHistory">bool true get created by from owner history</param>
        /// <returns>ContactKey</returns>
        internal CobieContact GetCreatedBy(IIfcActorSelect actorSelect, bool useOwnerHistory = false)
        {
            //As IfcActor have Owner History, and we are looking for ownerHistory, try and see if IfcActor is associated with the Actor Select
            IIfcActor actor;
            if (useOwnerHistory && _actors.TryGetValue(actorSelect, out actor))
                return GetCreatedBy(actor);

            CobieContact key;
            return _contacts.TryGetValue(actorSelect, out key) ? 
                key : 
                XbimContact;
        }

        internal DateTime? GetCreatedOn(IIfcActorSelect actorSelect)
        {
            IIfcActor actor;
            return _actors.TryGetValue(actorSelect, out actor) ? 
                GetCreatedOn(actor) : 
                _now;
        }


        internal CobieZone XbimDefaultZone
        {
            get
            {
                return _xbimDefaultZone ?? (_xbimDefaultZone = Target.Instances.New<CobieZone>(z =>
                {
                    z.Name = "Default Zone";
                    z.Created = GetCreatedInfo(XbimContact, _now);
                    z.Categories.Add(UnknownCategory);
                }));
            }
        }

        internal CobieSystem XbimDefaultSystem
        {
            get
            {
                return _xbimDefaultSystem ?? (_xbimDefaultSystem = Target.Instances.New<CobieSystem>(s =>
                {
                    s.Name = "Default System";
                    s.Created = GetCreatedInfo(XbimContact, _now);
                    s.Categories.Add(UnknownCategory);
                }));    
            }
            
        }

        internal CobieContact GetOrCreateContact(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || 
                string.Compare(email, "n/a", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(email, "User To Populate", StringComparison.OrdinalIgnoreCase)==0) 
                return null;

            CobieContact contact;
            var actorContactKey =
                _contacts.Values.FirstOrDefault(c => string.Compare(c.Email, email, StringComparison.OrdinalIgnoreCase) == 0);
            if (actorContactKey != null)
                return actorContactKey;
            if (SundryContacts.TryGetValue(email, out contact)) 
                return contact;

            if (_contactMapping.GetOrCreateTargetObject(email, out contact))
            {
                contact.Email = email;
                contact.Created = GetCreatedInfo(XbimContact, _now);
                contact.Category = UnknownRole;
            }
            SundryContacts.Add(email, contact);
            return contact;
        }


        private readonly Dictionary<Type, List<CobiePickValue>> _pickCache = new Dictionary<Type, List<CobiePickValue>>();

        internal TPickType GetPickValue<TPickType>(string value) where TPickType : CobiePickValue, IInstantiableEntity
        {
            var type = typeof (TPickType);
            List<CobiePickValue> candidate;
            if (_pickCache.TryGetValue(type, out candidate))
            {
                var found = candidate.FirstOrDefault(p => p.Value == value);
                if (found != null) return (TPickType)found;

                var pick = Target.Instances.New<TPickType>(p => p.Value = value);
                candidate.Add(pick);
                return pick;
            }

            var result = Target.Instances.New<TPickType>(p => p.Value = value);
            _pickCache.Add(type, new List<CobiePickValue>{result});
            return result;
        }

        internal CobieExternalObject GetExternalObject(object o)
        {
            return o == null ? null : GetExternalObject(o.GetType());
        }

        internal CobieExternalObject GetExternalObject(Type type)
        {
            return GetExternalObject(type.Name);
        }

        internal CobieExternalObject GetExternalObject(string value)
        {
            if (value == null) return null;

            CobieExternalObject o;
            if (_externalObjectMapping.GetOrCreateTargetObject(value, out o))
                o.Name = value;
            return o;
        }
    }


    
}
