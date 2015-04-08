using System.Linq;
using NPOI.SS.UserModel;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    internal class ValidatedAttribute : IVisualValue
    {
        private StatusOptions _status = StatusOptions.NotEvaluated;

        public StatusOptions Status
        {
            get
            {
                if (_status == StatusOptions.NotEvaluated)
                    Evaluate();
                return _status;
            }
        }

        internal enum StatusOptions
        {
            NotEvaluated,
            Passed,
            Failed,
            Invalid
        }

        private readonly Attribute _attribute;

        public ValidatedAttribute(COBieLiteUK.Attribute attribute)
        {
            _attribute = attribute;
        }

        public bool IsValidatedAttribute
        {
            get
            {
                if (_status == StatusOptions.NotEvaluated)
                    Evaluate();
                return (_status != StatusOptions.Invalid);
            }
        }
        
        private void Evaluate()
        {
            var firstCat = _attribute.Categories.FirstOrDefault(c => c.Classification == @"DPoW" && (c.Code == "Passed" || c.Code == "Failed"));
            if (firstCat == null)
            {
                _status = StatusOptions.Invalid;
                return;
            }
            _status = firstCat.Code == "Passed" 
                ? StatusOptions.Passed 
                : StatusOptions.Failed;
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

        public AttributeValue VisualValue
        {
            get { return _attribute.Value; }
        }
    }
}
