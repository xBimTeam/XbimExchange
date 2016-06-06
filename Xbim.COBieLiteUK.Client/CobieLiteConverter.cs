using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xbim.Ifc;
using Xbim.IO;
using XbimExchanger.COBieLiteUkToIfc;

namespace Xbim.COBieLiteUK.Client
{
    public class CobieLiteConverter : ICobieLiteConverter
    {
        public BackgroundWorker Worker { get; set; }

        public CobieLiteConverter()
        {
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };
            Worker.DoWork += CobieLiteUkWorker;
        }

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
        private void CobieLiteUkWorker(object sender, DoWorkEventArgs e)
        {
            var parameters = e.Argument as CobieConversionParams;
            if (parameters == null)
                return;
            if (string.IsNullOrEmpty(parameters.OutputFileName))
            {
                Worker.ReportProgress(0, "No output fine name specified in exporter.");
                return;
            }
            if (parameters.Facilities == null || !parameters.Facilities.Any())
            {
                Worker.ReportProgress(0, "No facilities provided to exporter.");
                return;
            }
            e.Result = GenerateFile(parameters); //returns the excel file name
        }

        /// <summary>
        /// Create XLS file from ifc/xbim files
        /// </summary>
        /// <param name="parameters">Params</param>
        private string GenerateFile(CobieConversionParams parameters)
        {
            var fileName = string.Empty;
            var exportType = parameters.ExportType.ToString();

            var timer = new Stopwatch();
            timer.Start();
            var index = 1;
            var path = Path.GetDirectoryName(parameters.OutputFileName);
            Debug.Assert(path != null);
            foreach (var facilityType in parameters.Facilities)
            {
                fileName = Path.GetFileNameWithoutExtension(
                    parameters.OutputFileName) + 
                    (parameters.Facilities.Count == 1 ? "" : "(" + index + ")"
                    );

                fileName = Path.Combine(path, fileName);
                if (parameters.Log)
                {
                    var logFile = Path.ChangeExtension(fileName, ".log");
                    Worker.ReportProgress(0, string.Format("Creating validation log file: {0}", logFile));
                    using (var log = File.CreateText(logFile))
                    {
                        facilityType.ValidateUK2012(log, false);
                    }
                }
                if ((exportType.Equals("XLS", StringComparison.OrdinalIgnoreCase)) ||
                    (exportType.Equals("XLSX", StringComparison.OrdinalIgnoreCase)))
                {
                    fileName = CreateExcelFile(parameters, fileName, facilityType);
                }
                else if (exportType.Equals("JSON", StringComparison.OrdinalIgnoreCase))
                {
                    fileName = CreateJsonFile(fileName, facilityType);
                }
                else if (exportType.Equals("XML", StringComparison.OrdinalIgnoreCase))
                {
                    fileName = CreateXmlFile(fileName, facilityType);
                }
                else if (exportType.Equals("IFC", StringComparison.OrdinalIgnoreCase))
                {
                    fileName = CreateIfcFile(fileName, facilityType);
                }
                index++;
            }
            timer.Stop();
            Worker.ReportProgress(0,
                string.Format("Time to generate = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));

            Worker.ReportProgress(0, "Finished COBie Generation");
            return fileName;
        }

        private string CreateIfcFile(string fileName, Facility facility)
        {
            var ifcName = Path.ChangeExtension(fileName, ".ifc");
            var f = new FileInfo(ifcName);
            if (f.Exists)
            {
                ifcName = Path.Combine(
                        // ReSharper disable once AssignNullToNotNullAttribute
                        f.DirectoryName,
                        Path.GetFileNameWithoutExtension(ifcName) + "(" + DateTime.Now.ToString("HH-mm-ss") + ").ifc"
                        );

            }
            var xbimFile = Path.ChangeExtension(ifcName, "xbim");
            Worker.ReportProgress(0, string.Format("Creating file: {0}", xbimFile));
            facility.ReportProgress.Progress = Worker.ReportProgress;var creds = new XbimEditorCredentials();
            var assembly = global::System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location); 
            creds.EditorsOrganisationName = fvi.CompanyName;
            creds.EditorsFamilyName = fvi.CompanyName;
            creds.ApplicationFullName = fvi.ProductName;
            creds.ApplicationVersion = fvi.ProductVersion;
            using (var ifcModel = IfcStore.Open(xbimFile,creds,true))
            {
               
                using (var txn = ifcModel.BeginTransaction("Convert from COBieLiteUK"))
                {
                    var coBieLiteUkToIfcExchanger = new CoBieLiteUkToIfcExchanger(facility, ifcModel);
                    coBieLiteUkToIfcExchanger.Convert();
                    txn.Commit();
                }
                Worker.ReportProgress(0, string.Format("Creating file: {0}", ifcName));
                ifcModel.SaveAs(ifcName, IfcStorageType.Ifc);
                ifcModel.Close();
            }
            return ifcName;
        }

        /// <summary>
        /// Generate a Excel File
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <param name="fileName">Root file name</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateExcelFile(CobieConversionParams parameters, string fileName, Facility facility)
        {
            var excelType = (ExcelTypeEnum) Enum.Parse(typeof (ExcelTypeEnum), parameters.ExportType.ToString(), true);
            var excelName = Path.ChangeExtension(fileName, excelType == ExcelTypeEnum.XLS ? ".xls" : ".xlsx");
            Worker.ReportProgress(0, string.Format("Creating file: {0}", excelName));
            using (var file = File.Create(excelName))
            {
                facility.ReportProgress.Progress = Worker.ReportProgress;
                string msg;
                facility.WriteCobie(file, excelType, out msg, parameters.Filter, parameters.TemplateFile);
            }
            //_worker.ReportProgress(0, msg); //removed for now, kill app for some reason
            return excelName;
        }

        /// <summary>
        /// Generate a JSON File
        /// </summary>
        /// <param name="fileName">Root file name</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateJsonFile(string fileName, Facility facility)
        {
            var jsonName = Path.ChangeExtension(fileName, ".json");
            Worker.ReportProgress(0, string.Format("Creating file: {0}", jsonName));
            facility.ReportProgress.Progress = Worker.ReportProgress;
            facility.WriteJson(jsonName, true);
            return jsonName;
        }

        /// <summary>
        /// Generate a XML File
        /// </summary>
        /// <param name="fileName">Root file name</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateXmlFile(string fileName, Facility facility)
        {
            var xmlName = Path.ChangeExtension(fileName, ".xml");
            Worker.ReportProgress(0, string.Format("Creating file: {0}", xmlName));
            facility.ReportProgress.Progress = Worker.ReportProgress;
            facility.WriteXml(xmlName, true);
            return xmlName;
        } 
    }
}