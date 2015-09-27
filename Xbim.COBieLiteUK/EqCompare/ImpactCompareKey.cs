using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class ImpactCompareKey : CompareEqRule<Impact>, IEqualityComparer<Impact>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ImpactCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public ImpactCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Impact obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Impact obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append((obj.Categories != null && obj.Categories.FirstOrDefault() != null) ? obj.Categories.First().CategoryString : string.Empty);
            sb.Append(obj.ImpactStage);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Impact obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append((obj.Categories != null && obj.Categories.FirstOrDefault() != null) ? obj.Categories.First().CategoryString : string.Empty);
            sb.Append(obj.Description);
            sb.Append(obj.ImpactStage);
            sb.Append(obj.Value);
            sb.Append(obj.ImpactUnit);
            sb.AppendDouble(obj.LeadInTime, 4);
            sb.AppendDouble(obj.Duration, 4);
            sb.AppendDouble(obj.LeadOutTime, 4);

            return sb.ToString();
        }
        #endregion
        

       
    }
}
