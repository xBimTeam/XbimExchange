using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.IO.CobieExpress;
using XbimExchanger.IfcToCOBieExpress;

namespace Tests
{
    [TestClass]
    public class CobieExpressTests
    {
        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void ConvertIfcToCoBieExpress()
        {
            //const string input = @"c:\CODE\SampleData\LakesideRestaurant\LakesideRestaurant.ifc" ;
            //const string input = @"c:\CODE\XbimGit\XbimExchange\Xbim.COBie.Client\2012-03-23-Duplex-Design.ifc";
            const string input = @"Duplex_MEP_20110907.ifc";
            var inputInfo = new FileInfo(input);
            var ifc = IfcStore.Open(input);
            var inputCount = ifc.Instances.Count;

            var w = new Stopwatch();
            var cobie = new CobieModel();
            using (var txn = cobie.BeginTransaction("Duplex conversion"))
            {
                var exchanger = new IfcToCoBieExpressExchanger(ifc, cobie);
                w.Start();
                exchanger.Convert();
                w.Stop();
                txn.Commit();
            }
            var output = Path.ChangeExtension(input, ".cobie");
            cobie.SaveAsStep21(output);

            //const string outputXml = "..\\..\\converted.xml";
            //using (var outXml = File.Create(outputXml))
            //{
            //    cobie.SaveAsXml(outXml, new XmlWriterSettings { IndentChars = "  ", Indent = true });
            //    outXml.Close();
            //}

            var outputInfo = new FileInfo(output);
            Console.WriteLine("Time to convert {0:N}MB file ({2} entities): {1}ms", inputInfo.Length/1e6f, w.ElapsedMilliseconds, inputCount);
            Console.WriteLine("Resulting size: {0:N}MB ({1} entities)", outputInfo.Length / 1e6f, cobie.Instances.Count);

            using (var txn = cobie.BeginTransaction("Renaming"))
            {
                MakeUniqueNames<CobieFacility>(cobie);
                MakeUniqueNames<CobieFloor>(cobie);
                MakeUniqueNames<CobieSpace>(cobie);
                MakeUniqueNames<CobieZone>(cobie);
                MakeUniqueNames<CobieComponent>(cobie);
                MakeUniqueNames<CobieSystem>(cobie);
                MakeUniqueNames<CobieType>(cobie);
                txn.Commit();
            }

            //save as XLSX
            output = Path.ChangeExtension(input, ".xlsx");
            string report;
            cobie.ExportToTable(output, out report);
        }

        //[TestMethod]
        //public void SaveCobieAsXlsx()
        //{
        //    const string input = @"c:\Users\mxfm2\Desktop\Jeff\CFH-IBI-B01-ZZ-M3-BA-001_MainBuilding_v3_2016.cobie";
        //    var cobie = CobieModel.OpenStep21(input);

        //    using (var txn = cobie.BeginTransaction("Renaming"))
        //    {
        //        MakeUniqueNames<CobieFacility>(cobie);
        //        MakeUniqueNames<CobieFloor>(cobie);
        //        MakeUniqueNames<CobieSpace>(cobie);
        //        MakeUniqueNames<CobieZone>(cobie);
        //        MakeUniqueNames<CobieComponent>(cobie);
        //        MakeUniqueNames<CobieSystem>(cobie);
        //        MakeUniqueNames<CobieType>(cobie);
        //        txn.Commit();
        //    }

        //    var output = Path.ChangeExtension(input, ".renamed.xlsx");
        //    string report;
        //    cobie.ExportToTable(output, out report);
        //}

        private static void MakeUniqueNames<T>(IModel model) where T : CobieAsset
        {
            var groups = model.Instances.OfType<T>().GroupBy(a => a.Name);
            foreach (var @group in groups)
            {
                if (group.Count() == 1)
                {
                    var item = group.First();
                    if (string.IsNullOrEmpty(item.Name))
                        item.Name = item.ExternalObject.Name;
                    continue;
                }

                var counter = 1;
                foreach (var item in group)
                {
                    if (string.IsNullOrEmpty(item.Name))
                        item.Name = item.ExternalObject.Name;
                    item.Name = string.Format("{0} ({1})", item.Name, counter++);
                }
            }
        }
    }
}
