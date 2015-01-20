using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.DPoW.Interfaces;
using System.IO;
using Xbim.DPoW.Interfaces.Converters;
using System.Linq;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"TestFiles\")]
    public class DPoWSerializationTests
    {
        [TestMethod]
        public void LoadDPoWFromString()
        {
            //var data = File.ReadAllText("dpowData.json");
            var data = File.ReadAllText("NewtownHighSchool.dpow");
            var settings = new JsonSerializerSettings()
            {
            };
            var dpc = new DPoWObjectConverter();
            settings.Converters.Add(dpc);
            var dpow = JsonConvert.DeserializeObject<PlanOfWork>(data, settings);

            Assert.AreEqual("Newtown High School", dpow.Project.ProjectName);
            Assert.AreEqual("dPOW Document Numbering", dpow.ClassificationSystem[0].ClassificationName);
            Assert.IsTrue(dpow.ProjectStages.Any(ps => ps.Jobs.Any(j => j.DPoWObjects.Any(t => t.GetType() == typeof(AssemblyType)))));

            var data2 = JsonConvert.SerializeObject(dpow, settings);

            //create ZONE and AssetType
            dpow.ProjectStages[1].Jobs[0].DPoWObjects.Add(new Zone() { DPoWObjectCategory = new ClassificationReference() { ClassificationCode = "A" }, DPoWObjectName = "Zone A", RequiredLOD = new RequiredLOD() { RequiredLODCode = "123" } });
            dpow.ProjectStages[1].Jobs[0].DPoWObjects.Add(new AssetType() { DPoWObjectCategory = new ClassificationReference() { ClassificationCode = "A" }, DPoWObjectName = "Zone A", RequiredLOD = new RequiredLOD() { RequiredLODCode = "123" } });

            dpow.Save("test1.dpow");
            dpow = PlanOfWork.Open("test1.dpow");

            Assert.IsTrue(dpow.ProjectStages[1].Jobs[0].DPoWObjects.Any( t => t.GetType() == typeof(Zone)));
            Assert.IsTrue(dpow.ProjectStages[1].Jobs[0].DPoWObjects.Any(t => t.GetType() == typeof(AssetType)));
        }
    }
}
