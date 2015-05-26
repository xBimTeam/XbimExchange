using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using Xbim.COBieLiteUK;

namespace Xbim.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CobieService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CobieService.svc or CobieService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CobieService : ICobieService
    {

        public Stream GetFacility(string id)
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/SampleHouse.json");
            var facility = Facility.ReadJson(path);
            var stream = new MemoryStream();
            facility.WriteJson(stream);
            stream.Seek(0, SeekOrigin.Begin);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/json";
            return stream;
        }
    }
}
