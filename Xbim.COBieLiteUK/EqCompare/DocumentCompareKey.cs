using System;
using System.Collections.Generic;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class DocumentCompareKey : CompareEqRule<Document>, IEqualityComparer<Document>
    {
        #region Constructors
        /// <summary>
        /// Constuctor
        /// </summary>
        public DocumentCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public DocumentCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Document obj)
        {
            return obj.Name ?? string.Empty;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Document obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Stage);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Document obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.ApprovalBy ?? string.Empty);
            sb.Append(obj.Stage);
            sb.Append(obj.Directory);
            sb.Append(obj.File);
            sb.Append(obj.Reference);
            

            return sb.ToString();
        }
        #endregion
        
    }
}
