namespace Moya.Runner
{
    using System.Collections.Generic;
    using Models;

    public interface ITestCaseExecuter
    {
        ICollection<ITestResult> RunTest(TestCase testCase);
    }
}