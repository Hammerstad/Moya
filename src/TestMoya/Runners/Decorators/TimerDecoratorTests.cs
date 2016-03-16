namespace TestMoya.Runners.Decorators
{
    using System;
    using System.Reflection;
    using System.Threading;
    using Moq;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runners;
    using Moya.Runners.Decorators;
    using Xunit;

    public class TimerDecoratorTests
    {
        public class Execute
        {
            private readonly Mock<IMoyaTestRunner> testRunnerMock;
            private ITimerDecorator timerDecorator;

            public Execute()
            {
                testRunnerMock = new Mock<IMoyaTestRunner>();
            }

            [Fact]
            public void DecoratedTestRunnerRunsMethod()
            {
                timerDecorator = new TimerDecorator(testRunnerMock.Object);
                bool methodRun = false;
                MethodInfo method = ((Action)(() => methodRun = true)).Method;
                testRunnerMock
                    .Setup(x => x.Execute(method))
                    .Callback(() => methodRun = true)
                    .Returns(new TestResult());

                timerDecorator.Execute(method);

                Assert.True(methodRun);
            }

            [Fact]
            public void ExecuteAddsDurationToResult()
            {
                MethodInfo method = ((Action)TestClass.MethodWithMoyaAttribute).Method;
                timerDecorator = new TimerDecorator(new StressTestRunner());

                var result = timerDecorator.Execute(method);

                Assert.True(result.Duration > 0);
            }
        }

        public class TestClass
        {
            [Stress]
            public static void MethodWithMoyaAttribute()
            {
                Thread.Sleep(5);
            }
        }
    }
}