using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;


namespace SerialisationHelper
{
    partial class MainApp
    {
        private static void Main()
        {
            if (true)
            {
                try
                {
// ReSharper disable UnusedVariable
                    var a = new XmlSerializer(typeof (Xbim.COBieLite.FacilityType));
                    var b = new XmlSerializer(typeof (Xbim.COBieLiteUK.FacilityType));
// ReSharper restore UnusedVariable  
                }
                catch (Exception exception)
                {
                    var exs = new List<Exception>();
                    while (exception.InnerException != null)
                    {
                        exs.Add(exception);
                        exception = exception.InnerException;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception.Message);
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            var fread = @"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.cs";
            var fwrite = @"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.RenamedClasses.cs";
            ProcessCobieLite(fread, fwrite);

            fread = @"..\..\..\Xbim.COBieLiteUK\Schemas\cobieliteuk.designer.cs";
            fwrite = @"..\..\..\Xbim.COBieLiteUK\Schemas\cobieliteuk.designer.RenamedClasses.cs";
            ProcessCobieLiteUk(fread, fwrite);

            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }

        private static string SavegeTypeReplacement(string file, string classname, string oldType, string newType)
        {
            var re = new Regex(@"\b" + oldType + @"\b");
            var code = GetClassCode(classname, file);
            var newcode = re.Replace(code, newType);
            return file.Replace(code, newcode);
        }

        private static string StandardListTypeReplace(string file, string className, string destType = "")
        {
            if (destType == "")
                destType = className.Replace("Collection", "");
            var ccode = GetClassCode(className, file);
            var ccodeRep = ReplaceList(ccode, destType + "Base", destType);
            file = file.Replace(ccode, ccodeRep);
            return file;
        }

        static private string ReplaceList(string classcode, string currentType, string newType)
        {
            string srch = string.Format("List<{0}>", currentType);
            string rep = string.Format("List<{0}>", newType);
            return classcode.Replace(srch, rep);
        }

        static private string GetClassCode(string classname, string sourcestring)
        {
            var mS = string.Format(@"public partial class {0} ", classname);
            var start = sourcestring.IndexOf(mS, StringComparison.Ordinal);

            var pos = start + mS.Length;
            int countBrace = 0;
            while (true)
            {
                var running = sourcestring.Substring(pos, 1);
                if (running == @"{")
                    countBrace ++;
                else if (running == @"}")
                {
                    if (countBrace == 1)
                        break;
                    countBrace--;
                }
                pos++;
            }

            return sourcestring.Substring(start, pos - start + 1);
        }

    }
}
