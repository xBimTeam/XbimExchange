using System;
using System.Collections.Generic;

namespace Xbim.COBie.EqCompare
{
    public class AttributeCompareKey : CompareEqRule<Xbim.CobieLiteUk.Attribute>, IEqualityComparer<Xbim.CobieLiteUk.Attribute>
    {

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeCompareKey() : base()
        {
        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public AttributeCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Xbim.CobieLiteUk.Attribute obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Xbim.CobieLiteUk.Attribute obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Value != null ? obj.Value.GetStringValue() : string.Empty);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Xbim.CobieLiteUk.Attribute obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.Value != null ? obj.Value.GetStringValue() : string.Empty);

            return sb.ToString();
        }
        #endregion
    }
}
