using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.WindowsUI.DPoWValidation.Models
{
    internal class FileGroup
    {
        public FileGroup(string name, string extensions)
        {
            GroupName = name;
            Extensions = extensions;
        }

        public string GroupName;
        public string Extensions;

        internal static FileGroup Union(string name, List<FileGroup> enabled)
        {
            var extensions =string.Join(";", enabled.Select(x => x.Extensions).ToArray());
            return new FileGroup(name, extensions);
        }

        internal static string GetFilter(List<FileGroup> enabled)
        {
            return string.Join("|", enabled.Select(x=>x.ToFilter()));           
        }

        private string ToFilter()
        {
            return $"{GroupName}|{Extensions}";
        }
    }
}
