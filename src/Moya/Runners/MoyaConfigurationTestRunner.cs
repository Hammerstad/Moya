namespace Moya.Runners
{
    using System;
    using System.Reflection;
    using Attributes;
    using Factories;
    using Models;

    /// <summary>
    /// The test runner responsible for running methods marked with
    /// the <see cref="MoyaConfigurationAttribute"/> attribute.
    /// </summary>
    public sealed class MoyaConfigurationTestRunner : IMoyaConfigurationTestRunner
    {
        /// <summary>
        /// A <see cref="IMoyaTestRunnerFactory"/> used to 
        /// </summary>
        private readonly IMoyaTestRunnerFactory _testRunnerFactory = MoyaTestRunnerFactory.DefaultInstance;

        /// <summary>
        /// Runs a user made method attributed with a <see cref="MoyaConfigurationAttribute"/> attribute.
        /// This method needs to have one argument, a <see cref="IMoyaTestRunnerFactory"/>, which can
        /// be populated with test-testrunner pairs.
        /// </summary>
        /// <param name="methodInfo">A method attributed with a <see cref="MoyaConfigurationAttribute"/> attribute.</param>
        /// <returns>A <see cref="ITestResult"/> object containing information 
        /// about the test run.</returns>
        public ITestResult Execute(MethodInfo methodInfo)
        {
            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var instance = Activator.CreateInstance(methodInfo.DeclaringType);
                methodInfo.Invoke(instance, new object[] { _testRunnerFactory });
                return new TestResult
                {
                    Outcome = TestOutcome.Success,
                    TestType = TestType.PreTest
                };
            }
            catch(Exception e)
            {
                return new TestResult
                {
                    Exception = e,
                    Outcome = TestOutcome.Failure,
                    TestType = TestType.PreTest
                };
            }
        }
    }
}