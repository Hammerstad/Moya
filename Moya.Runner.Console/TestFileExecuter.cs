namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class TestFileExecuter : ITestFileExecuter
    {
        public ICollection<ITestResult> TestResults { get; set; }

        private ITestCaseExecuter testCaseExecuter;

        public void RunMoyaOnFile(string filePath)
        {
            Console.WriteLine("Executed test for: " + filePath);
        }
    }
}