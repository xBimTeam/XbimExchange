using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Xbim.DPoW.Interfaces
{
    internal class DPoWObjectConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AssetType) || objectType == typeof(IEnumerable<AssetType>) || objectType == typeof(Assembly) || objectType == typeof(IEnumerable<Assembly>) || objectType == typeof(Zone) || objectType == typeof(IEnumerable<Zone>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (existingValue != null)
            {
                if (typeof(IEnumerable<AssetType>).IsAssignableFrom(existingValue.GetType()))
                    return serializer.Deserialize<IEnumerable<AssetType>>(reader);
                if (typeof(IEnumerable<Assembly>).IsAssignableFrom(existingValue.GetType()))
                    return serializer.Deserialize<IEnumerable<Assembly>>(reader);
                if (typeof(IEnumerable<Zone>).IsAssignableFrom(existingValue.GetType()))
                    return serializer.Deserialize<IEnumerable<Zone>>(reader);
                if (objectType == typeof (AssetType))
                    return serializer.Deserialize<AssetType>(reader);
                if (objectType == typeof (Assembly))
                    return serializer.Deserialize<Assembly>(reader);
                if (objectType == typeof (Zone))
                    return serializer.Deserialize<Zone>(reader);
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
