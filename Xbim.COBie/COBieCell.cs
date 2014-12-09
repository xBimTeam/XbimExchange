using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Xbim.COBie
{
    public class COBieCell
    {

        public COBieCell(string cellValue, COBieColumn cobieColumn)
        {
            CellValue = cellValue;
            COBieColumn = cobieColumn;
        }

        public string CellValue { get; private set; }
        public List<string> CellValues 
        {
            get
            {
                if (COBieColumn.AllowsMultipleValues)
                {
                    return CellValue.Split(new char [] {','}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
                else
                {
                    return new List<string>() { CellValue };
                }
            }
        }

        public COBieColumn COBieColumn { get; private set; }

        public bool IsAlphaNumeric()
        {
            return RegExAlphaNumeric.IsMatch(this.CellValue);
        }

        public bool IsDateTime()
        {
            DateTime dt;
            return DateTime.TryParse(this.CellValue, out dt);
        }

        public bool IsNumeric()
        {
            double d;
            return double.TryParse(this.CellValue, out d);
        }

        public bool IsText()
        {
            return !string.IsNullOrWhiteSpace(this.CellValue);
        }

        public bool IsEmailAddress()
        {
            bool isEmail = false;
            try
            {
                if (this.CellValue == Constants.DEFAULT_STRING) return isEmail; //false
                System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(this.CellValue);
                isEmail = true;
            }
            catch (FormatException)
            {
                // Do nothing
            }
            return isEmail;
        }

        static Regex RegExAlphaNumeric = new Regex(@"\w|[%]");
        
    }
}
