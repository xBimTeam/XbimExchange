using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Common.Exceptions;
using Xbim.Common.Step21;
using Xbim.COBieLiteUK.Client;
using Xbim.FilterHelper;
using Xbim.Ifc;
using Xbim.IO;
using XbimExchanger.COBieLiteUkToIfc;
using XbimExchanger.IfcToCOBieLiteUK;

// ReSharper disable once CheckNamespace
namespace Xbim.Client
{
    // ReSharper disable once InconsistentNaming
    public class COBieLiteWorker : ICOBieLiteWorker
    {
        public BackgroundWorker Worker { get; set; }

        public COBieLiteWorker()
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
        public void Run(Params args)
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
            var parameters = e.Argument as Params;
            if (parameters == null)
                return;
            if (string.IsNullOrEmpty(parameters.ModelFile) || !File.Exists(parameters.ModelFile))
            {
                Worker.ReportProgress(0, string.Format("File doesn't exist: {0}.", parameters.ModelFile));
                return;
            }
            e.Result = GenerateFile(parameters); //returns the excel file name
        }

        /// <summary>
        /// Create XLS file from ifc/xbim files
        /// </summary>
        /// <param name="parameters">Params</param>
        private string GenerateFile(Params parameters)
        {
            var fileName = string.Empty;
            var exportType = parameters.ExportType.ToString();

            var timer = new Stopwatch();
            timer.Start();

            var facilities = GenerateFacility(parameters);
            timer.Stop();
            Worker.ReportProgress(0,
                string.Format("Time to generate COBieLite data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
            timer.Reset();
            timer.Start();
            var index = 1;
            var path = Path.GetDirectoryName(parameters.ModelFile);
            Debug.Assert(path != null);
            foreach (var facilityType in facilities)
            {
                fileName = Path.GetFileNameWithoutExtension(parameters.ModelFile) + ((facilities.Count == 1)
                    ? ""
                    : "(" + index + ")");

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
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
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
        private string CreateExcelFile(Params parameters, string fileName, Facility facility)
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

        /// <summary>
        /// Generate the Facilities held within the Model
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <returns>List of Facilities</returns>
        private List<Facility> GenerateFacility(Params parameters)
        {
            var fileExt = Path.GetExtension(parameters.ModelFile);
            if (fileExt == null)
                return Enumerable.Empty<Facility>().ToList();

            switch (fileExt.ToLowerInvariant())
            {
                case ".xls":
                case ".xlsx":
                    return GeneratelExcelFacility(parameters);
                case ".json":
                    return GeneratelJsonFacility(parameters);
                case ".xml":
                    return GeneratelXmlFacility(parameters);
                default:
                    return GenerateFileFacility(parameters, fileExt);
            }
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<Facility> GeneratelExcelFacility(Params parameters)
        {
            var facilities = new List<Facility>();
            string msg;
            var facility = Facility.ReadCobie(parameters.ModelFile, out msg, parameters.TemplateFile);
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
        private List<Facility> GeneratelJsonFacility(Params parameters)
        {
            var facilities = new List<Facility>();
            var facility = Facility.ReadJson(parameters.ModelFile);
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
        private List<Facility> GeneratelXmlFacility(Params parameters)
        {
            var facilities = new List<Facility>();
            var facility = Facility.ReadXml(parameters.ModelFile);
            if (facility != null)
            {
                facilities.Add(facility);
            }
            return facilities;
        }

        /// <summary>
        /// Generate Facilities for a xbim or ifc type file
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        private List<Facility> GenerateFileFacility(Params parameters, string fileExt)
        {
            var facilities = new List<Facility>();
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            
            using (var model = IfcStore.Open(parameters.ModelFile))
            {
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
                    var roles = model.GetFileRoles();
                    var fedFilters = parameters.Filter.SetFedModelFilter(roles);
                    var rolesFacilities = new Dictionary<RoleFilter, List<Facility>>();
                    foreach (var filter in fedFilters)
                    {
                        var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(filter.Key.Model,
                            new List<Facility>(), Worker.ReportProgress, filter.Value, parameters.ConfigFile,
                            parameters.ExtId, parameters.SysMode);
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
                        return facilities;
                    var baseFacility = fedFacilities.First();
                    fedFacilities.RemoveAt(0);
                    if (parameters.Log)
                    {
                        var logfile = Path.ChangeExtension(parameters.ModelFile, "merge.log");
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
            }
            return facilities;
        }
    }

    /// <summary>
    /// Params Class, holds parameters for worker to access
    /// </summary>
    public class Params
    {
        public string ModelFile { get; set; }
        public string TemplateFile { get; set; }
        public ExportTypeEnum ExportType { get; set; }
        public bool FlipFilter { get; set; }
        public bool OpenExcel { get; set; }
        public RoleFilter Roles { get; set; }
        public bool FilterOff { get; set; }
        public EntityIdentifierMode ExtId { get; set; }
        public SystemExtractionMode SysMode { get; set; }
        public OutPutFilters Filter { get; set; }
        public string ConfigFile { get; set; }
        public bool Log { get; set; }
    }
}