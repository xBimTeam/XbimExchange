using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.CobieExpress;
using Xbim.Ifc;
using Xbim.IO.Memory;
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
            const string input = @"c:\Users\mxfm2\Desktop\COBieExpressTests\LakesideRestaurant.ifc";
            //const string input = @"Duplex_MEP_20110907.ifc";
            var inputInfo = new FileInfo(input);
            var ifc = IfcStore.Open(input);
            var inputCount = ifc.Instances.Count;

            var w = new Stopwatch();
            var cobie = new MemoryModel(new EntityFactory());
            using (var txn = cobie.BeginTransaction("Duplex conversion"))
            {
                var exchanger = new IfcToCoBieExpressExchanger(ifc, cobie);
                w.Start();
                exchanger.Convert();
                w.Stop();
                txn.Commit();
            }
            const string output = "..\\..\\converted.cobie";
            using (var outFile = File.Create(output))
            {
                cobie.SaveAsStep21(outFile);
                outFile.Close();
            }
            var outputInfo = new FileInfo(output);

            Console.WriteLine("Time to convert {0:N}MB file ({2} entities): {1}ms", inputInfo.Length/1e6f, w.ElapsedMilliseconds, inputCount);
            Console.WriteLine("Resulting size: {0:N}MB ({1} entities)", outputInfo.Length / 1e6f, cobie.Instances.Count);

        }
    }
}
