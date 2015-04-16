using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Xbim.COBieLiteUK.Converters;
using Formatting = System.Xml.Formatting;

namespace Xbim.COBieLiteUK
{
    public partial class Facility
    {
        public Facility()
        {
            Metadata = new Metadata();
        }

        /// <summary>
        /// Creates a new class inheryting from CobieObject and sets it to belong to the facility.
        /// </summary>
        /// <typeparam name="TNewCobieObject">The CobieObject type to create</typeparam>
        /// <returns></returns>
        public TNewCobieObject Create<TNewCobieObject>() where TNewCobieObject : CobieObject, new()
        {
            var retObject = new TNewCobieObject();
            retObject.SetFacility(this);
            return  retObject;
        }

        public IEnumerable<T> Get<T>(Func<T, bool> condition = null) where T : CobieObject
        {
            //make Facility part of the result
            var self = this as T;
            if (self != null)
            {
                if (condition == null) yield return self;
                else if (condition(self)) yield return self;
            }
            foreach (var child in GetDeep(condition))
                yield return child;
        }

        #region Enumerations

        public AreaUnit AreaUnits
        {
            get
            {
                if (String.IsNullOrEmpty(AreaUnitsCustom)) return AreaUnit.notdefined;

                //try to parse string value
                AreaUnit result;
                if (Enum.TryParse(AreaUnitsCustom, true, out result))
                    return result;

                //try to use aliases
                var enumMembers = typeof (AreaUnit).GetFields();
                foreach (var member in from member in enumMembers
                    let alias = member.GetCustomAttributes<AliasAttribute>()
                        .FirstOrDefault(
                            a => String.Equals(a.Value, AreaUnitsCustom, StringComparison.CurrentCultureIgnoreCase))
                    where alias != null
                    select member)
                    return (AreaUnit) member.GetValue(result);

                //if nothing fits it is a user defined value
                return AreaUnit.userdefined;
            }
            set
            {
                switch (value)
                {
                    case AreaUnit.notdefined:
                        AreaUnitsCustom = null;
                        break;
                    case AreaUnit.userdefined:
                        break;
                    default:
                        AreaUnitsCustom = Enum.GetName(typeof (AreaUnit), value);
                        break;
                }
            }
        }

        public LinearUnit LinearUnits
        {
            get
            {
                if (String.IsNullOrEmpty(LinearUnitsCustom)) return LinearUnit.notdefined;

                //try to parse string value
                LinearUnit result;
                if (Enum.TryParse(LinearUnitsCustom, true, out result))
                    return result;

                //try to use aliases
                var enumMembers = typeof (LinearUnit).GetFields();
                foreach (var member in from member in enumMembers
                    let alias = member.GetCustomAttributes<AliasAttribute>()
                        .FirstOrDefault(
                            a => String.Equals(a.Value, LinearUnitsCustom, StringComparison.CurrentCultureIgnoreCase))
                    where alias != null
                    select member)
                    return (LinearUnit) member.GetValue(result);

                //if nothing fits it is a user defined value
                return LinearUnit.userdefined;
            }
            set
            {
                switch (value)
                {
                    case LinearUnit.notdefined:
                        LinearUnitsCustom = null;
                        break;
                    case LinearUnit.userdefined:
                        break;
                    default:
                        LinearUnitsCustom = Enum.GetName(typeof (LinearUnit), value);
                        break;
                }
            }
        }

        public VolumeUnit VolumeUnits
        {
            get
            {
                if (String.IsNullOrEmpty(VolumeUnitsCustom)) return VolumeUnit.notdefined;

                //try to parse string value
                VolumeUnit result;
                if (Enum.TryParse(VolumeUnitsCustom, true, out result))
                    return result;

                //try to use aliases
                var enumMembers = typeof (VolumeUnit).GetFields();
                foreach (var member in from member in enumMembers
                    let alias = member.GetCustomAttributes<AliasAttribute>()
                        .FirstOrDefault(
                            a => String.Equals(a.Value, VolumeUnitsCustom, StringComparison.CurrentCultureIgnoreCase))
                    where alias != null
                    select member)
                    return (VolumeUnit) member.GetValue(result);

                //if nothing fits it is a user defined value
                return VolumeUnit.userdefined;
            }
            set
            {
                switch (value)
                {
                    case VolumeUnit.notdefined:
                        VolumeUnitsCustom = null;
                        break;
                    case VolumeUnit.userdefined:
                        break;
                    default:
                        VolumeUnitsCustom = Enum.GetName(typeof (VolumeUnit), value);
                        break;
                }
            }
        }

        public CurrencyUnit CurrencyUnit
        {
            get
            {
                if (String.IsNullOrEmpty(CurrencyUnitCustom)) return CurrencyUnit.notdefined;

                //try to parse string value
                CurrencyUnit result;
                if (Enum.TryParse(CurrencyUnitCustom, true, out result))
                    return result;

                //try to use aliases
                var enumMembers = typeof (CurrencyUnit).GetFields();
                foreach (var member in from member in enumMembers
                    let alias = member.GetCustomAttributes<AliasAttribute>()
                        .FirstOrDefault(
                            a => String.Equals(a.Value, CurrencyUnitCustom, StringComparison.CurrentCultureIgnoreCase))
                    where alias != null
                    select member)
                    return (CurrencyUnit) member.GetValue(result);

                //if nothing fits it is a user defined value
                return CurrencyUnit.userdefined;
            }
            set
            {
                switch (value)
                {
                    case CurrencyUnit.notdefined:
                        CurrencyUnitCustom = null;
                        break;
                    case CurrencyUnit.userdefined:
                        break;
                    default:
                        CurrencyUnitCustom = Enum.GetName(typeof (CurrencyUnit), value);
                        break;
                }
            }
        }

        #endregion

        #region XML serialization

        private static XmlSerializer GetXmlSerializer()
        {
            return new XmlSerializer(typeof (Facility));
        }

        public void WriteXml(string path, bool indented = false)
        {
            using (var stream = File.Create(path))
            {
                WriteXml(stream, indented);
                stream.Close();
            }
        }

        public void WriteXml(Stream stream, bool indented = false)
        {
            var serializer = GetXmlSerializer();
            using (var w = new StreamWriter(stream))
            {
                using (
                    var xmlWriter = new XmlTextWriter(w) {Formatting = indented ? Formatting.Indented : Formatting.None}
                    )
                {
                    serializer.Serialize(xmlWriter, this,
                        new XmlSerializerNamespaces(new[]
                        {
                            new XmlQualifiedName("cobielite", "http://openbim.org/schemas/cobieliteuk"),
                            new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance")
                        }));
                }
            }
        }

        public static Facility ReadXml(Stream stream)
        {
            var serializer = GetXmlSerializer();
            var facility = (Facility) serializer.Deserialize(stream);
            facility.SetFacility(facility);
            return facility;
        }

        public static Facility ReadXml(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var facility = ReadXml(stream);
                stream.Close();
                return facility;
            }
        }

        #endregion

        #region JSON serialization

        private static JsonSerializer GetJsonSerializer(bool indented = false)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
            serializerSettings.Converters.Add(new AttributeConverter());
            serializerSettings.Converters.Add(new KeyConverter());
            var serialiser = JsonSerializer.Create(serializerSettings);
            return serialiser;
        }

        #region Cloning through jsonSerialiser

        /// <summary>
        /// Clones a provided cobieobject via JsonSeriliser and then sets the result to belong to the cloning facility.
        /// </summary>
        /// <typeparam name="TNewCobieObject">The type of the object to clone.</typeparam>
        /// <param name="originalCobieObject">The CobieObject to be cloned via Json in memory.</param>
        /// <returns></returns>
        public TNewCobieObject Clone<TNewCobieObject>(TNewCobieObject originalCobieObject) where TNewCobieObject : CobieObject, new()
        {
            var mem = WriteJsonToMemory(originalCobieObject);
            var cloned = ReadJsonFrom<TNewCobieObject>(mem);
            cloned.SetFacility(this);
            return cloned;
        }

        public IEnumerable<TNewCobieObject> Clone<TNewCobieObject>(IEnumerable<TNewCobieObject> originalCobieObjects) where TNewCobieObject : CobieObject, new()
        {
            return originalCobieObjects.Select(Clone);
        }

        private JsonSerializer _cachedCloningSerialiser;

        private JsonSerializer CachedCloningSerialiser
        {
            get { return _cachedCloningSerialiser ?? (_cachedCloningSerialiser = GetJsonSerializer()); }
        }
        

        private byte[] WriteJsonToMemory<T>(T o)
        {
            var stream = new MemoryStream();
            using (var textWriter = new StreamWriter(stream))
            {
                CachedCloningSerialiser.Serialize(textWriter, o);
                stream.Flush();
            }
            return stream.GetBuffer();
        }

        private T ReadJsonFrom<T>(byte[] mem)
        {
            Stream stream = new MemoryStream(mem);
            using (var textReader = new StreamReader(stream))
            {
                var deserialised = (T)CachedCloningSerialiser.Deserialize(textReader, typeof(T));
                return deserialised;
            }
        }

        #endregion

        public void WriteJson(Stream stream, bool indented = false)
        {
            using (var textWriter = new StreamWriter(stream))
            {
                var serialiser = GetJsonSerializer(indented);
                serialiser.Serialize(textWriter, this);
            }
        }

        public void WriteJson(string path, bool indented = false)
        {
            using (var stream = File.Create(path))
            {
                WriteJson(stream, indented);
                stream.Close();
            }
        }


        public static Facility ReadJson(Stream stream)
        {
            using (var textReader = new StreamReader(stream))
            {
                var serialiser = GetJsonSerializer();
                var facility = (Facility) serialiser.Deserialize(textReader, typeof (Facility));
                facility.SetFacility(facility);
                return facility;
            }
        }

        public static Facility ReadJson(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var facility = ReadJson(stream);
                stream.Close();
                return facility;
            }
        }

        #endregion

        #region Reading COBie Spreadsheet

        public static Facility ReadCobie(string path, out string message, string version = "UK2012")
        {
            if (path == null) throw new ArgumentNullException("path");
            var ext = Path.GetExtension(path).ToLower().Trim('.');
            if (ext != "xls" && ext != "xlsx") throw new Exception("File must be an MS Excel file.");
            using (var file = File.OpenRead(path))
            {
                var type = ext == "xlsx" ? ExcelTypeEnum.XLSX : ExcelTypeEnum.XLS;
                var result = ReadCobie(file, type, out message, version);
                file.Close();
                return result;
            }
        }

        public static Facility ReadCobie(Stream stream, ExcelTypeEnum type, out string message,
            string version = "UK2012")
        {
            //use NPOI to open and access spreadsheet data
            IWorkbook workbook;
            switch (type)
            {
                case ExcelTypeEnum.XLS:
                    workbook = new HSSFWorkbook(stream);
                    break;
                case ExcelTypeEnum.XLSX:
                    workbook = new XSSFWorkbook(stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string msg;
            var flatList = ReadAllCobieObjects(workbook, out msg, version);
            message = msg ?? "";

            Debug.WriteLine("Reading all COBie objects: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();

            var facilities = flatList.OfType<Facility>().ToArray();
            if (facilities.Count() > 1)
                message +=
                    String.Format(
                        "There are {0} facilities in the data. Only first facility {1} will be used. This is an invalid COBie spreadsheet. \n",
                        facilities.Length, facilities[0].Name);
            var facility = facilities.FirstOrDefault();
            if (facility == null)
            {
                message +=
                    "There is no facility in the data. Default facility will be created as a root object. This is an invalid COBie spreadsheet. \n";
                facility = new Facility {Name = "Default facility"};
                flatList.Add(facility);
            }

            //create structure hierarchy
            var parallelMessage = new[] {""};
            var newTypes = new List<AssetType>();
            var typeDictionary = CreateTypeDictionary(flatList);
            Parallel.ForEach(flatList.ToArray(), o =>
            {
                string addToParentMsg;
                o.AddToParent(typeDictionary, facility, newTypes, out addToParentMsg, version);
                lock (parallelMessage)
                {
                    parallelMessage[0] += addToParentMsg;
                }
            });
            message += parallelMessage[0];

            //foreach (var cobieObject in flatList.ToArray())
            //{
            //    cobieObject.AddToParent(typeDictionary, facility, newTypes, out msg, version);
            //    message += msg;
            //}

            flatList.AddRange(newTypes);
            foreach (var cobieObject in flatList)
            {
                cobieObject.AfterCobieRead();
            }

            Debug.WriteLine("Building COBieLite hierarchy: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();


            //load metadate from the first sheet
            facility.Metadata = new Metadata();
            var log = new StringWriter();
            facility.Metadata.LoadFromCobie(workbook, log, version);
            message += log.ToString();

            //set facility for all objects
            facility.SetFacility(facility);

            return facility;
        }

        private static Dictionary<Type, CobieObject[]> CreateTypeDictionary(List<CobieObject> flatList)
        {
            var types =
                typeof(CobieObject).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(CobieObject).IsAssignableFrom(t));
            return types.ToDictionary(type => type, type => flatList.Where(o => o.GetType() == type).ToArray());
        }

        private static List<CobieObject> ReadAllCobieObjects(IWorkbook workbook, out string message, string version)
        {
            var result = new List<CobieObject>();
            message = "";
            var types =
                typeof (CobieObject).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof (CobieObject).IsAssignableFrom(t));
            foreach (var type in types)
            {
                var stop = new Stopwatch();
                stop.Start();

                string msg;
                result.AddRange(LoadFromCobie(type, workbook, out msg, version));
                message += msg;
                
                stop.Stop();
                Debug.WriteLine("   Loading {0}: {1}", type.Name, stop.ElapsedMilliseconds);
            }
            return result;
        }

        #endregion

        #region Writing COBie Spreadsheet

        public void WriteCobie(Stream stream, ExcelTypeEnum type, out string message,
            string version = "UK2012", bool useTemplate = true)
        {
            Stream templateStream = null;
            if (useTemplate)
            {
                var templateName = version + (type == ExcelTypeEnum.XLS ? ".xls" : ".xlsx");
                templateStream =
                    GetType()
                        .Assembly.GetManifestResourceStream(String.Format("{0}.Templates.{1}", GetType().Namespace,
                            templateName));
            }

            IWorkbook workbook;
            switch (type)
            {
                case ExcelTypeEnum.XLS:
                    workbook = templateStream == null ? new HSSFWorkbook() : new HSSFWorkbook(templateStream);
                    break;
                case ExcelTypeEnum.XLSX: //this is as it should be according to a standard
                    workbook = templateStream == null ? new XSSFWorkbook() : new XSSFWorkbook(templateStream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            var log = new StringWriter();
            var watch = new Stopwatch();
            watch.Start();

            WriteToCobie(workbook, log, null, new Dictionary<Type, int>(), new List<string>(), new Dictionary<string, int>(), version);

            watch.Stop();
            Debug.WriteLine("Creating NPOI model: {0}ms", watch.ElapsedMilliseconds);

            //refresh formulas
            switch (type)
            {
                case ExcelTypeEnum.XLS:
                    HSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                    break;
                case ExcelTypeEnum.XLSX:
                    XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            message = log.ToString();
            workbook.Write(stream);
        }

        public void WriteCobie(string path, out string message, string version = "UK2012", bool useTemplate = true)
        {
            if (path == null) throw new ArgumentNullException("path");
            var ext = Path.GetExtension(path).ToLower().Trim('.');
            if (ext != "xls" && ext != "xlsx")
            {
                //XLSX is Spreadsheet XML representation which is the one which should be used according to BS1192-4
                //therefore it is a default choice
                path += ".xlsx";
                ext = "xlsx";
            }
            using (var file = File.Create(path))
            {
                var type = ext == "xlsx" ? ExcelTypeEnum.XLSX : ExcelTypeEnum.XLS;
                WriteCobie(file, type, out message, version, useTemplate);
                file.Close();
            }
        }

        #endregion

        internal override void WriteToCobie(IWorkbook workbook, TextWriter loger, CobieObject parent,
            Dictionary<Type, int> rowNumCache, List<string> pickValuesCache, Dictionary<string, int> headerCache, string version = "UK2012")
        {
            base.WriteToCobie(workbook, loger, parent, rowNumCache, pickValuesCache, headerCache, version);

            //write metadata out
            if (Metadata != null)
                Metadata.WriteToCobie(workbook, loger, version);
        }

        internal override IEnumerable<CobieObject> GetChildren()
        {
            //enumerate base
            foreach (var child in base.GetChildren())
                yield return child;

            //enumerate own
            if (Floors != null)
                foreach (var floor in Floors)
                    yield return floor;
            if (AssetTypes != null)
                foreach (var assetType in AssetTypes)
                    yield return assetType;
            if (Contacts != null)
                foreach (var contact in Contacts)
                    yield return contact;
            if (Systems != null)
                foreach (var system in Systems)
                    yield return system;
            if (Zones != null)
                foreach (var zone in Zones)
                    yield return zone;
            if (Resources != null)
                foreach (var resource in Resources)
                    yield return resource;
            if (Stages != null)
                foreach (var stage in Stages)
                    yield return stage;
        }

        /// <summary>
        /// <p>
        /// This function sets facility object to all CobieObjects in the model
        /// so you can access referenced objects from keys as an enumerations.
        /// If you create new objects in the model you should always call this 
        /// method before you use any such enumerations. 
        /// </p>
        /// <p>
        /// You don't have to call it after you read the model from JSON, COBie XLS or XML
        /// as it is called automatically.
        /// </p>
        /// </summary>
        public void Refresh()
        {
            SetFacility(this);
        }

        #region Validation UK2012 (BS1192-4)

        private int _counter;
        // ReSharper disable once InconsistentNaming
        public void ValidateUK2012(TextWriter logger, bool fixIfPossible)
        {
            //initial counter value for automatic unique names
            _counter = 1001;
            _defaultSystem = null;
            _anyDefaultSpace = null;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Clarity of naming
            //To ensure the clarity of naming, the following should be done.
            //• Names should use the characters A‑Z, a‑z and 0‑9 with spaces and full stops.
            //• Contacts should be named by use of their valid email address, including “@”.
            //• Names should not contain commas or double spaces, nor unusual characters(e.g. &,%, ‘, “, <, >).
            //• Classifications should use the colon to separate code from description and should not use commas.
            var all = Get<CobieObject>().ToArray();
            var regex = new Regex("[,|&|%|‘|“|<|>]", RegexOptions.Compiled);
            var catRegex = new Regex("[:|,]", RegexOptions.Compiled);
            foreach (var o in all)
            {
                if (String.IsNullOrEmpty(o.Name))
                {
                    logger.WriteLine("Object of type {0} (description: {1}) doesn't have a 'Name'! This is illegal.",
                        o.GetType().Name, o.Description);
                    if (fixIfPossible) o.Name = String.Format("{0} {1}", o.GetType().Name, _counter++);
                }
                if (regex.IsMatch(o.Name))
                {
                    logger.WriteLine("Name {0} of {1} contains forbidden characters.", o.Name, o.GetType().Name);
                    if (fixIfPossible) o.Name = regex.Replace(o.Name, "");
                }
                foreach (var key in o.GetKeys().Where(key => regex.IsMatch(key.Name ?? "")))
                {
                    logger.WriteLine("Name {0} of {1} key contains forbidden characters.", key.Name,
                        key.GetSheet("UK2012"));
                    if (fixIfPossible) key.Name = regex.Replace(key.Name ?? "", "");
                }

                if (o.Categories != null && o.Categories.Any())
                {
                    foreach (var category in o.Categories)
                    {
                        if (category.Code != null && catRegex.IsMatch(category.Code))
                        {
                            logger.WriteLine("Category code {0} contains forbidden characters.", category.Code);
                            if (fixIfPossible) category.Code = catRegex.Replace(category.Code, "");
                        }

                        if (category.Description == null || !catRegex.IsMatch(category.Description)) continue;
                        logger.WriteLine("Category description {0} contains forbidden characters.", category.Description);
                        if (fixIfPossible) category.Description = catRegex.Replace(category.Description, "");
                    }
                }

                //Category entries should be provided.
                //Only Asset doesn't have a category in COBie XLS
                if ((o.Categories == null || !o.Categories.Any()) && !(o is Asset))
                {
                    logger.WriteLine("{0} '{1}' doesn't have a category defined.", o.GetType().Name, o.Name);
                    if (fixIfPossible)
                    {
                        if (o.Categories == null) o.Categories = new List<Category>();
                        o.Categories.Add(new Category {Code = "unknown"});
                    }
                }

                //CreatedBy is a foreign key which should be defined for all objects
                if (o.CreatedBy == null && fixIfPossible)
                    o.CreatedBy = GetDefaultContactKey();
            }
            stopWatch.Stop();
            Debug.WriteLine("   Validation of names (invalid characters) and categories: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();

            //Uniqueness of information should be ensured. Names should be unique within
            //their sheet, except that the System, Zone and Attribute names should be unique in
            //conjunction with other columns. (Martin Cerny: + Assembly, Connection, Job, Impact, Document, Coordinate, Issue)
            //a) On the “Attribute” sheet, every Attribute Name (column A), taken with Sheet‑Name (column E) and Row‑Name (column F) should be unique.
            //b) On the “System” sheet, every System Name (column A) taken with Component‑Names (column E) should be unique.
            //c) On the “Zone” sheet, every Zone Name (column A) taken with Space‑Names (column E) should be unique.
            CheckForUniqueNames(Contacts, logger, fixIfPossible);
            CheckForUniqueNames(Floors, logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Space>(), logger, fixIfPossible);
            CheckForUniqueNames(AssetTypes, logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Asset>(), logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Spare>(), logger, fixIfPossible);
            CheckForUniqueNames(Resources, logger, fixIfPossible);
            CheckForUniqueNames(Zones, logger, fixIfPossible);
            CheckForUniqueNames(Systems, logger, fixIfPossible);
            CheckForUniqueNames(Stages, logger, fixIfPossible);

            //Suplementary information
            CheckForUniqueNames(all.OfType<Assembly>(), logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Connection>(), logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Job>(), logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Impact>(), logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Document>(), logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Representation>(), logger, fixIfPossible);
            CheckForUniqueNames(all.OfType<Issue>(), logger, fixIfPossible);

            //attributes are only unique within a containing object
            foreach (var cobieObject in all.Where(cobieObject => cobieObject.Attributes != null))
                CheckForUniqueNames(cobieObject.Attributes, logger, fixIfPossible);

            stopWatch.Stop();
            Debug.WriteLine("   Checking unique names: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();

            var referenceWatch = new Stopwatch();
            referenceWatch.Start();

            //The integrity of references should be ensured as follows:
            //a) Every Space (location) should be assigned to one Floor (region). - If the name is unique this is granted by COBieLite data schema
            //b) Every Space (location) should be assigned to at least one Zone.
            var spaces = Get<Space>().ToList();
            var zones = Zones ?? new List<Zone>();
            foreach (
                var space in
                    spaces.Where(
                        space => !zones.Any(z => z.Spaces != null && z.Spaces.Select(s => s.Name).Contains(space.Name)))
                )
            {
                logger.WriteLine("Space '{0}' is not in any zone.", space.Name);
                if (!fixIfPossible) continue;

                if (Zones == null) Zones = new List<Zone>();
                var defaultZone = GetDefaultZone();
                defaultZone.Spaces.Add(new SpaceKey {Name = space.Name});
            }
            referenceWatch.Stop();
            Debug.WriteLine("   Every space in zone: " + referenceWatch.ElapsedMilliseconds);
            referenceWatch.Restart();

            //c) Every Floor and Zone should have at least one Space (location).
            if (Floors != null)
            {
                foreach (var floor in Floors.Where(f => f.Spaces == null || !f.Spaces.Any()).ToArray())
                {
                    logger.WriteLine("Floor {0} doesn't have any space assigned.", floor.Name);
                    if (!fixIfPossible) continue;
                    if (floor.Spaces == null) floor.Spaces = new List<Space>();
                    floor.Spaces.Add(GetNewDefaultSpace(false));
                }
            }
            if (Zones != null)
            {
                foreach (var zone in Zones.Where(z => z.Spaces == null || !z.Spaces.Any()))
                {
                    logger.WriteLine("Zone {0} doesn't have any space assigned.", zone.Name);
                    if (!fixIfPossible) continue;
                    if (zone.Spaces == null) zone.Spaces = new List<SpaceKey>();
                    var defaultSpace = GetAnyDefaultSpace();
                    zone.Spaces.Add(new SpaceKey {Name = defaultSpace.Name});
                }
            }
            referenceWatch.Stop();
            Debug.WriteLine("   Every floor and zone has a space: " + referenceWatch.ElapsedMilliseconds);
            referenceWatch.Restart();


            //d) Every Component should be assigned to at least one Space (location), from which it is used, inspected or maintained.
            //e) Every Component should be assigned to one Type. - This is granted by design of COBieLite 
            var assets = Get<Asset>().ToArray();
            foreach (var asset in assets.Where(a => a.Spaces == null || !a.Spaces.Any()))
            {
                logger.WriteLine("Component {0} is not assigned to any space.", asset.Name);
                if (!fixIfPossible) continue;
                var space = GetAnyDefaultSpace();
                if (asset.Spaces == null) asset.Spaces = new List<SpaceKey>();
                asset.Spaces.Add(new SpaceKey {Name = space.Name});
            }
            referenceWatch.Stop();
            Debug.WriteLine("   Every component is in space: " + referenceWatch.ElapsedMilliseconds);
            referenceWatch.Restart();

            //f) Every Component should be assigned to at least one System, identifying its function.
            foreach (
                var asset in
                    assets.Where(
                        a =>
                            Systems == null ||
                            !Systems.Any(s => s.Components != null && s.Components.Any(c => c.Name == a.Name))))
            {
                logger.WriteLine("Component {0} is not assigned to any system.", asset.Name);
                if (fixIfPossible)
                {
                    GetDefaultSystem().Components.Add(new AssetKey {Name = asset.Name});
                }
            }
            referenceWatch.Stop();
            Debug.WriteLine("   Every component is in system: " + referenceWatch.ElapsedMilliseconds);
            referenceWatch.Restart();

            //g) Every Type should apply to at least one Component.
            foreach (var type in AssetTypes.Where(t => t.Assets == null || !t.Assets.Any()))
            {
                logger.WriteLine("Type {0} doesn't contain any components.", type.Name);
                if (!fixIfPossible) continue;
                if (type.Assets == null) type.Assets = new List<Asset>();
                type.Assets.Add(GetNewDefaultAsset());
            }

            referenceWatch.Stop();
            Debug.WriteLine("   Every type has a component: " + referenceWatch.ElapsedMilliseconds);
            referenceWatch.Restart();

            stopWatch.Stop();
            Debug.WriteLine("   ------- Checking references (every type has a component, ...): " + stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();

            //h) Every reference to other sheets should be valid. - This is mostly granted by the schema itself. We only need to check the keys.
            ValidateKeys(logger, fixIfPossible);

            stopWatch.Stop();
            Debug.WriteLine("   Checking valid keys: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();
            //i) Every reference to PickList enumerations and classifications should be valid. - We don't store any specific pick lists like classifications etc. This depends on project specific settings.
            //j) Enumerations specified in the Attributes and PickLists should be adhered to.
        }

        private void ValidateKeys(TextWriter logger, bool fixIfPossible)
        {
            //get all cobie objects as a flat list to avoid tens of thousands of recursive searches
            var all = Get<CobieObject>().ToArray();
            //contacts are used in every single object so it is better to cache them and avoid tens of thousands of searches
            var defaultContact = GetDefaultContactKey();
            var cache = new Dictionary<Type, CobieObject[]>();
            foreach (var o in all)
            {
                foreach (var key in o.GetKeys().ToArray())
                {
                    CobieObject[] candidates;
                    //use type cache to get first level of candidates
                    if (!cache.TryGetValue(key.ForType, out candidates))
                    {
                        candidates = all.Where(c => c.GetType() == key.ForType).ToArray();
                        cache.Add(key.ForType, candidates);
                    }
                    //filter by name
                    candidates = candidates.Where(c => c.Name == key.Name).ToArray();

                    if (candidates.Length == 0)
                    {
                        logger.WriteLine("{0} key '{1}' from {2} '{3}' doesn't point to any object in the model.",
                            GetSheetName(key.ForType, "UK2012"), key.Name, GetSheetName(o.GetType(), "UK2012"), o.Name);
                        if (fixIfPossible)
                        {
                            var keyType = key.GetType().Name;
                            switch (keyType)
                            {
                                //these two are the most frequent keys
                                case "ContactKey":
                                    //set contact key to the default value
                                    ((ContactKey) key).Email = defaultContact.Email;
                                    break;
                                case "SpaceKey":
                                    //set space key to default value
                                    key.Name = GetAnyDefaultSpace().Name;
                                    break;
                                default:
                                    //Delete the key otherwise. It doesn't make a sense if it doesn't point to any object in the model.
                                    o.RemoveKey(key);
                                    break;
                            }
                        }
                    }
                    if (candidates.Length > 1)
                        logger.WriteLine(
                            "{0} key '{1}' from {2} '{3}' points to more than one object in the model. Ambiguous reference can't be fixed.",
                            key.ForType.Name, key.Name, o.GetType().Name, o.Name);
                }
            }
        }

        private Zone GetDefaultZone()
        {
            const string defaultName = "Default zone";
            if (Zones == null) Zones = new List<Zone>();
            var zone = Zones.FirstOrDefault(z => z.Name == defaultName);
            if (zone != null) return zone;

            zone = new Zone
            {
                Name = defaultName,
                CreatedOn = DateTime.Now,
                CreatedBy = GetDefaultContactKey(),
                Categories = GetDefaultCategories(),
                Spaces = new List<SpaceKey>()
            };
            Zones.Add(zone);
            return zone;
        }

        private void CheckForUniqueNames(IEnumerable<CobieObject> objects, TextWriter logger, bool fixIfPossible)
        {
            if (objects == null) return;

            var groups = objects.GroupBy(o => o.Name);
            foreach (var g in groups.Where(g => g.Count() > 1))
            {
                logger.WriteLine(
                    "{0} {1} doesn't have an unique name. There are {2} instances with the same name. If fixed it may break key references.",
                    g.First().GetType().Name, g.Key, g.Count());
                if (!fixIfPossible) continue;

                var counter = 0;
                foreach (var cobieObject in g)
                    cobieObject.Name = String.Format("{0} ({1})", cobieObject.Name, counter++);
            }
        }

        private Asset GetNewDefaultAsset()
        {
            var result = new Asset
            {
                Name = "Default component " + _counter++,
                CreatedBy = GetDefaultContactKey(),
                CreatedOn = DateTime.Now,
                Spaces = new List<SpaceKey>(new[] {new SpaceKey {Name = GetAnyDefaultSpace().Name}}),
                Description = "Default component"
            };
            var system = GetDefaultSystem();
            system.Components.Add(new AssetKey {Name = result.Name});
            return result;
        }

        private System _defaultSystem;
        private System GetDefaultSystem()
        {
            if (_defaultSystem != null) return _defaultSystem;

            const string name = "Default system";
            if (Systems == null) Systems = new List<System>();
            var system = Systems.FirstOrDefault(s => s.Name == name);
            if (system != null)
            {
                _defaultSystem = system;
                return system;
            }
            system = new System
            {
                Name = name,
                Categories = GetDefaultCategories(),
                CreatedBy = GetDefaultContactKey(),
                CreatedOn = DateTime.Now,
                Components = new List<AssetKey>()
            };
            _defaultSystem = system;
            Systems.Add(system);
            return system;
        }

        private Space _anyDefaultSpace;
        private Space GetAnyDefaultSpace()
        {
            if (_anyDefaultSpace != null) return _anyDefaultSpace;

            foreach (var floor in Floors ?? new List<Floor>())
            {
                foreach (var space in floor.Spaces ?? new List<Space>())
                {
                    if (!space.Name.StartsWith("Default space")) continue;
                    _anyDefaultSpace = space;
                    return space;
                }
            }

            _anyDefaultSpace = GetNewDefaultSpace(true);
            return _anyDefaultSpace;
        }

        private Space GetNewDefaultSpace(bool addToDefaultFloor)
        {
            var space = new Space
            {
                Name = "Default space " + _counter++,
                CreatedOn = DateTime.Now,
                CreatedBy = GetDefaultContactKey(),
                Categories = GetDefaultCategories(),
                Description = "Default description"
            };
            GetDefaultZone().Spaces.Add(new SpaceKey {Name = space.Name});

            if (!addToDefaultFloor) return space;

            if (Floors == null) Floors = new List<Floor>();
            var defaultFloor = GetDefaultFloor();
            defaultFloor.Spaces.Add(space);
            return space;
        }

        private ContactKey GetDefaultContactKey()
        {
            const string defaultEmail = "default.contact@default.def";
            if (Contacts == null) Contacts = new List<Contact>();
            var contact = Contacts.FirstOrDefault(c => c.Email == defaultEmail);
            if (contact != null) return new ContactKey {Email = defaultEmail};
            contact = new Contact
            {
                Email = defaultEmail,
                Categories = GetDefaultCategories(),
                CreatedOn = DateTime.Now,
                CreatedBy = new ContactKey {Email = defaultEmail},
                Company = "Default company",
                Phone = "+00 0000 0000"
            };
            Contacts.Add(contact);
            return new ContactKey {Email = defaultEmail};
        }

        private List<Category> GetDefaultCategories()
        {
            //unknown is the recomended value from BS 1192-4
            return new List<Category>(new[] {new Category {Code = "unknown"}});
        }

        private Floor GetDefaultFloor()
        {
            var defaultFloor = Floors.FirstOrDefault(f => f.Name == "Default floor");
            if (defaultFloor == null)
            {
                defaultFloor = new Floor
                {
                    Name = "Default floor",
                    Spaces = new List<Space>(),
                    CreatedOn = DateTime.Now,
                    CreatedBy = GetDefaultContactKey(),
                    Categories = GetDefaultCategories()
                };
                Floors.Add(defaultFloor);
            }
            return defaultFloor;
        }

        #endregion
    }

    public enum ExcelTypeEnum
    {
        // ReSharper disable InconsistentNaming
        XLS,
        XLSX
        // ReSharper restore InconsistentNaming
    }
}