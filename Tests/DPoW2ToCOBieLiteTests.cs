using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XbimExchanger.DPoW2ToCOBieLite;

namespace Tests
{
    [TestClass]
    public class DPoW2ToCOBieLiteTests
    {
        [TestMethod]
        public void SettingsTest()
        {
            var helper = new ClassificationMappings();
            helper.LoadCobieMaps();
        }
    }
}
