using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.CobieLiteUk.Validation;
using Xbim.CobieLiteUk.Validation.Reporting;
using Xbim.CobieLiteUk;
using System.IO;

namespace CValidate
{
    class Program
    {
        static void Main(string[] args)
        {
            string msg;
            var subFl = Facility.ReadCobie(@"..\..\Tests\ValidationFiles\VP\Submitted.xlsx", out msg);
            var reqFl = Facility.ReadCobie(@"..\..\Tests\ValidationFiles\VP\Required.xlsx", out msg);

            var validator = new FacilityValidator();
            var result = validator.Validate(reqFl, subFl);

            var tOut = Path.ChangeExtension(Path.GetTempFileName(), "xlsx");
            result.WriteCobie(tOut, out msg);
            
            const string repName = @"ValidationReport.xlsx";
            var xRep = new ExcelValidationReport();
            var ret = xRep.Create(result, repName);
        }
    }
}
