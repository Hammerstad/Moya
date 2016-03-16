namespace TestMoya
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Moya;
    using Moya.Exceptions;
    using Moya.Models;
    using Shouldly;
    using Xunit;

    public class TestCaseExecuterTests
    {
        public class RunTest
        {
            private readonly ITestCaseExecuter testCaseExecuter = new TestCaseExecuter();
            [Fact]
            public void RunTestWithDummyClassShouldReturnCollectionWithOneSuccessfullResult()
            {

                TestCase testCase = new TestCase
                {
                    ClassName = "Moya.Dummy.Test.Project.TestClass",
                    Id = Guid.NewGuid(),
                    MethodName = "OnlyWarmupMethod",
                    FilePath = GetMoyaDummyTestProjectDllPath()
                };

                var testResult = testCaseExecuter.RunTest(testCase);

                Assert.Equal(1, testResult.Count(x => x.Outcome == TestOutcome.Success));
                Assert.Equal(TestOutcome.Success, testResult.First().Outcome);
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

            [Fact]
            public void RunTestWithCustomPreTestRunnerShouldRunTestRunner()
            {
                ConfigureTestProject(testCaseExecuter);
                TestCase testCase = new TestCase
                {
                    ClassName = "Moya.Dummy.Test.Project.CustomPreTestExample",
                    Id = Guid.NewGuid(),
                    MethodName = "MyTestMethod",
                    FilePath = GetMoyaDummyTestProjectDllPath()
                };

                var result = testCaseExecuter.RunTest(testCase);

                result.First().Outcome.ShouldBe(TestOutcome.Success);
                result.First().TestType.ShouldBe(TestType.PreTest);
                result.Count.ShouldBe(1);
            }

            [Fact]
            public void RunTestWithCustomTestRunnerShouldRunTestRunner()
            {
                ConfigureTestProject(testCaseExecuter);
                TestCase testCase = new TestCase
                {
                    ClassName = "Moya.Dummy.Test.Project.CustomTestExample",
                    Id = Guid.NewGuid(),
                    MethodName = "MyTestMethod",
                    FilePath = GetMoyaDummyTestProjectDllPath()
                };

                var result = testCaseExecuter.RunTest(testCase);

                result.First().Outcome.ShouldBe(TestOutcome.Success);
                result.First().TestType.ShouldBe(TestType.Test);
                result.Count.ShouldBe(1);
            }

            [Fact]
            public void RunTestWithCustomPostTestRunnerShouldRunTestRunner()
            {
                ConfigureTestProject(testCaseExecuter);
                TestCase testCase = new TestCase
                {
                    ClassName = "Moya.Dummy.Test.Project.CustomPostTestExample",
                    Id = Guid.NewGuid(),
                    MethodName = "MyTestMethod",
                    FilePath = GetMoyaDummyTestProjectDllPath()
                };

                var result = testCaseExecuter.RunTest(testCase);

                result.First().Outcome.ShouldBe(TestOutcome.Success);
                result.First().TestType.ShouldBe(TestType.PostTest);
                result.Count.ShouldBe(1);
            }
        }

        

        private static string GetCurrentAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        private static void ConfigureTestProject(ITestCaseExecuter testCaseExecuter)
        {
            TestCase testCase = new TestCase
            {
                ClassName = "Moya.Dummy.Test.Project.Configuration",
                Id = Guid.NewGuid(),
                MethodName = "Configure",
                FilePath = GetMoyaDummyTestProjectDllPath()
            };
            testCaseExecuter.RunTest(testCase);
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