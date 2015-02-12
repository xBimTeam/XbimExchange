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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var result = new AttributeValueType();


            var strType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeStringValue");
            if (strType != null)
            {
                var str = new AttributeStringValueType();
                serializer.Populate(strType.Value.CreateReader(), str);
                result.Item = str;
                result.ItemElementName = ItemChoiceType.AttributeStringValue;
                return result;
            }


            var decType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeDecimalValue");
            if (decType != null)
            {
                var dec = new AttributeDecimalValueType();
                serializer.Populate(decType.Value.CreateReader(), dec);
                result.Item = dec;
                result.ItemElementName = ItemChoiceType.AttributeDecimalValue;
                return result;
            }

            var intType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeIntegerValue");
            if (intType != null)
            {
                var intVal = new AttributeIntegerValueType();
                serializer.Populate(intType.Value.CreateReader(), intVal);
                result.Item = intVal;
                result.ItemElementName = ItemChoiceType.AttributeIntegerValue;
                return result;
            }

            var bType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeBooleanValue");
            if (bType != null)
            {
                var bVal = new BooleanValueType();
                serializer.Populate(bType.Value.CreateReader(), bVal);
                result.Item = bVal;
                result.ItemElementName = ItemChoiceType.AttributeBooleanValue;
                return result;
            }

            var monType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeMonetaryValue");
            if (monType != null)
            {
                var mVal = new AttributeMonetaryValueType();
                serializer.Populate(monType.Value.CreateReader(), mVal);
                result.Item = mVal;
                result.ItemElementName = ItemChoiceType.AttributeMonetaryValue;
                return result;
            }

            var dateType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeDateValue");
            if (dateType != null && dateType.Value != null)
            {
                result.Item = dateType.Value.Value<DateTime>();
                result.ItemElementName = ItemChoiceType.AttributeDateValue;
                return result;
            }

            dateType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeTimeValue");
            if (dateType != null && dateType.Value != null)
            {
                result.Item = dateType.Value.Value<DateTime>();
                result.ItemElementName = ItemChoiceType.AttributeTimeValue;
                return result;
            }

            dateType = jObject.Properties().FirstOrDefault(p => p.Name == "AttributeDateTimeValue");
            if (dateType != null && dateType.Value != null)
            {
                result.Item = dateType.Value.Value<DateTime>();
                result.ItemElementName = ItemChoiceType.AttributeDateTimeValue;
                return result;
            }

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
                return;

            //handle datetime in a specific manner
            if (attrValue.ItemElementName == ItemChoiceType.AttributeDateValue)
            {
                var date = (DateTime)item;
                item = date.ToUniversalTime().ToString("yyyy-MM-ddZ");
            }
            if (attrValue.ItemElementName == ItemChoiceType.AttributeTimeValue)
            {
                var date = (DateTime)item;
                item = date.ToUniversalTime().ToString("HH:mm:ssZ");
            }
            if (attrValue.ItemElementName == ItemChoiceType.AttributeDateTimeValue)
            {
                var date = (DateTime)item;
                item = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            }

            var result = new JObject {{attrValue.ItemElementName.ToString(), JToken.FromObject(item)}};
            result.WriteTo(writer);
        }
    }
}