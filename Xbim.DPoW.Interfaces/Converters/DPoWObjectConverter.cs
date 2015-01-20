using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if (typeof(DPoWObject).IsAssignableFrom(objectType)) return true;
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = (string)jObject["$type"];

            object result = null;
            switch (type)
            {
                case "AssemblyType":
                    result = new AssemblyType();
                    break;
                case "AssetType":
                    result = new AssetType();
                    break;
                case "Zone":
                    result = new Zone();
                    break;
                default:
                    result = new AssemblyType();
                    break;
            }
            serializer.Populate(jObject.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObject = JObject.FromObject(value);
            jObject.AddFirst(new JProperty("$type", value.GetType().Name));
            jObject.WriteTo(writer);
        }
    }
}
