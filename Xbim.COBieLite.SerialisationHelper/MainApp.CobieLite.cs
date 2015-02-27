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
        private static void ProcessCobieLite(string fread, string fwrite)
        {
            var file = File.ReadAllText(fread);

            var classes = new HashSet<string>();

            var reClassList = new Regex(@"\b(\w+)Type1\b");
            var m = reClassList.Matches(file);
            foreach (Match match in m)
            {
                var cname = match.Groups[1].Value;
                if (!classes.Contains(cname))
                {
                    classes.Add(cname);
                }
            }

            Console.WriteLine("Classes found:");
            foreach (var @class in classes)
            {
                Console.WriteLine(" - " + @class);
                var srch = @"\b" + @class + @"Type\b";
                var replace = @"" + @class + "TypeBase";
                var reMoveToBase = new Regex(srch);
                reMoveToBase.Matches(file);
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
<<<<<<< HEAD

            // file = SavegeTypeReplacement(file, @"IntegerValueType", @"string", @"int?");
=======
>>>>>>> 2bc3340d48b3af7e45dbf5deb48ebe53ea08f584
            file = StringToNullableType(file, @"IntegerValueType", "IntegerValue", @"int");
            file = StringToNullableType(file, @"AttributeIntegerValueType", @"MinValueInteger", @"int");
            file = StringToNullableType(file, @"AttributeIntegerValueType", @"MaxValueInteger", @"int");

            // 
            // type attributes
            file = file.Replace("XmlElementAttribute(DataType = \"integer\"", "XmlElementAttribute(DataType = \"int\"");

<<<<<<< HEAD
            //var CollectionClasses = GetClassesByPattern(@"\b(\w+)CollectionType\b", file);
            //foreach (var collectionClass in CollectionClasses)
            //{
            //    var fullCName = collectionClass + "CollectionType";
            //    var currCode = getClassCode(fullCName, file);
            //    var newCode = InitCollectionClass(currCode);
            //    file = file.Replace(currCode, newCode);
            //}
=======
            var classesToInitialise = new[]
            {
                "AssemblyType", 
                "AttributeCollectionType", 
                "WarrantyCollectionType", 
                "WarrantyType", 
                "AttributeStringValueType", 
                "AllowedValueCollectionType", 
                "ContactAssignmentCollectionType", 
                "AssetKeyType",
                "SpaceAssignmentCollectionType",
                "ZoneAssignmentCollectionType",
                "SystemAssignmentCollectionType",
                "AssemblyAssignmentCollectionType",
                "DocumentCollectionType",
                "DocumentType",
                "IssueCollectionType",
                "IssueType",
                "ZoneCollectionType",
                "ZoneType",
                "SystemCollectionType",
                "SystemType",
                "SpaceCollectionType",
                "SpaceType",
                "FloorCollectionType",
                "FloorType",
                "ContactCollectionType",
                "ContactType",
                "ConnectionCollectionType",
                "ConnectionType",
                "ResourceCollectionType",
                "ResourceType",
                "JobCollectionType",
                "JobType",
                "SpareCollectionType",
                "SpareType",
                "AssetTypeCollectionType",
                "AssetTypeInfoType",
                "AssetCollectionType",
                "AssetInfoTypeBase",
                "AssetInfoType",
                "FacilityType",
                "AttributeType"
            };
            foreach (var classname in classesToInitialise)
            {
                file = CreateEmptyInitialiser(file, classname);
            }
>>>>>>> 2bc3340d48b3af7e45dbf5deb48ebe53ea08f584


            // fix namespace
            file = file.Replace(@"namespace Xbim.COBieLite.SerialisationHelper", "namespace Xbim.COBieLite");

            file = file.Replace(@"[System.Xml.Serialization.XmlIgnoreAttribute()]", "[System.Xml.Serialization.XmlIgnoreAttribute()][Newtonsoft.Json.JsonIgnore]");

            File.Delete(fwrite);
            File.WriteAllText(fwrite, file);
        }

    }
}
