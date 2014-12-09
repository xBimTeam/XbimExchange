using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Xbim.COBie
{
    [Serializable()]
    public class COBieColumn
    {
        #region Properties
        public string ColumnName { get; private set; }
        public int ColumnLength { get; private set; }
        public int ColumnOrder { get; private set; }
        public COBieAllowedType AllowedType { get; private set; }
        public COBieAttributeState AttributeState { get; private set; }
        public COBieKeyType KeyType { get; private set; }
        public string ReferenceColumnName { get; private set; }
        public bool AllowsMultipleValues { get; private set; }
        public List<string> Aliases { get; private set; }
        public PropertyInfo PropertyInfo { get; private set;}
        #endregion
        /// <summary>
        /// Constructor for COBieColumn
        /// </summary>
        /// <param name="propInfo">PropertyInfo</param>
        /// <param name="attr">COBieAttributes</param>
        /// <param name="aliases">List of strings</param>
        public COBieColumn(PropertyInfo propInfo, COBieAttributes attr, List<string> aliases)
        {
            PropertyInfo = propInfo;
            ColumnName = attr.ColumnName;
            ColumnLength = attr.MaxLength;
            AllowedType = attr.AllowedType;
            AttributeState = attr.State;
            ColumnOrder = attr.Order;
            KeyType = attr.KeyType;
            Aliases = aliases;
            ReferenceColumnName = attr.ReferenceColumnName;
            AllowsMultipleValues = attr.AllowedMultipleValues;
        }

        #region Methods
        /// <summary>
        /// Determines if this COBieColumn is a match for the supplied column name, using a basic heuristic match
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool IsMatch(string columnName)
        {
            if (IsMatchImpl(columnName))
                return true;

            if(IsMatchImpl(StripPunctuation(columnName)))
                return true;

            string singular = MakeSingular(columnName);
            if (singular != columnName)
            {
                // call back into our self if we are an obvious plural
                return IsMatch(singular);
            }

            return false;
        }

        
        private bool IsMatchImpl(string sourceName)
        {
            // Straight match, ignoring case
            if (String.Compare(sourceName, ColumnName, true) == 0)
            {
                return true;
            }
            // Check against known aliases. e.g. covers languages differences such as Colour/Color
            if (Aliases != null)
            {
                foreach (string alias in Aliases)
                {
                    if (String.Compare(sourceName, alias, true) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all punctuation and white space
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string StripPunctuation(string columnName)
        {
            var r = from ch in columnName
                   where (!Char.IsPunctuation(ch) && !Char.IsWhiteSpace(ch))
                   select ch;

            return new string(r.ToArray());
        }

        
        private string MakeSingular(string columnName)
        {
            // TODO: make less naive!
            if (columnName.EndsWith("s"))
                return (columnName.TrimEnd('s'));
            else
                return columnName;
        }
        #endregion
        

    }
}
