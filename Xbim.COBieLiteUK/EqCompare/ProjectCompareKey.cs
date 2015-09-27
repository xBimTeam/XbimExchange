using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class ProjectCompareKey : CompareEqRule<Project>, IEqualityComparer<Project>
    {
        #region Constructors

        public ProjectCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag
        /// </summary>
        public ProjectCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        } 
        #endregion


        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Project obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Project obj)
        {
            sb.Clear();

            sb.Append(obj.Name);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Project obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            //sb.Append(obj.ExternalEntity);
            //sb.Append(obj.ExternalId);

            return sb.ToString();
        }
        #endregion
        
    }
}
