using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.DPoW.Interfaces;
using System.IO;
using Xbim.DPoW.Interfaces.Converters;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"TestFiles\")]
    public class DPoWSerializationTests
    {
        [TestMethod]
        public void LoadDPoWFromString()
        {
            var data = File.ReadAllText("dpowData.json");
            var settings = new JsonSerializerSettings()
            {
            };
            settings.Converters.Add(new DPoWObjectConverter());
            var dpow = JsonConvert.DeserializeObject<PlanOfWork>(data, settings);

            Assert.AreEqual("Newtown High School", dpow.Project.ProjectName);
            Assert.AreEqual("New Uniclass", dpow.ClassificationSystem[0].ClassificationName);
        }
    }
}
