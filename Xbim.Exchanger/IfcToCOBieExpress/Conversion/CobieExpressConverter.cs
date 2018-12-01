using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Xbim.CobieExpress.IO;
using Xbim.CobieLiteUk;
using Xbim.Common;
using Xbim.Ifc;
using XbimExchanger.IfcHelpers;


namespace XbimExchanger.IfcToCOBieExpress.Conversion
{

    public class CobieExpressConverter : ICobieConverter
    {
        private static readonly ILogger Logger = XbimLogging.CreateLogger<CobieExpressConverter>();

        public CobieExpressConverter()
        {
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };
            Worker.DoWork += CobiekWorker;
        }



        public BackgroundWorker Worker { get; set; }

        /// <summary>
        /// Run the worker
        /// </summary>
        /// <param name="args"></param>
        public void Run(CobieConversionParams args)
        {
            Worker.RunWorkerAsync(args);
        }

        /// <summary>
        /// DOWork function for worker, generate excel COBie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CobiekWorker(object sender, DoWorkEventArgs e)
        {
            var parameters = e.Argument as CobieConversionParams;
            if (parameters == null)
            {
                const string message = "Invalid CobieConversionParams for exporter.";
                Worker.ReportProgress(0, message);
                Logger.LogError(message);
                return;
            }
            if (parameters.Source == null)
            {
                const string message = "No souce provided to exporter.";
                Worker.ReportProgress(0, message);
                Logger.LogError(message);
                return;
            }
            if (string.IsNullOrEmpty(parameters.OutputFileName))
            {
                const string message = "No output file name specified in exporter.";
                Worker.ReportProgress(0, message);
                Logger.LogError(message);
                return;
            }
            e.Result = GenerateFile(parameters); //returns the excel file names in an enumerable
        }

        /// <summary>
        /// Create XLS file from ifc/xbim files
        /// </summary>
        /// <param name="parameters">Params</param>
        private IEnumerable<string> GenerateFile(CobieConversionParams parameters)
        {
            var ret = new List<string>();
            var path = Path.GetDirectoryName(parameters.OutputFileName);
           var cobieModels = GetCobieModels(parameters);

            var index = 1;
            foreach (var facilityType in cobieModels)
            {
                var bareFileName = Path.GetFileNameWithoutExtension(parameters.OutputFileName);
                if (cobieModels.Count > 1)
                    bareFileName += "(" + index + ")";
                Worker.ReportProgress(0, string.Format("Beginning facility '{0}'", bareFileName));

                var timer = new Stopwatch();
                timer.Start();

                var fullFileName = Path.Combine(path, bareFileName);
                
                switch (parameters.ExportFormat)
                {
                    case ExportFormatEnum.XLS:
                    case ExportFormatEnum.XLSX:
                        string report = string.Empty;
                        fullFileName = CreateExcelFile(fullFileName, parameters, facilityType, out report);
                        if (!String.IsNullOrEmpty(report) && parameters.Log)
                        {
                            var logFile = Path.ChangeExtension(fullFileName, ".validation.log");
                            Worker.ReportProgress(0, string.Format("Creating validation log file: {0}", logFile));
                            File.WriteAllText(logFile, report);
                        }
                        break;
                    case ExportFormatEnum.STEP21:
                        fullFileName = CreateStepFile(fullFileName, parameters, facilityType);
                        break;
                    case ExportFormatEnum.JSON:
                       
                        //break;
                    case ExportFormatEnum.XML:
                        
                        //break;
                    case ExportFormatEnum.IFC:
                    default:
                        throw new NotImplementedException(String.Format("COBie Express does not currently support {0}", parameters.ExportFormat));
                        //break;
                }
                index++;
                timer.Stop();
                Worker.ReportProgress(0, string.Format("Time to save: {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
                ret.Add(fullFileName);
            }

            Worker.ReportProgress(0, "Finished COBie Generation");
            return ret;
        }

        private List<CobieModel> GetCobieModels(CobieConversionParams parameters)
        {
            List<CobieModel> ret = null;
            var timer = new Stopwatch();
            timer.Start();
            if (parameters.Source is string)
            {
                var sourceFile = (string)parameters.Source;
                if (!File.Exists(sourceFile))
                {
                    string message = string.Format("Source file not found {0}", sourceFile);
                    Worker.ReportProgress(0, message);
                    Logger.LogError(message);
                    return null;
                }
                var fileExt = Path.GetExtension(sourceFile);
                switch (fileExt.ToLowerInvariant())
                {
                    case ".xls":
                    case ".xlsx":
                        ret = GetCobieModelsFromExcelFilename(sourceFile, parameters.TemplateFile);
                        break;
                    case ".json":
                        ret = GetCobieModelsFromJsonFilename(sourceFile);
                        break;
                    case ".xml":
                        ret = GetCobieModelsFromXmlFilename(sourceFile);
                        break;
                    default:
                        ret = GetCobieModelsFromIfcStoreFilename(sourceFile, parameters);
                        break;
                }
            }
            else if (parameters.Source is IfcStore)
            {
                ret = GetCobieModels((IfcStore)parameters.Source, parameters);
            }

            timer.Stop();
            Worker.ReportProgress(0, string.Format("Time to generate COBieLite data: {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
            return ret;
        }
        private List<CobieModel> GetCobieModels(IfcStore model, CobieConversionParams parameters)
        {
            List<CobieModel> cobieModels = new List<CobieModel>();
            
            if (model.IsFederation == false)
            {
                var cobie = new CobieModel();
                using (var txn = cobie.BeginTransaction("begin conversion"))
                {
                    var exchanger = new IfcToCoBieExpressExchanger 
                        (model,
                        cobie,
                        Worker.ReportProgress,
                        parameters.Filter,
                        parameters.ConfigFile,
                        parameters.ExtId,
                        parameters.SysMode
                        );
                    exchanger.Convert();
                    cobieModels.Add(cobie);
                    txn.Commit();
                }
            }
            else
            {
                throw new NotImplementedException("Work to do on COBie Federated");
                //see COBieLitConverter for Lite code
            }
            return cobieModels;
        }

        private List<CobieModel> GetCobieModelsFromIfcStoreFilename(string sourceFile, CobieConversionParams parameters)
        {
            using (var model = IfcStore.Open(sourceFile))
            {
                return GetCobieModels(model, parameters);
            }
        }

        private List<CobieModel> GetCobieModelsFromXmlFilename(string sourceFile)
        {
            throw new NotImplementedException("COBie Express does not currently support reading XML files");
        }

        private List<CobieModel> GetCobieModelsFromJsonFilename(string sourceFile)
        {
            throw new NotImplementedException("COBie Express does not currently support reading JSON files");
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="templateFile"></param>
        /// <returns></returns>
        private List<CobieModel> GetCobieModelsFromExcelFilename(string sourceFile, string templateFile)
        {
            throw new NotImplementedException("COBie Express does not currently support reading excel files");
        }
        /// <summary>
        /// Generate a Excel File
        /// </summary>
        /// <param name="fileName">Root file name</param>
        /// <param name="parameters">Params</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateExcelFile(string fileName, CobieConversionParams parameters, CobieModel cobie, out string report )
        {
            //set export file name
            var excelType = (ExcelTypeEnum)Enum.Parse(typeof(ExcelTypeEnum), parameters.ExportFormat.ToString(), true);
            var excelName = Path.ChangeExtension(fileName, excelType == ExcelTypeEnum.XLS ? ".xls" : ".xlsx");
            cobie.ExportToTable(excelName, out report);
            return excelName;
        }

        /// <summary>
        /// Generate a Step File
        /// </summary>
        /// <param name="fileName">Root file name</param>
        /// <param name="parameters">Params</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateStepFile(string fileName, CobieConversionParams parameters, CobieModel cobie)
        {
            //set export file name
            var stepName = Path.ChangeExtension(fileName, ".cobie");
            cobie.SaveAsStep21(stepName);
            return stepName;
        }
    }
   
    
}
