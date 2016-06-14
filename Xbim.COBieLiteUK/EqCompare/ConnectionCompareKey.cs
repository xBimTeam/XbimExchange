using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.CobieLiteUk;

namespace Xbim.COBie.EqCompare
{
    public class ConnectionCompareKey : CompareEqRule<Connection>, IEqualityComparer<Connection>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionCompareKey()
        {

        }
        /// <summary>
        /// all constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public ConnectionCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Connection obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }



        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Connection obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append((obj.Categories != null && obj.Categories.FirstOrDefault() != null) ? obj.Categories.First().CategoryString : string.Empty);
            sb.Append((obj.ConnectedTo != null && obj.ConnectedTo.Name != null) ? obj.ConnectedTo.Name : string.Empty);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Connection obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append((obj.Categories != null && obj.Categories.FirstOrDefault() != null) ? obj.Categories.First().CategoryString : string.Empty);
            sb.Append((obj.ConnectedTo != null && obj.ConnectedTo.Name != null) ? obj.ConnectedTo.Name : string.Empty);
            sb.Append((obj.RealizingElement != null && obj.RealizingElement.Name != null) ? obj.RealizingElement.Name : string.Empty);
            sb.Append(obj.PortName1);
            sb.Append(obj.PortName2);

            return sb.ToString();
        } 
        #endregion



    }
}
