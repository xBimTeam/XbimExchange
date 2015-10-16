using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class RepresentationCompareKey : CompareEqRule<Representation>, IEqualityComparer<Representation>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public RepresentationCompareKey() : base()
        {

        }
        /// <summary>
        /// call constructo base to set IgnoreCase flag and CompareType
        /// </summary>
        public RepresentationCompareKey(StringComparison eqRule, CompareType compareMethod) : base(eqRule, compareMethod)
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Return Name filed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Name string</returns>
        protected override string BuildName(Representation obj)
        {
            return (obj.Name == null) ? string.Empty : obj.Name;
        }

        /// <summary>
        /// Get Best Fields to ID object, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of key fields concat together</returns>
        protected override string BuildKey(Representation obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append((obj.Categories != null && obj.Categories.FirstOrDefault() != null) ? obj.Categories.First().CategoryString : string.Empty);
            
            return sb.ToString();
        }

        /// <summary>
        /// All fileds but ignoring any list types as they are merged, called by CompareEqRule.Equals and CompareEqRule.GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>string of all fields concat together</returns>
        protected override string BuildFull(Representation obj)
        {
            sb.Clear();

            sb.Append(obj.Name);
            sb.Append(obj.Description);
            sb.AppendDouble(obj.X, 4);
            sb.AppendDouble(obj.Y, 4);
            sb.AppendDouble(obj.Z, 4);
            sb.AppendDouble(obj.SizeX, 4);
            sb.AppendDouble(obj.SizeY, 4);
            sb.AppendDouble(obj.SizeZ, 4);
            sb.AppendDouble(obj.Yaw, 4);
            sb.AppendDouble(obj.Pitch, 4);
            sb.AppendDouble(obj.Roll, 4);
            sb.Append(obj.RelativeTo);
            sb.Append(obj.LRM);

            return sb.ToString();
        }
        #endregion
       
    }
}
