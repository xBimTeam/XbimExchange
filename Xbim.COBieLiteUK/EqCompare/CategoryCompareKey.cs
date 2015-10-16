using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class CategoryCompareKey: CompareEqRule<Category>, IEqualityComparer<Category>
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CategoryCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag
        /// </summary>
        public CategoryCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion



        #region Methods
        /// <summary>
        /// No name field on Category
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override string BuildName(Category obj)
        {
            throw new NotImplementedException("Category has no Name property, set CompareMethod property to CompareType.Key or CompareType.Full");
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Category obj)
        {
            sb.Clear();

            sb.Append(obj.Classification);
            sb.Append(obj.Code);
            sb.Append(obj.Description);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Category obj)
        {
            return BuildKey(obj);
        } 
        #endregion

    }
}
