using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xbim.COBieLiteUK.WebOptimization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No file specified on command line...");
                Console.ReadKey();
                return;
            }

            var file = args[0];
            if (!File.Exists(file))
            {
                Console.WriteLine("File {0} doesn't exist.", file);
                Console.ReadKey();
                return;
            }

            var info = new FileInfo(file);
            var originalSize = info.Length;
            Console.WriteLine("File size {0}.", originalSize);
            Console.WriteLine("Loading file {0}.", file);

            var facility = Facility.ReadJson(file);

            Console.WriteLine("Analyzing original file.");
            FileAnalyses(file);

            Console.WriteLine("Analyzing original model.");
            ModelAnalyses(file, facility);

            Console.WriteLine("File loaded. Some of the entities will be deleted.");

            //strip out all ExternalSystem, CreatedBy and CreatedOn
            foreach (var o in facility.Get<CobieObject>())
            {
                o.ExternalSystem = null;
                o.CreatedBy = null;
                o.CreatedOn = null;
                o.Categories = null;
            }
            Console.WriteLine("Deleted CreatedBy, CreatedOn, Categories and ExternalSystem.");

            //save and compare
            var result = Path.ChangeExtension(file, ".min.json");
            Console.WriteLine("Saving result file: {0}", result);
            facility.WriteJson(result);

            Console.WriteLine("Result file created.");

            var resultInfo = new FileInfo(result);
            var resultSize = resultInfo.Length;

            Console.WriteLine("Compression: {0:P}", resultSize/(float) originalSize);

            
            Console.WriteLine("Analyzing result file.");
            FileAnalyses(result);

            Console.WriteLine("Finished. Press any key to quit.");
            Console.ReadKey();
        }

        public static void FileAnalyses(string file)
        {
            var result = Path.ChangeExtension(file, ".tokens.txt");

            var tokens = new Dictionary<string, int>();
            int filesize;
            using (var reader = File.OpenText(file))
            {
                var data = reader.ReadToEnd();
                filesize = data.Length;
                //analyse repetition of property names
                var propNameRegex = new Regex("\"[\\w0-9]+?\":");
                var matches = propNameRegex.Matches(data);
                reader.Close();
                for (var i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    if (!tokens.Keys.Contains(match.Value)) tokens.Add(match.Value, 0);
                    tokens[match.Value]++;
                }
            }

            using (var w = File.CreateText(result))
            {
                var total = tokens.Values.Aggregate(0, (a, b) => a + b);
                var totalMemory = tokens.Aggregate(0, (a, b) => a + b.Value * (b.Key.Length - 3));
                w.WriteLine("Total count of tokens:    {0:## ### ###}", total);
                w.WriteLine("Total file length:        {0:## ### ###}", filesize);
                w.WriteLine("Total tokens memory:      {0:## ### ###}", totalMemory);
                w.WriteLine("Total tokens memory rate: {0:P}", (float)totalMemory/filesize);
                w.WriteLine("------------------------------------------");
                //write header   
                w.WriteLine("{0,30} {1,10} {2,16} {3,16}", "Token name", "Count", "Percent count", "Percent memory");
                w.WriteLine("------------------------------------------------------------------------");
                foreach (var t in tokens.OrderByDescending(t => t.Value))
                {
                    w.WriteLine("{0,30} {1,10} {2,16:P} {3,16:P}", t.Key.Trim('"', ':'), t.Value,
                        (float) t.Value/(float) total,
                        (float) ((t.Key.Length - 3)*t.Value)/(float) filesize);
                }
                w.Close();
            }
        }

        public static void ModelAnalyses(string file, Facility f)
        {
            var results = new Dictionary<string, IEnumerable<CobieObject>>();


            var assemblies = f.Get<Assembly>().Where(i => i != null).ToList();
            var assets = f.Get<Asset>().Where(i => i != null).ToList();
            var assetsTypes = (f.AssetTypes ?? new List<AssetType>()).Where(i => i != null).ToList();
            var attributes = f.Get<Attribute>().Where(i => i != null).ToList();
            var connections = f.Get<Connection>().Where(i => i != null).ToList();
            var contacts = (f.Contacts ?? new List<Contact>()).Where(i => i != null).ToList();
            var documents = f.Get<Document>().Where(i => i != null).ToList();
            var floors = (f.Floors ?? new List<Floor>()).Where(i => i != null).ToList();
            var impacts = f.Get<Impact>().Where(i => i != null).ToList();
            var issues = f.Get<Issue>().Where(i => i != null).ToList();
            var jobs = f.Get<Job>().Where(i => i != null).ToList();
            var resources = f.Get<Resource>().Where(i => i != null).ToList();
            var spaces = f.Get<Space>().Where(i => i != null).ToList();
            var spares = f.Get<Spare>().Where(i => i != null).ToList();
            var systems = (f.Systems ?? new List<Xbim.COBieLiteUK.System>()).Where(i => i != null).ToList();
            var zones = (f.Zones ?? new List<Zone>()).Where(i => i != null).ToList();

            //report
            results.Add("Assemblies", assemblies);
            results.Add("Assets", assets);
            results.Add("AssetTypes", assetsTypes);
            results.Add("Attributes", attributes);
            results.Add("Connections", connections);
            results.Add("Contacts", contacts);
            results.Add("Documents", documents);
            results.Add("Floors", floors);
            results.Add("Impacts", impacts);
            results.Add("Issues", issues);
            results.Add("Jobs", jobs);
            results.Add("Resources", resources);
            results.Add("Spaces", spaces);
            results.Add("Spares", spares);
            results.Add("Systems", systems);
            results.Add("Zones", zones);

            var result = Path.ChangeExtension(file, ".objects.txt");
            int total = 0;
            using (var w = File.CreateText(result))
            {
                w.WriteLine("{0,15} {1,8}", "Type", "Count");
                w.WriteLine("-----------------------");
                foreach (var kvp in results.OrderByDescending(r => r.Value.Count()))
                {
                    var count = kvp.Value.Count();
                    total += count;
                    w.WriteLine("{0,15} {1,8}", kvp.Key, count);
                }
                w.WriteLine("-----------------------");
                w.WriteLine("{0,15} {1,8}", "Total:", total);
                w.Close();
            }
        }
    }
}