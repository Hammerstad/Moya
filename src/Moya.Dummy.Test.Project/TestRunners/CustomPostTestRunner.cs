namespace Moya.Dummy.Test.Project.TestRunners
{
    using System.Reflection;
    using Models;
    using Runners;

    public class CustomPostTestRunner : ICustomPreTestRunner
    {
        public ITestResult Execute(MethodInfo methodInfo)
        {
            return new TestResult
            {
                TestType = TestType.PostTest,
                TestOutcome = TestOutcome.Success
            };
        }
    }
}