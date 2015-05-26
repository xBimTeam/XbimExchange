using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieSQL.Model.Classification;
using Xbim.COBieSQL.Model.Enumerations;

namespace Xbim.COBieSQL.Model
{
    [Table("Facilities")]
    public class Facility: CobieObject
    {
        public virtual ICollection<ClassificationItem> Categories { get; set; }

        public virtual AreaUnit AreaUnit { get; set; }
        public virtual LinearUnit LinearUnit { get; set; }
        public virtual VolumeUnit VolumeUnit { get; set; }
        public virtual CurrencyUnit CurrencyUnit { get; set; }

        public string AreaMeasurementMethod { get; set; }
        public string Phase { get; set; }

        //site
        public uint SiteId { get; set; }
        public virtual Site Site { get; set; }

        //project
        public uint ProjectId { get; set; }
        public virtual Project Project { get; set; }

        //first level objects
        public virtual ICollection<Contact> Contacts { get; set; }
    }

    public class Project
    {
        public uint ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExternalObject { get; set; }
        public string ExternalIdentifier { get; set; }
    }

    public class Site
    {
        public uint SiteId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExternalObject { get; set; }
        public string ExternalIdentifier { get; set; }
    }
}
