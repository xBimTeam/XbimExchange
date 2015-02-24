using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;
using Xbim.DPoW;
using Attribute = Xbim.DPoW.Attribute;

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

        public static IEnumerable<AttributeType> GetCOBieAttributes(this DPoWAttributableObject obj)
        {
            IEnumerable<Xbim.DPoW.Attribute> sAttributes = obj.Attributes;
            var sAttrs = sAttributes as Attribute[] ?? sAttributes.ToArray();
            if (sAttributes == null || !sAttrs.Any())
                yield break;

            foreach (var sAttr in sAttrs)
            {
                //create attribute in target
                var tAttr = new AttributeType { AttributeName = sAttr.Name, AttributeDescription = sAttr.Definition, AttributeValue = new AttributeValueType() };
                switch (sAttr.ValueType)
                {
                    case ValueTypeEnum.NotDefined:
                        tAttr.AttributeValue.Item = new AttributeStringValueType { StringValue = sAttr.Value };
                        tAttr.AttributeValue.ItemElementName = ItemChoiceType.AttributeStringValue;
                        break;
                    case ValueTypeEnum.Boolean:
                        bool bVal;
                        if (bool.TryParse(sAttr.Value, out bVal))
                        {
                            tAttr.AttributeValue.Item = new BooleanValueType
                            {
                                BooleanValue = bVal,
                                BooleanValueSpecified = true
                            };
                            tAttr.AttributeValue.ItemElementName = ItemChoiceType.AttributeBooleanValue;
                        }
                        break;
                    case ValueTypeEnum.DateTime:
                        DateTime dtVal;
                        if (DateTime.TryParse(sAttr.Value, out dtVal))
                        {
                            tAttr.AttributeValue.Item = dtVal;
                            tAttr.AttributeValue.ItemElementName = ItemChoiceType.AttributeDateTimeValue;
                        }
                        break;
                    case ValueTypeEnum.Decimal:
                        float fVal;
                        if (float.TryParse(sAttr.Value, out fVal))
                        {
                            tAttr.AttributeValue.Item = new AttributeDecimalValueType
                            {
                                DecimalValue = fVal,
                                DecimalValueSpecified = true
                            };
                            tAttr.AttributeValue.ItemElementName = ItemChoiceType.AttributeDecimalValue;
                        }
                        break;
                    case ValueTypeEnum.Integer:
                        int iVal;
                        if (int.TryParse(sAttr.Value, out iVal))
                        {
                            tAttr.AttributeValue.Item = new AttributeIntegerValueType
                            {
                                IntegerValue = iVal
                            };
                            tAttr.AttributeValue.ItemElementName = ItemChoiceType.AttributeIntegerValue;
                        }
                        break;
                    case ValueTypeEnum.String:
                        tAttr.AttributeValue.Item = new AttributeStringValueType { StringValue = sAttr.Value };
                        tAttr.AttributeValue.ItemElementName = ItemChoiceType.AttributeStringValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //default action is to create string
                if (tAttr.AttributeValue.Item == null)
                {
                    tAttr.AttributeValue.Item = new AttributeStringValueType { StringValue = sAttr.Value };
                    tAttr.AttributeValue.ItemElementName = ItemChoiceType.AttributeStringValue;
                }
                yield return tAttr;
            }
        }
    }
}
