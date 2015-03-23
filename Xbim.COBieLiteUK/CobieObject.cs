using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
            foreach (var child in GetChildren())
                child.SetFacility(facility);
        }

        [XmlIgnore]
        [JsonIgnore]
        internal virtual Facility Facility
        {
            get { return _facility; }
        }

        internal virtual IEnumerable<CobieObject> GetChildren()
        {
            if (Documents != null)
                foreach (var document in Documents)
                    yield return document;
            if (Attributes != null)
                foreach (var attribute in Attributes)
                    yield return attribute;
            if (Issues != null)
                foreach (var issue in Issues)
                    yield return issue;
            if (Impacts != null)
                foreach (var impact in Impacts)
                    yield return impact;
            if (Representations != null)
                foreach (var representation in Representations)
                    yield return representation;
        }

        private string _parentSheet;
        private string _parentNameProperty;
        private string _parentNameValue;

        internal static List<CobieObject> LoadFromCobie(Type cobieType, IWorkbook workbook, out string message,
            string version = "UK2012")
        {
            if (!(typeof (CobieObject)).IsAssignableFrom(cobieType))
                throw new ArgumentException("cobieType has to be of CobieObject type", "cobieType");

            //refresh log for this run
            var log = new StringWriter();
            var result = new List<CobieObject>();

            //fill in object attributes first
            var sheetName = GetSheetName(cobieType, version);
            if (sheetName == null)
            {
                log.WriteLine("There is no sheet maping for a {0}.", cobieType.Name);
                message = log.ToString();
                return result;
            }

            //try to get object sheet
            var sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                log.WriteLine("There is no {0} sheet for a {1}.", sheetName, cobieType.Name);
                message = log.ToString();
                return result;
            }

            //get mappings
            var mappingAttributes = GetMapping(cobieType, version);
            var mappings = mappingAttributes as MappingAttribute[] ?? mappingAttributes.ToArray();
            if (!mappings.Any())
            {
                log.WriteLine("There is no mapping for a {0} parameters", cobieType.Name);
                message = log.ToString();
                return result;
            }

            var firstRowNum = sheet.FirstRowNum;
            var lastRowNum = sheet.LastRowNum;

            //fix mappings for the case columns are swapped for some reason
            var msg = FixMappings(mappings, sheet);
            if (!String.IsNullOrEmpty(msg))
                log.Write(msg);

            //iterate over rows in the sheet (skip the header)
            for (int i = firstRowNum + 1; i <= lastRowNum; i++)
            {
                //create new object per row
                var row = sheet.GetRow(i);
                var cObject = Activator.CreateInstance(cobieType) as CobieObject;
                if (cObject == null)
                    break;

                //check if there is any value in the row
                var anyValue = false;
                foreach (ICell cell in row)
                {
                    if (cell.CellType != CellType.Blank && cell.CellType != CellType.Error &&
                        cell.CellType != CellType.Formula)
                    {
                        anyValue = true;
                        break;
                    }
                }
                if (!anyValue) continue;


                //fill facility values using reflection
                foreach (var mapping in mappings)
                {
                    var cellIndex = CellReference.ConvertColStringToIndex(mapping.Column);
                    var cell = row.GetCell(cellIndex);

                    //use reflection to set the value if the value is available
                    if ((cell.CellType == CellType.Blank || cell.CellType == CellType.Error) && mapping.Required)
                    {
                        log.WriteLine(
                            "{0} is a required field for COBie {1}. Data will be processed but it will be incomplete.",
                            mapping.Header, version);
                    }

                    //this object should go into a list within a parent
                    if (mapping.Path.StartsWith("parent"))
                    {
                        if (cell.CellType != CellType.String)
                        {
                            log.WriteLine(
                                "Cell {0}{1} (sheet {2}) is a parent key. It must have a value. Resulting data will be incomplete.",
                                CellReference.ConvertNumToColString(cellIndex), i, sheet.SheetName);
                            continue;
                        }

                        //Store parent mapping information for a later use
                        if (mapping.Path == "parent")
                            cObject._parentSheet = cell.StringCellValue.Trim();

                        if (mapping.Path.StartsWith("parent."))
                        {
                            cObject._parentNameProperty = mapping.Path.Replace("parent.", "");
                            cObject._parentNameValue = cell.StringCellValue.Trim();
                        }
                        continue;
                    }

                    if (cell.CellType != CellType.Blank && cell.CellType != CellType.Error)
                        log.Write(SetMemberValue(cObject, mapping.Path, cell));
                }
                result.Add(cObject);
            }

            message = log.ToString();
            return result;
        }

        private static string FixMappings(IEnumerable<MappingAttribute> mappings, ISheet sheet)
        {
            var headerRow = sheet.GetRow(0);
            var log = new StringWriter();
            //fill facility values using reflection
            foreach (var mapping in mappings)
            {
                //Check column header for the case it is swapped and fix it if it is possible
                var cellIndex = CellReference.ConvertColStringToIndex(mapping.Column);
                var headerCell = headerRow.GetCell(cellIndex);
                if (headerCell != null && headerCell.CellType == CellType.String)
                {
                    var header = headerCell.StringCellValue.Trim().ToLower();
                    if (header != mapping.Header.ToLower())
                    {
                        //try to find the right one
                        var indexFixed = false;
                        foreach (ICell hCell in headerRow)
                        {
                            if (hCell.CellType == CellType.String &&
                                hCell.StringCellValue.Trim().ToLower() == mapping.Header.ToLower())
                            {
                                cellIndex = hCell.ColumnIndex;
                                //fix fur future passes
                                mapping.Column = CellReference.ConvertNumToColString(cellIndex);
                                indexFixed = true;
                                log.WriteLine(
                                    "Sheet {0} has {1} defined as a column {2} where it should be {3}. Data will be processed but this is a wrongly structured COBie spreadsheet.",
                                    sheet.SheetName, mapping.Header, CellReference.ConvertNumToColString(cellIndex),
                                    mapping.Column);
                                break;
                            }
                        }
                        if (!indexFixed)
                        {
                            log.WriteLine(
                                "Sheet {0} should have a column {1} defined as {2} but it is {3} instead. Data will be processed but this is a wrongly structured COBie spreadsheet.",
                                sheet.SheetName, mapping.Column, mapping.Header, header);
                            //fix fur future passes
                            mapping.Header = header;
                        }
                    }
                }
            }
            return log.ToString();
        }

        internal virtual void AddToParent(IEnumerable<CobieObject> parents, out string message, string version)
        {
            var log = new StringWriter();
            Type parentType = null;
            //try to set parent type from custom attributes
            var parentAttribute = GetType().GetCustomAttribute<ParentAttribute>();
            if (parentAttribute != null)
                parentType = parentAttribute.DataType;
            else
            //set parent type from sheet information
                parentType =
                    GetType()
                        .Assembly.GetTypes()
                        .FirstOrDefault(
                            t =>
                                t.GetCustomAttributes<SheetMappingAttribute>()
                                    .Any(a => a.Type == version && a.Sheet == _parentSheet));
            if (parentType == null)
            {
                log.WriteLine(
                    "There is no type mapping for a {0} sheet. Resulting data model will be incomplete as this {1} won't have a place to live in.",
                    _parentSheet, GetType().Name);
                message = log.ToString();
                return;
            }
            CobieObject parent;
            if (parentType == typeof (Facility))
            {
                parent = parents.OfType<Facility>().FirstOrDefault();
            }
            else
            {
                var parentProperty = parentType.GetProperty(_parentNameProperty);
                if (parentProperty == null)
                {
                    log.WriteLine(
                        "{0} is not defined within a sheet {1}. Resulting data will be incomplete as this {2}({3}) won't have a place to live in.",
                        _parentNameProperty, _parentSheet, GetType().Name, Name);
                    message = log.ToString();
                    return;
                }

                var cobieObjects = parents as CobieObject[] ?? parents.ToArray();
                parent =
                    cobieObjects.FirstOrDefault(
                        p => p.GetType() == parentType && parentProperty.GetValue(p).Equals(_parentNameValue));
            }

            if (parent == null)
            {
                log.WriteLine(
                    "This {0}({1}) doesn't have a parent from sheet {2} with name {3}. It won't exist in the resulting data. This is an invalid COBie record.",
                    GetType().Name, Name, _parentSheet, _parentNameValue);
                message = log.ToString();
                return;
            }

            //find a list for this type
            var listType = typeof (List<>);
            listType = listType.MakeGenericType(new[] {GetType()});
            var listPropInfo = parentType.GetProperties().FirstOrDefault(l => l.PropertyType == listType);
            if (listPropInfo == null)
                throw new Exception(
                    String.Format("Type {0} doesn't have a list of type {1} which would accomodate this object.",
                        parentType.Name, GetType().Name));

            //create list if it is null
            var list = listPropInfo.GetValue(parent);
            if (list == null)
            {
                list = Activator.CreateInstance(listType);
                listPropInfo.SetValue(parent, list);
            }
            //add this object
            var addMethod = listType.GetMethod("Add");
            addMethod.Invoke(list, new object[] {this});

            //report any problems
            message = log.ToString();
        }

        private static string SetMemberValue(object obj, string path, ICell cell)
        {
            var log = new StringWriter();

            //If the value is n/a it is the same as if it was not defined at all
            if (cell.CellType == CellType.String && cell.StringCellValue.ToLower() == "n/a")
                return "";

            var parts = path.Split('.').ToList();
            var actual = parts.First();
            parts.Remove(actual);
            var rest = String.Join(".", parts);

            var type = obj.GetType();

            //if it is a list, create instance of an item and set its member to the value.
            if (typeof (IList).IsAssignableFrom(type))
            {
                var addMethod = type.GetMethod("Add");

                var values = cell.StringCellValue.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var value in values)
                {
                    //create an instance of generic type
                    var itemType = type.GetGenericArguments()[0];
                    var item = Activator.CreateInstance(itemType);
                    addMethod.Invoke(obj, new[] {item});
                    var memberInfo = itemType.GetProperty(actual);
                    if (memberInfo == null)
                        log.WriteLine("Object {0} doesn't have a property {1}.", itemType.Name, actual);
                    else
                        memberInfo.SetValue(item, value);
                }
                return log.ToString();
            }


            var objPropertInfo = type.GetProperty(actual);
            if (objPropertInfo == null)
            {
                //try to get internal member
                objPropertInfo = type.GetProperty(actual,
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
                if (objPropertInfo == null)
                {
                    log.WriteLine("Property {0} is not defined in {1}. Sheet: {2}, Row: {3}, Cell: {4}", actual,
                        type.Name, cell.Sheet.SheetName, cell.RowIndex, cell.ColumnIndex);
                    return log.ToString();
                }
            }

            //set the value if this is the last part of the path
            if (String.IsNullOrEmpty(rest))
                SetSimpleValue(objPropertInfo, obj, cell);
            //or call this function recursively on the inner member
            else
            {
                //get or create object
                var propValue = objPropertInfo.GetValue(obj);
                if (propValue == null)
                {
                    //AttributeValue is the only abstract class. It needs to decide about the concrete class to use
                    if (objPropertInfo.PropertyType == typeof (AttributeValue))
                    {
                        switch (cell.CellType)
                        {
                            case CellType.Numeric:
                                propValue = new DecimalAttributeValue();
                                break;
                            case CellType.Boolean:
                                propValue = new BooleanAttributeValue();
                                break;
                            default:
                                propValue = new StringAttributeValue();
                                break;
                        }
                    }
                    else
                    {
                        //create new object 
                        propValue = Activator.CreateInstance(objPropertInfo.PropertyType);
                    }
                    //assign it to the property
                    objPropertInfo.SetValue(obj, propValue);
                }

                //call SetMemberValue recursively
                log.Write(SetMemberValue(propValue, rest, cell));
            }
            return log.ToString();
        }

        private static void SetSimpleValue(PropertyInfo info, object obj, ICell cell)
        {
            var type = info.PropertyType;
            type = type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>)
                ? Nullable.GetUnderlyingType(type)
                : type;

            if (typeof (String).IsAssignableFrom(type))
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

            if (type == typeof (DateTime))
            {
                var date = default(DateTime);
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        date = cell.DateCellValue;
                        break;
                    case CellType.String:
                        if (!DateTime.TryParse(cell.StringCellValue, null, DateTimeStyles.RoundtripKind, out date))
                            //set to default value according to specification
                            date = DateTime.Parse("1900-12-31T23:59:59", null, DateTimeStyles.RoundtripKind);
                        break;
                }
                info.SetValue(obj, date);
            }

            if (type == typeof (double))
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

            if (type == typeof (int))
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        info.SetValue(obj, (int) cell.NumericCellValue);
                        break;
                    case CellType.String:
                        int i;
                        if (int.TryParse(cell.StringCellValue, out i))
                            info.SetValue(obj, i);
                        break;
                }
            }
        }

        private static IEnumerable<MappingAttribute> GetMapping(Type type, string mapping)
        {
            var result = type.GetCustomAttributes(typeof (MappingAttribute))
                .Where(a => ((MappingAttribute) a).Type == mapping)
                .Cast<MappingAttribute>().ToList();

            //there is a special tweak for attributes where Value fiels needs to be processed as the first one 
            //in order to get the right data type
            var valueField = result.FirstOrDefault(m => m.Path == "Value.Value");
            if (valueField != null)
            {
                result.Remove(valueField);
                //insert it at the beginning
                result.Insert(0, valueField);
            }

            return result;
        }

        private static string GetSheetName(Type type, string mapping)
        {
            var attr =
                type.GetCustomAttributes(typeof (SheetMappingAttribute), true)
                    .FirstOrDefault(a => ((SheetMappingAttribute) a).Type == mapping) as SheetMappingAttribute;
            return attr == null ? null : attr.Sheet;
        }
    }
}