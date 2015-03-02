using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Xbim.COBieLite.CollectionTypes;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class JobCollectionType : ICollectionType<JobType>, IEnumerable<JobType>
    {
        public IEnumerator<JobType> GetEnumerator()
        {
            return Job.OfType<JobType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<JobType> InnerList
        {
            get { return Job ?? (Job = new List<JobType>()); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Job.OfType<JobType>().GetEnumerator();
        }
    }
}
