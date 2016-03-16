namespace Moya.Models
{
    /// <summary>
    /// Represents a test result from a Moya test
    /// </summary>
    public class TestResult : ITestResult
    {
        /// <summary>
        /// Represents the type of a Moya test, e.g. <see cref="F:TestType.PreTest"/>.
        /// </summary>
        public TestType TestType { get; set; }

        /// <summary>
        /// Represents the outcome of a Moya test, e.g. <see cref="F:TestOutcome.Success"/>.
        /// </summary>
        public TestOutcome TestOutcome { get; set; }

        /// <summary>
        /// Contains an error message if the test execution failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Represents the duration a Moya test took. Measured in milliseconds.
        /// </summary>
        public long Duration { get; set; }
    }
}