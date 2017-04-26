using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.CobieLiteUk;
using Xbim.CobieLiteUk.FilterHelper;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using XbimExchanger.IfcToCOBieExpress.Classifications;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.IfcToCOBieExpress
{
    public class IfcToCoBieExpressExchanger : XbimExchanger<IfcStore, IModel>
    {
        private readonly bool _classify;
        internal COBieExpressHelper Helper ;
        /// <summary>
        /// Instantiates a new IIfcToCOBieLiteUkExchanger class.
        /// </summary>
        public IfcToCoBieExpressExchanger(IfcStore source, IModel target, ReportProgressDelegate reportProgress = null, OutPutFilters filter = null, string configFile = null, EntityIdentifierMode extId = EntityIdentifierMode.IfcEntityLabels, SystemExtractionMode sysMode = SystemExtractionMode.System | SystemExtractionMode.Types, bool classify = false, CobieContact creatorContact = null) 
            : base(source, target)
        {
            ReportProgress.Progress = reportProgress; //set reporter
            Helper = new COBieExpressHelper(this, ReportProgress, filter, configFile, extId, sysMode, creatorContact);
            Helper.Init();

            _classify = classify;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IModel Convert()
        {
            var mapping = GetOrCreateMappings<MappingIfcBuildingToFacility>();
            var classifier = new Classifier(this);
            var buildings = SourceRepository.Instances.OfType<IIfcBuilding>().ToList();
            var facilities = new List<CobieFacility>(buildings.Count);
            foreach (var building in buildings)
            {
                var facility = TargetRepository.Instances.New<CobieFacility>();
                facility = mapping.AddMapping(building, facility);
                if(_classify)
                    classifier.Classify(facility);
                facilities.Add(facility);
            }
            return TargetRepository;
        }
    }
}
