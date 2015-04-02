using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.IO;
using XbimExchanger.IfcToCOBieLiteUK;
using Xbim.COBieLiteUK;

namespace Tests
{
    [TestClass]
    public class IfcToCOBieLiteUKTests
    {
        //[TestMethod]
        //public void IfcToCoBieLiteUkTest()
        //{
        //    using (var m = new XbimModel())
        //    {
        //        const string ifcTestFile = @"c:\Users\mxfm2\Desktop\NBS_LakesideRestaurant_EcoBuild2015_Revit2015.ifc";
        //        m.CreateFrom(ifcTestFile, null, null, true, true);
        //        var facilities = new List<Facility>();
        //        var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(m, facilities);
        //        facilities = ifcToCoBieLiteUkExchanger.Convert();

        //        foreach (var facilityType in facilities)
        //        {
        //            var jsonFile = Path.ChangeExtension(ifcTestFile, ".cobielite.json");
        //            facilityType.WriteJson(jsonFile, true);
        //            break;
        //        }
        //    }
        //}
    }
}
