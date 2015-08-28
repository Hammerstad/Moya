namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Utility;

    public class TestFileExecuter : ITestFileExecuter
    {
        public List<ITestResult> TestResults { get; set; }

        private ITestCaseExecuter testCaseExecuter;
        private readonly AssemblyScanner assemblyScanner;
        private readonly string assemblyFilePath;
        private readonly TextWriter consoleOut = Console.Out;
        
        public TestFileExecuter(string assemblyFilePath)
        {
            this.assemblyFilePath = assemblyFilePath;
            assemblyScanner = new AssemblyScanner(this.assemblyFilePath);
            TestResults = new List<ITestResult>();
        }

        public void RunAllTests()
        {
            DisableOutput();

            foreach (var type in assemblyScanner.GetTypes())
            {
                RunTestsForType(type);
            }

            EnableOutput();

            foreach (var testResult in TestResults)
            {
                Console.WriteLine("{0}", (testResult.Duration));
            }
        }

        private void RunTestsForType(Type type)
        {
            foreach (var memberInfo in assemblyScanner.GetMembersWithMoyaAttribute(type))
            {
                testCaseExecuter = new TestCaseExecuter();
                TestResults.AddRange(testCaseExecuter.RunTest(new TestCase
                {
                    ClassName = type.FullName,
                    Id = Guid.NewGuid(),
                    FilePath = assemblyFilePath,
                    MethodName = memberInfo.Name
                }));
            }
        }

        private static void DisableOutput()
        {
            Console.SetOut(TextWriter.Null);
        }

        private void EnableOutput()
        {
            Console.SetOut(consoleOut);
        }
    }
}