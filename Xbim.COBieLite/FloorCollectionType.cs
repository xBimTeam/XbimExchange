using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Xbim.COBieLite
{
    [JsonObject]
    public partial class FloorCollectionType : IEnumerable<FloorType>
    {
        public IEnumerator<FloorType> GetEnumerator()
        {
            return  this.Floor.OfType<FloorType>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Floor.OfType<FloorType>().GetEnumerator();
        }

       
    }
}
