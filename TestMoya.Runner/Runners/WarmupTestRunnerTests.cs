using System;
using System.Reflection;
using Moya.Attributes;
using Moya.Exceptions;
using Moya.Runner.Runners;
using Shouldly;
using Xunit;

namespace TestMoya.Runner.Runners
{
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
            ITestRunner testRunner = new TimerDecorator(warmupTestRunner);
            MethodInfo method = ((Action)testClass.MethodWithDurationWarmupAttribute).Method;

            var testResult = testRunner.Execute(method);

            testResult.Duration.ShouldBeGreaterThanOrEqualTo(100);
        }

        [Fact]
        public void ExecuteMethodWithNoAttributeShouldThrowMoyaAttributeNotFoundException()
        {
            MethodInfo method = ((Action)testClass.ResetState).Method;

            var exception = Record.Exception(() => warmupTestRunner.Execute(method));

            Assert.Equal(typeof(MoyaAttributeNotFoundException), exception.GetType());
            Assert.Equal("Unable to find Moya.Attributes.WarmupAttribute in Void ResetState()", exception.Message);
        }

        [Fact]
        public void ExecuteMethodWithStressAttributeShouldThrowMoyaAttributeNotFoundException()
        {
            MethodInfo method = ((Action)testClass.MethodWithStressAttribute).Method;

            var exception = Record.Exception(() => warmupTestRunner.Execute(method));

            Assert.Equal(typeof(MoyaAttributeNotFoundException), exception.GetType());
            Assert.Equal("Unable to find Moya.Attributes.WarmupAttribute in Void MethodWithStressAttribute()", exception.Message);
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

            [Warmup(Duration = 100)]
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