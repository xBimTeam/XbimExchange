using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using Xbim.COBie.Contracts;

namespace Xbim.COBie
{
    [Serializable()]
    public class COBieWorkbook : List<ICOBieSheet<COBieRow>> 
    {
        public ICOBieSheet<COBieRow> this[string sheetName]
        {
            get
            {
                return this.Where(r => sheetName.Equals(r.SheetName)).FirstOrDefault();
            }
        }


        internal void CreateIndices()
        {
            foreach (ICOBieSheet<COBieRow> item in this)
            {
                item.BuildIndices();
            }
        }

        internal void SetInitialHashCode()
        {
            foreach (ICOBieSheet<COBieRow> item in this)
            {
                item.SetRowsHashCode();
            }
        }

        
        /// <summary>
        /// Runs validation rules on each sheet and updates the Errors collection
        /// on each sheet.
        /// </summary>
        ///<param name="errorRowIdx">excel sheet = ErrorRowIndexBase.RowTwo, datasets = ErrorRowIndexBase.RowOne </param>
        public void Validate( ErrorRowIndexBase errorRowIdx, ICOBieValidationTemplate ValidationTemplate = null, Action<int> progressCallback = null) //default for excel row index's on error rows
        {
            // Enumerates the sheets and validates each
            foreach (var sheet in this)
            {
                if (sheet.SheetName != Constants.WORKSHEET_PICKLISTS) //skip validation on pick list
                {
                    if (ValidationTemplate != null)
                    {
                        sheet.Validate(this, errorRowIdx, ValidationTemplate.Sheet[sheet.SheetName]);
                    }
                    else { 
                        sheet.Validate(this, errorRowIdx, null); 
                    }
                }

                // Progress bar support
                if (progressCallback != null)
                {
                    // Call-back with the index of the last processed sheet
                    progressCallback(this.IndexOf(sheet));
                }
            }
        }
        
        
        /// <summary>
        /// Role Merge
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileRoles">bit fields enumeration to hold all the roles in one place using bitwise AND, OR, EXCLUSIVE OR</param>
        public void ValidateRoles(XbimModel model, COBieMergeRoles fileRoles) 
        {
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif
            if (fileRoles != COBieMergeRoles.Unknown) //if role is a single value of unknown then do no merging
            {
                var sheet = this[Constants.WORKSHEET_COMPONENT];
                List<string> typeGlobalIds = sheet.ValidateComponentMerge(model, fileRoles);

                sheet = this[Constants.WORKSHEET_TYPE];
                int remNo = sheet.ValidateTypeMerge(typeGlobalIds);

                sheet = this[Constants.WORKSHEET_COMPONENT];
                List<string> keyList = new List<string>();
                List<string> nameList = new List<string>();
                if (sheet.RemovedRows != null)
                {
                    foreach (var item in sheet.RemovedRows)
                    {
                        string sheetName = "Component";
                        COBieColumn colName = item.ParentSheet.Columns.Where(c => c.Value.ColumnName == "Name").Select(c => c.Value).FirstOrDefault();
                        var name = item[colName.ColumnOrder].CellValue;
                        nameList.Add(name);
                        keyList.Add(sheetName + name);
                    } 

                    sheet = this[Constants.WORKSHEET_ATTRIBUTE];
                    int attRemNo = sheet.ValidateAttributeMerge(keyList);

                    sheet = this[Constants.WORKSHEET_SYSTEM];
                    int systemRemNo = sheet.ValidateSystemMerge(nameList);
                }

                
            }

#if DEBUG
            timer.Stop();
            Console.WriteLine(String.Format("Time to generate Role Merge data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
#endif
        }
    }
}
