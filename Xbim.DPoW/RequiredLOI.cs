using System;
using System.Collections.Generic;

namespace Xbim.DPoW
{
    /// <summary>
    /// Required Level of Information
    /// </summary>
    public class RequiredLOI
    {
        /// <summary>
        /// LOI code
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// LOI description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Required attributes
        /// </summary>
        public List<RequiredAttribute> RequiredAttributes { get; set; }
    }
}
