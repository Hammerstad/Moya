namespace Moya.Runners
{
    using System.Reflection;
    using Models;

    /// <summary>
    /// The base interface for all test runners.
    /// </summary>
    public interface IMoyaTestRunner
    {
        /// <summary>
        /// Executes a given method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns>A <see cref="ITestResult"/> object containing information 
        /// about the test run.</returns>
        ITestResult Execute(MethodInfo methodInfo);
    }
}