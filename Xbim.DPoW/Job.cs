using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.DPoW
{
    public class Job
    {
        public string JobName { get; set; }
        public string JobDescription { get; set; }

        //[JsonConverter(typeof(DPoWObjectConverter<DPoWObject>))]
        public Guid ContactIdResponsibleFor { get; set; }

        public Guid Id { get; set; }

        public Job()
        {
            Id = Guid.NewGuid();
        }
    }
}
