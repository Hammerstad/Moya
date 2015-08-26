namespace Moya.Dummy.Test.Project.TestRunners
{
    using System.Reflection;
    using Models;
    using Runners;

    public class CustomTestRunner : ICustomTestRunner
    {
        public ITestResult Execute(MethodInfo methodInfo)
        {
            return new TestResult
            {
                TestType = TestType.Test,
                TestOutcome = TestOutcome.Success
            };
        }
    }
}