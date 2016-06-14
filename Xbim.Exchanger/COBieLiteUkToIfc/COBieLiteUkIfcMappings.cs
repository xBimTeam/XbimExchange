
using Xbim.CobieLiteUk;
using Xbim.Common;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteUkToIfc
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFromKey"></typeparam>
    /// <typeparam name="TFromObject"></typeparam>
    /// <typeparam name="TToObject"></typeparam>
    public abstract class CoBieLiteUkIfcMappings<TFromKey, TFromObject, TToObject> : IfcMappings<Facility, TFromKey, TFromObject, TToObject> where TToObject : IPersistEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public new CoBieLiteUkToIfcExchanger Exchanger { get { return (CoBieLiteUkToIfcExchanger) base.Exchanger; } }
    }
}
