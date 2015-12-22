using System;
using System.Collections.Generic;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    /// <summary>
    /// Primery Key Comparer for COBie Classes
    /// </summary>
    public class MetadataCompareKey : CompareEqRule<Metadata>, IEqualityComparer<Metadata>
    {
        #region Constructors

        public MetadataCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public MetadataCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }

        #endregion



        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Metadata obj)
        {
            throw new NotImplementedException("Metadata has no Name property, set CompareMethod property to CompareType.Key or CompareType.Full");
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Metadata obj)
        {
            sb.Clear();

            sb.Append(obj.Title);
            sb.Append(obj.Version);
            sb.Append(obj.Release);
            sb.Append(obj.Status);
            sb.Append(obj.Region);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Metadata obj)
        {
            return BuildKey(obj);
        }
        #endregion
        

        
    }
}
