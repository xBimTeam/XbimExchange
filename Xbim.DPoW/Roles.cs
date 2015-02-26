using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.DPoW
{
    /// <summary>
    /// Role represents abstract role in the project and plan of work. It can be assigned to a job in it's responsibility field.
    /// </summary>
    public class Role:DPoWAttributableObject
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of this role
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Rescription of this role
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Initializes new unique ID
        /// </summary>
        public Role()
        {
            Id = Guid.NewGuid();
        }
    }
}
