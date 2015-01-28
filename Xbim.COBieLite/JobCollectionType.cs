using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class JobCollectionType : ICollectionType<JobType>
    {
        public IEnumerator<JobType> GetEnumerator()
        {
            return this.Job.OfType<JobType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<JobType> InnerList
        {
            get { return Job; }
        }
    }
}
