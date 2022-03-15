using System.IO;
using System.Windows;
using Unity;
using Xbim.DPoWTools.Properties;
using Xbim.WindowsUI.DPoWValidation.Injection;
using Xbim.WindowsUI.DPoWValidation.ViewModels;

namespace XbimDPoWTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICanShow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(VerificationViewModel viewModel) 
            : this ()
        {
            LoadSettings(viewModel);
            ValidationGrid.DataContext = viewModel;
            CreateCobieGrid.DataContext = viewModel;
            ReportGrid.DataContext = viewModel;
            ComplianceGrid.DataContext = viewModel;
        }


        private static void LoadSettings(VerificationViewModel vm)
        {
            if (File.Exists(Settings.Default.LastOpenedRequirement))
                vm.RequirementFileSource = Settings.Default.LastOpenedRequirement;
            if (File.Exists(Settings.Default.LastOpenedSubmission))
                vm.SubmissionFileSource = Settings.Default.LastOpenedSubmission;
        }


        private void SaveSettings()
        {
            if (!(DataContext is VerificationViewModel)) 
                return;
            var vm = DataContext as VerificationViewModel;
            Settings.Default.LastOpenedRequirement = vm.RequirementFileSource;
            Settings.Default.LastOpenedSubmission = vm.SubmissionFileSource;
            Settings.Default.Save();
        }
        
        [Dependency]
        public ISaveFileSelector FileSelector { get; set; }
    }
}
