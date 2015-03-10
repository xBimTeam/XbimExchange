using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.COBieLiteToDPoW
{
    public class COBieLiteToDPoWExchanger : XbimExchanger<FacilityType, PlanOfWork>
    {
        /// <summary>
        /// Constructor of the exchanger class for conversion from COBieLite to Digital Plan of Work.
        /// </summary>
        /// <param name="facility">Input facility (root object of COBieLite data model)</param>
        /// <param name="plan">Output DPoW model - this is supposed to be an empty model.</param>
        public COBieLiteToDPoWExchanger(FacilityType facility, PlanOfWork plan) : base(facility, plan)
        {

        }

        /// <summary>
        /// Converts COBieLite model defined in constructor into DPoW.
        /// </summary>
        /// <returns>Digital Plan of Work</returns>
        public override PlanOfWork Convert()
        {
            var map = GetOrCreateMappings<MappingFacilityTypeToPlanOfWork>();
            return map.AddMapping(SourceRepository, TargetRepository);
        }
    }
}
