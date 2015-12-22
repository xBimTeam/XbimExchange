using System;
using System.Collections.Generic;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    class ContactCompareKey : CompareEqRule<Contact>, IEqualityComparer<Contact>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ContactCompareKey()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public ContactCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Contact obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Contact obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Email);
            sb.Append(obj.Phone);
            sb.Append(obj.Company);
            sb.Append(obj.OrganizationCode);
            sb.Append(obj.PostalBox);
            sb.Append(obj.PostalCode);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Contact obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.Email);
            sb.Append(obj.Company);
            sb.Append(obj.Phone);
            sb.Append(obj.Department);
            sb.Append(obj.OrganizationCode);
            sb.Append(obj.GivenName);
            sb.Append(obj.FamilyName);
            sb.Append(obj.Street);
            sb.Append(obj.PostalBox);
            sb.Append(obj.Town);
            sb.Append(obj.StateRegion);
            sb.Append(obj.PostalCode);
            sb.Append(obj.Country);


            return sb.ToString();
        }
        #endregion
    }
}
