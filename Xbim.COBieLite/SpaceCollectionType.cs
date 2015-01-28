using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.XbimExtensions.Transactions.Extensions;

namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class SpaceCollectionType : ICollectionType<SpaceType>
    {
        public IEnumerator<SpaceType> GetEnumerator()
        {
            return this.Space.OfType<SpaceType>().GetEnumerator();
        }

        [XmlIgnore]
        public List<SpaceType> InnerList
        {
            get { return Space; }
        }
    }
    
}
