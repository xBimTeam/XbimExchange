using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    class VisualValue : IVisualValue
    {
        private object _object;

        public VisualValue(object baseObject)
        {
            _object = baseObject;
        }

        public VisualAttentionStyle AttentionStyle { get; set; }
            
        AttributeValue IVisualValue.VisualValue
        {
            get
            {
                // todo: check other types
                switch (_object.GetType().Name)
                {
                    case "DateTime":
                        return new IntegerAttributeValue() { Value = (int)_object };
                    case "Int32":
                        return new IntegerAttributeValue() {Value = (int) _object};
                    case "String":
                        return new IntegerAttributeValue() { Value = (int)_object };
                    default:
                        return new StringAttributeValue() { Value = _object.ToString()};
                }
            }
        }
    }
}
