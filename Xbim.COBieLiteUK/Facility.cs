using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Xbim.COBieLiteUK.Converters;
using Formatting = System.Xml.Formatting;

namespace Xbim.COBieLiteUK
{
    public partial class Facility
    {
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
            var serialiser = JsonSerializer.Create(serializerSettings);
            return serialiser;
        }

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

        [SuppressMessage("ReSharper", "InconsistentNaming")] 
        private StringWriter log = new StringWriter();

        public void ReadCobie(string path, string version = "UK2012")
        {
            if (path == null) throw new ArgumentNullException("path");
            var ext = Path.GetExtension(path).ToLower().Trim('.');
            if (ext != "xls" && ext != "xlsx") throw new Exception("File must be an MS Excel file.");
            
            using (var file = File.OpenRead(path))
            {
                var type = ext == "xlsx" ? ExcelTypeEnum.XLSX : ExcelTypeEnum.XLS;
                ReadCobie(file, type, version);
                file.Close();
            }
        }

        public string ReadCobie(Stream stream, ExcelTypeEnum type, string version = "UK2012")
        {
            //refresh log for this run
            log = new StringWriter();

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

            var msg = LoadFromCobie(workbook, null, version);
            if(!String.IsNullOrEmpty(msg))
                log.Write(msg);

            return log.ToString();
        }

       

        #endregion

        internal override IEnumerable<CobieObject> GetChildren()
        {
            //enumerate base
            foreach (var child in base.GetChildren())
                yield return child;

            //enumerate own
            if (Floors != null)
                foreach (var floor in Floors)
                    yield return floor;
            if(AssetTypes != null)
                foreach (var assetType in AssetTypes)
                    yield return assetType;
            if(Contacts != null)
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
            if(Stages != null)
                foreach (var stage in Stages)
                    yield return stage;
        }
    }

    public enum ExcelTypeEnum
    {
        // ReSharper disable InconsistentNaming
        XLS,
        XLSX
        // ReSharper restore InconsistentNaming
    }
}