using System;
using System.Linq;

namespace Xbim.DPoW
{
    /// <summary>
    /// Responsibility to do a job
    /// </summary>
    public class Responsibility
    {
        /// <summary>
        /// ID of the role which is assigned to be responsible
        /// </summary>
        public Guid ResponsibleRoleId { get; set; }
        /// <summary>
        /// ID of the person who is assigned to be responsible
        /// </summary>
        public Guid ResponsibleContactId { get; set; }
        /// <summary>
        /// Gets responsible contact from plan of work
        /// </summary>
        /// <param name="pow">Plan of work</param>
        /// <returns>Responsible contact</returns>
        public Contact GetResponsibleContact(PlanOfWork pow)
        {
            return pow.Contacts == null ? null : pow.Contacts.FirstOrDefault(c => c.Id == ResponsibleContactId);
        }
        /// <summary>
        /// Gets responsible role from plan of work
        /// </summary>
        /// <param name="pow">Plan of work</param>
        /// <returns>Responsible role</returns>
        public Role GetResponsibleRole(PlanOfWork pow)
        {
            return pow.Roles == null ? null : pow.Roles.FirstOrDefault(r => r.Id == ResponsibleRoleId);
        }
    }
}
