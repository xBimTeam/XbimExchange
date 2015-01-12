using System;
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
                m.CreateFrom("2012-03-23-Duplex-Handover.ifc", "2012-03-23-Duplex-Handover.xbim", null, true, true);
                var helper = new CoBieLiteHelper(m,"UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    helper.WriteJson(Console.Out, facilityType);
                }
            }
        }

        [TestMethod]
        public void ConvertCoBieLiteToXml()
        {

            using (var m = new XbimModel())
            {
                m.CreateFrom("2012-03-23-Duplex-Handover.ifc", "2012-03-23-Duplex-Handover.xbim", null, true, true);
                var helper = new CoBieLiteHelper(m, "UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    helper.WriteXml(Console.Out,facilityType);
                    
                }
            }
        } 
        [TestMethod]
        public void ConvertCoBieLiteToBson()
        {

            using (var m = new XbimModel())
            {
                m.CreateFrom("2012-03-23-Duplex-Handover.ifc", "2012-03-23-Duplex-Handover.xbim", null, true, true);
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
                            helper.WriteBson(bw, facilityType);
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
