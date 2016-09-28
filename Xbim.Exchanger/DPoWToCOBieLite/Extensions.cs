using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLite;
using Xbim.COBieLite.CollectionTypes;
using Xbim.DPoW;
using Attribute = Xbim.DPoW.Attribute;

namespace XbimExchanger.DPoWToCOBieLite
{
    public static class Extensions
    {

        public static string GetEncodedClassification(this PlanOfWork pow, IEnumerable<Guid> classificationReferenceIds, string suffix = "")
        {
            var ids = classificationReferenceIds != null ? classificationReferenceIds.ToArray() : new Guid[]{};
            var idsNum = ids.Length;
            if (idsNum == 0)
                return null;
            
            var result = "";
            var processedCount = 0;

            //itterate over ids and get it all together
            foreach (var classification in pow.ClassificationSystems)
            {
                foreach (var reference in classification.ClassificationReferences)
                {
                    if (!ids.Contains(reference.Id)) continue;

                    result += String.Format("{0}:{1}:{2}|", classification.Name, reference.ClassificationCode,
                        suffix);

                    processedCount++;
                    if (processedCount == ids.Length) break;
                }
            }
            return result.Trim('|');
        }

        public static void Add(this AttributeCollectionType act, string name, string description, string value, string pset = null)
        {
            act.Add(new AttributeType()
                    {
                        AttributeName = name,
                        AttributeDescription = description,
                        propertySetName = pset,
                        AttributeValue = new AttributeValueType
                        {
                            Item = new AttributeStringValueType { StringValue = value },
                            ItemElementName = ItemChoiceType.AttributeStringValue
                        }
                    });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, int value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType
                {
                    Item = new AttributeIntegerValueType { IntegerValue = value },
                    ItemElementName = ItemChoiceType.AttributeIntegerValue
                }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, double value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType
                {
                    Item = new AttributeDecimalValueType { DecimalValue = value , DecimalValueSpecified = true},
                    ItemElementName = ItemChoiceType.AttributeDecimalValue
                }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, bool value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType
                {
                    Item = new BooleanValueType { BooleanValue = value , BooleanValueSpecified = true},
                    ItemElementName = ItemChoiceType.AttributeBooleanValue
                },
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
