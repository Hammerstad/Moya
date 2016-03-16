namespace Moya.Runner.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Utility;

    public class TestFileExecuter
    {
        private readonly List<ITestResult> testResults = new List<ITestResult>();
        private readonly AssemblyScanner assemblyScanner;
        private readonly string assemblyFilePath;
        private readonly TextWriter consoleOut = Console.Out;

        public TestFileExecuter(string assemblyFilePath)
        {
            this.assemblyFilePath = assemblyFilePath;
            assemblyScanner = new AssemblyScanner(this.assemblyFilePath);
        }

        public List<ITestResult> RunAllTests()
        {
            DisableOutput();

            foreach (var type in assemblyScanner.GetTypes())
            {
                RunTestsForType(type);
            }

            EnableOutput();

            return testResults;
        }

        private void RunTestsForType(Type type)
        {
            foreach (var memberInfo in assemblyScanner.GetMembersWithMoyaAttribute(type))
            {
                ITestCaseExecuter testCaseExecuter = new TestCaseExecuter();
                testResults.AddRange(testCaseExecuter.RunTest(new TestCase
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