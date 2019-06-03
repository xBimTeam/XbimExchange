using System.Diagnostics;
using Xbim.Common;

namespace Xbim.CobieLiteUk
{
    public class ProgressReporter
    {
        #region Properties

        /// <summary>
        /// Elapsed time in Milliseconds since last update to progress
        /// </summary>
        private long LastUpdate
        { get; set; }

        /// <summary>
        /// Elapsed time in Milliseconds before another update to progress is allowed
        /// </summary>
       public long UpdateInterval
        { get; set; }
        /// <summary>
        /// Timer
        /// </summary>
        private Stopwatch TimeMonitor
        { get; set; }

        /// <summary>
        /// Current number of records 
        /// </summary>
        private int CurrentRecord
        { get; set; }

        /// <summary>
        /// Total number of records
        /// </summary>
        private int TotalRecords
        { get; set; }

        /// <summary>
        /// Current % value for progress
        /// </summary>
        public double ProgressBarValue
        { get; private set; }

        /// <summary>
        /// Base value for the current stage
        /// </summary>
        public double ProgressBarBaseValue
        { get; private set; }

        /// <summary>
        /// % of bar the current stage is set to fill
        /// </summary>
        private double PercentageOfBar
        { get; set; }

        /// <summary>
        /// Message to display
        /// </summary>
        private string Message
        { get; set; }

        /// <summary>
        /// Progress report delegate, function to call
        /// </summary>
        public ReportProgressDelegate Progress
        { get; set; }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public ProgressReporter()
        {
            TimeMonitor = new Stopwatch();
            UpdateInterval       = 1;
            LastUpdate           = 0;
            ProgressBarValue     = 0; //current value of progress bar
            ProgressBarBaseValue = 0; //base value of progress bar for the next stage 
            PercentageOfBar      = 100; //all of the progress bar
        }

        /// <summary>
        /// Set delegate constructor
        /// </summary>
        /// <param name="progressFun">Function to pass to delegate</param>
        public ProgressReporter(ReportProgressDelegate progressFun) : base()
        {

            if (progressFun != null)
            {
                Progress = progressFun;
            }
        }

        /// <summary>
        /// Start the progress from zero
        /// </summary>
        /// <param name="totalRecords">Number of records we are going to progress through</param>
        /// <param name="percentageOfBar">% of progress we are to fill on first stage</param>
        /// <param name="message">Message to display</param>
        public void Reset (int totalRecords = 0, double percentageOfBar = 100, string message = null)
        {
            ProgressBarValue = 0; //at start of the progress bar
            TimeMonitor.Start();
            NextStage(totalRecords, percentageOfBar, message);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRecords">Number of records we are going to progress through</param>
        /// <param name="percentageOfBar">% of progress we are to fill on this stage</param>
        /// <param name="message">Message to display</param>
        public void NextStage(int totalRecords = 0, double percentageOfBar = 100 , string message = null)
        {
            if (Progress != null)
            {
                ProgressBarBaseValue = ProgressBarValue;
                if (!string.IsNullOrEmpty(message)) //change message
                {
                    Message = message;
                }
                
                CurrentRecord = 0;
                PercentageOfBar =  percentageOfBar - ProgressBarBaseValue;
                TotalRecords = totalRecords;
                UpdateStatus(true); 
            }
        }

        /// <summary>
        /// We are finished with progress
        /// </summary>
        /// <param name="delay">Delay time in Milli Seconds</param>
        public void Finalise(int delay = 0)
        {
            if (Progress != null)
            {
                Message += "... Finished";
                ProgressBarValue = 100;
                Progress((int)ProgressBarValue, Message);
                //add delay to allow progress indicter to catch up
                if (delay > 0)
                {
                    var delayNo = TimeMonitor.ElapsedMilliseconds + delay;
                    while (TimeMonitor.ElapsedMilliseconds < delayNo && TimeMonitor.IsRunning)
                    {

                    }
                }
                TimeMonitor.Stop();
            }
        }

        /// <summary>
        /// We are counting or progress
        /// </summary>
        public void IncrementAndUpdate()
        {
            if (Progress != null)
            {
                CurrentRecord++;
                UpdateStatus();
            }
        }

        /// <summary>
        /// Report just a message
        /// </summary>
        /// <param name="message"></param>
        public void ReportMessage(string message)
        {
            if (Progress != null)
            {
                Message = message;
                UpdateProgress(true);
            }
        }
        
        /// <summary>
        /// Updates the context with the latest status
        /// </summary>
        /// <remarks>Only updates every few hundred milliseconds unless the Update is forced.</remarks>
        /// <param name="forceUpdate">do not check update time to update</param>
        private void UpdateStatus(bool forceUpdate = false)
        {
            if (forceUpdate || UpdateIntervalPassed())
            {
                UpdateProgress(false);
                if (TimeMonitor.IsRunning)
                {
                    LastUpdate = TimeMonitor.ElapsedMilliseconds;
                }
            }
        }

        /// <summary>
        /// Check we  how long since last update
        /// </summary>
        /// <returns></returns>
        private bool UpdateIntervalPassed()
        {
            // Always update if timer not started 
            if (!TimeMonitor.IsRunning)
                return true;
            else
                return (TimeMonitor.ElapsedMilliseconds > (LastUpdate + UpdateInterval));

        }

        
        /// <summary>
        /// update progress
        /// </summary>
        /// <param name="msgOnly">if true only update message, but will set progress to 0</param>
        private void UpdateProgress(bool msgOnly)
        {
            if (Progress != null)
            {
                if (msgOnly)
                {
                    Progress(0, Message); //message only
                }
                else if (TotalRecords != 0 && CurrentRecord > 0)
                {
                    //var message = string.Format("{0} [{1}/{2}]", Message, CurrentRecord, TotalRecords);
                    double percent   = (((double)CurrentRecord / TotalRecords) * 100) * (PercentageOfBar / 100.0);
                    ProgressBarValue = ProgressBarBaseValue + percent;
                    if (ProgressBarValue > 100.0)
                    {
                        ProgressBarValue = 100;
                        //throw new ArgumentOutOfRangeException("Cannot be over 100");
                    }
                    if (ProgressBarValue < 1) ProgressBarValue = 1; //stops display of status bar in text list 
                    Progress((int)ProgressBarValue, Message);
                   
                }
                
            }
        }
    }
}
