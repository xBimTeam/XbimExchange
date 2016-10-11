using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Xbim.CobieLiteUk.Converters;
using Formatting = System.Xml.Formatting;
using Xbim.CobieLiteUk.FilterHelper;
using Xbim.COBie.EqCompare;
using Xbim.CobieLiteUk.Schemas;

namespace Xbim.CobieLiteUk
{
    public partial class Facility
    {
        private static readonly ILog Log = LogManager.GetLogger("Xbim.COBieLiteUK.Facility");

        public Facility()
        {
            Metadata = new Metadata();
            ReportProgress = new ProgressReporter();
        }

        /// <summary>
        /// Creates a new class inheriting from CobieObject and sets it to belong to the facility.
        /// </summary>
        /// <typeparam name="TNewCobieObject">The CobieObject type to create</typeparam>
        /// <returns>the created object, which will have to be added to a collection as appropriate to be in the facility tree</returns>
        public TNewCobieObject Create<TNewCobieObject>() where TNewCobieObject : CobieObject, new()
        {
            var retObject = new TNewCobieObject();
            retObject.SetFacility(this);
            return retObject;
        }

        /// <summary>
        /// This function is used to search for any object in the model which is a CobieObject. 
        /// You can optionally pass a condition to select specific elements. Search is optimized 
        /// for specific types so it is advisable to specify the type as specific as possible.
        /// </summary>
        /// <typeparam name="T">Type os the object to search for</typeparam>
        /// <param name="condition">Optional search condition</param>
        /// <returns>Collection of cobie objects of specified type</returns>
        public IEnumerable<T> Get<T>(Func<T, bool> condition = null) where T : CobieObject
        {
            var self = this as T;

            //optimization first:
            if (typeof(T) == typeof(Facility))
            {
                if (condition == null)
                    yield return self;
                else if (condition(self))
                    yield return self;
                yield break;
            }

            if (typeof(T) == typeof(AssetType))
            {
                if (AssetTypes == null)
                    yield break;
                foreach (var o in AssetTypes.Select(assetType => assetType as T))
                {
                    if (condition == null)
                        yield return o;
                    else if (condition(o))
                        yield return o;
                }
                yield break;
            }

            if (typeof(T) == typeof(Asset))
            {
                if (AssetTypes == null)
                    yield break;
                foreach (var type in AssetTypes.Where(a => a != null && a.Assets != null))
                {
                    foreach (var o in type.Assets.Select(o => o as T))
                    {
                        if (condition == null)
                            yield return o;
                        else if (condition(o))
                            yield return o;
                    }
                }
                yield break;
            }

            if (typeof(T) == typeof(Floor))
            {
                if (Floors == null)
                    yield break;
                foreach (var o in Floors.Select(o => o as T))
                {
                    if (condition == null)
                        yield return o;
                    else if (condition(o))
                        yield return o;
                }
                yield break;
            }

            if (typeof(T) == typeof(Space))
            {
                if (Floors == null)
                    yield break;
                foreach (var type in Floors.Where(a => a != null && a.Spaces != null))
                {
                    foreach (var o in type.Spaces.Select(o => o as T))
                    {
                        if (condition == null)
                            yield return o;
                        else if (condition(o))
                            yield return o;
                    }
                }
                yield break;
            }

            if (typeof(T) == typeof(System))
            {
                if (Systems == null)
                    yield break;
                foreach (var o in Systems.Select(o => o as T))
                {
                    if (condition == null)
                        yield return o;
                    else if (condition(o))
                        yield return o;
                }
                yield break;
            }

            if (typeof(T) == typeof(Zone))
            {
                if (Zones == null)
                    yield break;
                foreach (var o in Zones.Select(o => o as T))
                {
                    if (condition == null)
                        yield return o;
                    else if (condition(o))
                        yield return o;
                }
                yield break;
            }

            //generic selection (mostly for attributes, issues, etc.)

            //make Facility part of the result
            if (self != null)
            {
                if (condition == null)
                    yield return self;
                else if (condition(self))
                    yield return self;
            }
            foreach (var child in GetDeep(condition))
                yield return child;
        }

        #region Enumerations
        /// <summary>
        /// Enumeration of area units. If custom area units are from this enumeration it will
        /// be returned also here. Values from this enumeration are set into that respective field.
        /// If you set this property to 'None' custom field will be set to Null. If you set custom field
        /// to a value which doesn't exist in this enumeration, 'Custom' value will be returned as a value
        /// of this property.
        /// </summary>
        public AreaUnit AreaUnits
        {
            get { return GetEnumeration<AreaUnit>(AreaUnitsCustom); }
            set { SetEnumeration(value, s => AreaUnitsCustom = s); }
        }

        /// <summary>
        /// Enumeration of linear units. If custom linear units are from this enumeration it will
        /// be returned also here. Values from this enumeration are set into that respective field.
        /// If you set this property to 'None' custom field will be set to Null. If you set custom field
        /// to a value which doesn't exist in this enumeration, 'Custom' value will be returned as a value
        /// of this property.
        /// </summary>
        public LinearUnit LinearUnits
        {
            get { return GetEnumeration<LinearUnit>(LinearUnitsCustom); }
            set { SetEnumeration(value, s => LinearUnitsCustom = s); }
        }

        /// <summary>
        /// Enumeration of volume units. If custom volume units are from this enumeration it will
        /// be returned also here. Values from this enumeration are set into that respective field.
        /// If you set this property to 'None' custom field will be set to Null. If you set custom field
        /// to a value which doesn't exist in this enumeration, 'Custom' value will be returned as a value
        /// of this property.
        /// </summary>
        public VolumeUnit VolumeUnits
        {
            get { return GetEnumeration<VolumeUnit>(VolumeUnitsCustom); }
            set { SetEnumeration(value, s => VolumeUnitsCustom = s); }
        }

        /// <summary>
        /// Enumeration of currency units. If custom currency units are from this enumeration it will
        /// be returned also here. Values from this enumeration are set into that respective field.
        /// If you set this property to 'None' custom field will be set to Null. If you set custom field
        /// to a value which doesn't exist in this enumeration, 'Custom' value will be returned as a value
        /// of this property.
        /// </summary>
        public CurrencyUnit CurrencyUnit
        {
            get { return GetEnumeration<CurrencyUnit>(CurrencyUnitCustom); }
            set { SetEnumeration(value, s => CurrencyUnitCustom = s); }
        }

        #endregion

        #region XML serialization

        private static XmlSerializer GetXmlSerializer()
        {
            return new XmlSerializer(typeof(Facility));
        }

        /// <summary>
        /// This function will serialize the model as an XML
        /// </summary>
        /// <param name="path">Path where this model should be serialized</param>
        /// <param name="indented">Optional flag. XML will be indented if this is true. This will increase the size of the file.</param>
        public void WriteXml(string path, bool indented = false)
        {
            using (var stream = File.Create(path))
            {
                WriteXml(stream, indented);
                stream.Close();
            }
        }

        /// <summary>
        /// This function will serialize the model as an XML. Stream will be closed at the end
        /// </summary>
        /// <param name="stream">Target stream</param>
        /// <param name="indented">Optional flag. XML will be indented if this is true. This will increase the size of the file.</param>
        public void WriteXml(Stream stream, bool indented = false)
        {
            var serializer = GetXmlSerializer();
            using (var w = new StreamWriter(stream))
            {
                using (
                    var xmlWriter = new XmlTextWriter(w) { Formatting = indented ? Formatting.Indented : Formatting.None }
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
            Facility facility;
            var serializer = GetXmlSerializer();
            facility = (Facility)serializer.Deserialize(stream);
            facility.SetFacility(facility);
            return facility;
        }

        /// <summary>
        /// Attempts to find an XML file in a compressed archive.
        /// This function has been implemented to simplify the management of NBS Bim Toolkit exported projects
        /// </summary>
        /// <param name="path">A string poiting to the archive file name.</param>
        /// <returns>A valid facility or null if no suitable file has been found</returns>
        public static Facility ReadZip(string path)
        {
            //FileStream fs = File.OpenRead(path);
            using (var zf = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                foreach (var zipEntry in zf.Entries)
                {

                    FileInfo entryFileName = new FileInfo(zipEntry.Name);
                    if (entryFileName.Extension != ".xml")
                        continue;
                    using (var zipStream = zipEntry.Open())
                    {
                        var facility = ReadXml(zipStream);
                        zipStream.Close();
                        return facility;
                    }
                }
            }
            return null;
        }

        public static Facility ReadXml(string path, bool ignoreNamespaces = false)
        {
            if (ignoreNamespaces)
            {
                FileInfo f = new FileInfo(path);
                var serializer = GetXmlSerializer();
                using (var textreader = f.OpenText())
                {
                    var facility = (Facility)serializer.Deserialize(new NamespaceTolerantXmlTextReader(textreader));
                    facility.SetFacility(facility);
                    return facility;
                }
            }
            using (var stream = File.OpenRead(path))
            {
                var facility = ReadXml(stream);
                stream.Close();
                return facility;
            }
        }

        #endregion

        #region JSON serialization

        public static JsonSerializer GetJsonSerializer(bool indented = false)
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
            if (originalCobieObjects == null)
                return Enumerable.Empty<TNewCobieObject>();
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
                var facility = (Facility)serialiser.Deserialize(textReader, typeof(Facility));
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
            if (path == null)
                throw new ArgumentNullException("path");
            var ext = Path.GetExtension(path).ToLower().Trim('.');
            if (ext != "xls" && ext != "xlsx")
                throw new Exception("File must be an MS Excel file.");
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
                facility = new Facility { Name = "Default facility" };
                flatList.Add(facility);
            }

            //create structure hierarchy
            var parallelMessage = new[] { "" };
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
                typeof(CobieObject).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(CobieObject).IsAssignableFrom(t));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="assetfilters"></param>
        /// <param name="templateVersion">Use a template selected amongst the admissible values returned from the Templates.GetAvalilableTemplateTypes() function</param>
        /// <param name="useTemplate"></param>
        public void WriteCobie(Stream stream, ExcelTypeEnum type, out string message, OutPutFilters assetfilters = null, string templateVersion = "UK2012", bool useTemplate = true)
        {
            Stream templateStream = null;
            if (useTemplate)
            {
                var resourceName = Templates.FullResourceName(templateVersion, type);
                templateStream = GetType().Assembly.GetManifestResourceStream(resourceName);
                if (templateStream == null)
                {
                    Log.ErrorFormat("Template '{0}' could not be found in assembly streams.", resourceName);
                }
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

            ReportProgress.Reset(GetChildren().Count(), 100, "Creating Excel COBie");

            WriteToCobie(workbook, log, null, new Dictionary<Type, int>(), new List<string>(), new Dictionary<string, int>(), assetfilters, templateVersion);

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
            ReportProgress.Finalise(500);
        }

        public void WriteCobie(string path, out string message, OutPutFilters assetfilters = null, string version = "UK2012", bool useTemplate = true)
        {
            if (path == null)
                throw new ArgumentNullException("path");
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
                WriteCobie(file, type, out message, assetfilters, version, useTemplate);
                file.Close();
            }
        }

        #endregion

        internal override void WriteToCobie(IWorkbook workbook, TextWriter loger, CobieObject parent,
            Dictionary<Type, int> rowNumCache, List<string> pickValuesCache, Dictionary<string, int> headerCache, OutPutFilters assetfilters = null, string version = "UK2012")
        {
            base.WriteToCobie(workbook, loger, parent, rowNumCache, pickValuesCache, headerCache, assetfilters, version);

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
                foreach (var floor in Floors.Where(i => i != null))
                    yield return floor;
            if (AssetTypes != null)
                foreach (var assetType in AssetTypes.Where(i => i != null))
                    yield return assetType;
            if (Contacts != null)
                foreach (var contact in Contacts.Where(i => i != null))
                    yield return contact;
            if (Systems != null)
                foreach (var system in Systems.Where(i => i != null))
                    yield return system;
            if (Zones != null)
                foreach (var zone in Zones.Where(i => i != null))
                    yield return zone;
            if (Resources != null)
                foreach (var resource in Resources.Where(i => i != null))
                    yield return resource;
            if (Stages != null)
                foreach (var stage in Stages.Where(i => i != null))
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
                    if (fixIfPossible)
                        o.Name = String.Format("{0} {1}", o.GetType().Name, _counter++);
                }
                else if (regex.IsMatch(o.Name))
                {
                    logger.WriteLine("Name {0} of {1} contains forbidden characters.", o.Name, o.GetType().Name);
                    if (fixIfPossible)
                        o.Name = regex.Replace(o.Name, "");
                }
                foreach (var key in o.GetKeys().Where(key => regex.IsMatch(key.Name ?? "")))
                {
                    logger.WriteLine("Name {0} of {1} key contains forbidden characters.", key.Name,
                        key.GetSheet("UK2012"));
                    if (fixIfPossible)
                        key.Name = regex.Replace(key.Name ?? "", "");
                }

                if (o.Categories != null && o.Categories.Any())
                {
                    foreach (var category in o.Categories)
                    {
                        if (category.Code != null && catRegex.IsMatch(category.Code))
                        {
                            logger.WriteLine("Category code {0} contains forbidden characters.", category.Code);
                            if (fixIfPossible)
                                category.Code = catRegex.Replace(category.Code, "");
                        }

                        if (category.Description == null || !catRegex.IsMatch(category.Description))
                            continue;
                        logger.WriteLine("Category description {0} contains forbidden characters.", category.Description);
                        if (fixIfPossible)
                            category.Description = catRegex.Replace(category.Description, "");
                    }
                }

                //Category entries should be provided.
                //Only Asset doesn't have a category in COBie XLS
                if ((o.Categories == null || !o.Categories.Any()) && !(o is Asset))
                {
                    logger.WriteLine("{0} '{1}' doesn't have a category defined.", o.GetType().Name, o.Name);
                    if (fixIfPossible)
                    {
                        if (o.Categories == null)
                            o.Categories = new List<Category>();
                        o.Categories.Add(new Category { Code = "unknown" });
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
                if (!fixIfPossible)
                    continue;

                if (Zones == null)
                    Zones = new List<Zone>();
                var defaultZone = GetDefaultZone();
                defaultZone.Spaces.Add(new SpaceKey { Name = space.Name });
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
                    if (!fixIfPossible)
                        continue;
                    if (floor.Spaces == null)
                        floor.Spaces = new List<Space>();
                    floor.Spaces.Add(GetNewDefaultSpace(false, true));
                }
            }
            if (Zones != null)
            {
                foreach (var zone in Zones.Where(z => z.Spaces == null || !z.Spaces.Any()).ToArray())
                {
                    logger.WriteLine("Zone {0} doesn't have any space assigned.", zone.Name);
                    if (!fixIfPossible)
                        continue;
                    if (zone.Spaces == null)
                        zone.Spaces = new List<SpaceKey>();
                    var defaultSpace = GetAnyDefaultSpace(false);
                    zone.Spaces.Add(new SpaceKey { Name = defaultSpace.Name });
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
                if (!fixIfPossible)
                    continue;
                var space = GetAnyDefaultSpace();
                if (asset.Spaces == null)
                    asset.Spaces = new List<SpaceKey>();
                asset.Spaces.Add(new SpaceKey { Name = space.Name });
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
                    GetDefaultSystem().Components.Add(new AssetKey { Name = asset.Name });
                }
            }
            referenceWatch.Stop();
            Debug.WriteLine("   Every component is in system: " + referenceWatch.ElapsedMilliseconds);
            referenceWatch.Restart();

            //g) Every Type should apply to at least one Component.
            if (AssetTypes != null)
                foreach (var type in AssetTypes.Where(t => t.Assets == null || !t.Assets.Any()))
                {
                    logger.WriteLine("Type {0} doesn't contain any components.", type.Name);
                    if (!fixIfPossible)
                        continue;
                    if (type.Assets == null)
                        type.Assets = new List<Asset>();
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
                                    //set contact key to the default value if it is not set at all
                                    if (String.IsNullOrWhiteSpace(key.Name))
                                        ((ContactKey)key).Email = defaultContact.Email;
                                    //create contact with this email address
                                    else
                                    {
                                        if (Contacts == null)
                                            Contacts = new List<Contact>();
                                        if (Contacts.Any(c => c.Email == key.Name))
                                            break;
                                        Contacts.Add(new Contact { Email = key.Name, CreatedOn = DateTime.Now, CreatedBy = GetDefaultContactKey() });
                                    }
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
            if (Zones == null)
                Zones = new List<Zone>();
            var zone = Zones.FirstOrDefault(z => z.Name == defaultName);
            if (zone != null)
                return zone;

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
            if (objects == null)
                return;

            var groups = objects.GroupBy(o => o.Name);
            foreach (var g in groups.Where(g => g.Count() > 1))
            {
                logger.WriteLine(
                    "{0} {1} doesn't have an unique name. There are {2} instances with the same name. If fixed it may break key references.",
                    g.First().GetType().Name, g.Key, g.Count());
                if (!fixIfPossible)
                    continue;

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
                Spaces = new List<SpaceKey>(new[] { new SpaceKey { Name = GetAnyDefaultSpace().Name } }),
                Description = "Default component"
            };
            var system = GetDefaultSystem();
            system.Components.Add(new AssetKey { Name = result.Name });
            return result;
        }

        private System _defaultSystem;
        private System GetDefaultSystem()
        {
            if (_defaultSystem != null)
                return _defaultSystem;

            const string name = "Default system";
            if (Systems == null)
                Systems = new List<System>();
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
        private Space GetAnyDefaultSpace(bool addToDefaultZone = true)
        {
            if (_anyDefaultSpace != null)
                return _anyDefaultSpace;

            foreach (var floor in Floors ?? new List<Floor>())
            {
                foreach (var space in floor.Spaces ?? new List<Space>())
                {
                    if (!space.Name.StartsWith("Default space"))
                        continue;
                    _anyDefaultSpace = space;
                    return space;
                }
            }

            _anyDefaultSpace = GetNewDefaultSpace(true, addToDefaultZone);
            return _anyDefaultSpace;
        }

        private Space GetNewDefaultSpace(bool addToDefaultFloor, bool addToDefaultZone)
        {
            var space = new Space
            {
                Name = "Default space " + _counter++,
                CreatedOn = DateTime.Now,
                CreatedBy = GetDefaultContactKey(),
                Categories = GetDefaultCategories(),
                Description = "Default description"
            };
            if (addToDefaultZone)
                GetDefaultZone().Spaces.Add(new SpaceKey { Name = space.Name });

            if (!addToDefaultFloor)
                return space;

            if (Floors == null)
                Floors = new List<Floor>();
            var defaultFloor = GetDefaultFloor();
            defaultFloor.Spaces.Add(space);
            return space;
        }

        private ContactKey GetDefaultContactKey()
        {
            const string defaultEmail = "default.contact@default.def";
            if (Contacts == null)
                Contacts = new List<Contact>();
            var contact = Contacts.FirstOrDefault(c => c.Email == defaultEmail);
            if (contact != null)
                return new ContactKey { Email = defaultEmail };
            contact = new Contact
            {
                Email = defaultEmail,
                Categories = GetDefaultCategories(),
                CreatedOn = DateTime.Now,
                CreatedBy = new ContactKey { Email = defaultEmail },
                Company = "Default company",
                Phone = "+00 0000 0000"
            };
            Contacts.Add(contact);
            return new ContactKey { Email = defaultEmail };
        }

        private List<Category> GetDefaultCategories()
        {
            //unknown is the recomended value from BS 1192-4
            return new List<Category>(new[] { new Category { Code = "unknown" } });
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

        #region Merge Methods

        /// <summary>
        /// Mapping of type to EqualCompare Objects
        /// </summary>
        private Dictionary<Type, ICompareEqRule> TypeCompare
        { get; set; }
        /// <summary>
        /// Mapping of COBieType to mapping of Root object to drilled down objects tree, so we do not call an object twice (try end stop ininite loops!!, say not be a problem so controled by CheckInstance)
        /// </summary>
        Dictionary<Type, Dictionary<object, HashSet<CobieObject>>> ChainMapInst
        { get; set; }
        /// <summary>
        /// Controls if the ChainMapInst is used above
        /// </summary>
        private bool CheckInstance
        { get; set; }

        /// <summary>
        /// Logger for messages, file or output windows
        /// </summary>
        private CompareLogger Logger
        { get; set; }

        /// <summary>
        /// String Comparison rule to use in EqualCompare Objects
        /// </summary>
        private StringComparison EqRule
        { get; set; }

        /// <summary>
        /// Compare on Name, Key, Full
        /// </summary>
        private CompareType CompareTypeRule
        { get; set; }

    /// <summary>
    /// Merge a Facility to this Facility
    /// </summary>
    /// <param name="mFacility">Facility to merge</param>
    /// <param name="checkInstanc">Controls if the ChainMapInst is used</param>
    /// <param name="logIt">Logger</param>
    public void Merge(Facility mFacility, TextWriter logIt = null)
        {
            //setup merge properties
            Logger = new CompareLogger(logIt);
            Logger.WriteLine(this, mFacility);
            EqRule = StringComparison.OrdinalIgnoreCase;
            CompareTypeRule = CompareType.Full;
            CheckInstance = true;         //if true, check for repeat instances to avoid infinite loops in drill down
            ChainMapInst = new Dictionary<Type, Dictionary<object, HashSet<CobieObject>>>();
            TypeCompare = new Dictionary<Type, ICompareEqRule>();

            //merge the COBieObject child Lists held in Facilities
            MergeCOBieObject<Facility>(this, mFacility, 0, mFacility, this);

            //Check Facility Primary Key
            if (Name.Equals(mFacility.Name, EqRule))
            {
                mFacility.Name = string.Empty;
            }
            else
            {
                Logger.WriteLine(string.Format("Facility Names Do Not match: {0}/= {1}", Name, mFacility.Name));
            }

            //Metadata
            if (mFacility.Metadata == null)
            {
                //do nothing
            }
            else if (this.Metadata == null && mFacility.Metadata != null)
            {
                this.Metadata = mFacility.Metadata;
                mFacility.Metadata = null; //merged so clear out of passed in facility
            }
            else if (new MetadataCompareKey(EqRule, CompareType.Full).Equals(this.Metadata, mFacility.Metadata))
            {
                mFacility.Metadata = null; //merged so clear out of passed in facility
            }
            else
            {
                Logger.WriteLine("Metadata Do Not match");
            }

            //Project 
            if (mFacility.Project == null)
            {
                //do nothing
            }
            else if (this.Project == null && mFacility.Project != null)
            {
                this.Project = mFacility.Project;
                mFacility.Project = null; //merged so clear out of passed in facility
            }
            else if (new ProjectCompareKey(EqRule, CompareType.Key).Equals(this.Project, mFacility.Project))
            {
                mFacility.Project = null; //merged so clear out of passed in facility
            }
            else
            {
                Logger.WriteLine(string.Format("Project Objects Do Not match: {0}/= {1}", Project.Name, mFacility.Project.Name));
            }

            //Site
            if (mFacility.Site == null)
            {
                //do nothing
            }
            else if (this.Site == null && mFacility.Site != null)
            {
                this.Site = mFacility.Site;
                mFacility.Site = null; //merged so clear out of passed in facility
            }
            else if (new SiteCompareKey(EqRule, CompareType.Name).Equals(this.Site, mFacility.Site))
            {
                mFacility.Site = null; //merged so clear out of passed in facility
            }
            else
            {
                Logger.WriteLine(string.Format("Site Objects Do Not match: {0}/= {1}", Site.Name, mFacility.Site.Name));
            }

            var compareString = new StringCompareKey(EqRule, CompareType.Name);
            //Unit LinearUnitsCustom
            if (compareString.Equals(this.LinearUnitsCustom, mFacility.LinearUnitsCustom))
            {
                mFacility.LinearUnitsCustom = string.Empty;
            }
            else
            {
                Logger.WriteLine(string.Format("LinearUnitsCustom Do Not match: {0}/= {1}", LinearUnitsCustom, mFacility.LinearUnitsCustom));
            }

            //Unit AreaUnitsCustom
            if (compareString.Equals(this.AreaUnitsCustom, mFacility.AreaUnitsCustom))
            {
                mFacility.AreaUnitsCustom = string.Empty;
            }
            else
            {
                Logger.WriteLine(string.Format("AreaUnitsCustom Do Not match: {0}/= {1}", AreaUnitsCustom, mFacility.AreaUnitsCustom));
            }

            //Units VolumeUnitsCustom
            if (compareString.Equals(this.VolumeUnitsCustom, mFacility.VolumeUnitsCustom))
            {
                mFacility.VolumeUnitsCustom = string.Empty;
            }
            else
            {
                Logger.WriteLine(string.Format("VolumeUnitsCustom Do Not match: {0}/= {1}", VolumeUnitsCustom, mFacility.VolumeUnitsCustom));               
            }

            //Units CurrencyUnitCustom
            if (compareString.Equals(this.CurrencyUnitCustom, mFacility.CurrencyUnitCustom))
            {
                mFacility.CurrencyUnitCustom = string.Empty;
            }
            else
            {
                Logger.WriteLine(string.Format("CurrencyUnitCustom Do Not match: {0}/= {1}", CurrencyUnitCustom, mFacility.CurrencyUnitCustom));
            }

            //Units AreaMeasurement
            if (compareString.Equals(this.AreaMeasurement, mFacility.AreaMeasurement))
            {
                mFacility.AreaMeasurement = string.Empty;
            }
            else
            {
                Logger.WriteLine(string.Format("AreaMeasurement Do Not match: {0}/= {1}", AreaMeasurement, mFacility.AreaMeasurement));
            }

            //Phase
            if (compareString.Equals(this.Phase, mFacility.Phase))
            {
                mFacility.Phase = string.Empty;
            }
            else
            {
                Logger.WriteLine(string.Format("Phase Do Not match: {0}/= {1}", Phase, mFacility.Phase));
            }

            //Floors
            Floors = MergeLists<Floor>(Floors, mFacility.Floors, GetCompare<FloorCompareKey, Floor>());

            //AssetType
            AssetTypes = MergeLists<AssetType>(AssetTypes, mFacility.AssetTypes, GetCompare<AssetTypeCompareKey, AssetType>());

            //Contacts
            Contacts = MergeLists<Contact>(Contacts, mFacility.Contacts, GetCompare<ContactCompareKey, Contact>());

            //Systems
            Systems = MergeLists<System>(Systems, mFacility.Systems, GetCompare<NameCompareKey<System>, System>());

            //Zones
            Zones = MergeLists<Zone>(Zones, mFacility.Zones, GetCompare<NameCompareKey<Zone>, Zone>());

            //Resources
            Resources = MergeLists<Resource>(Resources, mFacility.Resources, GetCompare<NameCompareKey<Resource>, Resource>());

            //Stages
            Stages = MergeLists<ProjectStage>(Stages, mFacility.Stages, GetCompare<NameCompareKey<ProjectStage>, ProjectStage>());
            
        }


        /// <summary>
        /// Map COBie types to the compare object of type CompareEqRule, IEqualityComparer
        /// </summary>
        /// <typeparam name="T1">is CompareEqRule</typeparam>
        /// <typeparam name="T2"> Type we are testing</typeparam>
        /// <param name="type">Mapped Type</param>
        /// <param name="advancedCompare">if true extend equal test to other properties in equalcompare if supported</param>
        /// <returns>T</returns>
        private T1 GetCompare<T1, T2>() where T1 : CompareEqRule<T2>,  new()
        {
            T1 compareKey;
            
            if (TypeCompare.ContainsKey(typeof(T2)))
            {
                compareKey = (T1)TypeCompare[typeof(T2)];
            }
            else
            {
                compareKey = new T1();
                compareKey.EqRule = EqRule;
                compareKey.CompareMethod = CompareTypeRule;
                TypeCompare[typeof(T2)] = compareKey;
            }

            return compareKey;
        }

        /// <summary>
        /// Merge COBie Objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="to">Merge To</param>
        /// <param name="from">Merge from</param>
        /// <param name="rootObj">Starting onject of the merge</param>
        private void MergeCOBieObject<T>(T to, T from, int depthIndicator = 0, CobieObject rootObj = null, CobieObject parentObj = null) where T : CobieObject
        {
            //From fields:-
            //Name - ignored (but be used in compare, so to and from should alreadty be the same)
            //Description - ignored
            //CreatedBy - ignored
            //CreatedOn - ignored
            //ExternalSystem - ignored
            //ExternalEntity - ignored
            //ExternalId - ignored

            //Categories - end
            to.Categories = MergeSimpleLists(to.Categories, from.Categories, GetCompare<CategoryCompareKey, Category>(), depthIndicator, rootObj, parentObj);
            //Documents - drill down
            to.Documents = MergeLists<Document>(to.Documents, from.Documents, GetCompare<DocumentCompareKey, Document>(), depthIndicator, rootObj, parentObj);

            //Attributes - drill down
            to.Attributes = MergeLists<Attribute>(to.Attributes, from.Attributes, GetCompare<AttributeCompareKey, Attribute>(), depthIndicator, rootObj, parentObj);

            //Issues - drill down
            to.Issues = MergeLists<Issue>(to.Issues, from.Issues, GetCompare<IssueCompareKey, Issue>(), depthIndicator, rootObj, parentObj);

            //Impacts - drill down
            to.Impacts = MergeLists<Impact>(to.Impacts, from.Impacts, GetCompare<ImpactCompareKey, Impact>(), depthIndicator, rootObj, parentObj);

            //Representations - drill down 
            to.Representations = MergeLists<Representation>(to.Representations, from.Representations, GetCompare<RepresentationCompareKey, Representation>(), depthIndicator, rootObj, parentObj);

            //ProjectStages - end
            to.ProjectStages = MergeSimpleLists(to.ProjectStages, from.ProjectStages, GetCompare<EntityKeyCompareKey, IEntityKey>(), depthIndicator, rootObj, parentObj);

            //Class which inherit from
            if (to is Floor && from is Floor) //check is Floor and not null
            {
                MergeFloor(to as Floor, from as Floor, depthIndicator, rootObj, parentObj);
            }

            if (to is Space && from is Space) //check is Space and not null
            {
                MergeSpace(to as Space, from as Space, depthIndicator, rootObj, parentObj);
            }

            if (to is AssetType && from is AssetType)//check is AssetType and not null
            {
                MergeAssetType(to as AssetType, from as AssetType, depthIndicator, rootObj, parentObj);
            }

            if (to is Asset && from is Asset)//check is Asset and not null
            {
                MergeAsset(to as Asset, from as Asset, depthIndicator, rootObj, parentObj);
            }

            if (to is Spare && from is Spare)//check is Spare and not null
            {
                MergeSpare(to as Spare, from as Spare, depthIndicator, rootObj, parentObj);
            }

            if (to is Job && from is Job)//check is Job and not null
            {
                MergeJob(to as Job, from as Job, depthIndicator, rootObj, parentObj);
            }

            if (to is System && from is System)
            {
                MergeSystem(to as System, from as System, depthIndicator, rootObj, parentObj);
            }

            if (to is Zone && from is Zone)
            {
                MergeZone(to as Zone, from as Zone, depthIndicator, rootObj, parentObj);
            }

            if (to is Resource && from is Resource)
            {
                //no specific proerties to merge on Resource
            }

            if (to is ProjectStage && from is ProjectStage)
            {
                MergeProjectStage(to as ProjectStage, from as ProjectStage, depthIndicator, rootObj, parentObj);
            }
        }








        /// <summary>
        /// Merge Floor to a Floor
        /// </summary>
        /// <param name="to">Floor</param>
        /// <param name="from">Floor</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeFloor(Floor to, Floor from, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //From Fields: -
            //CobieObject.Name - Compare - to and from names should be the same
            //Elevation - Compare, Advanced = true, then to and from the same
            //Height - Compare, Advanced = true, then to and from the same

            //merge Spaces
            to.Spaces = MergeLists<Space>(to.Spaces, from.Spaces, GetCompare<SpaceCompareKey, Space>(), depthIndicator, rootObj, parentObj);

            //if camparer did not incSizing then check to see if tofloor fields have value, if not and fromfloor fields have a value, copy it over to tofloor
            //if (!GetComparer<FloorCompareKey>(typeof(Floor)).AdvancedCompare)
            //{
            //    if (to.Elevation == 0.0 && from.Elevation > 0.0)
            //    {
            //        to.Elevation = from.Elevation;
            //    }
            //    if (to.Height == 0.0 && from.Height > 0.0)
            //    {
            //        to.Height = from.Height;
            //    }
            //}
        }

        /// <summary>
        /// Merge Space To Space
        /// </summary>
        /// <param name="to">Space</param>
        /// <param name="from">Space</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeSpace(Space to, Space from, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //From Fields: -
            //CobieObject.Name - Compare - to and from names should be the same

            //RoomTag
            to.RoomTag += (from.RoomTag != null) ? " and " + from.RoomTag : string.Empty;

            //UsableHeight - Compare, Advanced = true, then to and from the same
            //GrossArea - Compare, Advanced = true, then to and from the same
            //NetArea - Compare, Advanced = true, then to and from the same

            //if camparer did not set AdvancedCompare then see if we need to copy through fromSpace sizing if none exists on the toSpace 
            //if (!GetComparer<SpaceCompareKey>(typeof(Space)).AdvancedCompare)
            //{
            //    if (to.UsableHeight == 0.0 && from.UsableHeight > 0.0)
            //    {
            //        to.UsableHeight = from.UsableHeight;
            //    }
            //    if (to.GrossArea == 0.0 && from.GrossArea > 0.0)
            //    {
            //        to.GrossArea = from.GrossArea;
            //    }
            //    if (to.NetArea == 0.0 && from.NetArea > 0.0)
            //    {
            //        to.NetArea = from.NetArea;
            //    }
            //}
        }

        /// <summary>
        /// Merge AssetType to AssetType
        /// </summary>
        /// <param name="to">AssetType</param>
        /// <param name="from">AssetType</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeAssetType(AssetType to, AssetType from, int depthIndicator, CobieObject rootObj, CobieObject parentObj) 
        {
            //From Fields: -
            //CobieObject.Name - Compare - to and from names should be the same
            //AssetTypeCustom - ignored
            //Manufacturer - Compare, Advanced = true, then to and from the same
            //ModelNumber - Compare, Advanced = true, then to and from the same
            //Warranty - ignored
            //ReplacementCost - ignored
            //ExpectedLife - ignored
            //DurationUnit - ignored
            //NominalLength - ignored
            //NominalWidth - ignored
            //NominalHeight - ignored
            //ModelReference - ignored
            //Shape - ignored
            //Size - ignored
            //Color - ignored
            //Finish - ignored
            //Grade - ignored
            //Material - ignored
            //Features - ignored
            //AccessibilityPerformance - ignored
            //CodePerformance - ignored
            //SustainabilityPerformance - ignored

            //merge Assets
            to.Assets = MergeLists<Asset>(to.Assets, from.Assets, GetCompare<AssetCompareKey, Asset>(), depthIndicator, rootObj, parentObj);

            //merge Spares
            to.Spares = MergeLists<Spare>(to.Spares, from.Spares, GetCompare<SpareCompareKey, Spare>(), depthIndicator, rootObj, parentObj);

            //merge Jobs
            to.Jobs = MergeLists<Job>(to.Jobs, from.Jobs, GetCompare<JobCompareKey, Job>(), depthIndicator, rootObj, parentObj);
            //AssemblyOf - ignored

            //if (!GetComparer<AssetTypeCompareKey>(typeof(AssetType)).AdvancedCompare)
            //{
            //    if (to.Manufacturer != null)
            //    {
            //        if (string.IsNullOrEmpty(to.Manufacturer.Email))
            //        {
            //            to.Manufacturer.Email = from.Manufacturer.Email;
            //        }
            //    }
            //    else
            //    {
            //        to.Manufacturer = from.Manufacturer;
            //    }

            //    if (string.IsNullOrEmpty(to.Manufacturer.Email))
            //    {
            //        to.Manufacturer.Email = from.Manufacturer.Email;
            //    }

            //    if (string.IsNullOrEmpty(to.ModelNumber))
            //    {
            //        to.ModelNumber = from.ModelNumber;
            //    }
            //}
        }

        /// <summary>
        /// Merge Job to Job
        /// </summary>
        /// <param name="to">Job</param>
        /// <param name="from">Job</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeJob(Job to, Job from, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //From Fields: -
            //CobieObject.Name - Compare - to and from names should be the same
            //Status - ignored
            //Duration - ignored
            //DurationUnit - ignored
            //Start - ignored
            //TaskStartUnit - ignored
            //Frequency - ignored
            //FrequencyUnit - ignored
            //TaskNumber - - Compare - to and from TaskNumber should be the same

            //merge Priors
            to.Priors = MergeSimpleLists(to.Priors, from.Priors, GetCompare<JobKeyCompareKey, JobKey>());
            //merge Resources
            to.Resources = MergeSimpleLists(to.Resources, from.Resources, GetCompare<EntityKeyCompareKey, IEntityKey>(), depthIndicator, rootObj, parentObj);

        }


        /// <summary>
        /// Merge Spare to Spare
        /// </summary>
        /// <param name="to">Spare</param>
        /// <param name="from">Spare</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeSpare(Spare to, Spare from, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //From Fields: -
            //CobieObject.Name - Compare - to and from names should be the same

            //merge Suppliers
            to.Suppliers = MergeSimpleLists(to.Suppliers, from.Suppliers, GetCompare<EntityKeyCompareKey, IEntityKey>(), depthIndicator, rootObj, parentObj);

            //SetNumber - Compare, Advanced = true, then to and from the same
            //PartNumber - Compare, Advanced = true, then to and from the same
            //if (!GetComparer<SpareCompareKey>(typeof(Spare)).AdvancedCompare)
            //{
            //    if (string.IsNullOrEmpty(to.SetNumber))
            //    {
            //        to.SetNumber = from.SetNumber;
            //    }
            //    if (string.IsNullOrEmpty(to.PartNumber))
            //    {
            //        to.PartNumber = from.PartNumber;
            //    }
            //}
        }

        /// <summary>
        /// Merge Asset to Asset
        /// </summary>
        /// <param name="to">Asset</param>
        /// <param name="from">Asset</param>
        /// <param name="rootObj">Root Object (started merge path)</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeAsset(Asset to, Asset from, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //From Fields: -
            //SerialNumber - Compare, Advanced = true, then to and from the same

            //Merge Spaces
            to.Spaces = MergeSimpleLists(to.Spaces, from.Spaces, GetCompare<EntityKeyCompareKey, IEntityKey>(), depthIndicator, rootObj, parentObj);

            //InstallationDate - ignored
            //WarrantyStartDate - ignored
            //TagNumber - Compare, Advanced = true, then to and from the same
            //BarCode - Compare, Advanced = true, then to and from the same
            //AssetIdentifier - ignored

            //mergeConnections
            to.Connections = MergeLists<Connection>(to.Connections, from.Connections, GetCompare<ConnectionCompareKey, Connection>(), depthIndicator, rootObj, parentObj);

            //AssemblyOf - ignored

            //if (!GetComparer<AssetCompareKey>(typeof(Asset)).AdvancedCompare)
            //{
            //    if (string.IsNullOrEmpty(to.SerialNumber))
            //    {
            //        to.SerialNumber = from.SerialNumber;
            //    }

            //    if (string.IsNullOrEmpty(to.TagNumber))
            //    {
            //        to.TagNumber = from.TagNumber;
            //    }

            //    if (string.IsNullOrEmpty(to.BarCode))
            //    {
            //        to.BarCode = from.BarCode;
            //    }
            //}
        }

        /// <summary>
        /// Merge System to System
        /// </summary>
        /// <param name="to">System</param>
        /// <param name="from">System</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeSystem(System to, System from, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //Merge Components, only prop of System
            to.Components = MergeSimpleLists(to.Components, from.Components, GetCompare<EntityKeyCompareKey, IEntityKey>(), depthIndicator, rootObj, parentObj);
        }

        /// <summary>
        /// Merge Zone to Zone
        /// </summary>
        /// <param name="to">Zone</param>
        /// <param name="from">Zone</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeZone(Zone to, Zone from, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //Merge Spaces, only prop of Zone
            to.Spaces = MergeSimpleLists(to.Spaces, from.Spaces, GetCompare<EntityKeyCompareKey, IEntityKey>(), depthIndicator, rootObj, parentObj);
        }

        /// <summary>
        /// Merge ProjectStage to ProjectStage
        /// </summary>
        /// <param name="to">ProjectStage</param>
        /// <param name="from">ProjectStage</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private void MergeProjectStage(ProjectStage projectStage1, ProjectStage projectStage2, int depthIndicator, CobieObject rootObj, CobieObject parentObj)
        {
            //From Fields: -
            //Start - ignored
            //End - ignored
        }


        /// <summary>
        /// Merge two CobieObject Lists together
        /// </summary>
        /// <typeparam name="T">CobieObject</typeparam>
        /// <param name="to">List to merge to</param>
        /// <param name="from">List to merge from</param>
        /// <param name="compareKey">IEqualityComparer</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private List<T> MergeLists<T>(List<T> to, List<T> from, IEqualityComparer<T> compareKey, int depthIndicator = 0, CobieObject rootObj = null, CobieObject parentObj = null)  where T : CobieObject
        {
            if (from != null && from.Any())
            {
                if (to != null)
                {
                    //Do merge
                    var total = to.Count + from.Count;
                    var toMerge = from.Except(to, compareKey);
                    int toMergeCount = 0;
                    IEnumerable<T> excluded = null;
                    if (toMerge.Any()) 
                    {
                        toMergeCount = toMerge.Count();
                        excluded = from.Except(toMerge, compareKey).ToList(); //ToList runs query now, otherwise we get full list as once toMerge is added to list "from.Except(to, compareKey)" becomes empty as it is late activated by Linq
                        //to = to.Union(toMerge, compareKey).ToList(); //reasigns pointer so is no longer the pointer from the parent object, 
                        to.AddRange(toMerge); //we do not want to reassign the "to" pointer, so us the AddRange method, 
                        if (Logger.IsLogging) //we want to know the removed as we are logging
                        {
                            from.Clear(); //again we do not want to reassign the pointer, so use passed list, clear then add back the excludes
                            from.AddRange(excluded);
                        }
                    }
                    else
                    {
                        excluded = from;
                    }

                    //Check to see if all merged in
                    if ((toMergeCount > 0) || (total != to.Count))
                    {
                        Logger.WriteLine(rootObj, parentObj, typeof(T), (total - to.Count), toMergeCount, depthIndicator);
                    }

                    //set up Chain of instances for this type (keyed also on parent object), too stop infinite loop i hope!!
                    if (CheckInstance && excluded.Any() && !ChainMapInst.ContainsKey(typeof(T)))
                    {
                        ChainMapInst[typeof(T)] = new Dictionary<object, HashSet<CobieObject>>();
                    }
                    //we need to dig deeper, so indicate in log
                    if (excluded.Any())
                    {
                        depthIndicator++;

                        //Drill down into cobieObject excluded objects lists to check embedded list fro duplicates - Category,Documents, Attributes,Issues, Impacts, Representations, ProjectStages
                        foreach (var item in excluded)
                        {
                            //do instance checking too stop infinite loop i hope!!
                            var rootKey = rootObj != null ? rootObj : item;
                            if (CheckInstance && !ChainMapInst[typeof(T)].ContainsKey(rootKey))
                            {
                                ChainMapInst[typeof(T)][rootKey] = new HashSet<CobieObject>();
                            }

                            if (!CheckInstance || !ChainMapInst[typeof(T)][rootKey].Contains(item))
                            {
                                if (CheckInstance && rootObj != null)
                                    ChainMapInst[typeof(T)][rootKey].Add(item); //instance check
                                                                                //Drill down
                                var toItem = to.Where(f => compareKey.Equals(f, item)).OfType<T>().First();
                                MergeCOBieObject<T>(toItem, item, depthIndicator, rootKey, item);
                            }
                        }
                    }
                }
                else
                {
                    to = from; //can reassign as passed back in return
                    from.Clear();//clear out as all merged , not reassigned as pointer
                }
            }
            return to;
        }
        /// <summary>
        /// Merge Lists with no drill down (do not inherit from CobieObject)
        /// </summary>
        /// <param name="to">List to merge to</param>
        /// <param name="from">List to merge from</param>
        /// <param name="compareKey">IEqualityComparer</param>
        /// <param name="rootObj">Starting object of the merge</param>
        /// <param name="parentObj">Object which caused thei mrthod to be called</param>
        private List<T> MergeSimpleLists<T>(List<T> to, List<T> from, IEqualityComparer<T> compareKey, int depthIndicator = 0, CobieObject rootObj = null, CobieObject parentObj = null) 
        {
            if (from != null && from.Any())
            {
                if (to != null)
                {
                    var total = to.Count + from.Count;
                    var toMerge = from.Except(to, compareKey);
                    int toMergeCount = 0;
                    IEnumerable<T> excluded = null;
                    if (toMerge.Any())
                    {
                        toMergeCount = toMerge.Count();
                        excluded = from.Except(toMerge, compareKey).ToList(); //ToList runs query now, otherwise we get full list as once toMerge is added to list "from.Except(to, compareKey)" becomes empty as it is late activated by Linq
                                                                              //to = to.Union(toMerge, compareKey).ToList(); //reasigns pointer so is no longer the pointer from the parent object, BAD 
                        to.AddRange(toMerge); //we do not want to reassign the "to" pointer, so us the AddRange method, GOOD
                        if (Logger.IsLogging) //we want to know the removed as we are logging
                        {
                            from.Clear(); //again we do not want to reassign the pointer, so use passed list, clear then add back the excludes
                            from.AddRange(excluded);
                        }
                    }
                    else
                    {
                        excluded = from;
                    }

                    //Check to see if all merged in
                    if ((toMergeCount > 0) || (total != to.Count))
                    {
                        Logger.WriteLine(rootObj, parentObj, typeof(T), (total - to.Count), toMergeCount, depthIndicator);
                    }
                }
                else
                {
                    to = from;
                    from = null;//clear out as all merged 
                }
            }
            return to;
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