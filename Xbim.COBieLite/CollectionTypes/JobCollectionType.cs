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
    public partial class JobCollectionType : ICollectionType<JobType>, IEnumerable<JobType>
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Job.OfType<JobType>().GetEnumerator();
        }
    }
}
