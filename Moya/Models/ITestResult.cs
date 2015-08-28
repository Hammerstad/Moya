namespace Moya.Models
{
    /// <summary>
    /// Represents a test result from a Moya test
    /// </summary>
    public interface ITestResult
    {
        /// <summary>
        /// Represents the result of a Moya test, e.g. 
        /// </summary>
        TestType TestType { get; set; }

        TestOutcome TestOutcome { get; set; }

        string ErrorMessage { get; set; }

        long Duration { get; set; } 
    }
}