using System;
using System.Collections.Generic;
using System.IO;

namespace Xbim.WindowsUI.DPoWValidation.Models
{
    public class SourceFile
    {
        public SourceFile()
        { }

        public string FileName { get; set; }

        public bool Exists
        {
            get { return File.Exists(FileName); }
        }

        public FileInfo FileInfo
        {
            get
            {
                return new FileInfo(FileName);
            }
        }

        public enum AllowedExtensions
        {
            Any,
            Cobie,
            CobieSpreadsheet,
        }

        public bool IsValidName(AllowedExtensions extensions)
        {
            List<string> acceptableExtensions = null;
            if (extensions == AllowedExtensions.Cobie)
            {
                acceptableExtensions = new List<string> { ".xlsx", ".xls", ".json", ".xml" };
            }
            else if (extensions == AllowedExtensions.CobieSpreadsheet)
            {
                acceptableExtensions = new List<string> { ".xlsx", ".xls"};
            }


            // based on https://stackoverflow.com/questions/422090/in-c-sharp-check-that-filename-is-possibly-valid-not-that-it-exists

            FileInfo fi = null;
            try
            {
                fi = new FileInfo(FileName);
            }
            catch (ArgumentException) { }
            catch (PathTooLongException) { }
            catch (NotSupportedException) { }
            if (ReferenceEquals(fi, null))
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(fi.Name)))
                    return false;
                if (string.IsNullOrEmpty(fi.Extension))
                    return false;
                if (acceptableExtensions != null)
                {
                    if (!acceptableExtensions.Contains(fi.Extension))
                        return false;
                }
                return true;
            }
        }
    }
}
