using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.COBieLiteUK;
using Xbim.DPoW;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger.COBieLiteToIfc;
using XbimExchanger.DPoWToCOBieLiteUK;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"COBieAttributes.config\")]
    [DeploymentItem(@"TestFiles\")]
    public class DPoWToCOBieLiteUKTests
    {
        [TestMethod]
        public void ConvertDPoWToCOBieLite()
        {
            //var pow = PlanOfWork.OpenJson("NewtownHighSchool.new.dpow");
            var pow = PlanOfWork.OpenJson("013-Lakeside_Restaurant.dpow");
            const string dir = "..\\..\\COBieLiteUK";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var outputs = new List<string>();
            string msg;
            foreach (var stage in pow.ProjectStages)
            {
                var facility = new Xbim.COBieLiteUK.Facility();
                var exchanger = new DPoWToCOBieLiteUKExchanger(pow, facility, stage);
                exchanger.Convert();
                var output = Path.Combine(dir, stage.Name + ".cobieliteUK.json");
                var xls = Path.Combine(dir, stage.Name + ".xlsx");
                outputs.Add(output);
                facility.WriteJson(output, true);
                facility.WriteCobie(xls, out msg);
            }

            //check all result files exist
            foreach (var output in outputs)
            {
                Assert.IsTrue(File.Exists(output));
            }

            //try to reload to make sure serialization and deserilization works in both directions
            foreach (var output in outputs)
            {
                var facility = Xbim.COBieLiteUK.Facility.ReadJson(output);
            }
        }
        
    }
}