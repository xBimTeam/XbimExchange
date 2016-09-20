using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using log4net;
using Xbim.Common.Step21;
using Xbim.CobieLiteUk;
using Xbim.CobieLiteUk.FilterHelper;
using Xbim.Ifc;
using Xbim.IO;
using XbimExchanger.COBieLiteUkToIfc;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.IfcToCOBieLiteUK.Conversion
{
    public class CobieLiteConverter : ICobieConverter
    {
        private static readonly ILog Logger = LogManager.GetLogger("Xbim.COBieLiteUK.Client.CobieLiteConverter");

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
            {
                const string message = "Invalid CobieConversionParams for exporter.";
                Worker.ReportProgress(0, message);
                Logger.Error(message);
                return;
            }
            if (parameters.Source == null)
            {
                const string message = "No souce provided to exporter.";
                Worker.ReportProgress(0, message);
                Logger.Error(message);
                return;
            }
            if (string.IsNullOrEmpty(parameters.OutputFileName))
            {
                const string message = "No output file name specified in exporter.";
                Worker.ReportProgress(0, message);
                Logger.Error(message);
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
            Debug.Assert(path != null);
            var facilities = GetFacilities(parameters);
            
            var index = 1;
            foreach (var facilityType in facilities)
            {
                var bareFileName = Path.GetFileNameWithoutExtension(parameters.OutputFileName);
                if (facilities.Count > 1)
                    bareFileName += "(" + index + ")";
                Worker.ReportProgress(0, string.Format("Beginning facility '{0}'", bareFileName));

                var timer = new Stopwatch();
                timer.Start();

                var fullFileName = Path.Combine(path, bareFileName);
                if (parameters.Log)
                {
                    var logFile = Path.ChangeExtension(fullFileName, ".validation.log");
                    Worker.ReportProgress(0, string.Format("Creating validation log file: {0}", logFile));
                    using (var log = File.CreateText(logFile))
                    {
                        facilityType.ValidateUK2012(log, false);
                    }

                    Worker.ReportProgress(0, string.Format("Time to validate: {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
                    timer.Restart();
                }
                switch (parameters.ExportFormat)
                {
                    case ExportFormatEnum.XLS:
                    case ExportFormatEnum.XLSX:
                        fullFileName = CreateExcelFile(fullFileName, parameters, facilityType);
                        break;
                    case ExportFormatEnum.JSON:
                        fullFileName = CreateJsonFile(fullFileName, facilityType);
                        break;
                    case ExportFormatEnum.XML:
                        fullFileName = CreateXmlFile(fullFileName, facilityType);
                        break;
                    case ExportFormatEnum.IFC:
                    case ExportFormatEnum.STEP21:
                        throw new NotSupportedException("COBie lite does not have a STEP21 option");
                    default:
                        fullFileName = CreateIfcFile(fullFileName, facilityType);
                        break;
                }
                index++;
                timer.Stop();
                Worker.ReportProgress(0, string.Format("Time to save: {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
                ret.Add(fullFileName);
            }
            
            

            Worker.ReportProgress(0, "Finished COBie Generation");
            return ret;
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
            using (var ifcModel = IfcStore.Create(IfcSchemaVersion.Ifc2X3, XbimStoreType.InMemoryModel))
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
        /// <param name="fileName">Root file name</param>
        /// <param name="parameters">Params</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateExcelFile(string fileName, CobieConversionParams parameters, Facility facility)
        {
            var excelType = (ExcelTypeEnum) Enum.Parse(typeof (ExcelTypeEnum), parameters.ExportFormat.ToString(), true);
            var excelName = Path.ChangeExtension(fileName, excelType == ExcelTypeEnum.XLS ? ".xls" : ".xlsx");
            // Worker.ReportProgress(0, string.Format("Creating file: {0}", excelName));
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

        private List<Facility> GetFacilities(CobieConversionParams parameters)
        {
            List<Facility> ret = null;
            var timer = new Stopwatch();
            timer.Start();
            if (parameters.Source is string)
            {
                var sourceFile = (string) parameters.Source;
                if (!File.Exists(sourceFile))
                {
                    string message =  string.Format("Source file not found {0}", sourceFile);
                    Worker.ReportProgress(0, message);
                    Logger.Error(message);
                    return null;
                }
                var fileExt = Path.GetExtension(sourceFile);
                switch (fileExt.ToLowerInvariant())
                {
                    case ".xls":
                    case ".xlsx":
                        ret = GetFacilitiesFromExcelFilename(sourceFile, parameters.TemplateFile);
                        break;
                    case ".json":
                        ret = GetFacilitiesFromJsonFilename(sourceFile);
                        break;
                    case ".xml":
                        ret = GetFacilitiesFromXmlFilename(sourceFile);
                        break;
                    default:
                        ret = GetFacilitiesFromIfcStoreFilename(sourceFile, parameters);
                        break;
                }
            }
            else if (parameters.Source is IfcStore)
            {
                ret = GetFacilities((IfcStore)parameters.Source, parameters);
            }

            timer.Stop();
            Worker.ReportProgress(0, string.Format("Time to generate COBieLite data: {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
            return ret;
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="templateFile"></param>
        /// <returns></returns>
        private List<Facility> GetFacilitiesFromExcelFilename(string parameters, string templateFile)
        {
            var facilities = new List<Facility>();
            string msg;
            var facility = Facility.ReadCobie(parameters, out msg, templateFile);
            if (facility != null)
            {
                facilities.Add(facility);
            }
            return facilities;
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<Facility> GetFacilitiesFromJsonFilename(string parameters)
        {
            var facilities = new List<Facility>();
            var facility = Facility.ReadJson(parameters);
            if (facility != null)
            {
                facilities.Add(facility);
            }
            return facilities;
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<Facility> GetFacilitiesFromXmlFilename(string parameters)
        {
            var facilities = new List<Facility>();
            var facility = Facility.ReadXml(parameters);
            if (facility != null)
            {
                facilities.Add(facility);
            }
            return facilities;
        }

        /// <summary>
        /// Generate Facilities for a xbim or ifc type file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<Facility> GetFacilitiesFromIfcStoreFilename(string sourceFile, CobieConversionParams parameters)
        {
            using (var model = IfcStore.Open(sourceFile))
            {
                var facilities = GetFacilities(model, parameters);
                return facilities;
            }
        }

        private List<Facility> GetFacilities(IfcStore model, CobieConversionParams parameters)
        {
            var facilities = new List<Facility>();
            if (model.IsFederation == false)
            {
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(
                    model,
                    facilities,
                    Worker.ReportProgress, 
                    parameters.Filter,
                    parameters.ConfigFile,
                    parameters.ExtId,
                    parameters.SysMode
                    );
                facilities = ifcToCoBieLiteUkExchanger.Convert();
            }
            else
            {
                var roles = model.GetFederatedFileRoles();
                var fedFilters = parameters.Filter.SetFedModelFilter(roles);
                var rolesFacilities = new Dictionary<RoleFilter, List<Facility>>();
                foreach (var filter in fedFilters)
                {
                    var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(
                        filter.Key.Model,
                        new List<Facility>(),
                         Worker.ReportProgress, 
                        filter.Value,
                        parameters.ConfigFile,
                        parameters.ExtId,
                        parameters.SysMode
                        );
                    ifcToCoBieLiteUkExchanger.ReportProgress.Progress = Worker.ReportProgress;
                    var rolesFacility = ifcToCoBieLiteUkExchanger.Convert();

                    //facilities.AddRange(rolesFacility);
                    if (rolesFacilities.ContainsKey(filter.Value.AppliedRoles))
                    {
                        rolesFacilities[filter.Value.AppliedRoles].AddRange(rolesFacility);
                    }
                    else
                    {
                        rolesFacilities.Add(filter.Value.AppliedRoles, rolesFacility);
                    }
                }
                var fedFacilities =
                    rolesFacilities.OrderByDescending(d => d.Key.HasFlag(RoleFilter.Architectural))
                        .SelectMany(p => p.Value)
                        .ToList();
                if (!fedFacilities.Any())
                {
                    return facilities;
                }
                var baseFacility = fedFacilities.First();
                fedFacilities.RemoveAt(0);
                if (parameters.Log)
                {
                    var logfile = Path.ChangeExtension(model.FileName, "merge.log");
                    using (var sw = new StreamWriter(logfile))
                    {
                        sw.AutoFlush = true;
                        foreach (var mergeFacility in fedFacilities)
                        {
                            baseFacility.Merge(mergeFacility, sw);
                        }
                    }
                }
                else
                {
                    foreach (var mergeFacility in fedFacilities)
                    {
                        baseFacility.Merge(mergeFacility);
                    }
                }
                facilities.Add(baseFacility);
            }
            return facilities;
        }
    }
}