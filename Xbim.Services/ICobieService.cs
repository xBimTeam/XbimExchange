using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICobieService" in both code and config file together.
    [ServiceContract]
    public interface ICobieService
    {
        [OperationContract]
        [WebGet(UriTemplate = "facilities/{id}", ResponseFormat = WebMessageFormat.Json)]
        Stream GetFacility(string id);

    }
}
