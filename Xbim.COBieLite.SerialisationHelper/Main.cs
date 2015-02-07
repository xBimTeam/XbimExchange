using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xbim.COBieLite;

namespace SerialisationHelper
{
    class MainApp
    {
        private static void Main()
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(FacilityType));
                
            }
            catch (Exception exception)
            {
                List<Exception> exs = new List<Exception>();
                while (exception.InnerException != null)
                {
                    exs.Add(exception);
                    exception = exception.InnerException;
                }
                Console.WriteLine(exception.Message);
            }

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

            // changes of Lists from base types to Type types
            file = StandardListTypeReplace(file, @"ZoneCollectionType", @"ZoneType");
            file = StandardListTypeReplace(file, @"AttributeCollectionType", @"AttributeType");
            file = StandardListTypeReplace(file, @"FloorCollectionType");
            file = StandardListTypeReplace(file, @"SpaceCollectionType");
            file = StandardListTypeReplace(file, @"AssetCollectionType", @"AssetInfoType");
            file = StandardListTypeReplace(file, @"AssetTypeCollectionType", @"AssetTypeInfoType");
            file = StandardListTypeReplace(file, @"SpareCollectionType");
            file = StandardListTypeReplace(file, @"JobCollectionType");
            file = StandardListTypeReplace(file, @"ResourceCollectionType");
            file = StandardListTypeReplace(file, @"ConnectionCollectionType");
            file = StandardListTypeReplace(file, @"ContactCollectionType");
            file = StandardListTypeReplace(file, @"SystemCollectionType");
            file = StandardListTypeReplace(file, @"IssueCollectionType");
            file = StandardListTypeReplace(file, @"DocumentCollectionType");
            //file = StandardListTypeReplace(file, @"");
            //file = StandardListTypeReplace(file, @"");
            //file = StandardListTypeReplace(file, @"");
            //file = StandardListTypeReplace(file, @"");
            file = StandardListTypeReplace(file, @"AssemblyAssignmentCollectionType", @"AssemblyType");
            //file = StandardListTypeReplace(file, @"", @"");
            //file = StandardListTypeReplace(file, @"", @"");
            //file = StandardListTypeReplace(file, @"", @"");

            // type replacements
            file = SavegeTypeReplacement(file, @"AttributeIntegerValueType", @"string", @"int");
            file = SavegeTypeReplacement(file, @"IntegerValueType", @"string", @"int");
            // type attributes
            file = file.Replace("XmlElementAttribute(DataType = \"integer\")", "XmlElementAttribute(DataType = \"int\")");
            

            // fix namespace
            file = file.Replace(@"namespace Xbim.COBieLite.SerialisationHelper", "namespace Xbim.COBieLite");

            File.Delete(fwrite);
            File.WriteAllText(fwrite, file);

            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }

        private static string SavegeTypeReplacement(string file, string classname, string oldType, string newType)
        {
            var code = getClassCode(classname, file);
            string newcode = code.Replace(" " + oldType + " ", " " + newType + " ");
            return file.Replace(code, newcode);
        }

        private static string StandardListTypeReplace(string file, string className, string destType = "")
        {
            if (destType == "")
                destType = className.Replace("Collection", "");
            var ccode = getClassCode(className, file);
            var ccodeRep = replaceList(ccode, destType + "Base", destType);
            file = file.Replace(ccode, ccodeRep);
            return file;
        }

        static private string replaceList(string classcode, string currentType, string newType)
        {
            string srch = string.Format("List<{0}>", currentType);
            string rep = string.Format("List<{0}>", newType);
            return classcode.Replace(srch, rep);
        }
        

        static private string getClassCode(string classname, string sourcestring)
        {
            var mS = string.Format(@"public partial class {0} ", classname);
            var start = sourcestring.IndexOf(mS);

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
