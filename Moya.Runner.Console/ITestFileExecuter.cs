namespace Moya.Runner.Console
{
    using System.Collections.Generic;
    using Models;

    public interface ITestFileExecuter
    {
        ICollection<ITestResult> TestResults { get; set; }

        void RunAllTests();
    }
}