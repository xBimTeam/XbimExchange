using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.DPoW.Interfaces.Converters
{
    public class DPoWObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DPoWObject)) return true;
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, typeof(Assembly));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            
            throw new NotImplementedException();
        }
    }
}
