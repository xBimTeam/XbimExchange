using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class NameCompareKey<T> : CompareEqRule<T>, IEqualityComparer<T> where T : CobieObject
    {
        #region Constructors
        /// <summary>
        /// Constuctor
        /// </summary>
        public NameCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag
        /// </summary>
        public NameCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(T obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(T obj)
        {
            return BuildName(obj);
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(T obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);

            return sb.ToString();
        }
        #endregion
    }
}
