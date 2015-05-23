using System;

namespace Xbim.CobieLiteUK.Validation
{
    /// <summary>
    /// An error occurred inside the Validation engine.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Initialiser from other exception.
        /// </summary>
        /// <param name="message">A custom validation message</param>
        /// <param name="exception">The underlying interpreted exception</param>
        public ValidationException(string message, Exception exception)
            : base (message, exception)
        {
        }

        /// <summary>
        /// A validation native exception.
        /// </summary>
        /// <param name="message">Custom error message</param>
        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
