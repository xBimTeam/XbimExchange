using System;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
{
    public class RequirementDetail
    {
        public enum EvaluationCriterion
        {
            IgnoreValue,
            ValueIsMeaningful,
            ExactMatch,
            MinumumValue,
            MaximumValue
        }

        public string Name;
        public string Description;
        public EvaluationCriterion Criterion = EvaluationCriterion.ValueIsMeaningful;

        public RequirementDetail(Attribute attrib)
        {
            Attribute = attrib;
            Name = attrib.Name;
            Description = attrib.Description;
        }

        public Attribute Attribute { get; private set; }

        internal bool IsSatisfiedBy(Attribute attribute)
        {
            if (attribute == null)
                return false;
            
            switch (Criterion)
            {
                case EvaluationCriterion.ValueIsMeaningful:
                    var v = attribute.Value.GetStringValue();
                    return v != "n/a" && v != "user to define";
                    break;
                default :
                    throw new NotImplementedException();
                    break;
            }
        }
    }
}
