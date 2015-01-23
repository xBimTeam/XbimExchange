using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using Xbim.COBieLite;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger.COBieLiteToIfc;

namespace Tests
{
    [DeploymentItem(@"TestFiles\")]
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
                    var exchanger = new CoBieLiteToIfcExchanger();
                    exchanger.Convert(model, facility);
                    txn.Commit();
                }
                model.SaveAs(@"C:\Users\Steve\Source\Repos\ConvertedFromCOBieLite.ifc", XbimStorageType.IFC);
            }

        }

    }
}
