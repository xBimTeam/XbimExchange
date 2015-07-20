using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Xbim.COBieLiteUK.Net40PortHelpers;

namespace Xbim.COBieLiteUK
{
    public partial class Metadata
    {
        internal void LoadFromCobie(IWorkbook workbook, TextWriter log, string version = "UK2012")
        {
            //fill in object attributes first
            var sheetName = GetSheetName(GetType(), version);
            if (sheetName == null)
            {
                log.WriteLine("There is no sheet maping for a {0}.", GetType().Name);
                return;
            }

            //try to get object sheet
            var sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                log.WriteLine("There is no {0} sheet for a {1}.", sheetName, GetType().Name);
                return;
            }

            //get mappings
            var mappingAttributes = GetMapping(GetType(), version);
            var mappings = mappingAttributes as MappingAttribute[] ?? mappingAttributes.ToArray();
            if (!mappings.Any())
            {
                log.WriteLine("There is no mapping for a {0} parameters", GetType().Name);
                return;
            }

            //fill facility values using reflection
            foreach (var mapping in mappings)
            {
                const int cellIndex = 1;
                int cellRow;
                if (!int.TryParse(mapping.Column, out cellRow))
                {
                    log.WriteLine(
                        "Metadata are expected to be defined in a single column with numbered rows. This mapping ({0}) doesn't contain row number.",
                        version);
                    continue;
                }
                cellRow--; //convert number to index

                var row = sheet.GetRow(cellRow);
                if (row == null) continue;

                var cell = row.GetCell(cellIndex);
                if (cell == null) continue;

                //use reflection to set the value if the value is available
                if (cell.CellType == CellType.Blank || cell.CellType == CellType.Error)
                    continue;

                SetMemberValue(mapping.Path, cell, log);
            }
        }

        private void SetMemberValue(string member, ICell cell, TextWriter log)
        {
            var info = GetType().GetProperty(member);
            if (info == null)
            {
                log.WriteLine("Property {0} is not defined in {1}", member, GetType().Name);
                return;
            }

            if(info.PropertyType != typeof(string))
                throw new NotImplementedException("This function was only designed to work for string properties.");

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
            info.SetValue(this, value);
        }

        internal void WriteToCobie(IWorkbook workbook, TextWriter log, string version = "UK2012")
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

            //write columns
            foreach (var mapping in mappings)
            {
            
                int rowNum;
                if (!int.TryParse(mapping.Column, out rowNum))
                {
                    log.WriteLine(
                        "Metadata are expected to be defined in a single column with numbered rows. This mapping ({0}) doesn't contain row number.",
                        version);
                    continue;
                }
                rowNum--; //convert number to index

                var row = sheet.GetRow(rowNum) ?? sheet.CreateRow(rowNum);
                
                //header cell;
                var headerCell = row.GetCell(0) ?? row.CreateCell(0);
                if(headerCell.CellType == CellType.Blank)
                    headerCell.SetCellValue(mapping.Header);

                //value cell
                var cell = row.GetCell(1) ?? row.CreateCell(1);
                var info = GetType().GetProperty(mapping.Path);
                if (info == null)
                {
                    log.WriteLine("Property {0} is not defined in {1}", mapping.Path, GetType().Name);
                    continue;
                }
                var value = (info.GetValue(this) as string) ?? "n/a";
                cell.SetCellValue(value);
            }
        }

        private static IEnumerable<MappingAttribute> GetMapping(Type type, string mapping)
        {
            var result = type.GetCustomAttributes(typeof (MappingAttribute), true)
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