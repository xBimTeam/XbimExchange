using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Common.Step21;
using Xbim.COBieLite;
using Xbim.Ifc;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;
using XbimExchanger.COBieLiteToIfc;
using XbimExchanger.IfcHelpers;
using XbimExchanger.IfcHelpers.Ifc2x3;
using Xbim.CobieLiteUk;

namespace Tests
{
    [DeploymentItem(@"ValidationFiles\")]
    [DeploymentItem(@"TestFiles\")]
    [DeploymentItem(@"COBieAttributes.config")]
    [DeploymentItem(@"x64\","x64")]
    [DeploymentItem(@"x86\","x86")]
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        [DeploymentItem(@"ValidationFiles\Lakeside_Restaurant-stage6-COBie.json")]
        public void CanSaveJsonToXlsxAndBack()
        {
            string msg;
            var compFac = Facility.ReadJson(@"Lakeside_Restaurant-stage6-COBie.json");
            compFac.WriteJson("testoutA.json", true);
            compFac.WriteCobie(@"testout.xlsx", out msg);

            var readback = Facility.ReadCobie(@"testout.xlsx", out msg);
            readback.WriteJson("testoutB.json", true);

            // todo: testoutB and testoutA should eventually be identical.
        }

        [TestMethod]
        public void ConverIfcToWexBim()
        {
            const string ifcFileFullName = @"Lakeside_Restaurant.ifc";

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

                        using (var model = IfcStore.Open(ifcFileFullName))
                        {
                            try
                            {                               
                                var geomContext = new Xbim3DModelContext(model);
                                geomContext.CreateContext();
                                model.SaveAsWexBim(binaryWriter);
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

            var credentials = new XbimEditorCredentials()
            {
                ApplicationDevelopersName = "XbimTeam",
                ApplicationFullName = "Xbim.Exchanger",
                EditorsOrganisationName = "Xbim Development Team",
                EditorsFamilyName = "Xbim Tester",
                ApplicationVersion = "3.0"
            };
            using (var model = IfcStore.Create(credentials, IfcSchemaVersion.Ifc2X3, XbimStoreType.InMemoryModel))
            {                                    
               
                using (var txn = model.BeginTransaction("Convert from COBieLite"))
                {
                    var exchanger = new CoBieLiteToIfcExchanger(facility, model);
                    exchanger.Convert();
                    txn.Commit();
                    //var err = model.Validate(model.Instances, Console.Out);
                }
                model.SaveAs(@"ConvertedFromCOBieLite.ifc", IfcStorageType.Ifc);
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
