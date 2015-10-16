using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.Common.Exceptions;
using Xbim.FilterHelper;
using Xbim.IO;
using XbimExchanger.IfcToCOBieLiteUK;

namespace Xbim.Client
{
    public class COBieLiteWorker :ICOBieLiteWorker
    {
        /// <summary>
        /// The worker
        /// </summary>
        public BackgroundWorker Worker
        { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public COBieLiteWorker()
        {
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = false;
            Worker.DoWork += CobieLiteUKWorker;
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
        void CobieLiteUKWorker(object sender, DoWorkEventArgs e)
        {
            Params parameters = e.Argument as Params;
            if ((string.IsNullOrEmpty(parameters.ModelFile)) || (!File.Exists(parameters.ModelFile)))
            {
                Worker.ReportProgress(0, string.Format("That file doesn't exist: {0}.", parameters.ModelFile));
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
            string fileName = string.Empty;
            string exportType = parameters.ExportType.ToString();
            //string fileExt = Path.GetExtension(parameters.ModelFile);
            Stopwatch timer = new Stopwatch();
            timer.Start();

            var facilities = GenerateFacility(parameters);
            timer.Stop();
            Worker.ReportProgress(0, string.Format("Time to generate COBieLite data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
            timer.Reset();
            timer.Start();
            int index = 1;
            foreach (var facilityType in facilities)
            {
                fileName = Path.GetFileNameWithoutExtension(parameters.ModelFile) + ((facilities.Count == 1) ? "" : "(" + index.ToString() + ")");
                string path = Path.GetDirectoryName(parameters.ModelFile);
                fileName = Path.Combine(path, fileName);
                if (parameters.Log)
                {
                    string logFile = Path.ChangeExtension(fileName, ".log");
                    Worker.ReportProgress(0, string.Format("Creating validation log file: {0}", logFile));
                    using (var log = File.CreateText(logFile))
                    {
                        facilityType.ValidateUK2012(log, false);
                    }
                }
                if ((exportType.Equals("XLS", StringComparison.OrdinalIgnoreCase)) || (exportType.Equals("XLSX", StringComparison.OrdinalIgnoreCase)))
                {
                    fileName =  CreateExcelFile(parameters, fileName, facilityType);
                }
                else if (exportType.Equals("JSON", StringComparison.OrdinalIgnoreCase))
                {
                    fileName = CreateJsonFile(parameters, fileName, facilityType);
                }
                else if (exportType.Equals("XML", StringComparison.OrdinalIgnoreCase))
                {
                    fileName = CreateXmlFile(parameters, fileName, facilityType);
                }
                index++; 
            }
            timer.Stop();
            Worker.ReportProgress(0, string.Format("Time to generate COBieLite Excel = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));

            Worker.ReportProgress(0, "Finished COBie Generation");
            return fileName;
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
            ExcelTypeEnum excelType = (ExcelTypeEnum)Enum.Parse(typeof(ExcelTypeEnum), parameters.ExportType.ToString(),true);
            var excelName = Path.ChangeExtension(fileName, excelType == ExcelTypeEnum.XLS ? ".xls" : ".xlsx");
            Worker.ReportProgress(0, string.Format("Creating file: {0}", excelName));
            string msg;
            using (var file = File.Create(excelName))
            {
                facility.ReportProgress.Progress = Worker.ReportProgress;
                facility.WriteCobie(file, excelType, out msg, parameters.Filter, parameters.TemplateFile, true);
            }
            //_worker.ReportProgress(0, msg); //removed for now, kill app for some reason
            return excelName;
        }

        /// <summary>
        /// Generate a JSON File
        /// </summary>
        /// <param name="parameters">Params</param>
        /// <param name="fileName">Root file name</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateJsonFile(Params parameters, string fileName, Facility facility)
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
        /// <param name="parameters">Params</param>
        /// <param name="fileName">Root file name</param>
        /// <param name="facility">Facility</param>
        /// <returns>file name</returns>
        private string CreateXmlFile(Params parameters, string fileName, Facility facility)
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
            string fileExt = Path.GetExtension(parameters.ModelFile);

            //chsck if federated
            if (fileExt.Equals(".xbimf", StringComparison.OrdinalIgnoreCase))
            {
                return GenerateFedFacility(parameters);
            }
            if ((fileExt.Equals(".xls", StringComparison.OrdinalIgnoreCase)) || (fileExt.Equals(".xlsx", StringComparison.OrdinalIgnoreCase)))
            {
                return GeneratelExcelFacility(parameters);
            }
            if (fileExt.Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                return GeneratelJsonFacility(parameters);
            }
            if (fileExt.Equals(".xml", StringComparison.OrdinalIgnoreCase))
            {
                return GeneratelXmlFacility(parameters);
            }

            //not Federated 
            return GenerateFileFacility(parameters, fileExt);
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
            using (var model = new XbimModel())
            {
                if (fileExt.Equals(".xbim", StringComparison.OrdinalIgnoreCase))
                {
                    model.Open(parameters.ModelFile, XbimExtensions.XbimDBAccess.Read, Worker.ReportProgress);
                }
                else
                {
                    var xbimFile = Path.ChangeExtension(parameters.ModelFile, "xbim");
                    model.CreateFrom(parameters.ModelFile, xbimFile, Worker.ReportProgress, true, true);

                }
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(model, facilities, Worker.ReportProgress, parameters.Filter, parameters.ConfigFile, parameters.ExtId, parameters.SysMode);
                facilities = ifcToCoBieLiteUkExchanger.Convert();
            }
            return facilities;
        }
        /// <summary>
        /// Generate Facilities for a xbimf federated file
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<Facility> GenerateFedFacility(Params parameters)
        {
            var facilities = new List<Facility>();
            try
            {
                using (var fedModel = new FederatedModel(new FileInfo(parameters.ModelFile)))
                {
                    if (fedModel.Model.IsFederation)
                    {
                        Dictionary<XbimModel, OutPutFilters> FedFilters = parameters.Filter.SetFedModelFilter<XbimModel>(fedModel.RefModelRoles);
                        Dictionary<RoleFilter, List<Facility>> RolesFacilities = new Dictionary<RoleFilter, List<Facility>>();
                        foreach (var item in FedFilters)
                        {
                            var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(item.Key, new List<Facility>(), Worker.ReportProgress, item.Value, parameters.ConfigFile, parameters.ExtId, parameters.SysMode);
                            ifcToCoBieLiteUkExchanger.ReportProgress.Progress = Worker.ReportProgress;
                            var rolesFacility = ifcToCoBieLiteUkExchanger.Convert();

                            //facilities.AddRange(rolesFacility);
                            if (RolesFacilities.ContainsKey(item.Value.AppliedRoles))
                            {
                                RolesFacilities[item.Value.AppliedRoles].AddRange(rolesFacility);
                            }
                            else
                            {
                                RolesFacilities.Add(item.Value.AppliedRoles, rolesFacility);
                            }

                        }
                        var fedFacilities = RolesFacilities.OrderByDescending(d => d.Key.HasFlag(RoleFilter.Architectural)).SelectMany(p => p.Value).ToList(); //pull architectural roles facilities to the top
                        //fedFacilities = RolesFacilities.Values.SelectMany(f => f).OrderByDescending(f => f.AssetTypes.Count).ToList(); //pull facility with largest number of AssetTypes to the top
                        //fedFacilities = RolesFacilities.Values.SelectMany(f => f).ToList(); //flatten value lists
                        if (fedFacilities.Any())
                        {
                            Facility baseFacility = fedFacilities.First();
                            fedFacilities.RemoveAt(0);
                            if (parameters.Log)
                            {
                                var logfile = Path.ChangeExtension(parameters.ModelFile, "merge.log");
                                using (StreamWriter sw = new StreamWriter(logfile))
                                //using (StreamWriter sw = new StreamWriter(Console.OpenStandardOutput())) //to debug console **slow**
                                {
                                    sw.AutoFlush = true;
                                    foreach (Facility mergeFacility in fedFacilities)
                                    {
                                        baseFacility.Merge(mergeFacility, sw);
                                    }
                                }
                            }
                            else
                            {
                                foreach (Facility mergeFacility in fedFacilities)
                                {
                                    baseFacility.Merge(mergeFacility, null);
                                }
                            }
                            facilities.Add(baseFacility);
                        }

                    }
                    else
                    {
                        throw new XbimException(string.Format("Model is not Federated: {0}", fedModel.FileNameXbimf));
                    }
                }
            }
            catch (ArgumentException Ex) //bad paths etc..
            {
                Worker.ReportProgress(0, Ex.Message);
            }
            catch (XbimException Ex) //not federated
            {
                Worker.ReportProgress(0, Ex.Message);
            }

            return facilities;
        }


    }

    /// <summary>
    /// Params Class, holds parameters for worker to access
    /// </summary>
    public class Params
    {
        public string ModelFile
        { get; set; }
        public string TemplateFile
        { get; set; }
        public ExportTypeEnum ExportType
        { get; set; }
        public bool FlipFilter
        { get; set; }
        public bool OpenExcel
        { get; set; }
        public RoleFilter Roles
        { get; set; }
        public bool FilterOff
        { get; set; }
        public EntityIdentifierMode ExtId
        { get; set; }
        public SystemExtractionMode SysMode
        { get; set; }
        public OutPutFilters Filter
        { get; set; }
        public string ConfigFile
        { get; set; }
        public bool Log
        { get; set; }
    }
}
