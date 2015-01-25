using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;

namespace XbimExchanger.DPoWToCOBieLite
{
    public static class Extensions
    {
        public static void Add(this AttributeCollectionType act, AttributeType attribute)
        {
            if (act == null) throw new ArgumentNullException();
            var attrs = act.Attribute != null ? act.Attribute.ToList() : new List<AttributeType>();
            attrs.Add(attribute);
            act.Attribute = attrs.ToArray();
        }

        public static void Add(this AttributeCollectionType act, IEnumerable< AttributeType> attributes)
        {
            if (act == null) throw new ArgumentNullException();
            var attrs = act.Attribute != null ? act.Attribute.ToList() : new List<AttributeType>();
            attrs.AddRange(attributes);
            act.Attribute = attrs.ToArray();
        }

        public static void Add(this AttributeCollectionType act, string name, string description, string value)
        {
            Add(act, new AttributeType()
                    {
                        AttributeName = name,
                        AttributeDescription = description,
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = value } }
                    });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, int value)
        {
            Add(act, new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                AttributeValue = new AttributeValueType() { Item = new AttributeIntegerValueType() { IntegerValue = value } }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, double value)
        {
            Add(act, new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                AttributeValue = new AttributeValueType() { Item = new AttributeDecimalValueType() { DecimalValue = value , DecimalValueSpecified = true} }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, bool value)
        {
            Add(act, new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                AttributeValue = new AttributeValueType() { Item = new BooleanValueType() { BooleanValue = value , BooleanValueSpecified = true} }
            });
        }
    }
}
