using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;


namespace SerialisationHelper
{
    partial class MainApp
    {

        private static string InitCollectionClass(string currCode)
        {
            var re = new Regex("private List<(\\w*)> (\\w*)Field;");
            foreach (Match mtc in re.Matches(currCode))
            {
                var sType = mtc.Groups[1].Value;
                var sName = mtc.Groups[1].Value;
                string newInit = string.Format("private List<{0}> {1}Field = new List<{0}>();", sType, sName);
                currCode = currCode.Replace(mtc.Value, newInit);
            }
            return currCode;
        }

        private static HashSet<string> GetClassesByPattern(string pattern, string file)
        {
            var reClassList = new Regex(pattern);
            var m = reClassList.Matches(file);
            HashSet<string> Classes = new HashSet<string>();
            foreach (Match match in m)
            {
                var cname = match.Groups[1].Value;
                if (!Classes.Contains(cname))
                {
                    Classes.Add(cname);
                }
            }
            return Classes;
        }

        private static void Main()
        {
            var f = new FileInfo("XmlSerializationCode.tcs");
            FixSerialisation(f, @"..\..\..\Xbim.COBieLite\COBieLite Schema\");

            f = new FileInfo("XmlSerializationCode.tcs");
            FixSerialisation(f, @"..\..\..\Xbim.COBieLiteUK\Schemas\");


            if (true)
            {
                try
                {
// ReSharper disable UnusedVariable
                    var a = new XmlSerializer(typeof (Xbim.COBieLite.FacilityType));
                    // var b = new XmlSerializer(typeof (Xbim.COBieLiteUK.FacilityType));
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
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    foreach (var ec in exs)
                    {
                        Console.WriteLine(ec.Message);
                    }
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            DirectoryInfo d = new DirectoryInfo(".");
            Console.WriteLine("Current folder: " + d.FullName);


            var fread = @"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.cs";
            var fwrite = @"..\..\..\Xbim.COBieLite\COBieLite Schema\cobielite.designer.RenamedClasses.cs";
            ProcessCobieLite(fread, fwrite);

            fread = @"..\..\..\Xbim.COBieLiteUK\Schemas\cobieliteuk.designer.cs";
            fwrite = @"..\..\..\Xbim.COBieLiteUK\Schemas\cobieliteuk.designer.RenamedClasses.cs";
            ProcessCobieLiteUk(fread, fwrite);

            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }

        private static void FixSerialisation(FileInfo f, string p)
        {
            if (!f.Exists)
                return;
            var d = new DirectoryInfo(p);
            if (!d.Exists)
                return;


            string fName = Path.ChangeExtension(f.Name, "cs");

            var destF = new FileInfo(Path.Combine(d.FullName, fName));
            if (destF.Exists)
                destF.Delete();
            var destStream = destF.CreateText();

            using (var rd = f.OpenText())
            {
                while (!rd.EndOfStream)
                {
                    var r = rd.ReadLine();
                    if (r == null)
                        break;
                    if (r.StartsWith("[assembly:System.Reflection.AssemblyVersionAttribute"))
                        continue;
                    destStream.WriteLine(r);
                }
            }
            destStream.Close();
            f.Delete();
        }

        private static string SavegeTypeReplacement(string file, string classname, string oldType, string newType)
        {
            var re = new Regex(@"\b" + oldType + @"\b");
            var code = GetClassCode(classname, file);
            var newcode = re.Replace(code, newType);
            return file.Replace(code, newcode);
        }


        private static string StringToNullableType(string file, string classname, string fieldName, string newType)
        {
            var code = GetClassCode(classname, file);
            var fld = new Regex("private string (" + fieldName + ")Field;", RegexOptions.IgnoreCase);
            var ms = fld.Matches(code);
            if (ms.Count == 0)
                return code;
            var m = ms[0];
            var replace = string.Format(
                @"private {0}? {1}Field;
        [XmlIgnore][Newtonsoft.Json.JsonIgnore]
        public bool {2}Specified
        {{
            get {{ return this.{1}Field.HasValue; }}
        }}
",
                newType,
                m.Groups[1].Value,
                fieldName
                );

            var newcode = fld.Replace(code, replace);

            // property
            string pattern =
                @"\[System.Xml.Serialization.XmlElementAttribute\(DataType = ""\w*""(, Order = (\d+))*\)].*?public string " +
                fieldName;
            var regexOptions = RegexOptions.Multiline | RegexOptions.Singleline;
            var regex = new Regex(pattern, regexOptions);

            replace = string.Format("public {0}? {1}", newType, fieldName);

            var tst = regex.Match(newcode);
            var order = tst.Groups[2].Value;
            if (order != "")
            {
                replace = "[System.Xml.Serialization.XmlElementAttribute(Order = " + order + ")]\r\n" + replace;
            }
            newcode = regex.Replace(newcode, replace);


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

        private static string ReplaceList(string classcode, string currentType, string newType)
        {
            string srch = string.Format("List<{0}>", currentType);
            string rep = string.Format("List<{0}>", newType);
            return classcode.Replace(srch, rep);
        }

        private static string GetClassCode(string classname, string sourcestring)
        {
            var mS = string.Format(@"public partial class {0} ", classname);
            var start = sourcestring.IndexOf(mS, StringComparison.Ordinal);
            if (start == -1)
            {
                mS = string.Format("public partial class {0}\r\n", classname);
                start = sourcestring.IndexOf(mS, StringComparison.Ordinal);
            }

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

        private static string CreateEmptyInitialiser(string file, string classname)
        {

            var oldCode = GetClassCode(classname, file);
            var newCode = oldCode;
            var firstBrace = newCode.IndexOf('{');
            newCode = newCode.Insert(firstBrace + 1, string.Format("\r\n        public {0}() {{}}", classname));


            return file.Replace(oldCode, newCode);
        }
    }
}