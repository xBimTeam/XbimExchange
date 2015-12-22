using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.COBieLiteToDPoW
{
    abstract class COBieLiteToDPoWMapping<TSource, TTarget>: XbimMappings<FacilityType, PlanOfWork, string, TSource, TTarget> where TTarget: new()
    {
       
    }
}
