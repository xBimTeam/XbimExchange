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
            var pow = PlanOfWork.OpenJson("NewtownHighSchool.new.dpow");
            const string dir = "..\\..\\COBieLiteUK";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var outputs = new List<string>();
            foreach (var stage in pow.ProjectStages)
            {
                var facility = new Xbim.COBieLiteUK.Facility();
                var exchanger = new DPoWToCOBieLiteUKExchanger(pow, facility, stage);
                exchanger.Convert();
                var output = Path.Combine(dir, stage.Name + ".cobieliteUK.json");
                outputs.Add(output);
                facility.WriteJson(output, true);
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