namespace TestMoya.Model
{
    using System;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Factories;
    using Moya.Models;
    using Moya.Runners;
    using Xunit;

    public class TestResultTests
    {
        private readonly IMoyaTestRunner _testRunner;

        public TestResultTests()
        {
            IMoyaTestRunnerDecorator testRunnerDecorator = new MoyaTestRunnerDecorator();
            _testRunner = testRunnerDecorator.DecorateTestRunner(new StressTestRunner());
        }

        [Fact]
        public void TestThrowingExceptionAddsExceptionToTestResult()
        {
            MethodInfo method = typeof(TestClass).GetMethod("MethodThrowingException");

            ITestResult testResult = _testRunner.Execute(method);

            Assert.NotNull(testResult.Exception);
            Assert.Equal(TestOutcome.Failure, testResult.TestOutcome);
        }

        class TestClass
        {
            [Stress]
            // ReSharper disable once UnusedMember.Local
            public void MethodThrowingException()
            {
                throw new Exception("Something bad happened!");
            }
        }
    }
}