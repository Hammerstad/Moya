namespace Moya.Models
{
    /// <summary>
    /// Represents the outcome of a Moya test, e.g. <see cref="F:TestOutcome.Success"/>.
    /// </summary>
    public enum TestOutcome
    {
        /// <summary>
        /// Represents a test which failed
        /// </summary>
        Failure,

        /// <summary>
        /// Represents a test which is ignored by the user
        /// </summary>
        Ignored,

        /// <summary>
        /// Represents a test which succeeded
        /// </summary>
        Success,

        /// <summary>
        /// Represents a test which is not found
        /// </summary>
        NotFound
    }
}