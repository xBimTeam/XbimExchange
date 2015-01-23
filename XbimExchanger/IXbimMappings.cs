using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XbimExchanger
{
    public interface IXbimMappings<TSourceRepository, TTargetRepository>
    {

        Type MapFromType { get; }
        Type MapToType { get; }

        Type MapKeyType { get; }

        XbimExchanger<TSourceRepository, TTargetRepository> Exchanger { get; set; }

        public IDictionary<object, object> Mappings { get; }

        public object CreateTargetObject();
        public bool GetTargetObject(object key, out object targetObject);
        public object GetOrCreateTargetObject(object key);
        public object AddMapping(object source, object target);

    }
}
