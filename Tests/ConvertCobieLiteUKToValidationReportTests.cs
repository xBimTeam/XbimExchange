using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPOI.HSSF.UserModel;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Reporting;

namespace Tests
{
     [DeploymentItem(@"TestFiles\")]
    [TestClass]
    public class ConvertCobieLiteUkToValidationReportTests
    {
        [TestMethod]
        public void ConvertCobieLiteUkToValidationReportTest()
        {
            var facility = Facility.ReadJson("ValidationReport.json");
            var validationReport = new XbimValidationReport();
            Assert.IsTrue(validationReport.Create(facility, "ValidationReport",
                XbimValidationReport.SpreadSheetFormat.Xlsx));
        }
    }
}
