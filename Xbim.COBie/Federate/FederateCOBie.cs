using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xbim.COBie.Contracts;
using Xbim.Common;

namespace Xbim.COBie.Federate
{
    public class FederateCOBie : ICOBieFederate, ICOBieContext
    {
        /// <summary>
        /// COBieProgress
        /// </summary>
        public COBieProgress cOBieProgress { get; set; } 
        
        /// <summary>
        /// set the error reporting to be either one (first row is labeled one) or 
        /// two based (first row is labeled two) on the rows of the tables/excel sheet
        /// </summary>
        public ErrorRowIndexBase ErrorRowStartIndex { get; set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="progressHandler">Report Delegate</param>
        public FederateCOBie(ReportProgressDelegate progressHandler = null)
        {
            if (progressHandler != null)
            {
                _progress = progressHandler;
                this.ProgressStatus += progressHandler;
            }

            cOBieProgress = new COBieProgress(this);
            ErrorRowStartIndex = ErrorRowIndexBase.RowTwo; //default for excel sheet
        }

        private ReportProgressDelegate _progress = null;

        public event ReportProgressDelegate ProgressStatus;

        /// <summary>
        /// Updates the delegates with the current percentage complete
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="current"></param>
        public void UpdateStatus(string message, int total = 0, int current = 0)
        {
            decimal percent = 0;
            if (total != 0 && current > 0)
            {
                message = string.Format("{0} [{1}/{2}]", message, current, total);
                percent = (decimal)current / total * 100;
                if ((percent > 0) && (percent < 1))
                {
                    percent = 1; //stops display of status bar in text list
                }
            }
            if (ProgressStatus != null)
                ProgressStatus((int)percent, message);

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_progress != null)
            {
                ProgressStatus -= _progress;
                _progress = null;
            }
        }

        /// <summary>
        /// Merge the workbooks together into a single federated workbook
        /// </summary>
        /// <param name="workbooks">List of Workbooks</param>
        /// <returns>Federated COBie Workbook</returns>
        public COBieWorkbook Merge(List<COBieWorkbook> workbooks)
        {
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif
            COBieWorkbook fedWorkbook = new COBieWorkbook();
            int index = 0;
            foreach (COBieWorkbook workbook in workbooks)
            {
                index++;
                foreach (ICOBieSheet<COBieRow> worksheet in workbook)
                {
                    ICOBieSheet<COBieRow> fedSheet = fedWorkbook[worksheet.SheetName];
                    if (fedSheet == null)
                    {                       
                        fedSheet = worksheet.CreateEmptySheet();  //create sheet as it does not exist
                        fedWorkbook.Add(fedSheet);
                    }
                    else if (worksheet.SheetName == Constants.WORKSHEET_PICKLISTS)
                    {
                        continue; //we have already added a pick list so jump on to next sheet
                    }

                    //copy the removed rows to the federated sheet
                    if (worksheet.RemovedRows != null)
                    {
                        foreach (var row in worksheet.RemovedRows)
                        {
                            fedSheet.AddRemovedRow(row);
                        }
                    }
                    

                    cOBieProgress.Initialise(string.Format("Federate workbook {0}, Merging {1}", index, worksheet.SheetName), worksheet.RowCount);
                    bool copy = (worksheet.SheetName == Constants.WORKSHEET_COORDINATE);
                    //copy rows to federated sheet, or add to removed rows if duplicate
                    for (int i = 0; i < worksheet.RowCount; i++)
                    {
                        cOBieProgress.IncrementAndUpdate();
                        if (copy) //copy the coordinate sheet without any removals
                        {
                            fedSheet.AddRow(worksheet[i]);
                        }
                        else
                        {
                            string hash = worksheet[i].RowMergeHashValue;
                            if (!fedSheet.HasMergeHashCode(hash, true))
                            {
                                fedSheet.AddRow(worksheet[i]);
                            }
                            else
                            {
                                fedSheet.AddRemovedRow(worksheet[i]);
                            }
                        }
                    }
                    //cOBieProgress.Finalise();
                }
            }
            
            fedWorkbook.SetInitialHashCode();//set the initial row hash value to compare against for row changes
            PopulateErrors(fedWorkbook);
#if DEBUG
            timer.Stop();
            Console.WriteLine(String.Format("Time to federate COBie = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
#endif
            return fedWorkbook;
        }

        /// <summary>
        /// Build Error lists on workbook
        /// </summary>
        /// <param name="fedWorkBook"></param>
        private void PopulateErrors(COBieWorkbook fedWorkBook)
        {

            cOBieProgress.Initialise("Validating Workbooks", fedWorkBook.Count, 0);
            cOBieProgress.ReportMessage("Building Indices...");
            fedWorkBook.CreateIndices();
            cOBieProgress.ReportMessage("Building Indices...Finished");

            // Validate the workbook
            cOBieProgress.ReportMessage("Starting Validation...");
            fedWorkBook.Validate(ErrorRowStartIndex, null, (lastProcessedSheetIndex) =>
            {
                // When each sheet has been processed, increment the progress bar
                cOBieProgress.IncrementAndUpdate();
            });
            cOBieProgress.ReportMessage("Finished Validation");

            cOBieProgress.Finalise();
        }      
    }
}
