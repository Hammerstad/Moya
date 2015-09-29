namespace Moya.Runners
{
    using Attributes;

    /// <summary>
    /// A test runner for the <see cref="StressAttribute"/> attribute. Can run a method
    /// multiple times, both in parallel and sequence.
    /// </summary>
    internal interface IStressTestRunner : ITestRunner
    {
        /// <summary>
        /// Represents the amount of parallel executions.
        /// </summary>
        int Users { get; }

        /// <summary>
        /// Represents the amount of sequential executions.
        /// </summary>
        int Times { get; }
    }
}