using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Xbim.COBieLiteUK
{
    public abstract partial class CobieObject
    {

        // ReSharper disable InconsistentNaming
        protected Facility _facility;
        // ReSharper restore InconsistentNaming
        internal void SetFacility(Facility facility)
        {
            _facility = facility;
            foreach(var child in GetChildren())
                child.SetFacility(facility);
        }

        [XmlIgnore][JsonIgnore]
        internal virtual Facility Facility { get { return _facility; } }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private StringWriter log = new StringWriter();

        internal virtual IEnumerable<CobieObject> GetChildren()
        {
            if (Documents != null)
                foreach (var document in Documents)
                    yield return document;
            if (Attributes != null)
                foreach (var attribute in Attributes)
                    yield return attribute;
            if(Issues != null)
                foreach (var issue in Issues)
                    yield return issue;
            if (Impacts != null)
                foreach (var impact in Impacts)
                    yield return impact;
            if (Representations != null)
                foreach (var representation in Representations)
                    yield return representation;
        }

        public string LoadFromCobie(IWorkbook workbook, IEnumerable<CobieObject> potentialParents, string version = "UK2012")
        {
            //refresh log for this run
            log = new StringWriter();

            //fill in object attributes first
            var sheetName = GetSheetName(GetType(), version);
            if (sheetName == null)
            {
                log.WriteLine("There is no sheet maping for a {0}.", GetType().Name);
                return log.ToString();
            }

            //try to get object sheet
            var sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                log.WriteLine("There is no {0} sheet for a {1}.", sheetName, GetType().Name);
                return log.ToString();
            }

            //get mappings
            var mappingAttributes = GetMapping(GetType(), version);
            var mappings = mappingAttributes as MappingAttribute[] ?? mappingAttributes.ToArray();
            if (!mappings.Any())
            {
                log.WriteLine("There is no mapping for a Facility parameters");
                return log.ToString();
            }

            var firstRowNum = sheet.FirstRowNum;
            var lastRowNum = sheet.LastRowNum;

            //fill facility values using reflection
            foreach (var mapping in mappings)
            {
                //iterate over rows in the sheet (skip the header)
                for (int i = firstRowNum + 1; i <= lastRowNum; i++)
                {
                    //TODO: Check column header for the case it is swapped
                    var cellIndex = CellReference.ConvertColStringToIndex(mapping.Column);
                    var row = sheet.GetRow(i);
                    var cell = row.GetCell(cellIndex);

                    //use reflection to set the value if the value is available
                    if (cell.CellType != CellType.Blank && cell.CellType != CellType.Error)
                        SetMemberValue(this, mapping.Path, cell);
                }
            }

            return log.ToString();
        }

        private void SetMemberValue(object obj, string path, ICell cell)
        {
            //If the value is n/a it is the same as if it was not defined at all
            if (cell.CellType == CellType.String && cell.StringCellValue.ToLower() == "n/a")
                return;

            var parts = path.Split('.').ToList();
            var actual = parts.First();
            parts.Remove(actual);
            var rest = String.Join(".", parts);

            var type = obj.GetType();
            var objPropertInfo = type.GetProperty(actual);
            if (objPropertInfo == null)
            {
                log.WriteLine("Property {0} is not defined in {1}. Sheet: {2}, Row: {3}, Cell: {4}", actual, type.Name, cell.Sheet.SheetName, cell.RowIndex, cell.ColumnIndex);
                return;
            }

            //set the value if this is the last part of the path
            if (String.IsNullOrEmpty(rest))
                SetSimpleValue(objPropertInfo, obj, cell);
            else
            {
                //get or create object
                var propValue = objPropertInfo.GetValue(obj);
                if (propValue == null)
                {
                    //create new object 
                    propValue = Activator.CreateInstance(objPropertInfo.PropertyType);
                    //assign it to the property
                    objPropertInfo.SetValue(obj, propValue);
                }

                //call SetMemberValue recursively
                SetMemberValue(propValue, rest, cell);
            }
        }

        private void SetSimpleValue(PropertyInfo info, object obj, ICell cell)
        {
            var type = info.PropertyType;
            if (typeof(String).IsAssignableFrom(type))
            {
                string value = null;
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        value = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                        break;
                    case CellType.String:
                        value = cell.StringCellValue;
                        break;
                    case CellType.Boolean:
                        value = cell.BooleanCellValue.ToString();
                        break;
                }
                info.SetValue(obj, value);
            }

            if (type == typeof(DateTime))
            {
                var date = default(DateTime);
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        date = cell.DateCellValue;
                        break;
                    case CellType.String:
                        DateTime.TryParse(cell.StringCellValue, null, DateTimeStyles.RoundtripKind,  out date);
                        break;
                }
                info.SetValue(obj, date);
            }

            if (type == typeof(double))
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        info.SetValue(obj, cell.NumericCellValue);
                        break;
                    case CellType.String:
                        double d;
                        if (double.TryParse(cell.StringCellValue, out d))
                            info.SetValue(obj, d);
                        break;
                }
            }

            if (type == typeof(int))
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        info.SetValue(obj, (int)cell.NumericCellValue);
                        break;
                    case CellType.String:
                        int i;
                        if (int.TryParse(cell.StringCellValue, out i))
                            info.SetValue(obj, i);
                        break;
                }
            }

            if (typeof (List<>).IsAssignableFrom(type))
            {
                //create an instance of generic type

                //set a member of the type to the value (recursion)

                //add the instance to the list
                throw new NotImplementedException();
            }
        }

        private IEnumerable<MappingAttribute> GetMapping(Type type, string mapping)
        {
            return
                type.GetCustomAttributes(typeof(MappingAttribute))
                    .Where(a => ((MappingAttribute)a).Type == mapping)
                    .Cast<MappingAttribute>();
        }

        private string GetSheetName(Type type, string mapping)
        {
            var attr =
                type.GetCustomAttributes(typeof(SheetMappingAttribute), true)
                    .FirstOrDefault(a => ((SheetMappingAttribute)a).Type == mapping) as SheetMappingAttribute;
            return attr == null ? null : attr.Sheet;
        }
    }
}
