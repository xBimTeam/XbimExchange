using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xbim.COBieLiteUK.Converters
{
    internal class AttributeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Attribute);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            if (jObject == null) return null;

            var result = new Attribute();

            //remove this converter to do regular conversion
            serializer.Converters.Remove(this);
            try
            {
                //this will populate everything except the value itself
                serializer.Populate(jObject.CreateReader(), result);

                //get value property, populate it into proper type and add to attribute
                AttributeValue value = null;
                var strType = jObject.GetValue("StringAttributeValue");
                if (strType != null)
                {
                    value = new StringAttributeValue();
                    serializer.Populate(strType.CreateReader(), value);
                }
                var boolType = jObject.GetValue("BooleanAttributeValue");
                if (boolType != null)
                {
                    value = new BooleanAttributeValue();
                    serializer.Populate(boolType.CreateReader(), value);
                }
                var dateType = jObject.GetValue("DateTimeAttributeValue");
                if (dateType != null)
                {
                    value = new DateTimeAttributeValue();
                    serializer.Populate(dateType.CreateReader(), value);
                }
                var decType = jObject.GetValue("DecimalAttributeValue");
                if (decType != null)
                {
                    value = new DecimalAttributeValue();
                    serializer.Populate(decType.CreateReader(), value);
                }
                var intType = jObject.GetValue("IntegerAttributeValue");
                if (intType != null)
                {
                    value = new IntegerAttributeValue();
                    serializer.Populate(intType.CreateReader(), value);
                }
                result.Value = value;
            }
            finally
            {
                //add converter back again
                serializer.Converters.Add(this);
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var attrValue = value as Attribute;
            if (attrValue == null)
                return;

            //remove this converter to do regular conversion
            serializer.Converters.Remove(this);
            try
            {
                var itemVal = attrValue.Value;
                var token = JToken.FromObject(value, serializer);
                var jObject = token as JObject;

                if (itemVal != null && jObject != null)
                {
                    //enhance the result
                    //change "Item" to propper attribute value type name
                    var item = jObject.GetValue("Value");
                    if (item != null)
                    {
                        jObject.Remove("Value");
                        jObject.Add(itemVal.GetType().Name, item);
                    }
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