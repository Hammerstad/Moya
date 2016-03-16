namespace Moya.Runners
{
    using System.Reflection;
    using Attributes;
    using Models;

    /// <summary>
    /// The base interface for all test runners.
    /// </summary>
    public interface IMoyaTestRunner
    {
        /// <summary>
        /// Executes a given method.
        /// </summary>
        /// <param name="methodInfo">A method attribute with a <see cref="MoyaAttribute"/> attribute.</param>
        /// <returns>A <see cref="ITestResult"/> object containing information 
        /// about the test run.</returns>
        ITestResult Execute(MethodInfo methodInfo);
    }
}