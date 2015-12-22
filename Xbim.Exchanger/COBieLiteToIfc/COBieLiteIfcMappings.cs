using Xbim.COBieLite;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteToIfc
{
    public abstract class CoBieLiteIfcMappings<TFromKey, TFromObject, TToObject> : IfcMappings<FacilityType, TFromKey, TFromObject, TToObject> where TToObject : IPersistIfcEntity, new()
    {
        public new CoBieLiteToIfcExchanger Exchanger { get { return (CoBieLiteToIfcExchanger) base.Exchanger; } }
    }
}
