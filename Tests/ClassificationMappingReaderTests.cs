using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"IfcToCOBieLiteUK\Classifications\DataFiles", @"IfcToCOBieLiteUK\Classifications\DataFiles")]
    public class ClassificationMappingReaderTests
    {
        [TestMethod]
        public void CanLoadMappingsWithSettings()
        {
            var r = new ClassificationMappingReader();
            DirectoryInfo d = new DirectoryInfo(".");
            Debug.WriteLine(d.FullName);
            Assert.IsTrue(r.HasFiles, @"Datareader required mapping files have not been found.");
        }

    }
}
