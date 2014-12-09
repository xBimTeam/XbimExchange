using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xbim.COBie.Contracts;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions.Interfaces;

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
                        fedSheet = CreateSheet(worksheet.SheetName); //create sheet as it does not exist
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
            try
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
            catch (Exception)
            {
                // TODO: Handle
                throw;
            }
        }

        /// <summary>
        /// Add Row to sheet
        /// </summary>
        /// <param name="sheetname">Sheet name we want to add row too</param>
        /// <returns>void</returns>
        //private void AddRow(ICOBieSheet<COBieRow> sheet, COBieRow row)
        //{
        //    switch (sheet.SheetName)
        //    {
        //        case Constants.WORKSHEET_CONTACT:
        //            ((COBieSheet<COBieContactRow>)sheet).AddRow((COBieContactRow)row);
        //            break;
        //        case Constants.WORKSHEET_FACILITY:
        //            ((COBieSheet<COBieFacilityRow>)sheet).AddRow((COBieFacilityRow)row);
        //            break;
        //        case Constants.WORKSHEET_FLOOR:
        //            ((COBieSheet<COBieFloorRow>)sheet).AddRow((COBieFloorRow)row);
        //            break;
        //        case Constants.WORKSHEET_SPACE:
        //            ((COBieSheet<COBieSpaceRow>)sheet).AddRow((COBieSpaceRow)row);
        //            break;
        //        case Constants.WORKSHEET_ZONE:
        //            ((COBieSheet<COBieZoneRow>)sheet).AddRow((COBieZoneRow)row);
        //            break;
        //        case Constants.WORKSHEET_TYPE:
        //            ((COBieSheet<COBieTypeRow>)sheet).AddRow((COBieTypeRow)row);
        //            break;
        //        case Constants.WORKSHEET_COMPONENT:
        //            ((COBieSheet<COBieComponentRow>)sheet).AddRow((COBieComponentRow)row);
        //            break;
        //        case Constants.WORKSHEET_SYSTEM:
        //            ((COBieSheet<COBieSystemRow>)sheet).AddRow((COBieSystemRow)row);
        //            break;
        //        case Constants.WORKSHEET_ASSEMBLY:
        //            ((COBieSheet<COBieAssemblyRow>)sheet).AddRow((COBieAssemblyRow)row);
        //            break;
        //        case Constants.WORKSHEET_CONNECTION:
        //            ((COBieSheet<COBieConnectionRow>)sheet).AddRow((COBieConnectionRow)row);
        //            break;
        //        case Constants.WORKSHEET_SPARE:
        //            ((COBieSheet<COBieSpareRow>)sheet).AddRow((COBieSpareRow)row);
        //            break;
        //        case Constants.WORKSHEET_RESOURCE:
        //            ((COBieSheet<COBieResourceRow>)sheet).AddRow((COBieResourceRow)row);
        //            break;
        //        case Constants.WORKSHEET_JOB:
        //            ((COBieSheet<COBieJobRow>)sheet).AddRow((COBieJobRow)row);
        //            break;
        //        case Constants.WORKSHEET_IMPACT:
        //            ((COBieSheet<COBieImpactRow>)sheet).AddRow((COBieImpactRow)row);
        //            break;
        //        case Constants.WORKSHEET_DOCUMENT:
        //            ((COBieSheet<COBieDocumentRow>)sheet).AddRow((COBieDocumentRow)row);
        //            break;
        //        case Constants.WORKSHEET_ATTRIBUTE:
        //            ((COBieSheet<COBieAttributeRow>)sheet).AddRow((COBieAttributeRow)row);
        //            break;
        //        case Constants.WORKSHEET_COORDINATE:
        //            ((COBieSheet<COBieCoordinateRow>)sheet).AddRow((COBieCoordinateRow)row);
        //            break;
        //        case Constants.WORKSHEET_ISSUE:
        //            ((COBieSheet<COBieIssueRow>)sheet).AddRow((COBieIssueRow)row);
        //            break;
        //        case Constants.WORKSHEET_PICKLISTS:
        //            ((COBieSheet<COBiePickListsRow>)sheet).AddRow((COBiePickListsRow)row);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// Add Row to sheet
        /// </summary>
        /// <param name="sheetname">Sheet name we want to add row too</param>
        /// <returns>void</returns>
        //private void AddRemovedRow(ICOBieSheet<COBieRow> sheet, COBieRow row)
        //{
        //    switch (sheet.SheetName)
        //    {
        //        case Constants.WORKSHEET_CONTACT:
        //            ((COBieSheet<COBieContactRow>)sheet).AddRemovedRow((COBieContactRow)row);
        //            break;
        //        case Constants.WORKSHEET_FACILITY:
        //            ((COBieSheet<COBieFacilityRow>)sheet).AddRemovedRow((COBieFacilityRow)row);
        //            break;
        //        case Constants.WORKSHEET_FLOOR:
        //            ((COBieSheet<COBieFloorRow>)sheet).AddRemovedRow((COBieFloorRow)row);
        //            break;
        //        case Constants.WORKSHEET_SPACE:
        //            ((COBieSheet<COBieSpaceRow>)sheet).AddRemovedRow((COBieSpaceRow)row);
        //            break;
        //        case Constants.WORKSHEET_ZONE:
        //            ((COBieSheet<COBieZoneRow>)sheet).AddRemovedRow((COBieZoneRow)row);
        //            break;
        //        case Constants.WORKSHEET_TYPE:
        //            ((COBieSheet<COBieTypeRow>)sheet).AddRemovedRow((COBieTypeRow)row);
        //            break;
        //        case Constants.WORKSHEET_COMPONENT:
        //            ((COBieSheet<COBieComponentRow>)sheet).AddRemovedRow((COBieComponentRow)row);
        //            break;
        //        case Constants.WORKSHEET_SYSTEM:
        //            ((COBieSheet<COBieSystemRow>)sheet).AddRemovedRow((COBieSystemRow)row);
        //            break;
        //        case Constants.WORKSHEET_ASSEMBLY:
        //            ((COBieSheet<COBieAssemblyRow>)sheet).AddRemovedRow((COBieAssemblyRow)row);
        //            break;
        //        case Constants.WORKSHEET_CONNECTION:
        //            ((COBieSheet<COBieConnectionRow>)sheet).AddRemovedRow((COBieConnectionRow)row);
        //            break;
        //        case Constants.WORKSHEET_SPARE:
        //            ((COBieSheet<COBieSpareRow>)sheet).AddRemovedRow((COBieSpareRow)row);
        //            break;
        //        case Constants.WORKSHEET_RESOURCE:
        //            ((COBieSheet<COBieResourceRow>)sheet).AddRemovedRow((COBieResourceRow)row);
        //            break;
        //        case Constants.WORKSHEET_JOB:
        //            ((COBieSheet<COBieJobRow>)sheet).AddRemovedRow((COBieJobRow)row);
        //            break;
        //        case Constants.WORKSHEET_IMPACT:
        //            ((COBieSheet<COBieImpactRow>)sheet).AddRemovedRow((COBieImpactRow)row);
        //            break;
        //        case Constants.WORKSHEET_DOCUMENT:
        //            ((COBieSheet<COBieDocumentRow>)sheet).AddRemovedRow((COBieDocumentRow)row);
        //            break;
        //        case Constants.WORKSHEET_ATTRIBUTE:
        //            ((COBieSheet<COBieAttributeRow>)sheet).AddRemovedRow((COBieAttributeRow)row);
        //            break;
        //        case Constants.WORKSHEET_COORDINATE:
        //            ((COBieSheet<COBieCoordinateRow>)sheet).AddRemovedRow((COBieCoordinateRow)row);
        //            break;
        //        case Constants.WORKSHEET_ISSUE:
        //            ((COBieSheet<COBieIssueRow>)sheet).AddRemovedRow((COBieIssueRow)row);
        //            break;
        //        case Constants.WORKSHEET_PICKLISTS:
        //            ((COBieSheet<COBiePickListsRow>)sheet).AddRemovedRow((COBiePickListsRow)row);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// Create the empty COBieSheet<COBieRow> to the correct type decided by sheet name
        /// </summary>
        /// <param name="sheetname">Sheet name we want to create</param>
        /// <returns>ICOBieSheet of COBieRow to the correct row type to match the sheet name</returns>
        private ICOBieSheet<COBieRow> CreateSheet(string sheetname)
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

        
    }
}
