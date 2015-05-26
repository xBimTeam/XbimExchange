using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieSQL.Model.Enumerations;

namespace Xbim.COBieSQL.Model
{
    [Table("Contacts")]
    public class Contact:CobieObject
    {
        [Required]
        public string Email { get; set; }

        public override string Name
        {
            get { return Email; }
            set { Email = value; }
        }

        public uint ContactRoleId { get; set; }
        public virtual ContactRole ContactRole { get; set; }

        public string Company { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string OrganizationCode { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Street { get; set; }
        public string PostalBox { get; set; }
        public string Town { get; set; }
        public string StateRegion { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
