using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.FilterHelper;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    public class IfcToCOBieLiteUkExchanger : XbimExchanger<IModel, List<Facility>>
    {
        private readonly bool _classify;
        internal CoBieLiteUkHelper Helper ;
        /// <summary>
        /// Instantiates a new IIfcToCOBieLiteUkExchanger class.
        /// </summary>
        public IfcToCOBieLiteUkExchanger(IModel source, List<Facility> target, ReportProgressDelegate reportProgress = null, OutPutFilters filter = null, string configFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types, bool classify = false) 
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
            var buildings = SourceRepository.Instances.OfType<IIfcBuilding>().ToList();
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
