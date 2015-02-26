using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xbim.COBieLite.Converters
{
    public class AttributeValueTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof (AttributeValueType) == objectType;
        }

        private static AttributeValueType GetValueType(JProperty property, ItemChoiceType type, object item, JsonSerializer serializer)
        {
            var result = new AttributeValueType();
            if (property.Value.Type == JTokenType.Null)
                result.Item = null;
            else
            {
                serializer.Populate(property.Value.CreateReader(), item);
                result.Item = item;
            }
            result.ItemElementName = type;
            return result;
        }

        private static AttributeValueType GetDateValueType(JProperty property, ItemChoiceType type)
        {
            var result = new AttributeValueType();
            if (property.Value.Type == JTokenType.Null)
                result.Item = null;
            else
                result.Item = property.Value.Value<DateTime>();
            result.ItemElementName = type;
            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var strType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeStringValue");
            if (strType != null)
                return GetValueType(strType, ItemChoiceType.AttributeStringValue, new AttributeStringValueType(), serializer);


            var decType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeDecimalValue");
            if (decType != null)
                return GetValueType(decType, ItemChoiceType.AttributeDecimalValue, new AttributeDecimalValueType(), serializer);

            var intType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeIntegerValue");
            if (intType != null)
                return GetValueType(intType, ItemChoiceType.AttributeIntegerValue, new AttributeIntegerValueType(), serializer);

            var bType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeBooleanValue");
            if (bType != null)
                return GetValueType(bType, ItemChoiceType.AttributeBooleanValue, new BooleanValueType(), serializer);

            var monType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeMonetaryValue");
            if (monType != null)
                return GetValueType(monType, ItemChoiceType.AttributeMonetaryValue, new AttributeMonetaryValueType(), serializer);

            var dateType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeDateValue");
            if (dateType != null && dateType.Value != null)
                return GetDateValueType(dateType, ItemChoiceType.AttributeDateValue);

            dateType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeTimeValue");
            if (dateType != null && dateType.Value != null)
                return GetDateValueType(dateType, ItemChoiceType.AttributeTimeValue);

            dateType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeDateTimeValue");
            if (dateType != null && dateType.Value != null)
                return GetDateValueType(dateType, ItemChoiceType.AttributeDateTimeValue);
            
            return null;
            //if it gets here it is one of date-time formats and it should be distinquished by the date-time format
            //between date, time and datetime written according to ISO 8601
            //var dateRegex = new Regex("^[0-9]{4}-[0-9]{2}-[0-9]{2}(Z|(-|\\+)[0-9]{2}:[0-9]{2})?(?!.)");
            //var timeRegexp = new Regex("^([0-9]){1,2}:([0-9]){1,2}:([0-9]){1,2}(\\.[0-9]+)*(Z|(-|\\+)[0-9]{2}:[0-9]{2})?(?!.)");
            //var dateTimeRegex = new Regex("^[0-9]{4}-[0-9]{2}-[0-9]{2}T([0-9]){1,2}:([0-9]){1,2}:([0-9]){1,2}(\\.[0-9]+)*(Z|(-|\\+)[0-9]{2}:[0-9]{2})?(?!.)");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var attrValue = value as AttributeValueType;
            if (attrValue == null)
                return;
            var item = attrValue.Item;
            if (item == null)
            {
                var nullResult = new JObject { { attrValue.ItemElementName.ToString(), JToken.Parse("null") } };
                nullResult.WriteTo(writer);
                return;
            }

            //try to guess the name of the type ruther than getting it from the enum as it might not be set correctly
            string typeName;
            if (_typeStringDictionary.TryGetValue(item.GetType(), out typeName))
            {
                //if it is not DateTime it is straightforward
                if (item.GetType() != typeof(DateTime))
                {
                    //use type name
                    var result = new JObject { { typeName, JToken.FromObject(item, serializer) } };
                    result.WriteTo(writer);
                    return;    
                }

                //if it is datetime, try to get specific type from enumeration
                var dtVal = "";
                var date = (DateTime)item;
                switch (attrValue.ItemElementName)
                {
                    case ItemChoiceType.AttributeDateValue:
                    {
                        dtVal = date.ToUniversalTime().ToString("yyyy-MM-ddZ");
                        typeName = "AttributeDateValue";
                    }
                        break;
                    case ItemChoiceType.AttributeTimeValue:
                    {
                        dtVal = date.ToUniversalTime().ToString("HH:mm:ssZ");
                        typeName = "AttributeTimeValue";
                    }
                        break;
                    case ItemChoiceType.AttributeDateTimeValue:
                    {
                        dtVal = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                        typeName = "AttributeDateTimeValue";
                    }
                        break;
                    default:
                    {
                        //if none of these enumeration values are specified it should be saved as AttributeDateTimeValue which will be all right for roundtripping
                        dtVal = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                        typeName = "AttributeDateTimeValue";
                    }
                        break;
                }
                //create new object and write it to writer
                (new JObject { { typeName, dtVal } }).WriteTo(writer);
                return;   
            }

            throw new Exception("Unexpected value type " + item.GetType().Name);
        }

        private Dictionary<string, Type> StringTypeDictionary = new Dictionary<string, Type>()
        {
            {"AttributeBooleanValue", typeof (BooleanValueType)},
            {"AttributeDateTimeValue", typeof (System.DateTime)},
            {"AttributeDateValue", typeof (System.DateTime)},
            {"AttributeDecimalValue", typeof (AttributeDecimalValueType)},
            {"AttributeIntegerValue", typeof (AttributeIntegerValueType)},
            {"AttributeMonetaryValue", typeof (AttributeMonetaryValueType)},
            {"AttributeStringValue", typeof (AttributeStringValueType)},
            {"AttributeTimeValue", typeof (System.DateTime)}
        };

        private readonly Dictionary<Type, string> _typeStringDictionary = new Dictionary<Type, string>()
        {
            {typeof (BooleanValueType), "AttributeBooleanValue"},
            {typeof (System.DateTime), "AttributeDateTimeValue"},
            {typeof (AttributeDecimalValueType), "AttributeDecimalValue"},
            {typeof (AttributeIntegerValueType), "AttributeIntegerValue"},
            {typeof (AttributeMonetaryValueType), "AttributeMonetaryValue"},
            {typeof (AttributeStringValueType), "AttributeStringValue"},
        };
    }
}