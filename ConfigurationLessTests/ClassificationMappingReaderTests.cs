using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications;

namespace ConfigurationLessTests
{
    [TestClass]
    [DeploymentItem(@"IfcToCOBieLiteUK\Classifications\DataFiles", @"IfcToCOBieLiteUK\Classifications\DataFiles")]
    public class ClassificationMappingReaderTests
    {
        [TestMethod]
        public void CanLoadMappingWithoutSettings()
        {
            var r = new ClassificationMappingReader();
            Assert.IsTrue(r.HasFiles, @"Datareader required mapping files have not been found.");
        }
    }
}
