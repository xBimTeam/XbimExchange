using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.FilterHelper;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    public class IfcToCOBieLiteUkExchanger : XbimExchanger<IModel, List<Facility>>
    {
        internal CoBieLiteUkHelper Helper ;
        /// <summary>
        /// Instantiates a new IIfcToCOBieLiteUkExchanger class.
        /// </summary>
        public IfcToCOBieLiteUkExchanger(IModel source, List<Facility> target, ReportProgressDelegate reportProgress = null, OutPutFilters filter = null, string configFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types) 
            : base(source, target)
        {
            ReportProgress.Progress = reportProgress; //set reporter
            Helper = new CoBieLiteUkHelper(source, ReportProgress, filter, configFile, extId, sysMode);
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
                facilities.Add(facility);
            }
            return facilities;
        }
    }
}
