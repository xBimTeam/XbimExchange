using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.DPoWToCOBieLite
{
    public abstract class DPoWToCOBieLiteMapping<TSourceType, TTargetType> : XbimMappings<PlanOfWork, FacilityType, string, TSourceType, TTargetType> where TTargetType: new()
    {

    }
}
