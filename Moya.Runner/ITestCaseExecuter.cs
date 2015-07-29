using Moya.Models;

namespace Moya.Runner
{
    public interface ITestCaseExecuter
    {
        TestResult RunTest(TestCase testCase);
    }
}