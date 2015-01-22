using System;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Exchange;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class ExchangeFrameworkTests
    {
        [TestMethod]
        public void DynamicRawTest()
        {
            dynamic raw = new Raw();
            raw.Name = "xyz";

            Assert.AreEqual("xyz", raw.Name);
        }
    }

    
}
