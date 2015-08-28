namespace Moya.Models
{
    /// <summary>
    /// Represents a test result from a Moya test
    /// </summary>
    public interface ITestResult
    {
        /// <summary>
        /// Represents the type of a Moya test, e.g. <see cref="F:TestType.PreTest"/>.
        /// </summary>
        TestType TestType { get; set; }

        /// <summary>
        /// Represents the outcome of a Moya test, e.g. <see cref="F:TestOutcome.Success"/>.
        /// </summary>
        TestOutcome TestOutcome { get; set; }

        /// <summary>
        /// Contains an error message if the test execution failed.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Represents the duration a Moya test took. Measured in milliseconds.
        /// </summary>
        long Duration { get; set; } 
    }
}