using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.FilterHelper;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public class IfcToCOBieLiteUkExchanger : XbimExchanger<XbimModel, List<Facility> >
    {
        private bool classify = false;
        internal CoBieLiteUkHelper Helper ;
        /// <summary>
        /// Instantiates a new IfcToCOBieLiteUkExchanger class.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="reportProgress"></param>
        /// <param name="filter"></param>
        /// <param name="configFile"></param>
        /// <param name="extId"></param>
        /// <param name="sysMode"></param>
        /// <param name="classify"></param>
        public IfcToCOBieLiteUkExchanger(XbimModel source, List<Facility> target, ReportProgressDelegate reportProgress = null, OutPutFilters filter = null, string configFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types, bool classify = false) : base(source, target)
        {
            ReportProgress.Progress = reportProgress; //set reporter
            Helper = new CoBieLiteUkHelper(source, ReportProgress, filter, configFile, extId, sysMode);
            this.classify = classify;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<Facility> Convert()
        {
            var mapping = GetOrCreateMappings<MappingIfcBuildingToFacility>();
            var buildings = SourceRepository.Instances.OfType<IfcBuilding>().ToList();
            var facilities = new List<Facility>(buildings.Count);
            foreach (var ifcBuilding in buildings)
            {
                var facility = new Facility();
                facility = mapping.AddMapping(ifcBuilding, facility);
                if(classify)       
                    facility.Classify();
                facilities.Add(facility);
            }
            return facilities;
        }
    }
}
