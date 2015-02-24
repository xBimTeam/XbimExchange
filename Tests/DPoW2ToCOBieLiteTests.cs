using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.COBieLite;
using Xbim.DPoW;
using XbimExchanger.DPoW2ToCOBieLite;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"TestFiles\")]
    public class DPoW2ToCOBieLiteTests
    {
        [TestMethod]
        public void SettingsTest()
        {
            var helper = new ClassificationMappings();
            helper.LoadCobieMaps();
        }

        [TestMethod]
        public void ConvertDPoWToCOBieLite()
        {
            var pow = PlanOfWork.OpenJson("NewtownHighSchool.new.dpow");
            const string dir = "Export";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var outputs = new List<string>();
            foreach (var stage in pow.ProjectStages)
            {
                var facility = new FacilityType();
                var exchanger = new DPoWToCOBieLiteExchanger(pow, facility, stage);
                exchanger.Convert();
                var output = Path.Combine(dir, stage.Name + ".json");
                outputs.Add(output);
                facility.WriteJson(output);
            }

            //check all result files exist
            foreach (var output in outputs)
            {
                Assert.IsTrue(File.Exists(output));
            }

            //try to reload to make sure serialization and deserilization works in both directions
            foreach (var output in outputs)
            {
                var facility = FacilityType.ReadJson(output);
            }
        }
    }
}