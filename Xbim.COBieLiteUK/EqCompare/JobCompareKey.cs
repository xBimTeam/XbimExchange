using System;
using System.Collections.Generic;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class JobCompareKey : CompareEqRule<Job>, IEqualityComparer<Job>
    {
        #region Constructors
        public JobCompareKey() : base()
        {

        }
        public JobCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }
        #endregion


        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Job obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Job obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.TaskNumber);

            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Job obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.Append(obj.Status);
            sb.AppendDouble(obj.Duration, 4);
            sb.Append(obj.DurationUnit);
            sb.AppendDouble(obj.Start, 4);
            sb.Append(obj.TaskStartUnit);
            sb.AppendDouble(obj.Frequency, 4);
            sb.Append(obj.FrequencyUnit);
            sb.Append(obj.TaskNumber);


            return sb.ToString();
        }
        #endregion
        
    }
}
