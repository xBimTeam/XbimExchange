using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.CobieLiteUK.Validation;
using Xbim.CobieLiteUK.Validation.Reporting;

namespace Xbim.COBieLiteUK.Validation.Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            const string xlsx = @"Lakeside_Restaurant_fabric_only.xlsx";
            string msg;
            var cobie = Facility.ReadCobie(xlsx, out msg);
            var req = Facility.ReadJson(@"003-Lakeside_Restaurant-stage6-COBie.json");
            var validator = new FacilityValidator();
            var result = validator.Validate(req, cobie);

            //create report
            using (var stream = File.Create(@"Lakeside_Restaurant_fabric_only.report.xlsx"))
            {
                var report = new ExcelValidationReport();
                report.Create(result, stream, ExcelValidationReport.SpreadSheetFormat.Xlsx);
                stream.Close();
            }
        }
    }
}
