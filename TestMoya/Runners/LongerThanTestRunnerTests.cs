namespace TestMoya.Runners
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runners;
    using Xunit;

    public class LongerThanTestRunnerTests
    {
        private readonly ILongerThanTestRunner longerThanTestRunner;
        private readonly TestClass testClass;

        public LongerThanTestRunnerTests()
        {
            longerThanTestRunner = new LongerThanTestRunner();
            testClass = new TestClass();
        }

        [Fact]
        public void ExecuteMethodWithLongerThanAttributeDoesNotRunMethod()
        {
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            longerThanTestRunner.Execute(method);

            Assert.True(TestClass.MethodWithLongerThanTenSecondsAttributeRun == 0);
        }

        [Fact]
        public void ExecuteMethodWithLongerThanAttributeWithSecondsDefinedInAttributeAddsSecondsToTestRunner()
        {
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsConstructorAttribute).Method;

            longerThanTestRunner.Execute(method);

            Assert.Equal(10, longerThanTestRunner.Seconds);
        }

        [Fact]
        public void ExecuteMethodWithNoLongerThanAttributeReturnsNotFound()
        {
            MethodInfo method = ((Action)testClass.MethodWithEmptyWarmupAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.NotFound);
        }

        [Fact]
        public void ExecuteMethodWithOnlyLongerThanAttributeReturnsFailure()
        {
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Failure);
        }

        [Fact]
        public void ExecuteMethodWithLongerThanAttributeWhichHasBeenRunReturnsSuccess()
        {
            longerThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 10, TestType = TestType.Test}
            };
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        class TestClass
        {
            public static int MethodWithLongerThanTenSecondsAttributeRun;

            private static readonly object MyLock = new object();

            public void ResetState()
            {
                MethodWithLongerThanTenSecondsAttributeRun = 0;
            }

            [Warmup]
            public void MethodWithEmptyWarmupAttribute()
            {
            }

            [LongerThan(Seconds = 10)]
            public void MethodWithLongerThanTenSecondsAttribute()
            {
                MethodWithLongerThanTenSecondsAttributeRun++;
            }

            [LongerThan(10)]
            public void MethodWithLongerThanTenSecondsConstructorAttribute()
            {
                MethodWithLongerThanTenSecondsAttributeRun++;
            }
        }
    }
}