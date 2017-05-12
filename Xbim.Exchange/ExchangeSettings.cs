using System;
using System.IO;
using System.Text.RegularExpressions;
using Xbim.Common;

namespace Xbim.Exchange
{
    internal class ExchangeSettings
    {
        /// <summary>
        /// Points to a requirement file for DPoW validation purposes
        /// </summary>
        public string DpowRequirementFile = "";

        /// <summary>
        /// Directory where the files are going to be saved.
        /// </summary>
        public DirectoryInfo OutputdDirectory;

        public bool DisplayVersion;

        public bool IsOption(string s)
        {
            if (s == "/ver")
            {
                Console.WriteLine(
                    XbimAssemblyInfo.AssemblyInformation(global::System.Reflection.Assembly.GetCallingAssembly()));
                DisplayVersion = true;
                return true;
            }

            var m = Regex.Match(s, "/out:(.*)", RegexOptions.None);
            if (m.Success)
            {
                var dName = m.Groups[1].Value;
                var d = new DirectoryInfo(dName);
                if (d.Parent == null || !d.Parent.Exists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid parameter for /out option, directory cannot be created.");
                    Console.ResetColor();
                    return true;
                }
                if (!d.Exists)
                {
                    d.Create();
                }
                OutputdDirectory = new DirectoryInfo(dName);
                return true;
            }
            m = Regex.Match(s, "/req:(.*)", RegexOptions.None);
            // ReSharper disable once InvertIf // more options might come.
            if (m.Success)
            {
                var fname = m.Groups[1].Value;
                if (!File.Exists(fname))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid parameter for /req option, file not found.");
                    Console.ResetColor();
                    return true;
                }
                if (!fname.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid parameter for /req option, file extension must be json.");
                    Console.ResetColor();
                    return true;
                }
                DpowRequirementFile = fname;
                return true;
            }
            return false;
        }
    }
}
