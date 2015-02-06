using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SerialisationHelper
{
    class MainApp
    {
        private static void Main()
        {
            //File.Copy(@"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.cs", @"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.2.cs");
            var fread = @"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.cs";
            var fwrite = @"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.RenamedClasses.cs";
            var file = File.ReadAllText(fread);

            HashSet<string> Classes = new HashSet<string>();

            var reClassList = new Regex(@"\b(\w+)Type1\b");
            var m = reClassList.Matches(file);
            foreach (Match match in m)
            {
                var cname = match.Groups[1].Value;
                if (!Classes.Contains(cname))
                {
                    Classes.Add(cname);
                }
            }

            Console.WriteLine("Classes found:");
            foreach (var @class in Classes)
            {
                
                Console.WriteLine(" - " + @class);
                var srch = @"\b"  + @class + @"Type\b";
                var replace = @"" + @class + "TypeBase";
                var reMoveToBase = new Regex(srch);
                var m2 = reMoveToBase.Matches(file);
                file = reMoveToBase.Replace(file, replace);

                srch = @"\b" + @class + @"Type1\b";
                replace = @"" + @class + "Type";
                var reMoveToType = new Regex(srch);
                file = reMoveToType.Replace(file, replace);

            }

            File.Delete(fwrite);
            File.WriteAllText(fwrite, file);

            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }
    }
}
