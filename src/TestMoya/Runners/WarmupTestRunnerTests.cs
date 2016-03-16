namespace TestMoya.Runners
{
    using System;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runners;
    using Moya.Runners.Decorators;
    using Shouldly;
    using Xunit;

    public class WarmupTestRunnerTests
    {
        public class Execute
        {
            private readonly WarmupTestRunner warmupTestRunner;
            private readonly TestClass testClass = new TestClass();

            public Execute()
            {
                warmupTestRunner = new WarmupTestRunner();
                testClass.ResetState();
            }

            [Fact]
            public void RunsMethod()
            {
                MethodInfo method = ((Action)testClass.MethodWithDurationWarmupAttribute).Method;

                warmupTestRunner.Execute(method);

                Assert.True(TestClass.MethodWithDurationWarmupAttributeRun > 0);
            }

            [Fact]
            public void TimerDecoratedWarmupWithDurationRunsForAtLeastDuration()
            {
                IMoyaTestRunner testRunner = new TimerDecorator(warmupTestRunner);
                MethodInfo method = ((Action)testClass.MethodWithDurationWarmupAttribute).Method;

                var testResult = testRunner.Execute(method);

                testResult.Duration.ShouldBeGreaterThanOrEqualTo(1000);
            }

            [Fact]
            public void WarmupAttributeSetToZeroRunsMethod()
            {
                MethodInfo method = ((Action)testClass.MethodWithZeroWarmupAttribute).Method;

                warmupTestRunner.Execute(method);

                Assert.True(TestClass.MethodWithZeroWarmupAttributeRun > 0);
            }

            [Fact]
            public void NoAttributeShouldReturnTestResultNotFound()
            {
                MethodInfo method = ((Action)testClass.ResetState).Method;

                var result = warmupTestRunner.Execute(method);

                Assert.Equal(TestOutcome.NotFound, result.Outcome);
            }

            [Fact]
            public void StressAttributeShouldReturnTestResultNotFound()
            {
                MethodInfo method = ((Action)testClass.MethodWithStressAttribute).Method;

                var result = warmupTestRunner.Execute(method);

                Assert.Equal(TestOutcome.NotFound, result.Outcome);
            }
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