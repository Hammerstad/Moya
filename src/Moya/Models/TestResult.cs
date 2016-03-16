namespace Moya.Models
{
    using System;

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
        public TestOutcome Outcome { get; set; }

        /// <summary>
        /// Contains an <see cref="Exception"/> if the test execution failed.
        /// The first <see cref="Exception"/> which occured is recorded.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Represents the duration a Moya test took. Measured in milliseconds.
        /// </summary>
        public long Duration { get; internal set; }

        /// <summary>
        /// Represents the name of the method called.
        /// </summary>
        public string MethodName { get; internal set; }

        /// <summary>
        /// Represents the full namespace of the method run.
        /// </summary>
        public string Namespace { get; internal set; }
    }
}