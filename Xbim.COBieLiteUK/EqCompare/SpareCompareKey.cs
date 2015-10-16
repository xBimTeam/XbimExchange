using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class SpareCompareKey : CompareEqRule<Spare>, IEqualityComparer<Spare>
    {

        #region Constructor
        public SpareCompareKey() : base()
        {

        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag
        /// </summary>
        public SpareCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion


        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Spare obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Spare obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.SetNumber);
            sb.Append(obj.PartNumber);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Spare obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.SetNumber);
            sb.Append(obj.PartNumber);

            return sb.ToString();
        }
        #endregion
       
    }
}
