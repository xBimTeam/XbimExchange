using Xbim.COBieLite;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    public abstract class CoBieLiteIfcMappings<TFromKey, TFromObject, TToObject> : IfcMappings<FacilityType, TFromKey, TFromObject, TToObject> where TToObject : IPersistIfcEntity, new()
    {

    }
}
