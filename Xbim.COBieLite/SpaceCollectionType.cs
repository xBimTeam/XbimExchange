using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class SpaceCollectionType : IEnumerable<SpaceType>
    {
        public IEnumerator<SpaceType> GetEnumerator()
        {
            return this.Space.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Space.GetEnumerator();
        }

        public void Add(SpaceType space)
        {
            Space.Add(space);
        }

    }
}
