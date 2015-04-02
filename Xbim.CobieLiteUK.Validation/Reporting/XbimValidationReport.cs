using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;
using Xbim.Common.Logging;

namespace Xbim.CobieLiteUK.Validation.Reporting
{

    public class XbimValidationReport
    {
        internal static readonly ILogger Logger = LoggerFactory.GetLogger();

        public enum SpreadSheetFormat
        {
            Xls,
            Xlsx
        }
        private List<ISheet> _assetReports = new List<ISheet>(); 
     
        public bool Create(Facility facility, string reportFilename, SpreadSheetFormat format)
        {
            string ssFileName;
            IWorkbook workBook;
            if (format == SpreadSheetFormat.Xlsx)
            {
                ssFileName = Path.ChangeExtension(reportFilename, "xlsx");
                workBook = new XSSFWorkbook();             
            }
            else
            {
                ssFileName = Path.ChangeExtension(reportFilename, "xls");
                workBook = new HSSFWorkbook();
            }

            // a(workBook);

            var summaryPage = workBook.CreateSheet("Summary");

            if (!CreateSummarySheet(summaryPage, facility)) 
                return false;
            var iRunningWorkBook = 1;
            foreach (var assetType in facility.AssetTypes)
            {
                // only report items with any assets submitted (a different report should be probablt be provided otherwise)
                if (assetType.GetSubmittedAssetsCount() < 1)
                    continue;

                var validName = WorkbookUtil.CreateSafeSheetName(string.Format(@"{0} {1}", iRunningWorkBook++, assetType.Name));

                var detailPage = workBook.CreateSheet(validName);
                if (!CreateDetailSheet(detailPage, assetType))
                    return false;
            }

            try
            {
                using (var spreadsheetFile = new FileStream(ssFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    workBook.Write(spreadsheetFile);
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Failed to save {0}, {1}", ssFileName,e.Message);
                return false;
            }

            return true;
        }

        private static bool CreateDetailSheet(ISheet detailSheet, AssetType assetType)
        {
            try
            {
                var excelRow = detailSheet.GetRow(0) ?? detailSheet.CreateRow(0);
                var excelCell = excelRow.GetCell(0) ?? excelRow.CreateCell(0);
                excelCell.SetCellValue("Asset type report");

                var rep = new AssetTypeDetailedGridReport(assetType);
                rep.PrepareReport();

                var iRunningRow = 2;
                var iRunningColumn = 0;


                var cellStyle = detailSheet.Workbook.CreateCellStyle();
                cellStyle.BorderBottom = BorderStyle.Thick;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillForegroundColor = IndexedColors.Grey50Percent.Index;

                var table = rep.AttributesGrid;

                var summaryRow = detailSheet.GetRow(iRunningRow) ?? detailSheet.CreateRow(iRunningRow);
                foreach (DataColumn tCol in table.Columns)
                {
                    if (tCol.AutoIncrement)
                        continue;
                    var runCell = summaryRow.GetCell(iRunningColumn) ?? summaryRow.CreateCell(iRunningColumn);
                    iRunningColumn++;
                    runCell.SetCellValue(tCol.ColumnName);
                    runCell.CellStyle = cellStyle;
                }
                iRunningRow++;

                foreach (DataRow row in table.Rows)
                {
                    iRunningColumn = 0;
                    excelRow = detailSheet.GetRow(iRunningRow) ?? detailSheet.CreateRow(iRunningRow);
                    iRunningRow++;

                    foreach (DataColumn tCol in table.Columns)
                    {
                        if (tCol.AutoIncrement || row[tCol] == DBNull.Value)
                            continue;
                        var runCell = excelRow.GetCell(iRunningColumn) ?? excelRow.CreateCell(iRunningColumn);
                        iRunningColumn++;

                        switch (tCol.DataType.Name)
                        {
                            case "String":
                                runCell.SetCellValue((string) row[tCol]);
                                break;
                            case "Int32":
                                runCell.SetCellValue(Convert.ToInt32(row[tCol]));
                                break;
                            default:
                                runCell.SetCellValue((string) row[tCol]);
                                break;
                        }
                    }
                }

                // sets all used columns to autosize
                for (int irun = 0; irun < iRunningColumn; irun++)
                {
                    detailSheet.AutoSizeColumn(irun);
                }

                return true;
            }
            catch (Exception e)
            {
                //log the error
                Logger.Error("Failed to create detail Sheet");
                return false;
            }
        }

        private static bool CreateSummarySheet(ISheet summaryPage, Facility facility)
        {
            try
            {
                
                var excelRow = summaryPage.GetRow(0) ?? summaryPage.CreateRow(0);  
                var excelCell = excelRow.GetCell(0) ?? excelRow.CreateCell(0);
                excelCell.SetCellValue("Validation Report Summary");
                
                var summaryReport = new AssetTypeSummaryReport(facility.AssetTypes);
                var table = summaryReport.GetReport();

                var iRunningRow = 2;
                var iRunningColumn = 0;

                var cellStyle = summaryPage.Workbook.CreateCellStyle();
                cellStyle.BorderBottom = BorderStyle.Thick;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillForegroundColor = IndexedColors.Grey50Percent.Index;

                var failCellStyle = summaryPage.Workbook.CreateCellStyle();
                failCellStyle.FillPattern = FillPattern.SolidForeground;
                failCellStyle.FillForegroundColor = IndexedColors.Red.Index;

                var summaryRow = summaryPage.GetRow(iRunningRow) ?? summaryPage.CreateRow(iRunningRow);
                foreach (DataColumn tCol in table.Columns)
                {
                    if (tCol.AutoIncrement)
                        continue;
                    var runCell = summaryRow.GetCell(iRunningColumn) ?? summaryRow.CreateCell(iRunningColumn);
                    iRunningColumn++;
                    runCell.SetCellValue(tCol.ColumnName);
                    runCell.CellStyle = cellStyle;
                }
                

                iRunningRow++;
                foreach (DataRow row in table.Rows)
                {
                    iRunningColumn = 0;
                    summaryRow = summaryPage.GetRow(iRunningRow) ?? summaryPage.CreateRow(iRunningRow);
                    iRunningRow++;
                    
                    foreach (DataColumn tCol in table.Columns)
                    {
                        if (tCol.AutoIncrement)
                            continue;
                        var runCell = summaryRow.GetCell(iRunningColumn) ?? summaryRow.CreateCell(iRunningColumn);
                        iRunningColumn++;

                        switch (tCol.DataType.Name)
                        {
                            case "String":
                                runCell.SetCellValue((string)row[tCol]);
                                break;
                            case "Int32":
                                runCell.SetCellValue(Convert.ToInt32(row[tCol]));
                                break;
                            default:
                                runCell.SetCellValue((string)row[tCol]);
                                break;
                        }
                    }
                }

                // sets all used columns to autosize
                for (int irun = 0; irun < iRunningColumn; irun++)
                {
                    summaryPage.AutoSizeColumn(irun);
                }

                return true;
            }
            catch (Exception e)
            {
                //log the error
                Logger.Error("Failed to create Summary Sheet");
                return false;
            }
        }
    }
}
