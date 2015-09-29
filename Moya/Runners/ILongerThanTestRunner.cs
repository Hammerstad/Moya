namespace Moya.Runners
{
    using System.Collections.Generic;
    using Attributes;
    using Models;

    /// <summary>
    /// Represents the test runner responsible for running tests marked with
    /// the <see cref="LongerThanAttribute"/> attribute.
    /// </summary>
    internal interface ILongerThanTestRunner : IPostTestRunner
    {
        /// <summary>
        /// Contains a list of previous test results. Used to determine if the tests
        /// in fact took longer than a given time.
        /// </summary>
        ICollection<ITestResult> previousTestResults { get; set; }

        /// <summary>
        /// How many seconds the tests are supposed to take longer than.
        /// </summary>
        int Seconds { get; }
    }
}