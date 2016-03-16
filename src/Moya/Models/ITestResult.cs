namespace Moya.Models
{
    using System;

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
        /// Contains an <see cref="Exception"/> if the test execution failed.
        /// The first <see cref="Exception"/> which occured is recorded.
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// Represents the duration a Moya test took. Measured in milliseconds.
        /// </summary>
        long Duration { get; } 

        /// <summary>
        /// Represents the name of the method called.
        /// </summary>
        string MethodName { get; }

        /// <summary>
        /// Represents the full namespace of the method run.
        /// </summary>
        string Namespace { get; }
    }
}