using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;

namespace XbimExchanger.DPoW2ToCOBieLite
{
    public static class Extensions
    {

        public static void Add(this AttributeCollectionType act, string name, string description, string value, string pset = null)
        {
            act.Add(new AttributeType()
                    {
                        AttributeName = name,
                        AttributeDescription = description,
                        propertySetName = pset,
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = value } }
                    });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, int value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType() { Item = new AttributeIntegerValueType() { IntegerValue = value } }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, double value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType() { Item = new AttributeDecimalValueType() { DecimalValue = value , DecimalValueSpecified = true} }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, bool value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType() { Item = new BooleanValueType() { BooleanValue = value , BooleanValueSpecified = true} }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
            });
        }
    }
}
