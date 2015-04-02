using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
{
    public class RequirementDetail
    {
        public enum EvaluationCriterion
        {
            IgnoreValue,
            ExactMatch,
            MinumumValue,
            MaximumValue
        }

        public string Name;
        public string Description;
        public EvaluationCriterion Criterion = EvaluationCriterion.IgnoreValue;

        public RequirementDetail(Attribute attrib)
        {
            Attribute = attrib;
            Name = attrib.Name;
            Description = attrib.Description;
        }

        public Attribute Attribute { get; private set; }
    }
}
