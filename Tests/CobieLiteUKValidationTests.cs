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
using Xbim.IO;
using XbimExchanger.IfcToCOBieLiteUK;

namespace Tests
{
    [DeploymentItem(@"DPoWValidationTestFiles\", @"DPoWValidationTestFiles\")]
    [TestClass]
    [DeploymentItem(@"ValidationFiles\")]
    public class CobieLiteUKValidationTests
    {
        [TestMethod]
        public void Validates()
        {
            const string ifcTestFile = @"Lakeside_Restaurant.ifc";
            Facility sub = null;
            
            //create validation file from IFC
            using (var m = new XbimModel())
            {
                var xbimTestFile = Path.ChangeExtension(ifcTestFile, "xbim");
                m.CreateFrom(ifcTestFile, xbimTestFile, null, true, true);
                var helper = new CoBieLiteUkHelper(m, "NBS Code");
                sub = helper.GetFacilities().FirstOrDefault();
            }

            var vd = new FacilityValidator();
            var req = Facility.ReadJson(@"Lakeside_Restaurant-stage6-COBie.json");
            var validated = vd.Validate(req, sub);
            validated.WriteJson(@"..\..\ValidationReport.json", true);
            validated.WriteXml(@"DPoWValidationTestFiles\validationReport.xml", true);
        }

        [TestMethod]
        public void FindsRequirements()
        {
            var fac = Facility.ReadJson(@"DPoWValidationTestFiles\013-Lakeside_Restaurant-stage6-COBie.json");
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
