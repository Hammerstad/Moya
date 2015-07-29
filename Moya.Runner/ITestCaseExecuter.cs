namespace Moya.Runner
{
    using Moya.Runner.Models;

    public interface ITestCaseExecuter
    {
        TestResult RunTest(TestCase testCase);
    }
}