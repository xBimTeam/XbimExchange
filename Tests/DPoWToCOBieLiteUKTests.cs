using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.COBieLiteUK;
using Xbim.DPoW;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger.COBieLiteToIfc;
using XbimExchanger.DPoWToCOBieLiteUK;
using ProjectStage = Xbim.DPoW.ProjectStage;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"COBieAttributes.config\")]
    [DeploymentItem(@"TestFiles\")]
    public class DPoWToCOBieLiteUKTests
    {
        [TestMethod]
        public void ConvertDPoWToCOBieLite()
        {
            //var pow = PlanOfWork.OpenJson("NewtownHighSchool.new.dpow");
            var pow = PlanOfWork.OpenJson("013-Lakeside_Restaurant.dpow");
            const string dir = "..\\..\\COBieLiteUK";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var outputs = new List<string>();
            string msg;
            foreach (var stage in pow.ProjectStages)
            {
                var facility = new Xbim.COBieLiteUK.Facility();
                var exchanger = new DPoWToCOBieLiteUKExchanger(pow, facility, stage);
                exchanger.Convert();
                var output = Path.Combine(dir, stage.Name + ".cobieliteUK.json");
                var xls = Path.Combine(dir, stage.Name + ".xlsx");
                outputs.Add(output);
                facility.WriteJson(output, true);
                facility.WriteCobie(xls, out msg);
            }

            //check all result files exist
            foreach (var output in outputs)
            {
                Assert.IsTrue(File.Exists(output));
            }

            //try to reload to make sure serialization and deserilization works in both directions
            foreach (var output in outputs)
            {
                var facility = Xbim.COBieLiteUK.Facility.ReadJson(output);
            }
        }

        [TestMethod]
        public void ConvertDPoWToAll()
        {
            var pow = PlanOfWork.OpenJson("013-Lakeside_Restaurant.dpow");
            const string dir = "..\\..\\COBieLiteUK";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string msg;
            foreach (var stage in pow.ProjectStages)
            {
                var json = Path.Combine(dir, stage.Name + ".cobie.json");
                var xlsx = Path.Combine(dir, stage.Name + ".xlsx");
                var ifc = Path.Combine(dir, stage.Name + ".ifc");

                var facility = new Xbim.COBieLiteUK.Facility();
                var cobieExchanger = new DPoWToCOBieLiteUKExchanger(pow, facility, stage);
                cobieExchanger.Convert();

                facility.WriteJson(json, true);
                facility.WriteCobie(xlsx, out msg);


                using (var ifcModel = XbimModel.CreateTemporaryModel())
                {
                    ifcModel.Initialise("Xbim Tester", "XbimTeam", "Xbim.Exchanger", "Xbim Development Team", "3.0");
                    ifcModel.Header.FileName.Name = stage.Name;
                    ifcModel.ReloadModelFactors();
                    using (var txn = ifcModel.BeginTransaction("Conversion from COBie"))
                    {
                        var ifcExchanger = new XbimExchanger.COBieLiteUkToIfc.CoBieLiteUkToIfcExchanger(facility, ifcModel);
                        ifcExchanger.Convert();
                        txn.Commit();
                    }
                    ifcModel.SaveAs(ifc, XbimStorageType.IFC);
                    ifcModel.Close();
                }
            }
        }

        [TestMethod]
        public void CheckDocumentsInDPoW()
        {
            var pow = PlanOfWork.OpenJson("013-Lakeside_Restaurant.dpow");
            var num = 0;
            foreach (var stage in pow.ProjectStages ?? new List<ProjectStage>())
            {
                foreach (var documentation in stage.DocumentationSet ?? new List<Documentation>())
                {
                    if (documentation.Attributes != null) 
                        num += documentation.Attributes.Count;
                }
            }
            //Assert.AreEqual(0, num);
        }
        
    }
}