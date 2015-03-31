using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using Xbim.COBieLiteUK;
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

            var summaryPage = workBook.CreateSheet("Summary");

            if (!CreateSummarySheet(summaryPage, facility)) return false;
            //add in here code to generate other sheets
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

        private bool CreateSummarySheet(ISheet summaryPage, Facility facility)
        {
            try
            {
                
                var excelRow = summaryPage.GetRow(0) ?? summaryPage.CreateRow(0);  
                var excelCell = excelRow.GetCell(0) ?? excelRow.CreateCell(0);
                excelCell.SetCellValue("Validation Report Summary");
                excelRow = summaryPage.GetRow(3) ?? summaryPage.CreateRow(3);

                var summaryReport = new AssetTypeSummaryReport(facility.AssetTypes);
                //write the asset report summary
                //foreach (var VARIABLE in COLLECTION)
                //{
                    
                //}
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
