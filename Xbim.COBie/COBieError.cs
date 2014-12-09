using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Xbim.COBie
{
    [Serializable()]
    [DebuggerDisplay("{SheetName}.{FieldName} : {ErrorDescription} - '{FieldValue}' [{Row},{Column}]")]
    public class COBieError
    {
        // this will contain all the cells that are mandatory but not supplied and user needs to fill in
        // as we need to generate spreadsheet even if there is missing mandatory data

        public COBieError()
        {
        }

        public COBieError(string sheetName, string fieldName, string errorDescription)
        {
            SheetName = sheetName;
            FieldName = fieldName;
            ErrorDescription = errorDescription;
        }

        public COBieError(string sheetName, string fieldName, string errorDescription, ErrorTypes errorType, ErrorLevels errorLevel, string initialRowHash, int column = 0, int row = 0)
        {
            SheetName = sheetName;
            FieldName = fieldName;
            ErrorDescription = errorDescription;
            ErrorType = errorType;
            Column = column;
            Row = row;
            InitialRowHash = initialRowHash;
            ErrorLevel = errorLevel;
        }

        public COBieError(string fieldName, string errorDescription)
        {
            FieldName = fieldName;
            ErrorDescription = errorDescription;
        }

        public string SheetName { get; private set; }
        public string FieldName { get; private set; }
        public string ErrorDescription { get; set; }
        public ErrorTypes ErrorType { get; set; }
        public ErrorLevels ErrorLevel { get; set; }

        public string FieldValue { get; set; }
        public int Column { get; set;}
        public int Row { get; set ;}

        public string InitialRowHash { get; private set; }

        public enum ErrorTypes
        {
            Value_Out_of_Bounds,
            AlphaNumeric_Value_Expected,
            Email_Value_Expected,
            ISODate_Value_Expected,
            Numeric_Value_Expected,
            Text_Value_Expected,
            PrimaryKey_Violation,
            Null_ForeignKey_Value,
            ForeignKey_Violation,
            PickList_Violation,
            None
        }

        /// <summary>
        /// Level of error
        /// </summary>
        public enum ErrorLevels
        {
            Error,
            Warning,
            None
        }
        
    }
}
