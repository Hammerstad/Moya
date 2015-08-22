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
            MethodInfo method = ((Action)testClass.MethodWithDurationWarmupAttribute).Method;

            warmupTestRunner.Execute(method);

            Assert.True(TestClass.MethodWithDurationWarmupAttributeRun > 0);
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
        public void ExecuteMethodWithWarmupAttributeSetToZeroRunsMethod()
        {
            MethodInfo method = ((Action)testClass.MethodWithZeroWarmupAttribute).Method;

            var testResult = warmupTestRunner.Execute(method);

            Assert.True(TestClass.MethodWithZeroWarmupAttributeRun > 0);
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
            public static int MethodWithDurationWarmupAttributeRun;
            public static int MethodWithZeroWarmupAttributeRun;

            public void ResetState()
            {
                MethodWithDurationWarmupAttributeRun = 0;
                MethodWithZeroWarmupAttributeRun = 0;
            }

            [Warmup(1)]
            public void MethodWithDurationWarmupAttribute()
            {
                MethodWithDurationWarmupAttributeRun++;
            }

            [Warmup(0)]
            public void MethodWithZeroWarmupAttribute()
            {
                MethodWithZeroWarmupAttributeRun++;
            }

            [Stress]
            public void MethodWithStressAttribute()
            {
            }
        }
    }
}