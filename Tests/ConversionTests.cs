using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger;
using XbimExchanger.COBieLiteToIfc;
using XbimExchanger.DPoWToCOBieLite;
using XbimExchanger.IfcHelpers;
using XbimGeometry.Interfaces;

namespace Tests
{
    [DeploymentItem(@"TestFiles\")]
    [DeploymentItem(@"COBieAttributes.config\")]
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void ConverIfcToWexBim()
        {
            const string ifcFileFullName = @"D:\Users\steve\My Documents\DPoW\001 NBS Lakeside Restaurant 2014.ifc";

            var fileName = Path.GetFileName(ifcFileFullName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var workingDir = Directory.GetCurrentDirectory();
            var coreFileName = Path.Combine(workingDir, fileNameWithoutExtension);
            var wexBimFileName = Path.ChangeExtension(coreFileName, "wexbim");
            var xbimFile = Path.ChangeExtension(coreFileName, "xbim");
            try
            {

                using (var wexBimFile = new FileStream(wexBimFileName, FileMode.Create))
                {
                    using (var binaryWriter = new BinaryWriter(wexBimFile))
                    {

                        using (var model = new XbimModel())
                        {
                            try
                            {
                                model.CreateFrom(ifcFileFullName, xbimFile, null, true);
                                var geomContext = new Xbim3DModelContext(model);
                                geomContext.CreateContext(XbimGeometryType.PolyhedronBinary);
                                geomContext.Write(binaryWriter);
                            }
                            finally
                            {
                                model.Close();
                                binaryWriter.Flush();
                                wexBimFile.Close();
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to process " + ifcFileFullName + " - " + e.Message);
            }
        }


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
