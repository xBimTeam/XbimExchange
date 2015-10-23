
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xbim.COBieLiteUK;
using Xbim.Common.Logging;
using Xbim.FilterHelper;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.QuantityResource;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.Ifc2x3.SharedFacilitiesElements;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.IO;
using Xbim.XbimExtensions.SelectTypes;
using XbimExchanger.IfcToCOBieLiteUK.EqCompare;
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
    public class CoBieLiteUkHelper
    {

        internal static readonly ILogger Logger = LoggerFactory.GetLogger();
        /// <summary>
        /// Object to use to report progress on Exchangers
        /// </summary>
        public ProgressReporter ReportProgress
        { get; set; }

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

        private Dictionary<IfcZone, HashSet<IfcSpace>> _zoneSpaces;
        private Dictionary<IfcSpace, HashSet<IfcZone>> _spaceZones;

        private Dictionary<IfcObjectDefinition, XbimAttributedObject> _attributedObjects;

        private Dictionary<string, string[]> _cobieFieldMap;
        
        private Dictionary<IfcObject, XbimIfcProxyTypeObject> _objectToTypeObjectMap;

        private Dictionary<XbimIfcProxyTypeObject, List<IfcElement>> _definingTypeObjectMap =
            new Dictionary<XbimIfcProxyTypeObject, List<IfcElement>>();

/*
        private Lookup<string, IfcElement> _elementTypeToElementObjectMap;
*/
        private Dictionary<IfcTypeObject, IfcAsset> _assetAsignments;
        private Dictionary<IfcSystem, ObjectDefinitionSet> _systemAssignment;
        private Dictionary<IfcObjectDefinition, List<IfcSystem>> _systemLookup;
        private HashSet<string> _cobieProperties = new HashSet<string>();
        private Dictionary<IfcElement, List<IfcSpatialStructureElement>> _spaceAssetLookup;
        private Dictionary<IfcSpace, IfcBuildingStorey> _spaceFloorLookup;
        private Dictionary<IfcSpatialStructureElement, List<IfcSpatialStructureElement>> _spatialDecomposition;
        private readonly Dictionary<string, int> _typeNames = new Dictionary<string, int>();

        #region Document Lookups
        /// <summary>
        /// Document to Object mapping
        /// </summary>
        public Dictionary<IfcRoot, IEnumerable<IfcDocumentSelect>> DocumentLookup
        { get; private set; }

        /// <summary>
        /// Documents not attached to ant IfcRoot object
        /// </summary>
        public IEnumerable<IfcDocumentSelect> OrphanDocs
        { get; private set; }

        /// <summary>
        /// Document to IfcRelAssociatesDocument mapping, fall back info from ifcRelAssociatesDocument history, if nothing set on IfcDocumentInformation dates
        /// </summary>
        public Dictionary<IfcDocumentSelect, IfcRelAssociatesDocument> DocumentOwnerLookup
        { get; private set; }

        #endregion

        #region Spare 

        public Dictionary<IfcRoot, IEnumerable<IfcConstructionProductResource>> SpareLookup
        { get; private set; }
        #endregion

        /// <summary>
        /// Property Sets used to establish systems as per responsibility matrix 
        /// </summary>
        public Dictionary<IfcPropertySet, IEnumerable<IfcObject>> SystemViaPropAssignment { get; private set; }
        #endregion

        #region Filters

        private OutPutFilters Filter  { get; set; }

        #endregion

        private readonly string _configFileName;
        private List<IfcActorSelect> _contacts;
        private Dictionary<IfcActorSelect, ContactKey> _createdByKeys;
        private Dictionary<IfcActorSelect, IfcActor> _actors;
        public static List<Category> UnknownCategory = new List<Category>(new[] { new Category { Code = "unknown" } });
        private readonly Contact _xbimCreatedBy = new Contact
        {
            Email = "Unknown@OpenBIM.org",
            GivenName = "XbimTeam",
            CreatedOn = DateTime.Now,
            CreatedBy = new ContactKey {Email = "Unknown@OpenBIM.org"},
            Categories = UnknownCategory
        };

       

    private Dictionary<string, Contact> _sundryContacts;

        /// <summary>
        /// Creates a default contact and adds it to the SundryContacts
        /// </summary>

        public Contact XbimCreatedBy
        {
            get
            {
                if (!_sundryContacts.ContainsKey(_xbimCreatedBy.Email))
                    _sundryContacts.Add(_xbimCreatedBy.Email, _xbimCreatedBy);
                return _xbimCreatedBy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="configurationFile"></param>
        public CoBieLiteUkHelper(XbimModel model, ProgressReporter reportProgress, OutPutFilters filter = null, string configurationFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types)
        {
            
            //set props
            _configFileName = configurationFile;
            Filter = filter != null ? filter : new OutPutFilters();
            _model = model;
            EntityIdentifierMode = extId;
            SystemMode = sysMode;
            _creatingApplication = model.Header.CreatingApplication;
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
        }

        /// <summary>
        /// Get Spare lookup and set SpareLookup property
        /// </summary>
        public void GetSpare()
        {
            Dictionary<IfcConstructionProductResource, List<IfcRoot>> spareToObjs = GetSpareResource();

            //reverse lookup to entity to list of documents
            SpareLookup = spareToObjs
                            .SelectMany(pair => pair.Value
                            .Select(val => new { Key = val, Value = pair.Key }))
                            .GroupBy(item => item.Key)
                            .ToDictionary(gr => gr.Key, gr => gr.Select(item => item.Value));
        }

        /// <summary>
        /// Convert all IfcRelAssignsToResource to a dictionary of IfcConstructionProductResource, List of IfcRoot
        /// </summary>
        /// <returns>Dictionary of IfcConstructionProductResource, List of IfcRoot</returns>
        private Dictionary<IfcConstructionProductResource, List<IfcRoot>> GetSpareResource()
        {
            Dictionary<IfcResource, List<IfcObjectDefinition>> resourceToObjs = null;

            var ifcRelAssignsToResource = _model.Instances.OfType<IfcRelAssignsToResource>().Where(r => r.RelatingResource is IfcConstructionProductResource && (r.RelatedObjectsType == null || r.RelatedObjectsType == IfcObjectType.Product || r.RelatedObjectsType == IfcObjectType.NotDefined)); //linked to IfcRoot objects

            var dups = ifcRelAssignsToResource.GroupBy(d => d.RelatingResource).SelectMany(grp => grp.Skip(1)); //get any duplicate related resource objects
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
            return resourceToObjs.ToDictionary(r => r.Key as IfcConstructionProductResource, r => r.Value.ConvertAll(x => (IfcRoot)x));
        } 


        /// <summary>
        /// Add document to List of Documents
        /// </summary>
        /// <param name="docsMappings">Mapping object</param>
        /// <param name="target">Target object holding the document list - CobieObject</param>
        /// <param name="ifcRoot">Object holding the documents</param>
        internal void AddDocuments(MappingIfcDocumentSelectToDocument docsMappings, CobieObject target, IfcRoot ifcRoot)
        {
            if (ifcRoot != null)
            {
                if (target.Documents == null)
                {
                    target.Documents = new List<Document>();
                }
                var facilitydocs = GetDocuments(ifcRoot);

                foreach (var docSel in facilitydocs)
                {
                    List<Document> docs = docsMappings.MappingMulti(docSel);
                    target.Documents.AddRange(docs);
                } 
            }
        }

        /// <summary>
        /// Return the documents associated with the object
        /// </summary>
        /// <param name="ifcRoot">Object to get associated documents</param>
        /// <returns></returns>
        public IEnumerable<IfcDocumentSelect> GetDocuments(IfcRoot ifcRoot)
        {
            if (DocumentLookup.ContainsKey(ifcRoot))
            {
                return DocumentLookup[ifcRoot];
            }

            return Enumerable.Empty<IfcDocumentSelect>();
        }

        /// <summary>
        /// Extract Document information
        /// </summary>
        private void GetDocumentSelects()
        {
            Dictionary<IfcDocumentSelect, List<IfcRoot>> docToObjs = GetAssociatedDocuments();

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
        /// <returns>List of IfcDocumentSelect</returns>
        private IEnumerable<IfcDocumentSelect> GetOrphanDocuments(Dictionary<IfcDocumentSelect, List<IfcRoot>> docToObjs)
        {
            //------GET ORPHAN DOCUMENTINFOS------
            //Get all documents information objects held in model
            var docAllInfos = _model.Instances.OfType<IfcDocumentInformation>();
            //Get the child document relationships
            var childDocRels = _model.Instances.OfType<IfcDocumentInformationRelationship>();

            //see if we have any documents not attached to IfcRoot objects, but could be attached as children documents to a parent document...

            //get the already attached to entity documents 
            var docInfosAttached = docToObjs.Select(dic => dic.Key).OfType<IfcDocumentInformation>();
            var docInfosNotAttached = docAllInfos.Except(docInfosAttached);
           
            //get document infos attached to the IfcDocumentReference, which are directly linked to IfcRoot objects
            var docRefsAttached = docToObjs.Select(Dictionary => Dictionary.Key).OfType<IfcDocumentReference>();//attached to IfcRoot docRefs
            if (docRefsAttached.Any())
            {
                var docRefsInfos = docAllInfos.Where(info => info.DocumentReferences != null && info.DocumentReferences.Any(doc => docRefsAttached.Contains(doc)));
                docInfosNotAttached = docAllInfos.Except(docRefsInfos); //remove the DocInfos attached to the DocRefs that are attached to IfcRoot Objects
                docInfosAttached = docInfosAttached.Union(docRefsInfos); //add the DocInfos attached to the DocRefs that are attached to IfcRoot Objects
            }
           
            List<IfcDocumentInformation> docChildren = docInfosAttached.ToList(); //first check on docs attached to IfcRoot Objects, and attached to IfcDocumentReference(which are attached to IfcRoot)
            int idx = 0;
            do
            {
                //get the relationships that are attached to the docs already associated with an IfcRoot object on first pass, then associated with all children, drilling down until nothing found
                docChildren = childDocRels.Where(docRel => docChildren.Contains(docRel.RelatingDocument)).SelectMany(docRel => docRel.RelatedDocuments).ToList(); //docs that are children to attached entity docs, drilling down
                docInfosNotAttached = docInfosNotAttached.Except(docChildren); //attached by association to the root parent document, so remove from none attached document list


            } while (docChildren.Any() && (++idx < 100)); //assume that docs are not embedded deeper than 100

            //------GET ORPHAN DOCUMENTREFERENCES------
            //get all the doc reference objects held in the model
            var docAllRefs = _model.Instances.OfType<IfcDocumentReference>();
           
            //checked on direct attached to object document references
            var docRefsNotAttached = docAllRefs.Except(docRefsAttached).ToList();

            //Check for document references held in the IfcDocumentInformation objects
            var docRefsAttachedDocInfo = docAllInfos.Where(docInfo => docInfo.DocumentReferences != null).SelectMany(docInfo => docInfo.DocumentReferences);
            //remove from Not Attached list
            docRefsNotAttached = docRefsNotAttached.Except(docRefsAttachedDocInfo).ToList();

            return docInfosNotAttached.Cast<IfcDocumentSelect>().Concat(docRefsNotAttached);
        }


        /// <summary>
        /// Document linked to objects
        /// </summary>
        /// <returns>IfcDocumentSelect attached to ifcRoot objects,</returns>
        private Dictionary<IfcDocumentSelect, List<IfcRoot>> GetAssociatedDocuments()
        {
            var ifcRelAssociatesDocuments = _model.Instances.OfType<IfcRelAssociatesDocument>(); //linked to IfcRoot objects

            //get fall back owner history
            DocumentOwnerLookup = ifcRelAssociatesDocuments.ToDictionary(p => p.RelatingDocument, p => p);

            var dups = ifcRelAssociatesDocuments.GroupBy(d => d.RelatingDocument).SelectMany(grp => grp.Skip(1)); //get any duplicate related documents objects

            //merge any duplicate IfcDocumentSelect IfcRoot objects to a single link of IfcDocumentSelect to IfcRoot list
            Dictionary<IfcDocumentSelect, List<IfcRoot>> docToObjs = null;
            if (dups.Any())
            {
                //remove the duplicates related documents objects and convert to dictionary
                docToObjs = ifcRelAssociatesDocuments.Except(dups).ToDictionary(p => p.RelatingDocument, p => p.RelatedObjects.ToList());
                //merge any duplicate related documents objects documents into list of single link of IfcDocumentSelect to IfcRoot list, as duplicate related object could hold different documents so lets not lose them
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
            if (usedNames != null && usedNames.Any())
            {
                var found = usedNames.Where(d => d.StartsWith(name, StringComparison.OrdinalIgnoreCase)).Select(n => n);

                if (found.Any())
                {
                    if ((found.Count() == 1) && (found.First().Length == name.Length)) //we match the whole name
                    {
                        return name + "(1)"; //first duplicate
                    }
                    var srch = name + "(";

                    //we have duplicates so get names that are in correct format
                    var correctFormat = found.Where(s => s.StartsWith(srch, StringComparison.OrdinalIgnoreCase) && s.EndsWith(")"));
                    if (correctFormat.Any())
                    {
                        var number = correctFormat.Max(s => GetNextNo(srch, s));//.OrderBy(s => s).LastOrDefault();
                        if (number > 0)
                        {
                            return srch + number.ToString() + ")";
                        }
                    }
                }
            }
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
            _systemAssignment = new Dictionary<IfcSystem, ObjectDefinitionSet>();
            if (SystemMode.HasFlag(SystemExtractionMode.System))
            {
                _systemAssignment =
                        _model.Instances.OfType<IfcRelAssignsToGroup>().Where(r => r.RelatingGroup is IfcSystem)
                        .Distinct(new IfcRelAssignsToGroupRelatedGroupObjCompare()) //make sure we do not have duplicate keys, or ToDictionary will throw ex. could lose RelatedObjects though. 
                        .ToDictionary(k => (IfcSystem)k.RelatingGroup, v => v.RelatedObjects);
                _systemLookup = new Dictionary<IfcObjectDefinition, List<IfcSystem>>();
                ReportProgress.NextStage(SystemAssignment.Count, 35);
                foreach (var systemAssignment in SystemAssignment)
                {
                    foreach (var objectDefinition in systemAssignment.Value)
                    {
                        if (_systemLookup.ContainsKey(objectDefinition))
                            _systemLookup[objectDefinition].Add(systemAssignment.Key);
                        else
                            _systemLookup.Add(objectDefinition, new List<IfcSystem>(new[] { systemAssignment.Key }));
                    }
                    ReportProgress.IncrementAndUpdate();
                }
            }

            //Use PropertySet Property with names matching config values on section name = SystemPropertyMaps with key=SystemMaps
            SystemViaPropAssignment = new Dictionary<IfcPropertySet, IEnumerable<IfcObject>>();
            if (SystemMode.HasFlag(SystemExtractionMode.PropertyMaps))
            {
                var props = GetPropMap("SystemMaps");
                ReportProgress.NextStage(props.Count(), 35);
                foreach (var propertyName in props)
                {
                    var propmap = propertyName.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (propmap.Count() == 2)
                    {
                        var sets = Model.Instances.OfType<IfcPropertySet>()
                            .Where(ps => ps.Name != null && propmap[0].Equals(ps.Name, StringComparison.OrdinalIgnoreCase)
                                    && ps.PropertyDefinitionOf.Any()
                                    && ps.HasProperties.OfType<IfcPropertySingleValue>().Where(psv => psv.Name == propmap[1]).Any()
                                    && !SystemViaPropAssignment.ContainsKey(ps)
                                    )
                            .SelectMany(ps => ps.PropertyDefinitionOf)
                            .Where(dbp => dbp.RelatedObjects.Where(e => _objectToTypeObjectMap.Keys.Contains(e)).Any()) //only none filtered objects
                            .ToDictionary(dbp => dbp.RelatingPropertyDefinition as IfcPropertySet, dbp => dbp.RelatedObjects.AsEnumerable());

                        SystemViaPropAssignment = SystemViaPropAssignment.Concat(sets).ToDictionary(p => p.Key, p => p.Value);
                    }
                    ReportProgress.IncrementAndUpdate();
                } 
            }
        }

        /// <summary>
        /// Get the property mappings for a given field name
        /// </summary>
        /// <param name="FiledKey">Field name</param>
        /// <returns>string[]</returns>
        public string[] GetPropMap(string FiledKey)
        {
            string[] propertyNames;
            if (_cobieFieldMap.TryGetValue("SystemMaps", out propertyNames))
            {
                return propertyNames;
            }
            return new string[] { };
        }
        

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<XbimIfcProxyTypeObject, List<IfcElement>> DefiningTypeObjectMap
        {
            get { return _definingTypeObjectMap; }
        }
        private void GetTypeMaps()
        {

            var relDefinesByType = _model.Instances.OfType<IfcRelDefinesByType>().Where(r => !Filter.ObjFilter(r.RelatingType)).ToList();
            //creates a dictionary of uniqueness for type objects
            var propertySetHashes = new Dictionary<string,string>();
            var proxyTypesByKey = new Dictionary<string, XbimIfcProxyTypeObject>();
            var relDefinesByRelType = relDefinesByType.Select(r => r.RelatingType);
            ReportProgress.NextStage(relDefinesByRelType.Count(), 17);
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

            var assemblyParts = new HashSet<IfcObjectDefinition>(_model.Instances.OfType<IfcRelAggregates>().SelectMany(a => a.RelatedObjects));
            var grouping = relDefinesByType.GroupBy(k => proxyTypesByKey[GetTypeObjectHashString(k.RelatingType)],
                kv => kv.RelatedObjects).ToList();
            ReportProgress.NextStage(grouping.Count(), 19);
            foreach (var group in grouping)
            {
                //filter on in assembly, and ifcElement if filtered in ProductFilter even if the ifcTypeObject is not filtered (passed filter in relDefinesByType assignment above)
                var allObjects = group.SelectMany(o => o).OfType<IfcElement>().Where(e => !assemblyParts.Contains(e) && !Filter.ObjFilter(e, false)).ToList();  
                _definingTypeObjectMap.Add(group.Key,allObjects);
                ReportProgress.IncrementAndUpdate();
            }
            
            _objectToTypeObjectMap = new Dictionary<IfcObject, XbimIfcProxyTypeObject>();


            ReportProgress.NextStage(_definingTypeObjectMap.Count(), 21);
            foreach (var typeObjectToObjects in _definingTypeObjectMap)
            {
                foreach (var ifcObject in typeObjectToObjects.Value.Where(t => !(t is IfcFeatureElement) && !assemblyParts.Contains(t)))
                {
                    _objectToTypeObjectMap.Add(ifcObject, typeObjectToObjects.Key);
                }
                ReportProgress.IncrementAndUpdate();
            }

            //**NOTE**: removed _classifiedObjects from existingAssets as some elements do not have a type but have a classification, this excludes them from the _objectToTypeObjectMap, not what we want I think
            
            //get all Assets that don't belong to an Ifc Type or are not classified
            //get all IfcElements that aren't classified or have a type
            //var existingAssets = _classifiedObjects.Keys.OfType<IfcElement>()
            //    .Concat(_objectToTypeObjectMap.Keys.OfType<IfcElement>()).Distinct();
            var existingAssets = _objectToTypeObjectMap.Keys.OfType<IfcElement>();

            //retrieve all the IfcElements from the model and exclude them if they are a member of an IfcType, 
            var unCategorizedAssets = _model.Instances.OfType<IfcElement>()
                .Where(t => !(t is IfcFeatureElement) && !assemblyParts.Contains(t) && !Filter.ObjFilter(t)) //filter ifcElement it ifcTypeObject it is defined by is in excluded list of ifcTypeobjects
                .Except(existingAssets);
            //convert to a Lookup with the key the type of the IfcElement and the value a list of IfcElements
            //if the object has a classification we use this to distinguish types

            var unCategorizedAssetsWithTypes = unCategorizedAssets.GroupBy
                (t=>GetProxyTypeObject(t).Name, v => v).ToDictionary(k=>k.Key,v=>v.ToList());
            ReportProgress.NextStage(unCategorizedAssetsWithTypes.Count(), 23);
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

            var assetRels = _model.Instances.OfType<IfcRelAssignsToGroup>()
                .Where(r => r.RelatingGroup is IfcAsset);

            _assetAsignments = new Dictionary<IfcTypeObject, IfcAsset>();
            ReportProgress.NextStage(assetRels.Count(), 25);
            foreach (var assetRel in assetRels)
            {
                foreach (var assetType in assetRel.RelatedObjects)
                    if ((assetType is IfcTypeObject) && !Filter.ObjFilter(assetType))
                        AssetAsignments[(IfcTypeObject)assetType] = (IfcAsset)assetRel.RelatingGroup;
                ReportProgress.IncrementAndUpdate();
            }
           
        }

        private static string GetTypeObjectHashString(IfcTypeObject typeObject)
        {
            var hashString = "";
            if (typeObject.HasPropertySets != null && typeObject.HasPropertySets.Any())
            {
                var labels = typeObject.HasPropertySets.Select(t => t.EntityLabel).OrderBy(e => e);

                foreach (var label in labels)
                {
                    hashString += label+":";
                }
            }
            //might be good to add classification
            hashString += typeObject.Name+":";
            hashString += typeObject.GetType().Name;
            return hashString;
        }

        private string ChangeNameFromStyleToType(IfcTypeObject ifcTypeObject)
        {
            if (ifcTypeObject is IfcDoorStyle )
                return "DoorType";
            if (ifcTypeObject is IfcWindowStyle)
                return "WindowType";
            return ifcTypeObject.GetType().Name.Substring(3);
            
        }

        private string BuildTypeName(IfcTypeObject ifcTypeObject)
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
        public XbimIfcProxyTypeObject GetProxyTypeObject(IfcElement element)
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
           if (categories != null && categories.Any())
           {
               //prefer the Uniclass2015 code
               foreach (var category in categories)
               {
                   if (category.Classification != null &&
                       category.Classification.ToUpperInvariant().Contains("UNICLASS2015"))
                       return new XbimIfcProxyTypeObject(this, string.Format("{0}Type {1}", element.GetType().Name.Substring(3), category.Code));
               }
               //otherwise take the first
               return new XbimIfcProxyTypeObject(this,string.Format("{0}Type {1}", element.GetType().Name.Substring(3), categories.First().Code));
           }
           //its unclassified
           if (!string.IsNullOrWhiteSpace(name))
           {
               
               return new XbimIfcProxyTypeObject(this, string.Format("{0}Type {1}",element.GetType().Name.Substring(3), name));
           }
           return new XbimIfcProxyTypeObject(this, AllocateTypeName(element.GetType().Name.Substring(3)+"Type"));
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
            COBiePropertyMapping propertyMaps = new COBiePropertyMapping(new FileInfo(tmpFile));
            _cobieFieldMap = propertyMaps.GetDictOfProperties();
            _cobieProperties = new HashSet<string>(_cobieFieldMap.SelectMany(pair => pair.Value).ToList());
            
            
            if (_configFileName == null)
            {
                File.Delete(tmpFile);
            }
        }

        private void GetPropertySets()
        {
            _attributedObjects = new Dictionary<IfcObjectDefinition, XbimAttributedObject>();
            var relProps = _model.Instances.OfType<IfcRelDefinesByProperties>().ToList();
            ReportProgress.NextStage(relProps.Count, 29);
            foreach (var relProp in relProps)
            {
                //get objects left after the IfcElement filters, plus none IfcElement (floors, spaces...)
                var filteredObjects = relProp.RelatedObjects.Where(obj => _objectToTypeObjectMap.Keys.Contains(obj) || !(obj is IfcElement));
                foreach (var ifcObject in filteredObjects)
                {
                    XbimAttributedObject attributedObject;
                    if (!_attributedObjects.TryGetValue(ifcObject, out attributedObject))
                    {
                        attributedObject = new XbimAttributedObject(ifcObject);
                        _attributedObjects.Add(ifcObject, attributedObject);
                    }
                    attributedObject.AddPropertySetDefinition(relProp.RelatingPropertyDefinition);  
                }
                ReportProgress.IncrementAndUpdate();
            }
            //process type objects ignoring pure proxies
            var defTypeToProxy = _definingTypeObjectMap.Keys.Where(t => t.IfcTypeObject != null);
            ReportProgress.NextStage(defTypeToProxy.Count(), 33);
            foreach (var typeObject in defTypeToProxy)
            {
                XbimAttributedObject attributedObject;
                if (!_attributedObjects.TryGetValue(typeObject.IfcTypeObject, out attributedObject))
                    {
                        attributedObject = new XbimAttributedObject(typeObject.IfcTypeObject);
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
            _spatialDecomposition = _model.Instances.OfType<IfcRelAggregates>().Where(r=>r.RelatingObject is IfcSpatialStructureElement)
                .ToDictionary(ifcRelAggregate => (IfcSpatialStructureElement) ifcRelAggregate.RelatingObject, ifcRelAggregate => ifcRelAggregate.RelatedObjects.OfType<IfcSpatialStructureElement>().ToList());
            ReportProgress.NextStage(_spatialDecomposition.Count(), 10);
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
                ReportProgress.IncrementAndUpdate();
            }

            var relZones = _model.Instances.OfType<IfcRelAssignsToGroup>().Where(r=>r.RelatingGroup is IfcZone).ToList();
            ReportProgress.NextStage(relZones.Count(), 13);
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
                ReportProgress.IncrementAndUpdate();
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



        public List<IfcActorSelect> Contacts
        {
            get { return _contacts; }
        }

        public Dictionary<string, Contact> SundryContacts
        {
            get { return _sundryContacts; }
        }

        public SystemExtractionMode SystemMode
        {
            get;
            internal set;
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
            ReportProgress.NextStage(_classifiedObjects.Count(), 8);
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
                    ReportProgress.IncrementAndUpdate();
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
                if (classification.ItemReference.HasValue && classification.Name.HasValue &&
                    string.CompareOrdinal(classification.ItemReference, classification.Name) == 0)
                {
                    var strRef = classification.ItemReference.Value.ToString();
                    var parts = strRef.Split(new[] {':', ';', '/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                        category.Description = parts[1];
                    if (parts.Length > 0)
                        category.Code = parts[0];
                }
                else
                {
                    category.Code = classification.ItemReference;
                    category.Description = classification.Name;
                }
                if (category.Code != null && string.CompareOrdinal(category.Code.ToLower(),"n/a")!=0 )
                    categories.Add(category);
            }
            return categories;
        }

        /// <summary>
        /// Set Category with code and description as single delimited string
        /// </summary>
        /// <param name="strRef">Uniclass string</param>
        /// <returns>List of Category Objects</returns>
        private List<Category> ConvertToCategories(string strRef)
        {
            var categories = new List<Category>();
            var category = new Category();
            var parts = strRef.Split(new[] { ':', ';', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
                category.Description = parts[1];
            if (parts.Length > 0)
                category.Code = parts[0];
            categories.Add(category);
            return categories;
        }

        /// <summary>
        /// Set Category with code and description
        /// </summary>
        /// <param name="code">Uniclass code</param>
        /// <param name="desc">Uniclass description</param>
        /// <returns>List of Category Objects</returns>
        private List<Category> ConvertToCategories(string code, string desc)
        {
            var categories = new List<Category>();
            var category = new Category();
            if (!string.IsNullOrEmpty(code))
                category.Code = code;
            if (!string.IsNullOrEmpty(desc))
                category.Description = desc;
            categories.Add(category);
            return categories;
        }

        /// <summary>
        /// Returns the COBie Category for this object, based on the Ifc Classification
        /// </summary>
        /// <param name="classifiedObject"></param>
        /// <returns></returns>
        public List<Category> GetCategories(IfcRoot classifiedObject, bool useProp = true)
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
            //get category from properties
            if (useProp && (classifiedObject is IfcObjectDefinition))
            {
               
                var code = GetCoBieProperty("CommonCategoryCode", classifiedObject as IfcObjectDefinition);
                var desc = GetCoBieProperty("CommonCategoryDescription", classifiedObject as IfcObjectDefinition);
                if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(desc))
                {
                    return ConvertToCategories(code, desc);
                }
                else
                {
                    var cat = GetCoBieProperty("CommonCategoryCode", classifiedObject as IfcObjectDefinition);
                    if (!string.IsNullOrEmpty(cat))
                    {
                        return ConvertToCategories(cat);
                    }
                    else if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(desc))
                    {
                        return UnknownCategory;
                    }
                    else //have code, or description
                    {
                        return ConvertToCategories(cat);
                    }
                }
            }
            return UnknownCategory;
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
        public TValue? GetCoBieProperty<TValue>(string valueName, IfcObject ifcObject) where TValue:struct
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

        

        private IfcTypeObject GetDefiningTypeObject(IfcObject ifcObject)
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
        public XbimAttributedObject GetAttributesObj(IfcObjectDefinition ifcObjectDefinition)
        {
            XbimAttributedObject attributedObject;
            if (_attributedObjects.TryGetValue(ifcObjectDefinition, out attributedObject))
            {
                return attributedObject;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObjectDefinition"></param>
        /// <returns></returns>
        public List<Attribute> GetAttributes(IfcObjectDefinition ifcObjectDefinition)
        {
            var uniqueAttributes = new Dictionary<string, Attribute>();
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
                        var newAttribute = XbimAttributedObject.ConvertToAttributeType(property);
                        var pSetDef = attributedObject.GetPropertySetDefinition(pSetName);
                        
                        if (pSetDef != null)
                        {
                            newAttribute.CreatedBy = GetCreatedBy(pSetDef);
                            newAttribute.CreatedOn = GetCreatedOn(pSetDef);
                            newAttribute.ExternalId = ExternalEntityIdentity(pSetDef);
                            newAttribute.ExternalSystem = ExternalSystemName(pSetDef);
                        }
                        else
                        {
                            newAttribute.CreatedBy = GetCreatedBy();
                            newAttribute.CreatedOn = GetCreatedOn();
                            newAttribute.ExternalSystem = ExternalSystemName();
                        }
                        
                        newAttribute.PropertySetName = pSetName;
                        Attribute existingAttribute;
                        if (uniqueAttributes.TryGetValue(newAttribute.Name, out existingAttribute))
                            //it is a duplicate so append the pset name
                        {
                            
                            var keyName = string.Format("{0}.{1}", existingAttribute.Name, existingAttribute.PropertySetName);
                            if(!uniqueAttributes.ContainsKey(keyName))
                            {
                                uniqueAttributes.Remove(existingAttribute.Name);
                                existingAttribute.Name = keyName;
                                uniqueAttributes.Add(keyName, existingAttribute); //update existing key
                            }
                            newAttribute.Name = string.Format("{0}.{1}", newAttribute.Name, newAttribute.PropertySetName);
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
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifcObjectDefinition"></param>
        /// <returns></returns>
        public Attribute MakeAttribute(IfcObjectDefinition ifcObjectDefinition, string attributeName, object attributeValue)
        {
            var newAttribute = new Attribute();
            newAttribute.CreatedBy = GetCreatedBy(ifcObjectDefinition);
            newAttribute.CreatedOn = GetCreatedOn(ifcObjectDefinition);
            newAttribute.ExternalId = ExternalEntityIdentity(ifcObjectDefinition);
            newAttribute.ExternalSystem = ExternalSystemName(ifcObjectDefinition);
            newAttribute.Name = attributeName;
            newAttribute.Value = AttributeValue.CreateFromObject(attributeValue);
            return newAttribute;
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

        internal string ExternalSystemName(IfcRoot ifcObject = null, bool usePropFirst = false)
        {
            if (ExternalReferenceMode == ExternalReferenceMode.IgnoreSystem ||
                ExternalReferenceMode == ExternalReferenceMode.IgnoreSystemAndEntityName)
                return null;
            if (usePropFirst && (ifcObject is IfcObjectDefinition))//support for COBie Toolkit for Autodesk Revit(had this in on old code, not sure if still relevant. this note date 8/10/2015)
            {
                var extSystem = GetCoBieProperty("CommonExtSystem", ifcObject as IfcObjectDefinition);
                if (!string.IsNullOrEmpty(extSystem))
                {
                    return extSystem;
                }
            }
            return "xBIM"; //GetCreatingApplication(ifcObject);
        }


 
        private void GetSpaceAssetLookup()
        {
          
            //get all elements that are contained in any spatial structure of this building
            _spaceAssetLookup = new Dictionary<IfcElement, List<IfcSpatialStructureElement>>(); 
           
            var ifcRelContainedInSpaces = _model.Instances.OfType<IfcRelContainedInSpatialStructure>().ToList();
            ReportProgress.NextStage(ifcRelContainedInSpaces.Count, 40);
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
                ReportProgress.IncrementAndUpdate();
            }
           
        }
        /// <summary>
        /// Returns all assets in the building but removes
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
            //remove
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
        public void GetContacts()
        {
            
            //get any actors and select their
            var ifcActors = _model.Instances.OfType<IfcActor>().ToList();
            var actors = new HashSet<IfcActorSelect>(ifcActors.Select(a => a.TheActor)); //unique actors

            var personOrgs =  new HashSet<IfcActorSelect>(_model.Instances.OfType<IfcPersonAndOrganization>().Where(p => !actors.Contains(p)));
            actors = new HashSet<IfcActorSelect>(actors.Concat(personOrgs));

            var orgAlreadyIn = actors.OfType<IfcPersonAndOrganization>().Select(po => po.TheOrganization);
            var orgs = _model.Instances.OfType<IfcOrganization>().Where(p => !orgAlreadyIn.Contains(p) && p.Addresses != null); //lets only see ones with any address info
            actors = new HashSet<IfcActorSelect>(actors.Union(orgs)); //union will exclude duplicates

            var personsAlreadyIn = actors.Where(a => a is IfcPerson);
            personsAlreadyIn = personsAlreadyIn.Union(actors.OfType<IfcPersonAndOrganization>().Select(po => po.ThePerson));//union will exclude duplicates
            var persons = new HashSet<IfcActorSelect>(_model.Instances.OfType<IfcPerson>().Where(p => !personsAlreadyIn.Contains(p)));

            _contacts = actors.Concat(persons).ToList();

            _actors = new Dictionary<IfcActorSelect, IfcActor>();
            //set progress report
            ReportProgress.NextStage(ifcActors.Count + actors.OfType<IfcPersonAndOrganization>().Count(), 5);
            foreach (var actor in ifcActors)
            {
                if(!_actors.ContainsKey(actor.TheActor))
                    _actors.Add(actor.TheActor,actor);
                ReportProgress.IncrementAndUpdate();
            }

            _createdByKeys = new Dictionary<IfcActorSelect, ContactKey>();
            //sort out createdByKeys, these will always be IfcPersonAndOrganization which are held in IfcOwnerHistory fields
            foreach (var actor in actors.OfType<IfcPersonAndOrganization>())
            {
                _createdByKeys.Add(actor, new ContactKey { Email = EmailAddressOf(actor) });
                ReportProgress.IncrementAndUpdate();
            }
            _sundryContacts = new Dictionary<string, Contact>();
        }

        public string EmailAddressOf(IfcActorSelect personOrg)
        {
            IfcPerson person = null;
            IfcOrganization organisation= null;
            if (personOrg is IfcPerson)
            {
                person = personOrg as IfcPerson;
            }
            if (personOrg is IfcOrganization)
            {
                organisation = personOrg as IfcOrganization;
            }
            if (personOrg is IfcPersonAndOrganization)
            {
                person = (personOrg as IfcPersonAndOrganization).ThePerson;
                organisation = (personOrg as IfcPersonAndOrganization).TheOrganization;
            }
            
            //get a default that will be unique
            var email = string.Format("unknown{0}@undefined.email", personOrg.EntityLabel);
            if ((organisation != null) && (organisation.Addresses != null))
            {
                var telecom = organisation.Addresses.OfType<IfcTelecomAddress>().FirstOrDefault(a=>a.ElectronicMailAddresses.Any(e=>!string.IsNullOrWhiteSpace(e)));
                if (telecom != null)
                    email = telecom.ElectronicMailAddresses.FirstOrDefault(e => !string.IsNullOrWhiteSpace(e));
            }
            //overwrite if we have it at person level
            if ((person != null) && (person.Addresses != null))
            {
                var telecom = person.Addresses.OfType<IfcTelecomAddress>().FirstOrDefault(a => a.ElectronicMailAddresses.Any(e => !string.IsNullOrWhiteSpace(e)));
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
        /// 
        /// </summary>
        /// <param name="ifcPropertySet"></param>
        /// <returns></returns>
        public IEnumerable<IfcObjectDefinition> GetSystemAssignments(IfcPropertySet ifcPropertySet)
        {
            IEnumerable<IfcObject> assignments;
            if (SystemViaPropAssignment.TryGetValue(ifcPropertySet, out assignments))
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

        internal string GetFacilityName(IfcBuilding building)
        {
            if (!string.IsNullOrWhiteSpace(building.Name))
                return building.Name;
            if (!string.IsNullOrWhiteSpace(_model.IfcProject.Name))
                return _model.IfcProject.Name;
            return "Unknown";
            
        }

        internal ContactKey GetCreatedBy(IfcRoot ifcRoot = null, bool usePropFirst = false)
        {
            ContactKey key;
            if (ifcRoot != null)
            {
                if (usePropFirst && (ifcRoot is IfcObjectDefinition))//support for COBie Toolkit for Autodesk Revit(had this in on old code, not sure if still relevant. this note date 8/10/2015)
                {
                    var email = GetCoBieProperty("CommonCreatedBy", ifcRoot as IfcObjectDefinition);
                    if (!string.IsNullOrEmpty(email))
                    {
                        return new ContactKey { Email = email };
                    }
                }
                else if (ifcRoot.OwnerHistory.LastModifyingUser != null)
                {
                    if (_createdByKeys.TryGetValue(ifcRoot.OwnerHistory.LastModifyingUser, out key))
                        return key;
                }
                else if (ifcRoot.OwnerHistory.OwningUser != null)
                {
                    if (_createdByKeys.TryGetValue(ifcRoot.OwnerHistory.OwningUser, out key))
                        return key;
                }

            }
            key = new ContactKey {Email=XbimCreatedBy.Email};
            return key;
        }

        internal DateTime? GetCreatedOn(IfcRoot ifcRoot = null, bool usePropFirst = false)
        {
            //use last modified date if we have one
            if (ifcRoot != null)
            {
                if (usePropFirst)
                {
                    DateTime? propDate;
                    if (GetCreatedOnFromProp(ifcRoot, out propDate))
                    {
                        return propDate;
                    }
                }
                var dateTime = ifcRoot.OwnerHistory.LastModifiedDate ?? ifcRoot.OwnerHistory.CreationDate;
                if (dateTime != null)
                {
                    return IfcTimeStamp.ToDateTime(dateTime);
                }
                
               
            }
            return DateTime.Now;
        }
        /// <summary>
        /// Get CreateDate from properties
        /// </summary>
        /// <param name="ifcRoot">object to get properties on</param>
        /// <param name="date">out Date</param>
        /// <returns>bool</returns>
        private bool GetCreatedOnFromProp(IfcRoot ifcRoot, out DateTime? date)
        {
            DateTime foundDate;
            if (ifcRoot is IfcObjectDefinition)//support for COBie Toolkit for Autodesk Revit(had this in on old code, not sure if still relevant. this note date 8/10/2015)
            {
                var createdOn = GetCoBieProperty("CommonCreatedOn", ifcRoot as IfcObjectDefinition);
                if (!string.IsNullOrEmpty(createdOn))
                {
                   if (DateTime.TryParse(createdOn, out foundDate))
                    {
                        date = foundDate;
                        return true;
                    }
                    else
                    {
                        //try and get just the date part of the date time, as a conversion above failed, so assume time might be corrupt
                        int idx = createdOn.IndexOf("T");
                        if (idx > -1)
                        {
                            string datestr = createdOn.Substring(0, idx);
                            if (DateTime.TryParse(datestr, out foundDate))
                            {
                                date = foundDate;
                                return true;
                            }
                        }
                    }
                }
            }
            date = null;
            return false;
        }

        /// <summary>
        /// Get ContactKey for CreatedBy, first from IfcActor oOwnerHistory, then IfcActorSelect returning the ContactKey for the IfcActorSelect, unless defaultOnNoActor is true then default is returned, no matches also returns default
        /// </summary>
        /// <param name="actorSelect">IfcActorSelect Object</param>
        /// <param name="defaultOnNoActor">bool true skips IfcActorSelect key lookup</param>
        /// <returns>ContactKey</returns>
        internal ContactKey GetCreatedBy(IfcActorSelect actorSelect, bool defaultOnNoActor = false)
        {
            IfcActor actor;
            if (_actors.TryGetValue(actorSelect, out actor))
            {
                return GetCreatedBy(actor);
            }
            if (defaultOnNoActor)
            {
                return new ContactKey { Email = XbimCreatedBy.Email };
            }
            ContactKey key;
            if (_createdByKeys.TryGetValue(actorSelect, out key))
            {
                return key;
            }
            return new ContactKey {Email = XbimCreatedBy.Email};
        }

        internal DateTime? GetCreatedOn(IfcActorSelect actorSelect)
        {
            IfcActor actor;
            if (_actors.TryGetValue(actorSelect, out actor))
                return GetCreatedOn(actor);
            return DateTime.Now;
        }

        

        internal Zone CreateXbimDefaultZone()
        {
            return new Zone
            {
                Name = "Default Zone",
                CreatedBy = new ContactKey{Email=XbimCreatedBy.Email},
                CreatedOn = DateTime.Now,
                Spaces = new List<SpaceKey>(),
                Categories = UnknownCategory
            };
        }

        internal Xbim.COBieLiteUK.System CreateUndefinedSystem()
        {
            return new Xbim.COBieLiteUK.System
            {
                Name = "Default System",
                CreatedBy = new ContactKey { Email = XbimCreatedBy.Email },
                CreatedOn = DateTime.Now,
                Components = new List<AssetKey>(),
                Categories = UnknownCategory
            };
        }

        internal ContactKey GetOrCreateContactKey(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || 
                string.Compare(email, "n/a", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(email, "User To Populate", StringComparison.OrdinalIgnoreCase)==0) return null;
            Contact contact;
            var actorContactKey =
                _createdByKeys.Values.FirstOrDefault(c => String.Compare(c.Email, email, StringComparison.OrdinalIgnoreCase) == 0);
            if (actorContactKey != null)
                return actorContactKey;
            if (!SundryContacts.TryGetValue(email, out contact))
            {
                contact = new Contact { Email = email, CreatedBy = GetCreatedBy(), CreatedOn = GetCreatedOn(),Categories = UnknownCategory};
                SundryContacts.Add(email, contact);
            }
            return new ContactKey { Email = email };
        }
    }


    
}
