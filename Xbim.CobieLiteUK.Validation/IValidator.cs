namespace Xbim.CobieLiteUK.Validation
{
    /// <summary>
    /// Options that determine the interruption behaviour for operations.
    /// </summary>
    public enum TerminationMode
    {
        /// <summary>
        /// Performs a full validation of the DPoW.
        /// </summary>
        ExecuteCompletely,
        /// <summary>
        /// Stops as soon as the first fail is encountered.
        /// </summary>
        StopOnFirstFail
    }

    internal interface IValidator
    {        
        /// <summary>
        /// Determines behaviour that regulate the conclusion of the validation process.
        /// </summary>
        TerminationMode TerminationMode { get; set; }
        
        /// <summary>
        /// true if the validator has encountered failures in the data.
        /// </summary>
        bool HasFailures { get; }
    }
}