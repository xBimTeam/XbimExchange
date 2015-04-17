using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.CobieLiteUK.Validation
{
    /// <summary>
    /// An error occurred inside the Validation engine.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message, Exception exception)
            : base (message, exception)
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
