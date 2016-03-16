namespace Moya.Dummy.Test.Project.TestRunners
{
    using System.Reflection;
    using Models;
    using Runners;

    public class CustomPreTestRunner : ICustomPreTestRunner
    {
        public ITestResult Execute(MethodInfo methodInfo)
        {
            return new TestResult
            {
                TestType = TestType.PreTest,
                Outcome = TestOutcome.Success,
            };
        }
    }
}