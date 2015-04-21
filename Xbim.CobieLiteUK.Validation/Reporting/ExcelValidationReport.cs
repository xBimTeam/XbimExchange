using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Xbim.COBieLiteUK;
using Xbim.Common.Logging;
using System.Collections.Generic;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    /// <summary>
    /// Can create an Excel report containing summary and detailed reports on provided and missing information.
    /// Use the Create function to produce the report.
    /// </summary>
    public class ExcelValidationReport
    {
        internal static readonly ILogger Logger = LoggerFactory.GetLogger();

        /// <summary>
        /// Determines the format to be saved.
        /// </summary>
        public enum SpreadSheetFormat
        {
            /// <summary>
            /// Excel Binary File Format
            /// </summary>
            Xls,
            /// <summary>
            /// Excel xml File Format
            /// </summary>
            Xlsx
        }

        /// <summary>
        /// Creates the report in file format
        /// </summary>
        /// <param name="facility">the result of a DPoW validation to be transformed into report form.</param>
        /// <param name="suggestedFilename">target file for the spreadsheet (warning, the extension is automatically determined depending on the format)</param>
        /// <param name="format">determines the excel format to use</param>
        /// <returns>true if successful, errors are cought and passed to Logger</returns>
        public bool Create(Facility facility, string suggestedFilename, SpreadSheetFormat format)
        {
            var ssFileName = Path.ChangeExtension(suggestedFilename, format == SpreadSheetFormat.Xlsx ? "xlsx" : "xls");
            if (File.Exists(ssFileName))
            {
                File.Delete(ssFileName);
            }
            try
            {
                using (var spreadsheetStream = new FileStream(ssFileName, FileMode.Create, FileAccess.Write))
                {
                    return Create(facility, spreadsheetStream, format);
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Failed to save {0}, {1}", ssFileName, e.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates the report.
        /// </summary>
        /// <param name="facility">the result of a DPoW validation to be transformed into report form.</param>
        /// <param name="filename">target file for the spreadsheet</param>
        /// <returns>true if successful, errors are cought and passed to Logger</returns>
        public bool Create(Facility facility, String filename)
        {
            if (filename == null)
                return false;
            SpreadSheetFormat format;
            var ext = Path.GetExtension(filename).ToLowerInvariant();
            if (ext != "xlsx")
                format = SpreadSheetFormat.Xlsx;
            else if (ext != "xls")
                format = SpreadSheetFormat.Xls;
            else
                return false;
            return Create(facility, filename, format);
        }

        /// <summary>
        /// Creates the report.
        /// </summary>
        /// <param name="facility">the result of a DPoW validation to be transformed into report form.</param>
        /// <param name="destinationStream">target stream for the spreadsheet</param>
        /// <param name="format">determines the excel format to use</param>
        /// <returns>true if successful, errors are cought and passed to Logger</returns>
        public bool Create(Facility facility, Stream destinationStream, SpreadSheetFormat format)
        {
            var workBook = format == SpreadSheetFormat.Xlsx
                // ReSharper disable once RedundantCast
                ? (IWorkbook)new XSSFWorkbook()
                // ReSharper disable once RedundantCast
                : (IWorkbook)new HSSFWorkbook();
            

            var facReport = new FacilityReport(facility);

            
            var summaryPage = workBook.CreateSheet("Summary");
            if (!CreateSummarySheet(summaryPage, facility)) 
                return false;
            
            // reports on Documents
            //
            var documentsPage = workBook.CreateSheet("Documents");
            if (!CreateDocumentDetailsSheet(documentsPage, facility.Documents))
                return false;


            var iRunningWorkBook = 1;
            // reports on AssetTypes details
            //
            // ReSharper disable once LoopCanBeConvertedToQuery // might restore once code is stable
            foreach (var assetType in facReport.AssetRequirementGroups)
            {
                // only report items with any assets submitted (a different report should probably be provided otherwise)

                if (assetType.GetSubmittedAssetsCount() < 1)
                    continue;
                var firstOrDefault = assetType.RequirementCategories.FirstOrDefault(cat => cat.Classification == @"Uniclass2015");
                if (firstOrDefault != null)
                {
                    var tName = firstOrDefault.Code;
                    var validName = WorkbookUtil.CreateSafeSheetName(string.Format(@"{0} {1}", iRunningWorkBook++, tName));

                    var detailPage = workBook.CreateSheet(validName);
                    if (!CreateDetailSheet(detailPage, assetType))
                        return false;
                }
            }

            // reports on Zones details

            // ReSharper disable once LoopCanBeConvertedToQuery // might restore once code is stable
            foreach (var zoneGroup in facReport.ZoneRequirementGroups)
            {
                // only report items with any assets submitted (a different report should probably be provided otherwise)
                if (zoneGroup.GetSubmittedAssetsCount() < 1)
                    continue;
                var firstOrDefault = zoneGroup.RequirementCategories.FirstOrDefault(cat => cat.Classification == @"Uniclass2015");
                if (firstOrDefault != null)
                {
                    var tName = firstOrDefault.Code;
                    var validName = WorkbookUtil.CreateSafeSheetName(string.Format(@"{0} {1}", iRunningWorkBook++, tName));

                    var detailPage = workBook.CreateSheet(validName);
                    if (!CreateDetailSheet(detailPage, zoneGroup))
                        return false;
                }
            }
            try
            {
                workBook.Write(destinationStream);
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Failed to stream excel report: {1}", e.Message);
                return false;
            }
            return true;
        }

        private bool CreateDocumentDetailsSheet(ISheet documentPage, List<Document> list)
        {

            try
            {
                var excelRow = documentPage.GetRow(0) ?? documentPage.CreateRow(0);
                var excelCell = excelRow.GetCell(0) ?? excelRow.CreateCell(0);
                SetHeader(excelCell);
                excelCell.SetCellValue("Documents Report");
                var iRunningRow = 2;

                var drep = new DocumentsDetailedReport(list);
                iRunningRow = WriteReportToPage(documentPage, drep.AttributesGrid , iRunningRow, false);
                
                Debug.WriteLine(iRunningRow);
                return true;
            }
            catch (Exception e)
            {
                //log the error
                Logger.Error("Failed to create Summary Sheet", e);
                return false;
            }
        }

        private static bool CreateDetailSheet(ISheet detailSheet, TwoLevelRequirementPointer<AssetType, Asset> requirementPointer)
        {
            try
            {
                var excelRow = detailSheet.GetRow(0) ?? detailSheet.CreateRow(0);
                var excelCell = excelRow.GetCell(0) ?? excelRow.CreateCell(0);
                SetHeader(excelCell);
                excelCell.SetCellValue("Requirement report");

                var rep = new TwoLevelDetailedGridReport<AssetType, Asset>(requirementPointer);
                rep.PrepareReport();

                var iRunningRow = 2;
                var iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"Name:"); // writes cell and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(requirementPointer.Name); // writes cell and moves index forward

                iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"External system:"); // writes cell and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(requirementPointer.ExternalSystem); // writes cell and moves index forward

                iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"External id:"); // writes cell and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(requirementPointer.ExternalId); // writes cell and moves index forward

                iRunningRow++; // one empty row

                iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"Matching categories:"); // writes cell and moves index forward

                foreach (var cat in rep.RequirementCategories)
                {
                    iRunningColumn = 0;
                    excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                    (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(cat.Classification); // writes cell and moves index forward
                    (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(cat.Code); // writes cell and moves index forward
                    (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(cat.Description); // writes cell and moves index forward
                }

                iRunningRow++; // one empty row
                iRunningColumn = 0;

                var cellStyle = detailSheet.Workbook.CreateCellStyle();
                cellStyle.BorderBottom = BorderStyle.Thick;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillForegroundColor = IndexedColors.Grey50Percent.Index;

                var table = rep.AttributesGrid;

                excelRow = detailSheet.GetRow(iRunningRow) ?? detailSheet.CreateRow(iRunningRow);
                foreach (DataColumn tCol in table.Columns)
                {
                    if (tCol.AutoIncrement)
                        continue;
                    excelCell = excelRow.GetCell(iRunningColumn) ?? excelRow.CreateCell(iRunningColumn);
                    iRunningColumn++;
                    excelCell.SetCellValue(tCol.Caption);
                    excelCell.CellStyle = cellStyle;
                }
                iRunningRow++;

                var writer = new ExcelCellVisualValue(BorderStyle.Thin);
                foreach (DataRow row in table.Rows)
                {
                    excelRow = detailSheet.GetRow(iRunningRow) ?? detailSheet.CreateRow(iRunningRow);
                    iRunningRow++;

                    iRunningColumn = -1;
                    foreach (DataColumn tCol in table.Columns)
                    {
                        if (tCol.AutoIncrement)
                            continue;
                        iRunningColumn++;
                        if (row[tCol] == DBNull.Value)
                            continue;
                        excelCell = excelRow.GetCell(iRunningColumn) ?? excelRow.CreateCell(iRunningColumn);

                        // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                        if (row[tCol] is IVisualValue)
                        {
                            writer.SetCell(excelCell, (IVisualValue)row[tCol]);
                        }
                        else
                        {
                            switch (tCol.DataType.Name)
                            {
                                case "String":
                                    excelCell.SetCellValue((string)row[tCol]);
                                    break;
                                case "Int32":
                                    excelCell.SetCellValue(Convert.ToInt32(row[tCol]));
                                    break;
                                default:
                                    excelCell.SetCellValue((string)row[tCol]);
                                    break;
                            }
                        }
                    }
                }

                //// sets all used columns to autosize
                //for (var irun = 0; irun < iRunningColumn; irun++)
                //{
                //    detailSheet.AutoSizeColumn(irun);
                //}

                return true;
            }
            catch (Exception e)
            {
                //log the error
                Logger.Error("Failed to create detail Sheet", e);
                return false;
            }
        }

        private static bool CreateDetailSheet(ISheet detailSheet, TwoLevelRequirementPointer<Zone, Space> requirementPointer)
        {
            try
            {
                var excelRow = detailSheet.GetRow(0) ?? detailSheet.CreateRow(0);
                var excelCell = excelRow.GetCell(0) ?? excelRow.CreateCell(0);
                SetHeader(excelCell);
                excelCell.SetCellValue("Asset type requirement report");

                var rep = new TwoLevelDetailedGridReport<Zone, Space>(requirementPointer);
                rep.PrepareReport();


                var iRunningRow = 2;
                var iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"Name:"); // writes cell and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(requirementPointer.Name); // writes cell and moves index forward

                iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"External system:"); // writes cell and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(requirementPointer.ExternalSystem); // writes cell and moves index forward

                iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"External id:"); // writes cell and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(requirementPointer.ExternalId); // writes cell and moves index forward

                iRunningRow++; // one empty row

                iRunningColumn = 0;
                excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(@"Matching categories:"); // writes cell and moves index forward

                foreach (var cat in rep.RequirementCategories)
                {
                    iRunningColumn = 0;
                    excelRow = detailSheet.GetRow(iRunningRow++) ?? detailSheet.CreateRow(iRunningRow - 1); // prepares a row and moves index forward
                    (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(cat.Classification); // writes cell and moves index forward
                    (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(cat.Code); // writes cell and moves index forward
                    (excelRow.GetCell(iRunningColumn++) ?? excelRow.CreateCell(iRunningColumn - 1)).SetCellValue(cat.Description); // writes cell and moves index forward
                }

                iRunningRow++; // one empty row
                iRunningColumn = 0;

                var cellStyle = detailSheet.Workbook.CreateCellStyle();
                cellStyle.BorderBottom = BorderStyle.Thick;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillForegroundColor = IndexedColors.Grey50Percent.Index;

                var table = rep.AttributesGrid;

                excelRow = detailSheet.GetRow(iRunningRow) ?? detailSheet.CreateRow(iRunningRow);
                foreach (DataColumn tCol in table.Columns)
                {
                    if (tCol.AutoIncrement)
                        continue;
                    excelCell = excelRow.GetCell(iRunningColumn) ?? excelRow.CreateCell(iRunningColumn);
                    iRunningColumn++;
                    excelCell.SetCellValue(tCol.Caption);
                    excelCell.CellStyle = cellStyle;
                }
                iRunningRow++;

                var writer = new ExcelCellVisualValue(BorderStyle.Thin);
                foreach (DataRow row in table.Rows)
                {
                    excelRow = detailSheet.GetRow(iRunningRow) ?? detailSheet.CreateRow(iRunningRow);
                    iRunningRow++;

                    iRunningColumn = -1;
                    foreach (DataColumn tCol in table.Columns)
                    {
                        if (tCol.AutoIncrement)
                            continue;
                        iRunningColumn++;
                        if (row[tCol] == DBNull.Value)
                            continue;
                        excelCell = excelRow.GetCell(iRunningColumn) ?? excelRow.CreateCell(iRunningColumn);

                        // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                        if (row[tCol] is IVisualValue)
                        {
                            writer.SetCell(excelCell, (IVisualValue)row[tCol]);
                        }
                        else
                        {
                            switch (tCol.DataType.Name)
                            {
                                case "String":
                                    excelCell.SetCellValue((string)row[tCol]);
                                    break;
                                case "Int32":
                                    excelCell.SetCellValue(Convert.ToInt32(row[tCol]));
                                    break;
                                default:
                                    excelCell.SetCellValue((string)row[tCol]);
                                    break;
                            }
                        }
                    }
                }

                //// sets all used columns to autosize
                //for (var irun = 0; irun < iRunningColumn; irun++)
                //{
                //    detailSheet.AutoSizeColumn(irun);
                //}

                return true;
            }
            catch (Exception e)
            {
                //log the error
                Logger.Error("Failed to create detail Sheet", e);
                return false;
            }
        }


        /// <summary>
        /// sets the Classification preferred for priority purposes.
        /// </summary>
        public string PreferredClassification = "Uniclass2015";

        private bool CreateSummarySheet(ISheet summaryPage, Facility facility)
        {
            try
            {
                var excelRow = summaryPage.GetRow(0) ?? summaryPage.CreateRow(0);  
                var excelCell = excelRow.GetCell(0) ?? excelRow.CreateCell(0);
                SetHeader(excelCell);
                excelCell.SetCellValue("Validation Report Summary");
                var iRunningRow = 2;
                
                var assetTypesReport = new GroupingObjectSummaryReport<CobieObject>(facility.AssetTypes);
                iRunningRow = WriteReportToPage(summaryPage, assetTypesReport.GetReport(PreferredClassification), iRunningRow);

                var zonesReport = new GroupingObjectSummaryReport<CobieObject>(facility.Zones);
                iRunningRow = WriteReportToPage(summaryPage, zonesReport.GetReport(PreferredClassification), iRunningRow);

                var docReport = new DocumentsReport(facility.Documents);
                iRunningRow = WriteReportToPage(summaryPage, docReport.GetReport("ResponsibleRole"), iRunningRow);

                Debug.WriteLine(iRunningRow);
                return true;
            }
            catch (Exception e)
            {
                //log the error
                Logger.Error("Failed to create Summary Sheet", e);
                return false;
            }
        }

        private int WriteReportToPage(ISheet summaryPage, DataTable table, int startingRow, Boolean autoSize = true)
        {
            if (table == null)
                return startingRow;
            
            var iRunningColumn = 0;

            var cellStyle = summaryPage.Workbook.CreateCellStyle();
            cellStyle.BorderBottom = BorderStyle.Thick;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.FillForegroundColor = IndexedColors.Grey50Percent.Index;

            var failCellStyle = summaryPage.Workbook.CreateCellStyle();
            failCellStyle.FillPattern = FillPattern.SolidForeground;
            failCellStyle.FillForegroundColor = IndexedColors.Red.Index;

            IRow excelRow = summaryPage.GetRow(startingRow) ?? summaryPage.CreateRow(startingRow);
            ICell excelCell;
            foreach (DataColumn tCol in table.Columns)
            {
                if (tCol.AutoIncrement)
                    continue;
                var runCell = excelRow.GetCell(iRunningColumn) ?? excelRow.CreateCell(iRunningColumn);
                iRunningColumn++;
                runCell.SetCellValue(tCol.Caption);
                runCell.CellStyle = cellStyle;
            }

            startingRow++;
            var writer = new ExcelCellVisualValue(BorderStyle.Thin);
            foreach (DataRow row in table.Rows)
            {
                excelRow = summaryPage.GetRow(startingRow) ?? summaryPage.CreateRow(startingRow);
                startingRow++;
                iRunningColumn = -1;
                foreach (DataColumn tCol in table.Columns)
                {
                    if (tCol.AutoIncrement)
                        continue;
                    iRunningColumn++;
                    if (row[tCol] == DBNull.Value)
                        continue;
                    excelCell = excelRow.GetCell(iRunningColumn) ?? excelRow.CreateCell(iRunningColumn);

                    if (row[tCol] is IVisualValue)
                    {
                        writer.SetCell(excelCell, (IVisualValue) row[tCol]);
                    }
                    else
                    {
                        switch (tCol.DataType.Name)
                        {
                            case "String":
                                excelCell.SetCellValue((string) row[tCol]);
                                break;
                            case "Int32":
                                excelCell.SetCellValue(Convert.ToInt32(row[tCol]));
                                break;
                            default:
                                excelCell.SetCellValue((string) row[tCol]);
                                break;
                        }
                    }
                }
            }

            if (autoSize)
            {
                // sets all used columns to autosize
                for (int irun = 0; irun < iRunningColumn; irun++)
                {
                    summaryPage.AutoSizeColumn(irun);
                }
            }
            return startingRow + 1;
        }

        private static void SetHeader(ICell excelCell)
        {
            var font = excelCell.Sheet.Workbook.CreateFont();
            font.FontHeightInPoints = 14;
            font.Boldweight = (short) FontBoldWeight.Bold;
            excelCell.CellStyle = excelCell.Sheet.Workbook.CreateCellStyle();
            excelCell.CellStyle.SetFont(font);
        }
    }
}
