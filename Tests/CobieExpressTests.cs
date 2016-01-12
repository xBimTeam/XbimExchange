using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.CobieExpress;
using Xbim.Ifc;
using Xbim.IO.Memory;
using XbimExchanger.IfcToCOBieExpress;

namespace Tests
{
    [TestClass]
    public class CobieExpressTests
    {
        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void ConvertIfcToCOBieExpress()
        {
            var ifc = IfcStore.Open("Duplex_MEP_20110907.ifc");
            var cobie = new MemoryModel(new EntityFactory());

            //var currentLabel = -1;
            //cobie.EntityNew += entity =>
            //{
            //    currentLabel = entity.EntityLabel;
            //};

            using (var txn = cobie.BeginTransaction("Duplex conversion"))
            {
                var exchanger = new IfcToCoBieExpressExchanger(ifc, cobie);
                exchanger.Convert();
                txn.Commit();
            }
            

            cobie.SaveAsStep21(File.Create("..\\..\\Duplex_MEP.cobie"));
        }
    }
}
