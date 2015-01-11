using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.CobieLite.Validation
{
    public class Validator
    {
        public enum ValidationRunningMode
        {
            /// <summary>
            /// Performs a full validation of the DPoW
            /// </summary>
            Complete,
            /// <summary>
            /// Stops as soon as the first fail is encountered
            /// </summary>
            StopOnFirstFail
        }

        /// <summary>
        /// Validate is the entry point for the most efficient validation mechanism.
        /// </summary>
        /// <param name="validationRequirement">the candidate object to be validated.</param>
        /// <returns></returns>
        static ValidationResult Validate(IValidationRequirement validationRequirement, ValidationRunningMode runningModemode = ValidationRunningMode.Complete)
        {
            validationRequirement.LastValidationResult = new ValidationResult(validationRequirement);
            return validationRequirement.LastValidationResult;
        }
    }
}
