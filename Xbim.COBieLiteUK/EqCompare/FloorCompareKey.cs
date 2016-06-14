using System;
using System.Collections.Generic;
using Xbim.CobieLiteUk;

namespace Xbim.COBie.EqCompare
{
    public class FloorCompareKey : CompareEqRule<Floor>, IEqualityComparer<Floor>
    {
        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        public FloorCompareKey() : base()
        {

        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag
        /// </summary>
        public FloorCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {
            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Floor obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Floor obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.AppendDouble(obj.Elevation, 4);
            sb.AppendDouble(obj.Height, 4);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Floor obj)
        {
            return BuildKey(obj);
        }

        #endregion

    }
}
