using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XbimExchanger
{
    public interface IXbimMappings
    {
        IDictionary<object, object> Mappings { get; }
        bool Create(object key);
        bool Get(object key, out object toObject);
        object GetOrCreate(object key);
        object AddMapping(object from, object to);
    }
}
