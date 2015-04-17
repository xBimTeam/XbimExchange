using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Contracts;
using System.Reflection;
using Xbim.COBie.Rows;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Xbim.COBie.Serialisers
{
    public class COBieXLSDeserialiser : ICOBieDeserialiser
    {
        #region Properties
        public string FileName { get; set; }
        public COBieWorkbook WorkBook { get; set; }
        private List<string> SheetNames { get; set; }
        private HSSFWorkbook XlsWorkbook { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor to do all sheets
        /// </summary>
        /// <param name="file"></param>
        public COBieXLSDeserialiser(string file)
        {
            SheetNames = new List<string>();
            WorkBook = new COBieWorkbook();
            FileName = file;
            GetSheetNames();//get all required sheets from Constants class
        }

        /// <summary>
        /// Constructor to do a single sheet
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sheetname"></param>
        public COBieXLSDeserialiser(string file, string sheetname)
            : this(file)
        {
            SheetNames = new List<string>() { sheetname };
        }

        #endregion
        
        #region Methods
        /// <summary>
        /// Uses reflection to extract the WORKSHEET_name from the Constants class
        /// </summary>
        private void GetSheetNames()
        {
            SheetNames = typeof(Constants).GetFields().Where(fld => fld.Name.Contains("WORKSHEET_")).Select(fld => (string)fld.GetValue(null)).ToList();
        }

        /// <summary>
        /// Create the empty COBieSheet to the correct type decided by sheet name
        /// </summary>
        /// <param name="sheetname">Sheet name we want to create</param>
        /// <returns>ICOBieSheet of COBieRow to the correct row type to match the sheet name</returns>
        private ICOBieSheet<COBieRow> GetSheetType(string sheetname)
        {
            switch (sheetname)
            {
                case Constants.WORKSHEET_CONTACT:
                    return new COBieSheet<COBieContactRow>(Constants.WORKSHEET_CONTACT);
                case Constants.WORKSHEET_FACILITY:
                    return new COBieSheet<COBieFacilityRow>(Constants.WORKSHEET_FACILITY);
                case Constants.WORKSHEET_FLOOR:
                    return new COBieSheet<COBieFloorRow>(Constants.WORKSHEET_FLOOR);
                case Constants.WORKSHEET_SPACE:
                    return new COBieSheet<COBieSpaceRow>(Constants.WORKSHEET_SPACE);
                case Constants.WORKSHEET_ZONE:
                    return new COBieSheet<COBieZoneRow>(Constants.WORKSHEET_ZONE);
                case Constants.WORKSHEET_TYPE:
                    return new COBieSheet<COBieTypeRow>(Constants.WORKSHEET_TYPE);
                case Constants.WORKSHEET_COMPONENT:
                    return new COBieSheet<COBieComponentRow>(Constants.WORKSHEET_COMPONENT);
                case Constants.WORKSHEET_SYSTEM:
                    return new COBieSheet<COBieSystemRow>(Constants.WORKSHEET_SYSTEM);
                case Constants.WORKSHEET_ASSEMBLY:
                    return new COBieSheet<COBieAssemblyRow>(Constants.WORKSHEET_ASSEMBLY);
                case Constants.WORKSHEET_CONNECTION:
                    return new COBieSheet<COBieConnectionRow>(Constants.WORKSHEET_CONNECTION);
                case Constants.WORKSHEET_SPARE:
                    return new COBieSheet<COBieSpareRow>(Constants.WORKSHEET_SPARE);
                case Constants.WORKSHEET_RESOURCE:
                    return new COBieSheet<COBieResourceRow>(Constants.WORKSHEET_RESOURCE);
                case Constants.WORKSHEET_JOB:
                    return new COBieSheet<COBieJobRow>(Constants.WORKSHEET_JOB);
                case Constants.WORKSHEET_IMPACT:
                    return new COBieSheet<COBieImpactRow>(Constants.WORKSHEET_IMPACT);
                case Constants.WORKSHEET_DOCUMENT:
                    return new COBieSheet<COBieDocumentRow>(Constants.WORKSHEET_DOCUMENT);
                case Constants.WORKSHEET_ATTRIBUTE:
                    return new COBieSheet<COBieAttributeRow>(Constants.WORKSHEET_ATTRIBUTE);
                case Constants.WORKSHEET_COORDINATE:
                    return new COBieSheet<COBieCoordinateRow>(Constants.WORKSHEET_COORDINATE);
                case Constants.WORKSHEET_ISSUE:
                    return new COBieSheet<COBieIssueRow>(Constants.WORKSHEET_ISSUE);
                case Constants.WORKSHEET_PICKLISTS:
                    return new COBieSheet<COBiePickListsRow>(Constants.WORKSHEET_PICKLISTS);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Read XLS file into the HSSFWorkbook object
        /// </summary>
        private void GetXLSFileData()
        {
            using (FileStream excelFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                XlsWorkbook = new HSSFWorkbook(excelFile, true);
            }
        }

       
        /// <summary>
        /// DeSerialise the date held in the sheet into a COBieWorkbook
        /// </summary>
        /// <returns>COBieWorkbook with date imported from XLS file</returns>
        public COBieWorkbook Deserialise()
        {
            try
            {
                GetXLSFileData(); //Read XLS file into the HSSFWorkbook object

                foreach (string sheetname in SheetNames)
                {
                    ISheet excelSheet = XlsWorkbook.GetSheet(sheetname); //get sheet name in XLS file
                    if (excelSheet != null)
                    {
                        ICOBieSheet<COBieRow> thisSheet = GetSheetType(sheetname); 
                        int COBieColumnCount = thisSheet.Columns.Count;
                        //no checking on Sheet column count to XLS sheet column count, just extract up to the column number in the COBieSheet/Row
                        int rownumber = 0;
                        int columnCount = 0;
                        foreach (IRow row in excelSheet)
                        {
                            if (rownumber == 0) //this will be the headers so get how many we have
                            {
                                foreach (ICell cell in row)
                                {
                                    columnCount++;
                                }
                            }
                            else
                            {
                                bool addRow = false;
                                //check we have some data on the row
                                for (int i = 0; i < columnCount; i++)
                                {
                                    ICell cell = row.GetCell(i);
                                    if ((cell != null) && (cell.CellType != CellType.Blank))
                                    {
                                        addRow = true;
                                        break;
                                    }
                                }
                                //add a none blank row
                                if (addRow)
                                {
                                    COBieRow sheetRow = thisSheet.AddNewRow(); //add a new empty COBie row to the sheet
                                    for (int i = 0; i < thisSheet.Columns.Count(); i++) //changed from columnCount to supported column count of the sheet
                                    {
                                        string cellValue = ""; //default value
                                        ICell cell = row.GetCell(i);
                                        if (cell != null)
                                            switch (cell.CellType)
                                            {
                                                case CellType.String:
                                                    cellValue = cell.StringCellValue;
                                                    break;
                                                case CellType.Numeric:
                                                    if (sheetRow[i].COBieColumn.AllowedType == COBieAllowedType.ISODate)
                                                        cellValue = cell.DateCellValue.ToString(Constants.DATE_FORMAT);
                                                    else if (sheetRow[i].COBieColumn.AllowedType == COBieAllowedType.ISODateTime)
                                                    {
                                                        DateTime date = DateTime.Now;
                                                        try
                                                        {
                                                            date = cell.DateCellValue; 
                                                        }
                                                        catch
                                                        {
                                                            // If we can't read a valid date, just use the current date.
                                                            date = DateTime.Now;    
                                                        }
                                                        cellValue = date.ToString(Constants.DATETIME_FORMAT);
                                                    }
                                                    else
                                                        cellValue = cell.NumericCellValue.ToString();
                                                    break;
                                                case CellType.Boolean:
                                                    cellValue = cell.BooleanCellValue.ToString();
                                                    break;
                                                case CellType.Error:
                                                    cellValue = cell.ErrorCellValue.ToString();
                                                    break;
                                                case CellType.Blank:
                                                case CellType.Formula:
                                                case CellType.Unknown:
                                                    cellValue = cell.StringCellValue;
                                                    break;
                                                default:
                                                    break;
                                            }

                                        if (i < COBieColumnCount) //check we are in the column range of the COBieRow and add value
                                        {
                                            COBieColumn cobieColumn = thisSheet.Columns.Where(idxcol => idxcol.Key == i).Select(idxcol => idxcol.Value).FirstOrDefault();
                                            sheetRow[i] = new COBieCell(cellValue, cobieColumn);
                                        }
                                    }
                                }
                            }
                            rownumber++;
                        }
                        WorkBook.Add(thisSheet);
                    }
                    
                }
            }
            catch (FileNotFoundException)
            {
                //TODO: Report this
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            WorkBook.CreateIndices();
            return WorkBook;
        }
        #endregion

    }
}
