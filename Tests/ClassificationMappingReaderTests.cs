using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications;

namespace Tests
{
    [TestClass]
    public class ClassificationMappingReaderTests
    {
        [TestMethod]
        [DeploymentItem(@"TestFiles\ClassificationFiles", @"TestFiles\ClassificationFiles")]
        public void CanLoadMappingsWithSettings()
        {
            var r = new ClassificationMappingReader();
            DirectoryInfo d = new DirectoryInfo(".");
            Debug.WriteLine(d.FullName);
            Assert.IsTrue(r.HasFiles, @"ClassificationMappingReader required mapping files have not been found.");
        }
    }
}
