namespace Xbim.CobieLiteUK.Validation
{
    public enum TerminationMode
    {
        /// <summary>
        /// Performs a full validation of the DPoW
        /// </summary>
        ExecuteCompletely,
        /// <summary>
        /// Stops as soon as the first fail is encountered
        /// </summary>
        StopOnFirstFail
    }

    public interface IValidator
    {
        // TResult Validate<TRequirement, TSubmitted, TResult>(TRequirement requirement, TSubmitted submitted);

        // IEnumerable<IValidator> SubValidators();
        
        TerminationMode TerminationMode { get; set; }

        bool HasFailures { get; }
    }
}