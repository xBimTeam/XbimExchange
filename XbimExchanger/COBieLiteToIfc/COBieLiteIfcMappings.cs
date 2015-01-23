using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.XbimExtensions.Interfaces;

namespace XbimExchanger.COBieLiteToIfc
{
    public abstract class COBieLiteIfcMappings<TFromKey, TFromObject, TToObject> : IfcMappings<FacilityType, TFromKey, TFromObject, TToObject> where TToObject : IPersistIfcEntity, new()
    {

    }
}
