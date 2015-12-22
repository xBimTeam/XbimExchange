using System;
using System.Collections.Generic;

namespace Xbim.COBie.EqCompare
{
    public class StringCompareKey : CompareEqRule<string>, IEqualityComparer<string>
    {
        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        public StringCompareKey() : base()
        {

        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public StringCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(string obj)
        {
            return (obj == null) ? string.Empty : obj;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(string obj)
        {
            return BuildName(obj);
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(string obj)
        {
            return BuildName(obj);
        }
        #endregion

        
    }
}
