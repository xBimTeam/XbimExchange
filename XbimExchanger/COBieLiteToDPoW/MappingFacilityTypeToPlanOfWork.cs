using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.COBieLiteToDPoW
{
    /// <summary>
    /// This is the main class used to convert COBieLite to DPoW. It does all the branching for different parts of the model 
    /// on the first level of object hierarchy.
    /// </summary>
    class MappingFacilityTypeToPlanOfWork : COBieLiteToDPoWMapping<FacilityType, PlanOfWork>
    {
        protected override PlanOfWork Mapping(FacilityType source, PlanOfWork target)
        {
            throw new NotImplementedException();
        }
    }
}
