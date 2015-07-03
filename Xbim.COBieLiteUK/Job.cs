using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.COBieLiteUK
{
    public partial class Job
    {
        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (Priors != null) 
            foreach (var key in Priors)
                yield return key;
            if (Resources == null) yield break;
            foreach (var key in Resources)
                yield return key;
        }

        internal override void RemoveKey(IEntityKey key)
        {
            base.RemoveKey(key);
            var job = key as JobKey;
            if (job != null && Priors != null)
                Priors.Remove(job);
            var res = key as ResourceKey;
            if (res != null && Resources != null)
                Resources.Remove(res);
        }

        [XmlIgnore, JsonIgnore]
        public override string ObjectType
        {
            get { return JobType; }
            set { JobType = value; }
        }
    }
}
