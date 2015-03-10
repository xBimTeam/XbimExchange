using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Converters;
using Xbim.COBieLite;
using Xbim.COBieLite.Converters;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;
using Xbim.XbimExtensions.Interfaces;
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
            var facility = FacilityType.ReadJson("COBieLite.json");
            using (var model = XbimModel.CreateTemporaryModel())
            {
                model.Initialise("Xbim Tester", "XbimTeam", "Xbim.Exchanger", "Xbim Development Team", "3.0");
                model.ReloadModelFactors();
                using (var txn = model.BeginTransaction("Convert from COBieLite"))
                {
                    var exchanger = new CoBieLiteToIfcExchanger(facility, model);
                    exchanger.Convert();
                    txn.Commit();
                    //var err = model.Validate(model.Instances, Console.Out);
                }
                model.SaveAs(@"ConvertedFromCOBieLite.ifc", XbimStorageType.IFC);
            }
        }

        [TestMethod]
        public void UnitConversionTests()
        {
            var converter = new IfcUnitConverter("squaremetres");
            var meterCubics = new[] {" cubic-metres ", "cubicmetres", "cubicmeters", "m3", "cubic meters"};
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
