using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xbim.CobieLiteUk.Converters
{
    internal class KeyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IEntityKey).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //this doesn't have to be implemented because converter states it only can write, not read
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var key = value as IEntityKey;
            if(key == null) return;

            serializer.Converters.Remove(this);
            try
            {
                var token = JToken.FromObject(value, serializer);
                var jObject = token as JObject;

                if (jObject != null)
                {
                    jObject.Add("KeyType", key.ForType.Name);
                    jObject.WriteTo(writer);
                }
                else
                {
                    token.WriteTo(writer);
                }
            }
            finally
            {
                //add converter back again
                serializer.Converters.Add(this);
            }
        }
    }
}
