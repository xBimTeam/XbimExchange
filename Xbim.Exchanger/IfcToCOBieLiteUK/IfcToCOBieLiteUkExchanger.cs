using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.FilterHelper;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    public class IfcToCOBieLiteUkExchanger : XbimExchanger<XbimModel, List<Facility>>
    {
        private readonly bool _classify;
        internal CoBieLiteUkHelper Helper ;
        /// <summary>
        /// Instantiates a new IfcToCOBieLiteUkExchanger class.
        /// </summary>
        public IfcToCOBieLiteUkExchanger(XbimModel source, List<Facility> target, ReportProgressDelegate reportProgress = null, OutPutFilters filter = null, string configFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types, bool classify = false) 
            : base(source, target)
        {
            ReportProgress.Progress = reportProgress; //set reporter
            Helper = new CoBieLiteUkHelper(source, ReportProgress, filter, configFile, extId, sysMode);
            this._classify = classify;
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
                if(_classify)       
                    facility.Classify();
                facilities.Add(facility);
            }
            return facilities;
        }
    }
}
