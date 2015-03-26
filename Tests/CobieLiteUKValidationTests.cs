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

namespace Tests
{
    [DeploymentItem(@"DPoWValidationTestFiles\", @"DPoWValidationTestFiles\")]
    [TestClass]
    public class CobieLiteUKValidationTests
    {
        [TestMethod]
        public void Validates()
        {
            var vd = new FacilityValidator();
            var req = Facility.ReadJson(@"DPoWValidationTestFiles\013-Lakeside_Restaurant-stage6-COBie.json");
            var sub = Facility.ReadJson(@"DPoWValidationTestFiles\NBS_LakesideRestaurant_EcoBuild2015_Revit2015_.json");
            var validated = vd.Validate(req, sub);
            validated.WriteJson(@"DPoWValidationTestFiles\validationReport.json", true);
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
