using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.DPoW.Interfaces
{
    public class Job
    {
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        
        //[JsonConverter(typeof(DPoWObjectConverter<DPoWObject>))]
         [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public List<DPoWObject> DPoWObjects { get; set; }
        public List<Document> Documents { get; set; }
        public Contact ResponsibleFor { get; set; }

        public Job()
        {
            DPoWObjects = new List<DPoWObject>();
            Documents = new List<Document>();
        }
    }
}
