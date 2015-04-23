using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.CobieLiteUK.Validation;
using Xbim.CobieLiteUK.Validation.Reporting;
using Xbim.COBieLiteUK;
using Xbim.IO;
using XbimExchanger.IfcToCOBieLiteUK;

namespace Tests
{
    [TestClass]
    [DeploymentItem(@"ValidationFiles\")]
    public class CobieLiteUKValidationTests
    {
        [TestMethod]
        public void CanSaveValidatedVacility()
        {
            var validated = GetValidated(@"Lakeside_Restaurant-stage6-COBie.json");
            validated.WriteJson(@"..\..\ValidationReport.json", true);
            validated.WriteXml(@"..\..\ValidationReport.xml", true);
            validated.WriteJson(@"ValidationReport.json", true);
        }

        [TestMethod]
        public void CanSaveValidationReport()
        {
            // stage 0 is for documents
            // stage 1 is for zones
            // stage 6 is for assettypes
            var validated = GetValidated(@"Lakeside_Restaurant-stage6-COBie.json");
            const string repName = @"..\..\ValidationReport.xlsx";
            var xRep = new ExcelValidationReport();
            var ret = xRep.Create(validated, repName);
            Assert.IsTrue(ret, "File not created");
        }

        private static Facility GetValidated(string requirementFile)
        {
            const string ifcTestFile = @"Lakeside_Restaurant_fabric_only.ifczip";
            Facility sub = null;

            //create validation file from IFC
            using (var m = new XbimModel())
            {
                var xbimTestFile = Path.ChangeExtension(ifcTestFile, "xbim");
                m.CreateFrom(ifcTestFile, xbimTestFile, null, true, true);
                var facilities = new List<Facility>();
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(m, facilities);
                facilities = ifcToCoBieLiteUkExchanger.Convert();
                sub = facilities.FirstOrDefault();
            }
            Assert.IsTrue(sub!=null);
            var vd = new FacilityValidator();
            var req = Facility.ReadJson(requirementFile);
            var validated = vd.Validate(req, sub);
            return validated;
        }

        [TestMethod]
        public void ValidateXlsLakeside()
        {
            const string xlsx = @"LakesideWithDocuments.xls";
            string msg;
            var cobie = Facility.ReadCobie(xlsx, out msg);
            var req = Facility.ReadJson(@"Lakeside_Restaurant-stage6-COBie.json");
            var validator = new FacilityValidator();
            var result = validator.Validate(req, cobie);
            result.WriteJson(@"..\..\XlsLakesideWithDocumentsValidationStage6.json", true);
        }

        [TestMethod]
        public void ValidateXlsLakeside2()
        {
            const string xlsx = @"c:\Users\mxfm2\Dropbox\Martin\Lakeside_Restaurant_fabric_only.xlsx";
            string msg;
            var cobie = Facility.ReadCobie(xlsx, out msg);
            var req = Facility.ReadJson(@"c:\Users\mxfm2\Dropbox\Martin\003-Lakeside_Restaurant-stage6-COBie.json");
            var validator = new FacilityValidator();
            var result = validator.Validate(req, cobie);
            
            //create report
            using (var stream = File.Create(@"c:\Users\mxfm2\Dropbox\Martin\Lakeside_Restaurant_fabric_only.report.xlsx"))
            {
                var report = new ExcelValidationReport();
                report.Create(result, stream, ExcelValidationReport.SpreadSheetFormat.Xlsx);
                stream.Close();
            }
        }

        [TestMethod]
        public void ValidateXlsLakesideForStage0()
        {
            var result = LakeSide0();
            result.WriteJson(@"..\..\XlsLakesideWithDocumentsValidationStage0.json", true);
        }

        [TestMethod]
        public void LakeSideXls0ValidationReport()
        {
            var validated = LakeSide0();
            const string repName = @"..\..\LakeSideXls0ValidationReport.xlsx";
            var xRep = new ExcelValidationReport();
            var ret = xRep.Create(validated, repName);
            Assert.IsTrue(ret, "File not created");
        }

        private static Facility LakeSide0()
        {
            const string xlsx = @"LakesideWithDocuments.xls";
            string msg;
            var cobie = Facility.ReadCobie(xlsx, out msg);
            var req = Facility.ReadJson(@"Lakeside_Restaurant-stage0-COBie.json");
            var validator = new FacilityValidator();
            var result = validator.Validate(req, cobie);
            return result;
        }
    }
}
