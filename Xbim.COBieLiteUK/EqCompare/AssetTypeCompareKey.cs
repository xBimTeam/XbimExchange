using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    class AssetTypeCompareKey : CompareEqRule<AssetType>, IEqualityComparer<AssetType>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public AssetTypeCompareKey() : base()
        {

        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag
        /// </summary>
        public AssetTypeCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }

        #endregion



        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(AssetType obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }
        /// <summary>
        /// Get Best Fields to ID object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override string BuildKey(AssetType obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append((obj.Manufacturer != null && obj.Manufacturer.Email != null) ? obj.Manufacturer.Email : string.Empty);
            sb.Append(obj.ModelNumber);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override string BuildFull(AssetType obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.AssetTypeCustom);
            sb.Append((obj.Manufacturer != null && obj.Manufacturer.Email != null) ? obj.Manufacturer.Email : string.Empty);
            sb.Append(obj.ModelNumber);
            sb.AppendDouble(obj.ReplacementCost, 4);
            sb.AppendDouble(obj.ExpectedLife, 4);
            sb.Append(obj.DurationUnit);
            sb.AppendDouble(obj.NominalLength, 4);
            sb.AppendDouble(obj.NominalWidth, 4);
            sb.AppendDouble(obj.NominalHeight, 4);
            sb.Append(obj.Shape);
            sb.Append(obj.Size);
            sb.Append(obj.Color);
            sb.Append(obj.Finish);
            sb.Append(obj.Grade);
            sb.Append(obj.Material);
            sb.Append(obj.Constituents);
            sb.Append(obj.Features);
            sb.Append(obj.AccessibilityPerformance);
            sb.Append(obj.CodePerformance);
            sb.Append(obj.SustainabilityPerformance);

            return sb.ToString();
        } 
        #endregion
    }
}
