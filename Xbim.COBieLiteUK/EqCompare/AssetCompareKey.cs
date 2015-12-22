using System;
using System.Collections.Generic;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class AssetCompareKey : CompareEqRule<Asset>, IEqualityComparer<Asset>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public AssetCompareKey() : base()
        {
        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public AssetCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Asset obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Asset obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.SerialNumber);
            sb.Append(obj.TagNumber);
            sb.Append(obj.BarCode);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Asset obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.SerialNumber);
            sb.Append(obj.InstallationDate);
            sb.Append(obj.WarrantyStartDate);
            sb.Append(obj.TagNumber);
            sb.Append(obj.BarCode);
            sb.Append(obj.AssetIdentifier);

            return sb.ToString();
        } 
        #endregion
    }
}
