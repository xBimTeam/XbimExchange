using Xbim.CobieLiteUK.Validation.Extensions;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    internal class ValidatedAttribute : IVisualValue
    {
        private StatusOptions _status = StatusOptions.NotEvaluated;

        public ValidatedAttribute(Attribute attribute)
        {
            _attribute = attribute;
        }

        public StatusOptions Status
        {
            get
            {
                if (_status == StatusOptions.NotEvaluated)
                    _status = _attribute.ValidationResult();
                return _status;
            }
        }

        public AttributeValue VisualValue
        {
            get { return _attribute.Value; }
        }
  
        private readonly Attribute _attribute;


        public bool IsValidatedAttribute
        {
            get
            {
                if (_status == StatusOptions.NotEvaluated)
                    _status = _attribute.ValidationResult();
                return (_status != StatusOptions.Invalid);
            }
        }
        
        public VisualAttentionStyle AttentionStyle
        {
            get
            {
                switch (Status)
                {
                    case StatusOptions.Passed:
                        return VisualAttentionStyle.Green;
                    case StatusOptions.Failed:
                        return VisualAttentionStyle.Red;
                    default:  
                        return VisualAttentionStyle.Amber;
                }                
            }
        }
    }
}
