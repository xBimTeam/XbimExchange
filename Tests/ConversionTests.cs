using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
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
            var facility = FacilityType.ReadJson("COBieLite.json");

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
        public void ConvertDpoWtoCoBieLite()
        {
            var dpow = PlanOfWork.Open("NewtownHighSchool.dpow");
            var facility = new FacilityType();
            var exchanger = new DpoWtoCoBieLiteExchanger(dpow, facility);
            exchanger.Convert();

            using (var tw = File.CreateText("NewtownHighSchool.COBieLite.json"))
            {
                CoBieLiteHelper.WriteJson(tw, facility);
                tw.Close();
            }

        }

        [TestMethod]
        public void ConvertDpoWtoCoBieLiteDpoWObjects()
        {
            var dpow = PlanOfWork.Open("NewtownHighSchool.dpow");
            var facility = new FacilityType();
            var stage = dpow.ProjectStages.FirstOrDefault(s => s.Jobs.Any(j => j.DPoWObjects != null && j.DPoWObjects.Any()));
            Assert.IsNotNull(stage);
            var exchanger = new DpoWtoCoBieLiteExchanger(dpow, facility, stage);
            exchanger.Convert();

            using (var tw = File.CreateText("NewtownHighSchool.COBieLite.json"))
            {
                CoBieLiteHelper.WriteJson(tw, facility);
                tw.Close();
            }

            Assert.IsTrue(facility.AssetTypes.AssetType.Any());
        }

        [TestMethod]
        public void UnitConversionTests()
        {
            var converter = new IfcUnitConverter("squaremetres");
            var meterCubics = new[] { " cubic-metres ", "cubicmetres", "cubicmeters", "m3", "cubic meters" };
            foreach (var cubic in meterCubics)
            {
                converter.Convert(cubic);
                Assert.IsTrue(Math.Abs(converter.ConversionFactor - 1.0) < 1e-9);
                Assert.IsNotNull(converter.SiUnitName);
                Assert.IsTrue(converter.SiUnitName.Value == IfcSIUnitName.CUBIC_METRE);
                Assert.IsNull(converter.SiPrefix);
            }
            

        }



    }

}
