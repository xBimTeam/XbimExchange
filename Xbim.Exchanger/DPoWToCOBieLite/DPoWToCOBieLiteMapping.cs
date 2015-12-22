using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLite
{
    abstract class DPoWToCOBieLiteMapping<TSourceType, TTargetType> : XbimMappings<PlanOfWork, FacilityType, string, TSourceType, TTargetType> where TTargetType : new()
    {

    }
}
