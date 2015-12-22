using System;
using System.Collections.Generic;
using System.Text;

namespace Xbim.COBie.EqCompare
{
    /// <summary>
    /// Control case sensertive on strings compares
    /// </summary>
    public abstract class CompareEqRule<T> : ICompareEqRule
    {
        #region Properties

        /// <summary>
        /// Save hash values for reuse
        /// </summary>
        Dictionary<T, int> HashMap = new Dictionary<T, int>();

        
        /// <summary>
        /// String Builder
        /// </summary>
        protected StringBuilder sb
        { get; set; }

        /// <summary>
        /// String compare rule
        /// </summary>
        private StringComparison eqRule;

        public StringComparison EqRule
        {
            get { return eqRule; }
            set
            {
                eqRule = value;
                IgnoreCase = (eqRule == StringComparison.OrdinalIgnoreCase || eqRule == StringComparison.InvariantCultureIgnoreCase || eqRule == StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Set What to compare, Neam, Key or Full
        /// </summary>
        private CompareType compareMethod;

        public CompareType CompareMethod
        {
            get { return compareMethod; }
            set
            {
                if (compareMethod != value)
                {
                    HashMap.Clear();//hash values would change, so clear
                }
                compareMethod = value;
            }
        }

        /// <summary>
        /// Ignore Case on string compares
        /// </summary>
        private bool ignoreCase;

        public bool IgnoreCase
        {
            get { return ignoreCase; }
            set
            {
                if (ignoreCase != value)
                {
                    HashMap.Clear(); //hash values would change, so clear
                }
                ignoreCase = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CompareEqRule()
        {
            sb = new StringBuilder();
            CompareMethod = CompareType.Key;
            EqRule = StringComparison.CurrentCulture;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eqRule">StringComparison</param>
        /// <param name="compareMethod">CompareType</param>
        public CompareEqRule(StringComparison eqRule, CompareType compareMethod)
        {
            sb = new StringBuilder();
            EqRule = eqRule;
            CompareMethod = compareMethod;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Equals method
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            if (Object.ReferenceEquals(x, y)) //same instance
                return true;


            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) //one is null
                return false;
            //if (HashMap.ContainsKey(x) && HashMap.ContainsKey(y) && (HashMap[x] == HashMap[y]))
            //{
            //    return true;
            //}
            switch (CompareMethod)
            {
                case CompareType.Name:
                    return BuildName(x).Equals(BuildName(y), EqRule);
                case CompareType.Key:
                    return BuildKey(x).Equals(BuildKey(y), EqRule);
                case CompareType.Full:
                    return BuildFull(x).Equals(BuildFull(y), EqRule);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Get Hash Code method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return 0;
            if (HashMap.ContainsKey(obj))
                return HashMap[obj];

            int hashcode = 0;
            string hashString = string.Empty;
            
            //hashString = BuildKey(obj);
            switch (CompareMethod)
            {
                case CompareType.Name:
                    hashString = BuildName(obj);
                    break;
                case CompareType.Key:
                    hashString = BuildKey(obj);
                    break;
                case CompareType.Full:
                    hashString = BuildFull(obj);
                    break;
                default:
                    break;
            }

            if (IgnoreCase)
            {
                hashcode = hashString.ToUpper().GetHashCode();
            }
            else
            {
                hashcode = hashString.GetHashCode();
            }

            int primeNo = 16777619;

            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = (hash * primeNo) + hashcode;
                HashMap[obj] = hash;
                return hash;
            }
        }

        /// <summary>
        /// Abstract methods used in Equals and GetHashCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected abstract string BuildName(T obj);
        protected abstract string BuildKey(T obj);
        protected abstract string BuildFull(T obj); 
        #endregion
    }

    /// <summary>
    /// Compare Methods Enum
    /// </summary>
    public enum CompareType
    {
        Name,
        Key,
        Full
    }
}
