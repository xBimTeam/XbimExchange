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

namespace XbimExchanger.IfcToCOBieLiteUK
{
    /// <summary>
    /// 
    /// </summary>
    public class IfcToCOBieLiteUkExchanger : XbimExchanger<XbimModel, List<Facility> >
    {
        internal CoBieLiteUkHelper Helper ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public IfcToCOBieLiteUkExchanger(XbimModel source, List<Facility> target, ReportProgressDelegate reportProgress = null, OutPutFilters filter = null, string configFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types ) : base(source, target)
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
            var buildings = SourceRepository.Instances.OfType<IfcBuilding>().ToList();
            var facilities = new List<Facility>(buildings.Count);
            foreach (var ifcBuilding in buildings)
            {
                var facility = new Facility();
                facilities.Add(mapping.AddMapping(ifcBuilding, facility));
            }
            return facilities;
        }
    }
}
