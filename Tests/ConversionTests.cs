using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger;
using XbimExchanger.COBieLiteToIfc;
using XbimExchanger.DPoWToCOBieLite;

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

            using (var tw = File.CreateText("NewtownHighSchool.COBieLite.json"))
            {
                CoBieLiteHelper.WriteJson(tw, facility);
                tw.Close();
            }

        }

        [TestMethod]
        public void ConvertDPoWToCOBieLiteDPoWObjects()
        {
            var dpow = PlanOfWork.Open("NewtownHighSchool.dpow");
            var facility = new FacilityType();
            var stage = dpow.ProjectStages.FirstOrDefault(s => s.Jobs.Any(j => j.DPoWObjects != null && j.DPoWObjects.Any()));
            Assert.IsNotNull(stage);
            var exchanger = new DPoWToCOBieLiteExchanger(dpow, facility, stage);
            exchanger.Convert();

            using (var tw = File.CreateText("NewtownHighSchool.COBieLite.json"))
            {
                CoBieLiteHelper.WriteJson(tw, facility);
                tw.Close();
            }

            Assert.IsTrue(facility.AssetTypes.AssetType.Count() > 0);
        }

    }

}
