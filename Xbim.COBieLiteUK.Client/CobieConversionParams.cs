using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using log4net;
using Xbim.FilterHelper;
using Xbim.Ifc;
using XbimExchanger.IfcToCOBieLiteUK;

namespace Xbim.COBieLiteUK.Client
{
    /// <summary>
    /// Params Class, holds parameters for worker to access
    /// </summary>
    public class CobieConversionParams
    {
        private static readonly ILog Logger = LogManager.GetLogger("Xbim.COBieLiteUK.Client");

        public List<Facility> Facilities { get; set; }
        public string OutputFileName { get; set; }
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

        public void GetFacilities(string sourceFile, ICOBieLiteConverter worker)
        {
            if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile))
            {
                Logger.Error(string.Format("Facilities source not found: {0}.", sourceFile));
                return;
            }
            
            var timer = new Stopwatch();
            timer.Start();
            var fileExt = Path.GetExtension(sourceFile);
            switch (fileExt.ToLowerInvariant())
            {
                case ".xls":
                case ".xlsx":
                    GeneratelExcelFacility(sourceFile);
                    break;
                case ".json":
                    GeneratelJsonFacility(sourceFile);
                    break;
                case ".xml":
                    GeneratelXmlFacility(sourceFile);
                    break;
                default:
                    GenerateFacilitiesFromStore(sourceFile);
                    break;
            }
            timer.Stop();
            worker.Worker.ReportProgress(0, string.Format("Time to generate COBieLite data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private void GeneratelExcelFacility(string parameters)
        {
            var facilities = new List<Facility>();
            string msg;
            var facility = Facility.ReadCobie(parameters, out msg, TemplateFile);
            if (facility != null)
            {
                facilities.Add(facility);
            }
            Facilities = facilities;
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private void GeneratelJsonFacility(string parameters)
        {
            var facilities = new List<Facility>();
            var facility = Facility.ReadJson(parameters);
            if (facility != null)
            {
                facilities.Add(facility);
            }
            Facilities = facilities;
        }

        /// <summary>
        /// Get the facility from the COBie Excel sheets
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private void GeneratelXmlFacility(string parameters)
        {
            var facilities = new List<Facility>();
            var facility = Facility.ReadXml(parameters);
            if (facility != null)
            {
                facilities.Add(facility);
            }
            Facilities = facilities;
        }

        /// <summary>
        /// Generate Facilities for a xbim or ifc type file
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private void GenerateFacilitiesFromStore(string parameters)
        {
            var facilities = new List<Facility>();
            using (var model = IfcStore.Open(parameters))
            {
                if (model.IsFederation == false)
                {
                    var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(
                        model,
                        facilities,
                        null, // Worker.ReportProgress, // todo: restore worker
                        Filter,
                        ConfigFile,
                        ExtId,
                        SysMode
                        );
                    facilities = ifcToCoBieLiteUkExchanger.Convert();
                }
                else
                {
                    var roles = model.GetFileRoles();
                    var fedFilters = Filter.SetFedModelFilter(roles);
                    var rolesFacilities = new Dictionary<RoleFilter, List<Facility>>();
                    foreach (var filter in fedFilters)
                    {
                        var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(
                            filter.Key.Model,
                            new List<Facility>(),
                            null, // Worker.ReportProgress, // todo: restore worker
                            filter.Value, 
                            ConfigFile,
                            ExtId, 
                            SysMode
                            );
                        // todo: restore worker
                        // ifcToCoBieLiteUkExchanger.ReportProgress.Progress = Worker.ReportProgress;
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
                        Facilities = facilities;
                        return;
                    }
                    var baseFacility = fedFacilities.First();
                    fedFacilities.RemoveAt(0);
                    if (Log)
                    {
                        var logfile = Path.ChangeExtension(parameters, "merge.log");
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
            Facilities =  facilities;
        }
    }
}
