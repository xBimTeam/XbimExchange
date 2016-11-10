using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XplorerPlugin.CobieExport
{
    internal enum ExportTypeEnum
    {
        Xls,
        Xlsx,
        Json,
        Xml,
        Ifc
    }

    internal static class ExportTypeEnumExtensions
    {
        internal static ExportTypeEnum GetExcelType(this string enumValue)
        {
            ExportTypeEnum excelType;
            try
            {
                excelType = (ExportTypeEnum)Enum.Parse(typeof(ExportTypeEnum), enumValue);
            }
            catch (Exception)
            {
                throw;
            }
            return excelType;
        }
    }
}
