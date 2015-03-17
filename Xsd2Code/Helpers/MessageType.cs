using System.ComponentModel;

namespace Xsd2Code.Library.Helpers
{
    /// <summary>
    /// Identifies subtype of a message
    /// </summary>
    [DefaultValue(Debug)]
    public enum MessageType
    {
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Warning
        /// </summary>
        Warning,
        /// <summary>
        /// Information
        /// </summary>
        Information,
        /// <summary>
        /// Debug
        /// </summary>
        Debug
    }
}