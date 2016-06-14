using System;
using System.Collections.Generic;
using Xbim.CobieLiteUk;

namespace Xbim.COBie.EqCompare
{
    public class SpaceCompareKey : CompareEqRule<Space>, IEqualityComparer<Space>
    {
        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        public SpaceCompareKey() : base()
        {

        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public SpaceCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion
        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Space obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Space obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.AppendDouble(obj.UsableHeight, 4);
            sb.AppendDouble(obj.GrossArea, 4);
            sb.AppendDouble(obj.NetArea, 4);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Space obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.RoomTag);
            sb.AppendDouble(obj.UsableHeight, 4);
            sb.AppendDouble(obj.GrossArea, 4);
            sb.AppendDouble(obj.NetArea, 4);


            return sb.ToString();
        }
        #endregion

    }
}
