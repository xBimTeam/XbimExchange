using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.DPoW
{
    public class Job
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //[JsonConverter(typeof(DPoWObjectConverter<DPoWObject>))]
        public Responsibility Responsibility { get; set; }

        public Guid Id { get; set; }

        public Job()
        {
            Id = Guid.NewGuid();
        }
    }
}
