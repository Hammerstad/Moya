namespace TestMoya.Runners
{
    using System;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runners;
    using Shouldly;
    using Xunit;

    public class WarmupTestRunnerTests
    {
        private readonly WarmupTestRunner warmupTestRunner;
        private readonly TestClass testClass = new TestClass();

        public WarmupTestRunnerTests()
        {
            warmupTestRunner = new WarmupTestRunner();
            testClass.ResetState();
        }

        [Fact]
        public void ExecuteRunsMethod()
        {
            MethodInfo method = ((Action)testClass.MethodWithEmptyWarmupAttribute).Method;

            warmupTestRunner.Execute(method);

            Assert.True(TestClass.MethodWithEmptyWarmupAttributeRun > 0);
        }

        [Fact]
        public void ExecuteTimerDecoratedWarmupWithDurationAttributeRunsForAtLeastDuration()
        {
            IMoyaTestRunner testRunner = new TimerDecorator(warmupTestRunner);
            MethodInfo method = ((Action)testClass.MethodWithDurationWarmupAttribute).Method;

            var testResult = testRunner.Execute(method);

            testResult.Duration.ShouldBeGreaterThanOrEqualTo(1000);
        }

        [Fact]
        public void ExecuteMethodWithNoAttributeShouldReturnTestResultNotFound()
        {
            MethodInfo method = ((Action)testClass.ResetState).Method;

            var result = warmupTestRunner.Execute(method);

            Assert.Equal(TestOutcome.NotFound, result.TestOutcome);
        }

        [Fact]
        public void ExecuteMethodWithStressAttributeShouldReturnTestResultNotFound()
        {
            MethodInfo method = ((Action)testClass.MethodWithStressAttribute).Method;

            var result = warmupTestRunner.Execute(method);

            Assert.Equal(TestOutcome.NotFound, result.TestOutcome);
        }

        class TestClass
        {
            public static int MethodWithEmptyWarmupAttributeRun;

            public void ResetState()
            {
                MethodWithEmptyWarmupAttributeRun = 0;
            }

            [Warmup]
            public void MethodWithEmptyWarmupAttribute()
            {
                MethodWithEmptyWarmupAttributeRun++;
            }

            [Warmup(Duration = 1)]
            public void MethodWithDurationWarmupAttribute()
            {
            }

            [Stress]
            public void MethodWithStressAttribute()
            {
            }
        }
    }
}