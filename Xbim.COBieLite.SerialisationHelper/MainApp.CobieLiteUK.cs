using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SerialisationHelper
{
    partial class MainApp
    {
        private static void ProcessCobieLiteUK(string fread, string fwrite)
        {
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
                var srch = @"\b" + @class + @"Type\b";
                var replace = @"" + @class + "TypeBase";
                var reMoveToBase = new Regex(srch);
                var m2 = reMoveToBase.Matches(file);
                file = reMoveToBase.Replace(file, replace);

                srch = @"\b" + @class + @"Type1\b";
                replace = @"" + @class + "Type";
                var reMoveToType = new Regex(srch);
                file = reMoveToType.Replace(file, replace);
            }

            // Schema Alterations

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

            // Schema Alterations (bug in schema? Worth pointing out to COBie standards?)
            file = SavegeTypeReplacement(file, @"ZoneAssignmentCollectionType", @"ZoneKeyType", @"List<ZoneKeyType>");

            // type replacements
            file = SavegeTypeReplacement(file, @"AttributeIntegerValueType", @"string", @"int");
            file = SavegeTypeReplacement(file, @"IntegerValueType", @"string", @"int");


            // 
            // type attributes
            file = file.Replace("XmlElementAttribute(DataType = \"integer\"", "XmlElementAttribute(DataType = \"int\"");


            // fix namespace
            file = file.Replace(@"namespace Xbim.COBieLiteUK.SerialisationHelper", "namespace Xbim.COBieLiteUK");

            File.Delete(fwrite);
            File.WriteAllText(fwrite, file);
        }

    }
}
