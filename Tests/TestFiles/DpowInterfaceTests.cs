using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.DPoW.Interfaces;

namespace Tests.TestFiles
{
    [TestClass]
    [DeploymentItem(@"TestFiles\")]
    public class DpowInterfaceTests
    {
        [TestMethod]
        public void DeserialiseJsonTest()
        {
            using (StreamReader file = File.OpenText("dpowData.json"))
            {
                var serializer = new JsonSerializer();
                var dpow = (PlanOfWork)serializer.Deserialize(file, typeof(PlanOfWork));
            }
        }
    }
}
