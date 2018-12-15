using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using Xbim.WindowsUI.DPoWValidation.Models;
using Xbim.WindowsUI.DPoWValidation.ViewModels;

namespace Xbim.WindowsUI.DPoWValidation.Commands
{
    public class SelectInputFileCommand : ICommand
    {
        private readonly SourceFile _linkedFile;
        private readonly VerificationViewModel _vm;
        public bool IncludeCobie = true;
        public bool IncludeCobieSchemas = true;
        public bool IncludeBIM = false;
        public bool AllowCompressedSchemas = false;

        public SelectInputFileCommand(SourceFile linkedFile, VerificationViewModel viewModel)
        {
            _linkedFile = linkedFile;
            _vm = viewModel;
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

        public void Execute(object parameter)
        {
            var bimFormats = new FileGroup("BIM formats", "*.ifc;*.ifcxml;*.xbim;*.ifczip");
            var cobieSpreadsheet = new FileGroup("COBie spreadsheets", "*.xls;*.xlsx");
            var cobieSchemas = AllowCompressedSchemas
                ? new FileGroup("COBie schemas", "*.json;*.xml;*.zip")
                : new FileGroup("COBie schemas", "*.json;*.xml");

            var enabled = new List<FileGroup>();
            if (IncludeCobie)
            {
                enabled.Add(cobieSpreadsheet);
                if (IncludeCobieSchemas)
                    enabled.Add(cobieSchemas);
            }
            
            if (IncludeBIM) enabled.Add(bimFormats);

            // joined group
            var all = FileGroup.Union("All valid models", enabled);
            enabled.Insert(0, all);
            
            var dlg = new OpenFileDialog
            {
                Filter = FileGroup.GetFilter(enabled)
            };
            if (_linkedFile.Exists)
            {
                dlg.InitialDirectory = Path.GetDirectoryName(_linkedFile.FileName);
            }

            var result = dlg.ShowDialog();
            if (result != DialogResult.OK)
                return;

            _linkedFile.FileName = dlg.FileName;

            // todo: this should rather send an update for a property in the associated linkedFile
            _vm.FilesUpdate();
        }
    }
}
