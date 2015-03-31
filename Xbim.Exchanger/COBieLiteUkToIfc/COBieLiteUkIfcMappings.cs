
using Xbim.COBieLiteUK;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger.IfcHelpers;

namespace XbimExchanger.COBieLiteUkToIfc
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFromKey"></typeparam>
    /// <typeparam name="TFromObject"></typeparam>
    /// <typeparam name="TToObject"></typeparam>
    public abstract class CoBieLiteUkIfcMappings<TFromKey, TFromObject, TToObject> : IfcMappings<Facility, TFromKey, TFromObject, TToObject> where TToObject : IPersistIfcEntity, new()
    {

        /// <summary>
        /// 
        /// </summary>
        public new CoBieLiteUkToIfcExchanger Exchanger { get { return (CoBieLiteUkToIfcExchanger) base.Exchanger; } }
    }
}
