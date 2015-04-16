using System;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
{
    /// <summary>
    /// Contains an evaluation criterion to apply.
    /// </summary>
    internal class RequirementDetail
    {
        /// <summary>
        /// Determines the behaviour of the evaluation mechanism for the requirement
        /// </summary>
        public enum EvaluationCriterion
        {
            /// <summary>
            /// Any vaue other than null is accepted; not implemented
            /// </summary>
            IgnoreValue,
            /// <summary>
            /// the default for DPoW, accepts any value other than null, "n/a" and "user to populate"
            /// </summary>
            ValueIsMeaningful,
            /// <summary>
            /// Not implemented
            /// </summary>
            ExactMatch,
            /// <summary>
            /// Not implemented
            /// </summary>
            MinumumValue,
            /// <summary>
            /// Not implemented
            /// </summary>
            MaximumValue
        }

        public string Name { get; set; }

        // public string Description { get; set; }
        
        /// <summary>
        /// Determines the behaviour of the evaluation mechanism for the requirement
        /// </summary>
        public EvaluationCriterion Criterion = EvaluationCriterion.ValueIsMeaningful;

        /// <summary>
        /// Constructor that completes initialisation from a DPoW attribute.
        /// </summary>
        /// <param name="attrib"></param>
        public RequirementDetail(Attribute attrib)
        {
            Attribute = attrib;
            Name = attrib.Name;
            // Description = attrib.Description;
        }

        /// <summary>
        /// The underlying DPoW Attribute used on initialisation.
        /// </summary>
        public Attribute Attribute { get; private set; }

        internal bool IsSatisfiedBy(Attribute attribute)
        {
            if (attribute == null)
                return false;

            var v = attribute.Value.GetStringValue().ToLowerInvariant();
            return Evaluate(v);
        }

        internal bool IsSatisfiedBy(CobieValue propValue)
        {
            if (propValue == null)
                return false;
            var oValue = propValue.ToObject();
            return oValue != null && Evaluate(oValue.ToString());
        }

        private bool Evaluate(string v)
        {
            switch (Criterion)
            {
                case EvaluationCriterion.ValueIsMeaningful:
                    return v != "n/a" && v != "user to populate";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}