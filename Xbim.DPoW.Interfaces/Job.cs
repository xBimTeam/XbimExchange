using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.DPoW.Interfaces
{
    public class Job
    {
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        [JsonConverter(typeof(DPoWObjectConverter<DPoWObject>))]
        public IEnumerable<DPoWObject> DPoWObjects { get; set; }
        public IEnumerable<Document> Documents { get; set; }
        public Contact ResponsibleFor { get; set; }
    }
}
