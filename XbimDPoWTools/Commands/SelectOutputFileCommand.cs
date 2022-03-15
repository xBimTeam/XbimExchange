using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using Unity;
using Xbim.WindowsUI.DPoWValidation.Injection;
using Xbim.WindowsUI.DPoWValidation.Models;
using Xbim.WindowsUI.DPoWValidation.ViewModels;

namespace Xbim.WindowsUI.DPoWValidation.Commands
{
    public class SelectOutputFileCommand : ICommand
    {
        private readonly SourceFile _linkedFile;
        private readonly VerificationViewModel _vm;
        public bool IncludeIfc = false;
        
        public SelectOutputFileCommand(SourceFile linkedFile, VerificationViewModel model)
        {
            FileSelector = ContainerBootstrapper.Instance.Container.Resolve<ISaveFileSelector>();
            _linkedFile = linkedFile;
            _vm = model;
        }

        public bool CanExecute(object parameter)
        {
            return _vm.FilesCanChange;
        }

        public event EventHandler CanExecuteChanged;

        public void ChangesHappened()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }

        ISaveFileSelector FileSelector { get; set; }

        public bool COBieSpreadSheet { get; set; } = true;
        public bool COBieSchemas { get; set; } = true;
        public bool TextFormat { get; set; } = false;

        public void Execute(object parameter)
        {
            var filters = new List<FileGroup>();
            if (COBieSpreadSheet)
            {
                filters.Add(new FileGroup("COBie Excel OpenFormat", "*.xlsx"));
                filters.Add(new FileGroup("COBie Excel Binary", "*.xls"));
            }
            if (COBieSchemas)
            {
                filters.Add(new FileGroup("COBie Schema in Json format", "*.json"));
                filters.Add(new FileGroup("COBie Schema in Xml format", "*.xml"));
            }
            if (TextFormat)
            {
                filters.Add(new FileGroup("Text files", "*.txt"));
            }
            
            FileSelector.Filter = FileGroup.GetFilter(filters);

            if (_linkedFile.Exists)
            {
                FileSelector.InitialDirectory = Path.GetDirectoryName(_linkedFile.FileName);
            }

            var result = FileSelector.ShowDialog();
            if (result != DialogResult.OK)
                return;

            _linkedFile.FileName = FileSelector.FileName;
            _vm.FilesUpdate();
        }
    }
}
