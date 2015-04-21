using System;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    class VisualValue : IVisualValue
    {
        private readonly object _object;

        public VisualValue(object baseObject)
        {
            _object = baseObject;
        }

        public VisualAttentionStyle AttentionStyle { get; set; }
            
        AttributeValue IVisualValue.VisualValue
        {
            get
            {
                if (_object == null)
                    return new StringAttributeValue { Value = ""};
                switch (_object.GetType().Name)
                {
                    case "DateTime":
                        return new DateTimeAttributeValue() {Value = _object as DateTime?};
                    case "Int32":
                        return new IntegerAttributeValue {Value = (int) _object};
                    case "String":
                        return new StringAttributeValue() { Value = _object.ToString()};
                    default:
                        return new StringAttributeValue { Value = _object.ToString()};
                }
            }
        }
    }
}
