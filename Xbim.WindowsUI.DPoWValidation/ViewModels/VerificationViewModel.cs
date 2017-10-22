using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Practices.Unity;
using Xbim.CobieLiteUk;
using Xbim.Ifc;
using Xbim.WindowsUI.DPoWValidation.Commands;
using Xbim.WindowsUI.DPoWValidation.Extensions;
using Xbim.WindowsUI.DPoWValidation.Injection;
using Xbim.WindowsUI.DPoWValidation.IO;
using Xbim.WindowsUI.DPoWValidation.Models;
using cobieUKValidation = Xbim.CobieLiteUk.Validation;

// TODO: this viewmodel is all wrong... a lot of this should be moved to the model.
// 2017-10-21. I'm posting changes to make it work, it will have to be refactored.

namespace Xbim.WindowsUI.DPoWValidation.ViewModels
{
    public class VerificationViewModel : INotifyPropertyChanged
    {
        #region commands




        // for verification
        public SelectInputFileCommand SelectRequirement { get; set; }

        public SelectInputFileCommand SelectModelSubmission { get; set; }

        public SelectOutputFileCommand SelectReport { get; set; }

        public ValidateCommand Verify { get; set; }

        public ValidateAndSaveCommand VerifyAndSave { get; set; }


        // for BIM conversion to COBie
        public SelectInputFileCommand SelectBimSubmission { get; set; }

        public SelectOutputFileCommand SelectCOBieToWrite { get; set; }

        public ConvertoCobieCommand SaveBimToCobie { get; set; }

        // for Cobie compliance
        public SelectInputFileCommand SelectComplianceCobie { get; set; }
        public SelectOutputFileCommand SelectComplianceReport { get; set; }
        public SelectOutputFileCommand SelectComplianceFixed { get; set; }


        public RelayCommand ComplianceReportCommand { get; set; }

        public RelayCommand ComplianceImproveCommand { get; set; }

        // to be clarified

        public FacilitySaveCommand ExportFacility { get; set; }
        #endregion

        string _lastComplianceCheckedFile = "";
        Facility _complianceCheckFacility;

        private void RunComplianceReport(object parameter)
        {
            // todo: prepare background task to enable progress update.

            ActivityStatus = "Reading COBie.";
            ActivityProgress = 10;

            string read;
            _complianceCheckFacility = Facility.ReadCobie(ComplianceSourceFileInfo.FileName, out read);
            if (_complianceCheckFacility == null)
            {
                ActivityStatus = "Error: The provided files could not be read.";
                ActivityProgress = 0;
                return;
            }
         
            using (var logger = ComplianceReportFileInfo.FileInfo.CreateText())
            {
                ActivityStatus = "Checking COBie.";
                ActivityProgress = 40;
                _complianceCheckFacility.ValidateUK2012(logger, true);
            }
            // TODO: check: I suppose the file is only created if the model needs fixing
            if (ComplianceReportFileInfo.FileInfo.Exists)
            {
                _lastComplianceCheckedFile = ComplianceSourceFileInfo.FileName;
                ActivityStatus = "Opening file.";
                ActivityProgress = 95;
                Process.Start(ComplianceReportFileInfo.FileInfo.FullName);
                
                // if file exists but fixing field is empty then autofill
                //
                if (ComplianceReportFileInfo.Exists && string.IsNullOrEmpty(FixedCobie))
                {
                    FixedCobie = Path.ChangeExtension(ComplianceSourceFileInfo.FileName, "fixed" + ComplianceSourceFileInfo.FileInfo.Extension);
                }
            }
            ActivityStatus = "Completed.";
            ActivityProgress = 0;
        }

        private void FixCobie(object sender)
        {
            if (_complianceCheckFacility == null)
                return;

            ActivityStatus = "Writing fixed COBie.";
            ActivityProgress = 10;


            var file = FixedCobie;
            string log;
            _complianceCheckFacility.WriteCobie(file, out log);
            if (File.Exists(file))
                Process.Start(file);

            ActivityStatus = "Writing completed.";
            ActivityProgress = 0;
        }

        public bool IsWorking { get; set; }

        public bool FilesCanChange
        {
            get { return !IsWorking; }
        }
        
        public string RequirementFileSource
        {
            get { return RequirementFileInfo.FileName; }
            set
            {
                RequirementFileInfo.FileName = value;
                Verify.ChangesHappened();
            }
        }

        private Facility _requirementFacility;

        internal Facility RequirementFacility
        {
            get { return _requirementFacility; }
            set
            {
                _requirementFacility = value;
                RequirementFacilityVm = new DpoWFacilityViewModel(_requirementFacility);

                if (PropertyChanged == null)
                    return;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"RequirementFacilityVM"));
            }
        }

        public DpoWFacilityViewModel RequirementFacilityVm { get; private set; }

        private Facility _submissionFacility;

        internal Facility SubmissionFacility
        {
            get { return _submissionFacility; }
            set
            {
                _submissionFacility = value;
                SubmissionFacilityVm = new DpoWFacilityViewModel(_submissionFacility);

                if (PropertyChanged == null)
                    return;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"SubmissionFacilityVM"));
            }
        }

        public DpoWFacilityViewModel SubmissionFacilityVm { get; private set; }

        private Facility _validationFacility;

        internal Facility ValidationFacility
        {
            get { return _validationFacility; }
            set
            {
                _validationFacility = value;
                ValidationFacilityVm = new DpoWFacilityViewModel(_validationFacility);
                ExportFacility = new FacilitySaveCommand(this);

                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ValidationFacilityVM")); // notiffy that the VM has also changed
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ValidationFacility"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ExportFacility"));
            }
        }

        public DpoWFacilityViewModel ValidationFacilityVm { get; private set; }


        public string SubmissionFileSource
        {
            get { return SubmissionFileInfo.FileName; }
            set
            {
                SubmissionFileInfo.FileName = value;
                Verify.ChangesHappened();
            }
        }


        public string ReportFileSource
        {
            get { return ReportFileInfo.FileName; }
            set
            {
                ReportFileInfo.FileName = value;
                Verify.ChangesHappened();
            }
        }

        public string BimFileSource
        {
            get { return BimFileInfo.FileName; }
            set
            {
                // the underlying model
                BimFileInfo.FileName = value;
                // the command depending on this info
                SaveBimToCobie.ChangesHappened();
            }
        }

        public string COBieToWrite
        {
            get { return COBieToWriteFileInfo.FileName; }
            set
            {
                // the underlying model
                COBieToWriteFileInfo.FileName = value;
                // the command depending on this info
                SaveBimToCobie.ChangesHappened();
            }
        }

        public string ComplianceSourceString { get { return ComplianceSourceFileInfo.FileName; } set { ComplianceSourceFileInfo.FileName = value; } }
        public string ComplianceReportFile { get { return ComplianceReportFileInfo.FileName; } set { ComplianceReportFileInfo.FileName = value; } }
        public string FixedCobie { get { return ComplianceFixedFileInfo.FileName; } set { ComplianceFixedFileInfo.FileName = value; } }
       
        
        // verification
        internal SourceFile RequirementFileInfo;
        internal SourceFile SubmissionFileInfo;
        internal SourceFile ReportFileInfo;

        // COBie compliance
        internal SourceFile ComplianceSourceFileInfo;
        internal SourceFile ComplianceReportFileInfo;
        internal SourceFile ComplianceFixedFileInfo;

        // model conversion to cobie
        internal SourceFile BimFileInfo;
        internal SourceFile COBieToWriteFileInfo;
        
        public VerificationViewModel()
        {
            IsWorking = false;

            RequirementFileInfo = new SourceFile(this);
            SubmissionFileInfo = new SourceFile(this);
            ReportFileInfo = new SourceFile(this);

            // COBie compliance
            ComplianceSourceFileInfo = new SourceFile(this, "ComplianceSourceString");
            ComplianceReportFileInfo = new SourceFile(this, "ComplianceReportFile");
            ComplianceFixedFileInfo = new SourceFile(this, "FixedCobie");

            // model conversion to cobie
            BimFileInfo = new SourceFile(this);
            COBieToWriteFileInfo = new SourceFile(this);

            // for verification
            SelectRequirement = new SelectInputFileCommand(RequirementFileInfo, this) { AllowCompressedSchemas = true };
            SelectModelSubmission = new SelectInputFileCommand(SubmissionFileInfo, this) { IncludeBIM = true };
            SelectReport = new SelectOutputFileCommand(ReportFileInfo, this);

            // for COBie Conversion
            SelectBimSubmission = new SelectInputFileCommand(BimFileInfo, this) { IncludeBIM = true, AllowCompressedSchemas = true, IncludeCobie = false };
            SelectCOBieToWrite = new SelectOutputFileCommand(COBieToWriteFileInfo, this) { COBieSchemas = false };
            SaveBimToCobie = new ConvertoCobieCommand(this);

            // compliance report
            SelectComplianceCobie = new SelectInputFileCommand(ComplianceSourceFileInfo, this) { IncludeCobieSchemas = false, AllowCompressedSchemas = false };
            SelectComplianceReport = new SelectOutputFileCommand(ComplianceReportFileInfo, this) { TextFormat = true, COBieSchemas = false, COBieSpreadSheet = false };
            SelectComplianceFixed = new SelectOutputFileCommand(ComplianceFixedFileInfo, this) { COBieSchemas = false };

            ComplianceReportCommand = new RelayCommand(RunComplianceReport, CanRunComplianceReport);
            ComplianceImproveCommand = new RelayCommand(FixCobie, CanRunComplianceImprove );

            Verify = new ValidateCommand(this);
            ExportFacility = new FacilitySaveCommand(this);
            VerifyAndSave = new ValidateAndSaveCommand(this);
        }

        private bool CanRunComplianceImprove(object obj)
        {
            if (_complianceCheckFacility == null)
                return false;
            if (!ComplianceFixedFileInfo.IsValidName(SourceFile.AllowedExtensions.CobieSpreadsheet))
                return false;
            return (ComplianceSourceString == _lastComplianceCheckedFile);
        }

        private bool CanRunComplianceReport(object obj)
        {
            return ComplianceSourceFileInfo.Exists
                && ComplianceReportFileInfo.IsValidName(SourceFile.AllowedExtensions.Text);
        }

        public void UpdateProperty(string prop)
        {
            if (PropertyChanged == null)
                return;
            // verification
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void FilesUpdate()
        {
            if (PropertyChanged == null) 
                return;

            // verification
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"RequirementFileSource"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"SubmissionFileSource"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ReportFileSource"));
            
            Verify.ChangesHappened();
            VerifyAndSave.ChangesHappened();

            // cobie conversion
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"BimFileSource"));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"COBieToWrite"));
            SaveBimToCobie.ChangesHappened();

            // Compliance
            
        }

        internal void ExecuteSaveCobie()
        {
            ActivityStatus = "Loading model file";
            // once done loading the model file execute FacilityExportCobie
            LoadModelFile(BimFileInfo.FileName, FacilityExportCobie);
        }

        internal void ExecuteValidation()
        {
            IsWorking = true;
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"FilesCanChange"));
            SelectRequirement.ChangesHappened();
            SelectModelSubmission.ChangesHappened();
            SelectReport.ChangesHappened();

            ActivityStatus = "Loading requirement file";
            var fReader = new FacilityReader();
            RequirementFacility = fReader.LoadFacility(RequirementFileSource);
            
            ActivityStatus = "Loading submission file";
            // once done loading the model file execute ValidateFacilities
            LoadModelFile(SubmissionFileSource, ValidateFacilities);
        }

        private BackgroundWorker _worker;

        private void OpenSubmissionCobieFile(object s, DoWorkEventArgs args)
        {
            var cobieFilename = args.Argument as string;
            if (string.IsNullOrEmpty(cobieFilename))
                return;
            if (!File.Exists(cobieFilename))
                return;
            
            switch (Path.GetExtension(cobieFilename.ToLowerInvariant()))
            {
                case ".json": 
                    SubmissionFacility = Facility.ReadJson(cobieFilename);
                    break;
                case ".xml":
                    SubmissionFacility = Facility.ReadXml(cobieFilename);
                    break;
                case ".xls":
                case ".xlsx":
                    string msg;
                    SubmissionFacility = Facility.ReadCobie(cobieFilename, out msg);
                    break;
            }
            args.Result = SubmissionFacility;
        }
        
        private void OpenIfcFile(object s, DoWorkEventArgs args)
        {
            var worker = s as BackgroundWorker;
            var ifcFilename = args.Argument as string;
            try
            {
                if (worker == null) throw new Exception("Could not access background thread");
                var model = IfcStore.Open(ifcFilename, null, null, worker.ReportProgress);
                if (worker.CancellationPending) // if a cancellation has been requested then don't open the resulting file
                {
                    try
                    {
                        model.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    return;
                }
                args.Result = model;
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Error reading " + ifcFilename);
                var indent = "\t";
                while (ex != null)
                {
                    sb.AppendLine(indent + ex.Message);
                    ex = ex.InnerException;
                    indent += "\t";
                }
                args.Result = new Exception(sb.ToString());
            }
        }
        
        private string _activityStatus;
        public string ActivityStatus
        {
            get { return _activityStatus; }

            set
            {
                _activityStatus = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ActivityStatus"));
            }
        }

        private int _activityProgress;
        public int ActivityProgress
        {
            get { return _activityProgress; }
            set
            {
                _activityProgress = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ActivityProgress"));
            }
        }
        public string ActivityDescription { get; set; }

        private IfcStore _model;

        private void CreateWorker(Action<object, RunWorkerCompletedEventArgs> nextAction)
        {
            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true, 
                WorkerSupportsCancellation = true
            };
            _worker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                ActivityProgress = args.ProgressPercentage;
                ActivityStatus = (string) args.UserState;
                // Debug.WriteLine("{0}% {1}", args.ProgressPercentage, (string) args.UserState);
            };

            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(nextAction);
        }
        
        private void FacilityExportCobie(object sender, RunWorkerCompletedEventArgs args)
        {
            // we might be getting a model or a facility in the args, in case try to convert.
            //
            var fac = GetFacility(args);
            if (fac is Facility) //all ok; this is the model facility
            {
                var asFac = fac as Facility;
                string msg;

                var fileToWrite = COBieToWriteFileInfo.FileName;

                asFac.WriteCobie(fileToWrite, out msg);
                if (OpenOnExported && File.Exists(fileToWrite))
                {
                    Process.Start(fileToWrite);
                }
            }
            else //we have a problem
            {
                PresentErrorMessage(args);
            }
        }

        private void ValidateFacilities(object sender, RunWorkerCompletedEventArgs args)
        {
            // we might be getting a model or a facility in the args, in case try to convert.
            //
            var fac = GetFacility(args);
            if (fac is Facility) //all ok; this is the model facility
            {
                ValidateFacilities(RequirementFacility, fac as Facility);
            }
            else //we have a problem
            {
                PresentErrorMessage(args);
            }
        }

        private object GetFacility(RunWorkerCompletedEventArgs args)
        {
            if (args.Result is IfcStore)
            {
                // a valid bim model
                //
                _model = args.Result as IfcStore;
                ActivityProgress = 0;
                // prepare the facility
                SubmissionFacility = FacilityFromIfcConverter.FacilityFromModel(_model);
                return SubmissionFacility;
            }
            else if (args.Result is Facility) //all ok; this is the model facility
            {
                return args.Result;
            }
            return null;
        }

        private void PresentErrorMessage(RunWorkerCompletedEventArgs args)
        {
            var errMsg = args.Result as String;
            if (!string.IsNullOrEmpty(errMsg))
            {
                ActivityStatus = "Error Opening File";
            }
            if (args.Result is Exception)
            {
                var sb = new StringBuilder();
                var ex = args.Result as Exception;
                var indent = "";
                while (ex != null)
                {
                    sb.AppendFormat("{0}{1}\n", indent, ex.Message);
                    ex = ex.InnerException;
                    indent += "\t";
                }
                ActivityStatus = "Error Opening Ifc File\r\n\r\n" + sb;
            }
            else
            {
                ActivityStatus = "Error/Ready";
            }
            ActivityProgress = 0;
        }

        [Dependency]
        public ISaveFileSelector FileSelector { get; set; }

        private void ValidateFacilities(Facility requirement, Facility submitted)
        {
            if (requirement != null && submitted != null)
            {
                ActivityStatus = "Validation in progress";
                var f = new cobieUKValidation.FacilityValidator();
                ValidationFacility = f.Validate(requirement, submitted);
                ActivityStatus = "Validation completed";
                ExportValidatedFacility();                
            }
            else
            {
                // todo: notify
            }
        }

        private void CloseAndDeleteTemporaryFiles()
        {
            if (_worker != null && _worker.IsBusy)
                _worker.CancelAsync(); //tell it to stop
            if (_model == null)
                return;
            _model.Dispose();
            _model = null;
        }

        public void LoadModelFile(string modelFileName, Action<object, RunWorkerCompletedEventArgs> nextAction)
        {
            var fInfo = new FileInfo(modelFileName);
            if (!fInfo.Exists) // file does not exist; do nothing
                return;
            
            // there's no going back; if it fails after this point the current file should be closed anyway
            CloseAndDeleteTemporaryFiles();
            CreateWorker(nextAction);
            var ext = fInfo.Extension.ToLower();
            switch (ext)
            {
                case ".json": 
                case ".xml":
                case ".xls":
                case ".xlsx": 
                    _worker.DoWork += OpenSubmissionCobieFile;
                    _worker.RunWorkerAsync(modelFileName);
                    break;
                case ".ifc": //it is an Ifc File
                case ".ifcxml": //it is an IfcXml File
                case ".ifczip": //it is a zip file containing xbim or ifc File
                case ".zip": //it is a zip file containing xbim or ifc File                   
                case ".xbimf":
                case ".xbim": 
                    _worker.DoWork += OpenIfcFile;
                    _worker.RunWorkerAsync(modelFileName);
                    break;
            }
        }

        internal void ExportValidatedFacility()
        {
            if (File.Exists(ReportFileInfo.FileInfo.FullName))
                File.Delete(ReportFileInfo.FileInfo.FullName);
            var thread = new Thread(WorkThreadFunction);
            thread.Start();
            thread.Join();
            if (OpenOnExported && File.Exists(ReportFileInfo.FileInfo.FullName))
            {
                Process.Start(ReportFileInfo.FileInfo.FullName);
            }
        }

        public void WorkThreadFunction()
        {
            try
            {
                ActivityStatus = ValidationFacility.ExportFacility(ReportFileInfo.FileInfo);
            }
            catch (Exception ex)
            {
                ActivityStatus = @"Error.\r\n\r\n" + ex.Message;
            }
        }
        
        public bool OpenOnExported { get; set; }
    }
}