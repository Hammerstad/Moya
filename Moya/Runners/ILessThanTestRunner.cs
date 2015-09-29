namespace Moya.Runners
{
    using System.Collections.Generic;
    using Attributes;
    using Models;

    /// <summary>
    /// Represents the test runner responsible for running tests marked with
    /// the <see cref="LessThanAttribute"/> attribute.
    /// </summary>
    internal interface ILessThanTestRunner : IPostTestRunner
    {
        /// <summary>
        /// Contains a list of previous test results. Used to determine if the tests
        /// in fact took less than a given time.
        /// </summary>
        ICollection<ITestResult> previousTestResults { get; set; }

        /// <summary>
        /// How many seconds the tests are supposed to take less than.
        /// </summary>
        int Seconds { get; }
    }
}