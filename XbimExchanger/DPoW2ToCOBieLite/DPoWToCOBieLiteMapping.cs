using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    abstract class DPoWToCOBieLiteMapping<TSourceType, TTargetType> : XbimMappings<PlanOfWork, FacilityType, string, TSourceType, TTargetType> where TTargetType : new()
    {

    }
}
