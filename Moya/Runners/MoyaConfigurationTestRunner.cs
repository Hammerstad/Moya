namespace Moya.Runners
{
    using System;
    using System.Reflection;
    using Factories;
    using Models;

    public class MoyaConfigurationTestRunner : IMoyaConfigurationTestRunner
    {
        public IMoyaTestRunnerFactory MoyaTestRunnerFactory { get; set; }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var instance = Activator.CreateInstance(methodInfo.DeclaringType);
                methodInfo.Invoke(instance, new object[] { MoyaTestRunnerFactory });
                return new TestResult
                {
                    TestOutcome = TestOutcome.Success,
                    TestType = TestType.PreTest
                };
            }
            catch(Exception e)
            {
                return new TestResult
                {
                    ErrorMessage = e.Message,
                    TestOutcome = TestOutcome.Failure,
                    TestType = TestType.PreTest
                };
            }
        }
    }
}