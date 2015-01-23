using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XbimExchanger
{
    public abstract class XbimMapper<TModelFrom, TModelTo>
    {
        private ConcurrentDictionary<Type, IXbimMappings> _mapsDict = new ConcurrentDictionary<Type, IXbimMappings>();
        /// <summary>
        /// Mappings dictionary where key is a TFromType
        /// </summary>
        protected IDictionary<Type, IXbimMappings> MappingsDictionary { get { return _mapsDict; } }

        protected bool AddMappings<TFromKey, TFromObject, TToObject>(XbimMappings<TFromKey, TFromObject, TToObject> mappings) where TToObject: new()
        {
            return _mapsDict.TryAdd(typeof(TFromObject), mappings);
        }

        public IXbimMappings GetMappings(Type typeFrom)
        {
            IXbimMappings result;
            if (_mapsDict.TryGetValue(typeFrom, out result))
                return result;
            return null;
        }

        public XbimMappings<TFromKey, TFromObject, TToObject> GetMappings<TFromKey, TFromObject, TToObject>() where TToObject : new()
        { 
            IXbimMappings result;
            if (_mapsDict.TryGetValue(typeof(TFromKey), out result))
                return result as XbimMappings<TFromKey, TFromObject, TToObject>;
            return null;
        }

        /// <summary>
        /// Find a root as a mappings where TFromObject is TModelFrom and TToObject is TModelTo and executes it.
        /// It suppose that all other mappings will be called inside of this in a hierarchy fllowing hierarchy of the
        /// model. If not root mapping is found this function will throw an exception.
        /// </summary>
        /// <returns></returns>
        public TModelTo Execute(TModelFrom source)
        { 
            //find root as mappings
            var root = GetMappings(typeof(TModelFrom));
            //if (root == null)
            //    throw new Exception("Root mapping doesn't exist within this mapper.");
            //root.Create()
            //return root.AddMapping(source, )
            throw new NotImplementedException();
        }

    }
}
