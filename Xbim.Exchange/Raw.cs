using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Exchange
{
    public class Raw : DynamicObject
    {
        private Dictionary<string, object> _data = new Dictionary<string,object>();

        public object this[string key]
        {
            get 
            {
                object result;
                if (_data.TryGetValue(key, out result))
                    return result;
                return null;
            }
            set 
            {
                if (_data.Keys.Contains(key))
                    _data[key] = value;
                else
                    _data.Add(key, value);
            }
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var key = indexes[0] as string;
            if (key == null) return false;
            
            this[key] = value;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var key = indexes[0] as string;
            if (key == null)
            {
                result = null;
                return false;
            }

            result = this[key];
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        public static Raw FromObject(object data)
        {
            var result = new Raw();
            var type = data.GetType();
            var propInfos = type.GetProperties();
            foreach (var info in propInfos)
            {
                var value = info.GetValue(data);
                if (value != null)
                {
                    var pType = value.GetType();
                    if (pType.IsPrimitive)
                        result[info.Name] = value;
                    if (pType.is)
                }
            }
        }

    }
}
