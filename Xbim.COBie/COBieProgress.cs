using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Xbim.COBie
{
    /// <summary>
    /// A class to help send progress updates back to the calling context
    /// </summary>
    public class COBieProgress
    {

        private COBieProgress() {}

        public COBieProgress(ICOBieContext context)
        {
            Context = context;
        }

        #region Private fields

        bool _initialised = false;
        Stopwatch _timer = new Stopwatch();
        long _lastUpdate = 0;
        int UpdateInterval = 500;       // Update every 500ms

        #endregion

        ICOBieContext Context
        {
            get;
            set;
        }

        
        public void Initialise(string message, int totalRecords = 0, int currentRecord = 0) 
        {
            Message = message;
            TotalRecords = totalRecords;
            CurrentRecord = currentRecord;
            _initialised = true;

            _timer.Start();

            UpdateStatus(true);
        }

        public void Finalise()
        {
            if (_initialised == true)
            {
                Message += "... Finished";
                UpdateStatus(true);
            }
        }

        public int CurrentRecord
        { 
            get; 
            set; 
        }

        public int TotalRecords
        { 
            get; 
            set; 
        }

        public string Message
        {
            get;
            set;
        }

        public void Increment()
        {
            CurrentRecord++;
        }

        public void IncrementAndUpdate()
        {
            Increment();
            UpdateStatus();
        }


        /// <summary>
        /// Updates the context with the latest status
        /// </summary>
        /// <remarks>Only updates every few hundred milli-seconds unless the Update is forced.</remarks>
        /// <param name="forceUpdate"></param>
        public void UpdateStatus(bool forceUpdate = false)
        {
            if (forceUpdate || UpdateIntervalPassed())
            {
                Context.UpdateStatus(Message, TotalRecords, CurrentRecord);
                _lastUpdate = _timer.ElapsedMilliseconds;
            }
        }

        private bool UpdateIntervalPassed()
        {
            // Always update if timer not started (i.e not yet initialised)
            if (_timer.IsRunning == false)
                return true;
            else
                return (_timer.ElapsedMilliseconds > (_lastUpdate + UpdateInterval));
            
        }

        public void ReportMessage(string message)
        {
            Context.UpdateStatus(message, TotalRecords, CurrentRecord);
        }
    }
}
