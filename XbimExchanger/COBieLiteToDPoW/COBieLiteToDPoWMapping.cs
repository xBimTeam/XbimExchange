using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;

namespace XbimExchanger.COBieLiteToDPoW
{
    abstract class COBieLiteToDPoWMapping<TSource, TTarget>: XbimMappings<FacilityType, PlanOfWork, string, TSource, TTarget> where TTarget: new()
    {
       
    }
}
