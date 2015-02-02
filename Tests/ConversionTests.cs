using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger;
using XbimExchanger.COBieLiteToIfc;
using XbimExchanger.DPoWToCOBieLite;
using XbimExchanger.IfcHelpers;

namespace Tests
{
    [DeploymentItem(@"TestFiles\")]
    [DeploymentItem(@"COBieAttributes.config\")]
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void ConvertCobieLiteToIfc()
        {
            var data = File.ReadAllText("COBieLite.json");
            var facility = JsonConvert.DeserializeObject<FacilityType>(data);

            using (var model = XbimModel.CreateTemporaryModel())
            {
                using (var txn = model.BeginTransaction("Convert from COBieLite"))
                {
                    var exchanger = new CoBieLiteToIfcExchanger(facility, model);
                    exchanger.Convert();
                    txn.Commit();
                }
                model.SaveAs(@"ConvertedFromCOBieLite.ifc", XbimStorageType.IFC);
            }

        }

        [TestMethod]
        public void ConvertDPoWToCOBieLite()
        {
            var dpow = PlanOfWork.Open("NewtownHighSchool.dpow");
            var facility = new FacilityType();
            var exchanger = new DPoWToCOBieLiteExchanger(dpow, facility);
            exchanger.Convert();

        }

        [TestMethod]
        public void UnitConversionTests()
        {
            var converter = new IfcUnitConverter("squaremetres");
            var meterCubics = new string[] { " cubic-metres ", "cubicmetres", "cubicmeters", "m3", "cubic meters" };
            foreach (var cubic in meterCubics)
            {
                converter.Convert(cubic);
                Assert.IsTrue(converter.ConversionFactor == 1.0);
                Assert.IsNotNull(converter.SiUnitName);
                Assert.IsTrue(converter.SiUnitName.Value == IfcSIUnitName.CUBIC_METRE);
                Assert.IsNull(converter.SiPrefix);
            }
            

        }



    }

}
