using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
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

        protected CobieObject()
        {
            //create default unique name for the case no name is set.
            Name = Guid.NewGuid().ToString();
        }

        [XmlIgnore]
        [JsonIgnore]
        internal virtual Facility Facility
        {
            get { return _facility; }
        }

        internal IEnumerable<T> GetDeep<T>(Func<T, bool> condition = null) where T: CobieObject
        {
            var children = GetChildren().ToArray();
            foreach (var child in children.OfType<T>())
            {
                if(condition == null) yield return child;
                else if(condition(child)) yield return child;
            }
            //traverse tree down
            foreach (var subChild in children.SelectMany(child => child.GetDeep(condition)))
            {
                yield return subChild;
            }
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

        internal virtual IEnumerable<IEntityKey> GetKeys()
        {
            if (CreatedBy != null)
                yield return CreatedBy;
            if (ProjectStages == null) yield break;
            foreach (var key in ProjectStages)
                yield return key;
        }

        // ReSharper disable InconsistentNaming
        protected string _parentSheet;
        protected string _parentNameProperty;
        protected string _parentNameValue;
        // ReSharper restore InconsistentNaming

        private static IEnumerable<MappingAttribute> _mappings; 

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
            _mappings = GetMapping(cobieType, version).ToList();
            if (!_mappings.Any())
            {
                log.WriteLine("There is no mapping for a {0} parameters", cobieType.Name);
                message = log.ToString();
                return result;
            }

            var firstRowNum = sheet.FirstRowNum;
            var lastRowNum = sheet.LastRowNum;

            //fix mappings for the case columns are swapped for some reason
            var msg = FixMappings(_mappings, sheet);
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
                foreach (var mapping in _mappings)
                {
                    var cellIndex = CellReference.ConvertColStringToIndex(mapping.Column);
                    var cell = row.GetCell(cellIndex);
                    if(cell == null) continue;

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

            //merge duplicates (mostly caused by compound keys)
            var toRemove = new List<CobieObject>();
            foreach (var cobieObject in result)
            {
                if (toRemove.Contains(cobieObject)) continue; //this should always preserve the first original value
                var duplicates = cobieObject.MergeDuplicates(result, log);
                if (duplicates == null) continue;
                toRemove.AddRange(duplicates);
            }
            foreach (var duplicate in toRemove)
                result.Remove(duplicate);

            message = log.ToString();
            return result;
        }

        internal virtual void WriteToCobie(IWorkbook workbook, TextWriter log, CobieObject parent,
            string version = "UK2012")
        {
            var mappings = GetMapping(GetType(), version).ToList();
            if (!mappings.Any())
            {
                log.WriteLine("There are no mappings for a type '{0}'", GetType().Name);
                return;
            }

            //get or create a sheet
            var sheetName = GetSheetName(GetType(), version);
            var sheet = workbook.GetSheet(sheetName) ?? workbook.CreateSheet(sheetName);

            var row = GetNextEmptyRow(sheet);
            if (row.RowNum == 0)
            {
                //set up header if this is the very first row in the sheet
                SetUpHeader(sheet.CreateRow(0), mappings);
                row = sheet.CreateRow(1);
            }
            if (row.RowNum == 1)
            {
                //make sure all headers are in there
                FixHeaderForWriting(sheet.GetRow(0), mappings);
            }

            //write columns
            foreach (var mapping in mappings)
            {
                var cellIndex = CellReference.ConvertColStringToIndex(mapping.Column);
                var cell = row.GetCell(cellIndex) ?? row.CreateCell(cellIndex);
                
                //set default column style
                cell.CellStyle = sheet.GetColumnStyle(cellIndex);

                //if it is a parent or parent name, set it differently
                if (mapping.Path.ToLower() == "parent")
                {
                    if (parent == null)
                        throw new Exception(
                            String.Format(
                                "Object (type: {0}, name: {1}) can't exist on its own but no parent object was supplied.",
                                GetType().Name, Name));
                    var parentSheet = GetSheetName(parent.GetType(), version);
                    if (String.IsNullOrEmpty(parentSheet))
                        throw new Exception(
                            String.Format("Parent object (type: {0}, name: {1}) doesn't have a mapping defined in {2}",
                                parent.GetType().Name, parent.Name, version));
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(parentSheet);

                    if (!String.IsNullOrEmpty(mapping.PickList))
                    {
                        WritePickListValue(workbook, mapping, parentSheet);
                    }
                    continue;
                }
                if (mapping.Path.ToLower().StartsWith("parent."))
                {
                    if (parent == null)
                        throw new Exception(
                            String.Format("{0} (name: {1}) can't exist on its own but no parent object was supplied.",
                                sheet.SheetName, Name));
                    var parentPath = mapping.Path.Replace("parent.", "");
                    var parentPropInfo = parent.GetType().GetProperty(parentPath);
                    //keys are always string properties
                    var parentKeyValue = parentPropInfo.GetValue(parent) as String;

                    if (String.IsNullOrEmpty(parentKeyValue))
                    {
                        log.WriteLine(
                            "Parent object (type: {0}) doesn't have a key property '{1}' defined. This will result into invalid and inconsistent COBie file.",
                            parent.GetType().Name, parentPath);
                    }
                    else
                    {
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue(parentKeyValue);
                    }

                    if (!String.IsNullOrEmpty(mapping.PickList))
                    {
                        WritePickListValue(workbook, mapping, parentKeyValue);
                    }
                    continue;
                }

                var value = GetCobieProperty(mapping, log);
                if (value == null)
                    cell.SetCellValue("n/a");
                else
                {
                    switch (value.ValueType)
                    {
                        case CobieValueType.Boolean:
                            if (value.BooleanValue.HasValue)
                                cell.SetCellValue(value.BooleanValue ?? false);
                            else
                                cell.SetCellValue("n/a");
                            break;
                        case CobieValueType.DateTime:
                            var dtVal = value.DateTimeValue ?? //default date value
                                        DateTime.Parse("1900-12-31T23:59:59", null, DateTimeStyles.RoundtripKind);
                            cell.SetCellValue(dtVal.ToString("O").Substring(0, 19));
                            break;
                        case CobieValueType.Double:
                            if (value.DoubleValue.HasValue)
                                cell.SetCellValue(value.DoubleValue ?? 0);
                            else
                                cell.SetCellValue("n/a");
                            break;
                        case CobieValueType.Integer:
                            if (value.IntegerValue.HasValue)
                                cell.SetCellValue(value.IntegerValue ?? 0);
                            else
                                cell.SetCellValue("n/a");
                            break;
                        case CobieValueType.String:
                            cell.SetCellValue(value.StringValue ?? "n/a");
                            break;
                        default:
                            throw new Exception("Unexpected data type");
                    }
                    if (!String.IsNullOrEmpty(mapping.PickList) && value.ValueType == CobieValueType.String)
                    {
                        if (mapping.Path.StartsWith("Categories")) //categories need to be handled differently
                        {
                            if(Categories != null)
                                foreach (var category in Categories)
                                {
                                    if(string.IsNullOrEmpty(category.Classification))
                                        WritePickListValue(workbook, mapping, category.CategoryString);
                                    else
                                    {
                                        var alterMapping = new MappingAttribute { PickList = mapping.PickList.Substring(0, mapping.PickList.IndexOf('.') + 1) + category.Classification };
                                        WritePickListValue(workbook, alterMapping, category.CategoryString);
                                    }
                                }
                        }
                        else
                            WritePickListValue(workbook, mapping, value.StringValue);
                    }
                }
            }

            //call for all child objects but with this as a parent
            foreach (var child in GetChildren())
            {
                child.WriteToCobie(workbook, log, this, version);
            }
        }

        private void FixHeaderForWriting(IRow row, List<MappingAttribute> mappings)
        {
            foreach (var mapping in mappings)
            {
                var cellIndex = CellReference.ConvertColStringToIndex(mapping.Column);
                var cell = row.GetCell(cellIndex) ?? row.CreateCell(cellIndex);
                if (cell.CellType == CellType.Blank)
                {
                    cell.CellStyle = row.RowStyle;
                    cell.SetCellValue(mapping.Header);
                }

                if(cell.CellType == CellType.String && cell.StringCellValue.Trim() != mapping.Header)
                    throw new Exception("Wrong template header!");
            }
        }

        private IRow GetNextEmptyRow(ISheet sheet)
        {
            foreach (IRow row in sheet)
            {
                var isEmpty = true;
                foreach (ICell cell in row)
                {
                    if (cell.CellType != CellType.Blank)
                    {
                        isEmpty = false;
                        break;
                    }
                }
                if (isEmpty) return row;
            }
            return sheet.CreateRow(sheet.LastRowNum + 1);
        }

        private void WritePickListValue(IWorkbook workbook, MappingAttribute mapping, string value)
        {
            if (string.IsNullOrEmpty(mapping.PickList) || string.IsNullOrEmpty(value) || value == "n/a")
                return;

            var pickPath = mapping.PickList.Split('.');
            var sheetName = pickPath[0];
            var columnName = pickPath[1];

            var sheet = workbook.GetSheet(sheetName) ?? workbook.CreateSheet(sheetName);
            var header = sheet.GetRow(0) ?? sheet.CreateRow(0);
            var colIndex = -1;
            foreach (ICell col in header)
                if (col.CellType == CellType.String && col.StringCellValue.ToLower() == columnName.ToLower())
                {
                    colIndex = col.ColumnIndex;
                    break;
                }
                else if (col.CellType == CellType.Blank)
                {
                    colIndex = col.ColumnIndex;
                    col.SetCellValue(columnName);
                    break;
                }
            if (colIndex == -1) //create new header
            {
                var headerCell = header.CreateCell(header.LastCellNum == -1 ? 0 : header.LastCellNum);
                colIndex = headerCell.ColumnIndex;
                headerCell.SetCellValue(columnName);
                headerCell.CellStyle = header.RowStyle;
            }
            var rowIndex = 1;
            foreach (IRow row in sheet)
            {
                var cell = row.GetCell(colIndex);
                if (cell == null || cell.CellType == CellType.Blank)
                {
                    rowIndex = row.RowNum;
                    break;
                }

                if (cell.CellType == CellType.String && cell.StringCellValue == value)
                    return; //do nothing if the value exists already

                //set row index to the next cell
                rowIndex = row.RowNum + 1;
            }
            {
                var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
                var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                cell.SetCellValue(value);
                cell.CellStyle = sheet.GetColumnStyle(colIndex);
            }
        }

        private ICell GetNextEmptyCell(IRow row)
        {
            foreach (ICell cell in row)
            {
                if (cell.CellType == CellType.Blank)
                    return cell;
            }
            return row.CreateCell(row.LastCellNum);
        }

        public CobieValue GetCobieProperty(string name)
        {
            var mapping = GetType().GetCustomAttributes(typeof (MappingAttribute))
                .FirstOrDefault(a => ((MappingAttribute) a).Header == name) as MappingAttribute;
            return mapping == null ? null : GetCobieProperty(mapping, new StringWriter());
        }

        protected internal CobieValue GetCobieProperty(MappingAttribute mapping, TextWriter log)
        {
            var path = mapping.Path;
            var parts = path.Split('.').ToList();
            object instance = this;

            for (var i = 0; i < parts.Count; i++)
            {
                var type = instance.GetType();
                var part = parts[i];
                var propInfo = type.GetProperty(part) ?? type.GetProperty(part,
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
                if (propInfo == null)
                    throw new Exception(String.Format("Member {0} is not defined in {1}", part, type.Name));

                var value = propInfo.GetValue(instance);

                //simple values (there is no further path)
                if (i == parts.Count - 1)
                {
                    //get non nullable base type of property
                    var propType = propInfo.PropertyType;
                    propType = propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof (Nullable<>)
                        ? Nullable.GetUnderlyingType(propType)
                        : propType;
                    if (propType == typeof (bool))
                        return new CobieValue {BooleanValue = (bool?) value, ValueType = CobieValueType.Boolean};
                    if (propType == typeof (double))
                        return new CobieValue {DoubleValue = (double?) value, ValueType = CobieValueType.Double};
                    if (propType == typeof (int))
                        return new CobieValue {IntegerValue = (int?) value, ValueType = CobieValueType.Integer};
                    if (propType == typeof (DateTime))
                        return new CobieValue {DateTimeValue = (DateTime?) value, ValueType = CobieValueType.DateTime};
                    if (propType == typeof (string))
                        return new CobieValue {StringValue = (string) value, ValueType = CobieValueType.String};
                    if (propType.IsEnum)
                    {
                        return value != null
                            ? new CobieValue
                            {
                                StringValue = Enum.GetName(propType, value),
                                ValueType = CobieValueType.String
                            }
                            : new CobieValue {StringValue = "n/a", ValueType = CobieValueType.String};
                    }
                    //some other type. This shouldn't happen as the schema only uses basic types and enumerations
                    return new CobieValue {ValueType = CobieValueType.Unknown};
                }

                //anything else needs to have an object for further access and iterations. Return null if the object is null.
                if (value == null) return null;


                //key list
                var enumerable = value as IEnumerable;
                if (enumerable != null)
                {
                    var result = new List<string>();
                    var keyName = parts[i + 1];
                    foreach (var item in enumerable)
                    {
                        var keyPropInfo = item.GetType().GetProperty(keyName) ??
                                          item.GetType()
                                              .GetProperty(keyName,
                                                  BindingFlags.NonPublic | BindingFlags.Instance |
                                                  BindingFlags.GetProperty);
                        if (keyPropInfo == null)
                            throw new Exception(String.Format("Member {0} is not defined in {1}", keyName,
                                item.GetType().Name));
                        var keyValue = keyPropInfo.GetValue(item) as string;
                        if (keyValue != null)
                            result.Add(keyValue);
                    }

                    return new CobieValue {StringValue = String.Join(",", result), ValueType = CobieValueType.String};
                }

                //nested object - set new instance and let this to iterate over
                instance = value;
            }

            //this should never happen.
            return null;
        }

        private void SetUpHeader(IRow row, IEnumerable<MappingAttribute> mappings)
        {
            foreach (var mapping in mappings)
            {
                var cellIndex = CellReference.ConvertColStringToIndex(mapping.Column);
                var cell = row.GetCell(cellIndex) ?? row.CreateCell(cellIndex);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(mapping.Header);
            }
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

        protected internal virtual List<CobieObject> MergeDuplicates(List<CobieObject> objects, TextWriter log)
        {
            //by default no objects are to be removed
            return new List<CobieObject>();
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
                    if (value.Trim() == ":") continue; //this happens for some automatically created category strings
                    
                    //create an instance of generic type
                    var itemType = type.GetGenericArguments()[0];
                    var item = Activator.CreateInstance(itemType);
                    addMethod.Invoke(obj, new[] {item});
                    var memberInfo = itemType.GetProperty(actual);
                    if (memberInfo == null)
                    {
                        //try to get internal member
                        memberInfo = itemType.GetProperty(actual,
                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
                        if (memberInfo == null)
                        {
                            log.WriteLine("Object {0} doesn't have a property {1}.", itemType.Name, actual);
                            continue;
                        }
                    }
                    memberInfo.SetValue(item, value.Trim());

                    //find a classification name for category
                    if (actual == "CategoryString")
                    {
                        var clsInfo = itemType.GetProperty("Classification");
                        var clsName = GetClassificationName(cell.Sheet.Workbook, value.Trim());
                        if (clsName != null)
                            clsInfo.SetValue(item, clsName);
                    }
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
                SetSimpleValue(objPropertInfo, obj, cell, log);
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

        private static string GetClassificationName(IWorkbook workbook, string code)
        {
            var pickListAttr = _mappings.FirstOrDefault(m => !String.IsNullOrEmpty(m.PickList));
            if (pickListAttr == null) return null;

            var parts = pickListAttr.PickList.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return null;

            var pickListName = parts[0];
            var sheet = workbook.GetSheet(pickListName);
            if (sheet == null) return null;

            foreach (IRow row in sheet)
            {
                foreach (ICell cell in row)
                {
                    if(cell.CellType != CellType.String || cell.StringCellValue != code) continue;
                    var index = cell.ColumnIndex;
                    var header = sheet.GetRow(0);
                    var headerCell = header.GetCell(index);
                    if (headerCell.CellType == CellType.String)
                        return headerCell.StringCellValue;
                }
            }

            return null;
        }

        private static void SetSimpleValue(PropertyInfo info, object obj, ICell cell, TextWriter log)
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
                    default:
                        log.WriteLine("There is no suitable value for {0} in cell {1}{2}, sheet {3}",
                            info.Name, CellReference.ConvertNumToColString(cell.ColumnIndex), cell.RowIndex + 1,
                            cell.Sheet.SheetName);
                        break;
                }
                info.SetValue(obj, value);
                return;
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
                    default:
                        log.WriteLine("There is no suitable value for {0} in cell {1}{2}, sheet {3}",
                            info.Name, CellReference.ConvertNumToColString(cell.ColumnIndex), cell.RowIndex + 1,
                            cell.Sheet.SheetName);
                        break;
                }
                info.SetValue(obj, date);
                return;
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
                    default:
                        log.WriteLine("There is no suitable value for {0} in cell {1}{2}, sheet {3}",
                            info.Name, CellReference.ConvertNumToColString(cell.ColumnIndex), cell.RowIndex + 1,
                            cell.Sheet.SheetName);
                        break;
                }
                return;
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
                    default:
                        log.WriteLine("There is no suitable value for {0} in cell {1}{2}, sheet {3}",
                            info.Name, CellReference.ConvertNumToColString(cell.ColumnIndex), cell.RowIndex + 1,
                            cell.Sheet.SheetName);
                        break;
                }
                return;
            }

            //enumeration
            if (type.IsEnum)
            {
                if (cell.CellType != CellType.String)
                {
                    log.WriteLine("There is no suitable value for {0} in cell {1}{2}, sheet {3}",
                        info.Name, CellReference.ConvertNumToColString(cell.ColumnIndex), cell.RowIndex + 1,
                        cell.Sheet.SheetName);
                    return;
                }
                try
                {
                    //try to use aliases
                    var enumMembers = type.GetFields();
                    foreach (var member in from member in enumMembers
                        let alias = member.GetCustomAttributes<AliasAttribute>()
                            .FirstOrDefault(
                                a =>
                                    String.Equals(a.Value, cell.StringCellValue,
                                        StringComparison.CurrentCultureIgnoreCase))
                        where alias != null
                        select member)
                    {
                        var enumObj = Activator.CreateInstance(type);
                        var enumVal = member.GetValue(enumObj);
                        info.SetValue(obj, enumVal);
                        return;
                    }

                    //if there was no alias try to parse the value
                    var val = Enum.Parse(type, cell.StringCellValue, true);
                    info.SetValue(obj, val);
                    return;
                }
                catch (Exception)
                {
                    log.WriteLine("There is no suitable value for {0} in cell {1}{2}, sheet {3}",
                        info.Name, CellReference.ConvertNumToColString(cell.ColumnIndex), cell.RowIndex + 1,
                        cell.Sheet.SheetName);
                }
            }

            //if not suitable type was found, report it as a bug
            throw new Exception("Unsupported type " + type.Name + " for value '" + cell.ToString() + "'");
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

    public class CobieValue
    {
        public double? DoubleValue;
        public int? IntegerValue;
        public string StringValue;
        public DateTime? DateTimeValue;
        public bool? BooleanValue;

        public CobieValueType ValueType;
    }

    public enum CobieValueType
    {
        Unknown,
        Double,
        Integer,
        String,
        DateTime,
        Boolean
    }
}