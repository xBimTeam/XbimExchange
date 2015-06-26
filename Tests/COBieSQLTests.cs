using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBieSQL;
using Xbim.COBieSQL.Model;

namespace Tests
{
    [TestClass]
    public class CobieSqlTests
    {
        [TestMethod]
        public void AccessData()
        {
            using (var ctx = new CobieContext("CobieTests"))
            {
                ctx.Facilities.Add(new Facility()
                {
                    Name = "Very first facility",
                    Description = "Great great description"
                });
            }
        }
    }
}
