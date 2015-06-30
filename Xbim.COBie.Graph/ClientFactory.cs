using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Xbim.COBie.Graph.ContractResolvers;

namespace Xbim.COBie.Graph
{
    public class ClientFactory
    {
        public static GraphClient GetClient(string userName, string password, string urlEndpoint)
        {
            if(String.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException();
            if (!Uri.IsWellFormedUriString(urlEndpoint, UriKind.Absolute)) throw new Exception("Wrong service URL format.");

            //create url with username and password
            var index = urlEndpoint.IndexOf('/')+2;
            var newUrl = urlEndpoint.Insert(index, String.Format("{0}:{1}@", userName, password));

            var client = new GraphClient(new Uri(newUrl))
            {
                //set up serialization (contract resolver, custom converters)
                JsonContractResolver = new NoReferencesContractResolver()
            };


            //connect to database
            client.Connect();

            return client;
        }
    }
}
