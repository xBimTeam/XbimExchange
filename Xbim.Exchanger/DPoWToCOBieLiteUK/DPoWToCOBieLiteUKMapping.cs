using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.DPoW;
using FacilityType = Xbim.COBieLiteUK.Facility;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    abstract class DpoWtoCoBieLiteUkMapping<TSourceType, TTargetType> : XbimMappings<PlanOfWork, FacilityType, string, TSourceType, TTargetType> where TTargetType : new()
    {

    }
}
