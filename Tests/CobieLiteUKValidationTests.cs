using System.Data;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.CobieLiteUK.Validation;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Reporting;
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
            var validated = GetValidated();
            validated.WriteJson(@"..\..\ValidationReport.json", true);
            validated.WriteXml(@"..\..\ValidationReport.xml", true);
            validated.WriteJson(@"ValidationReport.json", true);
        }

        [TestMethod]
        public void CanSaveValidationReport()
        {
            var validated = GetValidated();
            var rep = new XbimValidationReport();

            const string repName = @"..\..\ValidationReport.xlsx";

            if (File.Exists(repName))
            {
                File.Delete(repName);
            }
            rep.Create(validated, repName, XbimValidationReport.SpreadSheetFormat.Xlsx);
        }

        private static Facility GetValidated()
        {
            const string ifcTestFile = @"Lakeside_Restaurant.ifc";
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
            var req = Facility.ReadJson(@"Lakeside_Restaurant-stage6-COBie.json");
            var validated = vd.Validate(req, sub);
            return validated;
        }

        [TestMethod]
        public void FindsRequirements()
        {
            var fac = Facility.ReadJson(@"Lakeside_Restaurant-stage6-COBie.json");
            foreach (var ast in fac.AssetTypes)
            {
                var atv = new AssetTypeValidator(ast);
                foreach (var rq in atv.RequirementDetails)
                {
                    Debug.WriteLine(rq.Name);
                }
            }
        }

        //[TestMethod]
        //public void Indents()
        //{
        //    var infile =
        //        @"C:\Users\Bonghi\Google Drive\UNN\_Research\2014 12 01 - DPOW\_modelInfo\_UseData\2012-03-23-Duplex-Handover.json";
        //    var outfile = Path.ChangeExtension(infile, "indented.json");
        //    var fac = Facility.ReadJson(infile);
        //    fac.WriteJson(outfile, true);
        //}

    }
}
