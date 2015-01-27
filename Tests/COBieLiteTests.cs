using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBieLite;
using Xbim.IO;

namespace Xbim.Tests.COBie
{
    [TestClass] 
    [DeploymentItem(@"TestFiles\")]
    [DeploymentItem(@"COBieAttributes.config")]
    public class CoBieLiteTests
    {
       
        [TestMethod]
        public void ConvertCoBieLiteToJson()
        {
            using (var m = new XbimModel())
            {
                var IfcTestFile = "2012-03-23-Duplex-Handover.ifc";
               // IfcTestFile = @"D:\Users\steve\xBIM\Test Models\Autodesk\002ALakesiderestaurant.ifc";
                var XbimTestFile = Path.ChangeExtension(IfcTestFile, "xbim");
                m.CreateFrom(IfcTestFile, XbimTestFile, null, true, true);
                var helper = new CoBieLiteHelper(m,"UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    CoBieLiteHelper.WriteJson(Console.Out, facilityType);
                }
            }
        }

        [TestMethod]
        public void ConvertCoBieLiteToXml()
        {

            using (var m = new XbimModel())
            {
                var IfcTestFile = "2012-03-23-Duplex-Handover.ifc";
               // var IfcTestFile = @"D:\Users\steve\xBIM\Test Models\BimAlliance BillEast\Model 1 Duplex Apartment\Duplex_MEP_20110907.ifc";
                var xbimTestFile = Path.ChangeExtension(IfcTestFile, "xbim");
                m.CreateFrom(IfcTestFile, xbimTestFile, null, true, true);
                var helper = new CoBieLiteHelper(m, "UniClass");
                var facilities = helper.GetFacilities();
                var i = 1;
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    var outName = string.Format("Facility{0}.xml", i++);
                    var f = new FileInfo(outName);
                    Debug.WriteLine("Writing to " + f.FullName);
                    using (TextWriter writer = File.CreateText(outName))
                    {
                        CoBieLiteHelper.WriteXml(writer, facilityType);
                    }
                    CoBieLiteHelper.WriteXml(Console.Out, facilityType);
                }
            }
        } 
        [TestMethod]
        public void ConvertCoBieLiteToBson()
        {

            using (var m = new XbimModel())
            {
                var IfcTestFile = "2012-03-23-Duplex-Handover.ifc";
               // IfcTestFile = @"C:\Data\dev\XbimTeam\XbimExchange\Tests\TestFiles\Standard_Classroom_CIC_6_Project_mod2.ifc";
                var XbimTestFile = Path.ChangeExtension(IfcTestFile, "xbim");
                m.CreateFrom(IfcTestFile, XbimTestFile, null, true, true);
                var helper = new CoBieLiteHelper(m, "UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    using (var sw = new FileStream("facility.bson",FileMode.Create))
                    {
                        using (var bw = new BinaryWriter(sw))
                        {
                            CoBieLiteHelper.WriteBson(bw, facilityType);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void ConvertCoBieLiteToIfc()
        {

            using (var m = new XbimModel())
            {
                m.CreateFrom("2012-03-23-Duplex-Handover.ifc", "2012-03-23-Duplex-Handover.xbim", null, true, true);
                var helper = new CoBieLiteHelper(m, "UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    using (var sw = new FileStream("facility.bson", FileMode.Create))
                    {                       
                         helper.WriteIfc(Console.Out, facilityType);
                    }
                }
            }
        }
    }
}
