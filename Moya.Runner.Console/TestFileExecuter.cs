using Moya.Extensions;

namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Utility;

    public class TestFileExecuter : ITestFileExecuter
    {
        public ICollection<ITestResult> TestResults { get; set; }

        private ITestCaseExecuter testCaseExecuter;
        private readonly AssemblyScanner assemblyScanner;

        public TestFileExecuter(string filePath)
        {
            assemblyScanner = new AssemblyScanner(filePath);
        }

        public void RunAllTests()
        {
            foreach (var type in assemblyScanner.GetTypes())
            {
                foreach (var memberInfo in assemblyScanner.GetMembersWithMoyaAttribute(type))
                {
                    Console.WriteLine("{0} :: {1}".FormatWith(type, memberInfo));
                }
            }
        }
    }
}