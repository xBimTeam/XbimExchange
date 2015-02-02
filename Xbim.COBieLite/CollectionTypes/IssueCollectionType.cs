using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class IssueCollectionType : ICollectionType<IssueType>, IEnumerable<IssueType>
    {
        public IEnumerator<IssueType> GetEnumerator()
        {
            return this.Issue.OfType<IssueType>().GetEnumerator();
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<IssueType> InnerList
        {
            get { return Issue; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Issue.OfType<IssueType>().GetEnumerator();
        }
    }
}
