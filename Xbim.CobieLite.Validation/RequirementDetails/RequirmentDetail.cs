using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.CobieLite.Validation.RequirementDetails
{
    public class RequirmentDetail
    {
        public enum EvaluationCriterion
        {
            IgnoreValue,
            ExactMatch,
            MinumumValue,
            MaximumValue
        }

        public string Name;
        public EvaluationCriterion Criterion = EvaluationCriterion.IgnoreValue;
        private COBieLiteUK.Attribute attrib;

        public RequirmentDetail(COBieLiteUK.Attribute attrib)
        {
            // TODO: Complete member initialization
            this.attrib = attrib;
        }


    }
}
