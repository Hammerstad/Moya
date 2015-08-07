using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Moya.Exceptions;
using Moya.Models;
using Moya.Runner;
using Shouldly;
using Xunit;

namespace TestMoya.Runner
{
    public class TestCaseExecuterTests
    {
        private readonly ITestCaseExecuter testCaseExecuter = new TestCaseExecuter();

        [Fact]
        public void RunTestWithDummyClassShouldReturnCollectionWithOneSuccessfullResult()
        {
            TestCase testCase = new TestCase
            {
                ClassName = "Moya.Dummy.Test.Project.TestClass",
                Id = Guid.NewGuid(),
                MethodName = "AMethod",
                FilePath = GetMoyaDummyTestProjectDllPath()
            };

            var testResult = testCaseExecuter.RunTest(testCase);

            Assert.Equal(1, testResult.Count);
            Assert.Equal(TestOutcome.Success, testResult.First().TestOutcome);
        }

        [Fact]
        public void RunTestWithInvalidClassShouldThrowMoyaException()
        {
            TestCase testCase = new TestCase
            {
                ClassName = "Moya.Dummy.Test.Project.NotATestClass",
                Id = Guid.NewGuid(),
                MethodName = "AMethod",
                FilePath = GetMoyaDummyTestProjectDllPath()
            };
            const string ExpectedExceptionMessageStart = "Unable to find method from assembly.Assembly file path: ";
            const string ExpectedExceptionMessageEnd = "\nClass name: Moya.Dummy.Test.Project.NotATestClass\nMethod name: AMethod";

            var exception = Record.Exception(() => testCaseExecuter.RunTest(testCase));

            Assert.Equal(typeof(MoyaException), exception.GetType());
            exception.Message.ShouldStartWith(ExpectedExceptionMessageStart);
            exception.Message.ShouldEndWith(ExpectedExceptionMessageEnd);
        }

        private static string GetCurrentAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        private static string GetMoyaDummyTestProjectDllPath()
        {
#if DEBUG
            return GetCurrentAssemblyDirectory() + @"\..\..\..\Moya.Dummy.Test.Project\bin\Debug\Moya.Dummy.Test.Project.dll";
#else
            return GetCurrentAssemblyDirectory() + @"\..\..\..\Moya.Dummy.Test.Project\bin\Release\Moya.Dummy.Test.Project.dll";
#endif
        }
    }
}