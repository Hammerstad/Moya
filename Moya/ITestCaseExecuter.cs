namespace Moya
{
    using System.Collections.Generic;
    using Attributes;
    using Models;

    /// <summary>
    /// Represents an instance used to run <see cref="TestCase"/>s.
    /// </summary>
    public interface ITestCaseExecuter
    {
        /// <summary>
        /// Runs a <see cref="TestCase"/> for a specific method.
        /// Returns a <see cref="ICollection{ITestResult}"/> containing one 
        /// <see cref="ITestResult"/> per <see cref="MoyaAttribute"/> surrounding
        /// the run method.
        /// </summary>
        /// <param name="testCase">A <see cref="TestCase"/> to be run.</param>
        /// <returns>A collection of <see cref="ITestResult"/> from the test run.</returns>
        ICollection<ITestResult> RunTest(TestCase testCase);
    }
}