using System;
using System.Collections.Generic;
using Xbim.CobieLiteUk;

namespace Xbim.COBie.EqCompare
{
    public class IssueCompareKey : CompareEqRule<Issue>, IEqualityComparer<Issue>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public IssueCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public IssueCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion
        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Issue obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Issue obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append((obj.IssueWith != null && obj.IssueWith.Name != null) ? obj.IssueWith.Name : string.Empty);
            sb.Append((obj.IssueWith != null) ? obj.IssueWith.KeyType.ToString() : string.Empty);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Issue obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.Risk);
            sb.Append(obj.Chance);
            sb.Append(obj.Impact);
            sb.Append((obj.Owner != null && obj.Owner.Email != null) ? obj.Owner.Email : string.Empty); 
            sb.Append(obj.Mitigation);
            sb.Append((obj.IssueWith != null && obj.IssueWith.Name != null) ? obj.IssueWith.Name : string.Empty);
            sb.Append((obj.IssueWith != null) ? obj.IssueWith.KeyType.ToString() : string.Empty);

            return sb.ToString();
        }
        #endregion


       
    }
}
