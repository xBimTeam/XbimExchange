using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.COBie
{
    /// <summary>
    /// Class representing relationship between COBie columns in a workbook
    /// </summary>
    public class COBieColumnRelationship
    {
        const string errorMessage = "Expected a fully qualified reference name like 'Sheet.Column'";

        /// <summary>
        /// Constructs a class representing the relationships between a Foreign Key column and its Primary key column elsewhere in the 
        /// workbook
        /// </summary>
        /// <param name="workbook">The COBie Workbook holding all the sheets</param>
        /// <param name="column">The foreign key column</param>
        public COBieColumnRelationship(COBieWorkbook workbook, COBieColumn column)
        {

            if(workbook == null)
                throw new ArgumentNullException("workbook");
            if (column.KeyType != COBieKeyType.ForeignKey && column.KeyType != COBieKeyType.CompoundKey_ForeignKey)
                throw new ArgumentException(String.Format("Column '{0}' is not a foreign key column", column.ColumnName));
            if (string.IsNullOrEmpty(column.ReferenceColumnName))
                throw new ArgumentException(errorMessage);

            string[] sheetRefInfo = column.ReferenceColumnName.Split('.');

            if (sheetRefInfo.Length != 2)
                throw new ArgumentException(errorMessage);

            SheetName = sheetRefInfo[0];
            ColumnName = sheetRefInfo[1];
            Sheet = workbook[SheetName];

            if(Sheet == null)
                throw new ArgumentException(String.Format("Sheet '{0}' was not found in the workbook", SheetName));

            if (Sheet.Indices.ContainsKey(ColumnName) == false)
                throw new ArgumentException(String.Format("Column '{0}' was not found in the '{1}' workbook", ColumnName, SheetName));

        }

        public string SheetName
        {
            get;
            private set;
        }

        public string ColumnName
        {
            get;
            private set;
        }

        public ICOBieSheet<COBieRow> Sheet
        {
            get;
            private set;
        }

        public bool HasKeyMatch(string foreignKeyValue)
        {
            return (Sheet.Indices[ColumnName].Contains(foreignKeyValue.Trim(), StringComparer.OrdinalIgnoreCase));
        }

        public bool HasPartialMatch(string foreignKeyValue, char separator)
        {
            foreignKeyValue = foreignKeyValue.Trim();

            //check both sides of : for match
            string[] components = foreignKeyValue.Split(separator);
            string first = components[0].Trim();
            string last = components[1].Trim();

            var index = Sheet.Indices[ColumnName];

            // First look for cases where the following matches:
            //  FK    11-11 11 14:Exhibition Hall
            //  PK    11-11 11 14 : Exhibition Hall

            var nearMatch = index.Where(s => { 
                var split = s.Split(':'); 
                return ((split.Last().Trim().Equals(last, StringComparison.OrdinalIgnoreCase)) && 
                    (split.First().Trim().Equals(first, StringComparison.OrdinalIgnoreCase))); 
            });
            if (nearMatch.Any())
                return true;

                // Then look for case where this matches:
            //  FK    11-11 11 14  (OR Exhibition Hall)
            //  PK    11-11 11 14 : Exhibition Hall

            return index.Where(s => { 
                var split = s.Split(':'); 
                return ((split.Last().Trim().Equals(foreignKeyValue, StringComparison.OrdinalIgnoreCase)) || 
                    (split.First().Trim().Equals(foreignKeyValue, StringComparison.OrdinalIgnoreCase))); 
            }).Any(); 
        }
    }
}
