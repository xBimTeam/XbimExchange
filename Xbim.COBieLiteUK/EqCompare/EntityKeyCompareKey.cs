using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    class EntityKeyCompareKey : CompareEqRule<IEntityKey>, IEqualityComparer<IEntityKey>
    {

        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        public EntityKeyCompareKey() : base()
        {

        }

        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public EntityKeyCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(IEntityKey obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(IEntityKey obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(IEntityKey obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }
        #endregion

    }
}
