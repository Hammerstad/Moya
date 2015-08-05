namespace TestMoya.Runner.Runners
{
    using System;
    using System.Reflection;
    using System.Threading;
    using Moq;
    using Moya.Models;
    using Moya.Runner.Runners;
    using Xunit;

    public class TimerDecoratorTests
    {
        private readonly Mock<ITestRunner> testRunnerMock;
        private readonly TimerDecorator timerDecorator;

        public TimerDecoratorTests()
        {
            testRunnerMock = new Mock<ITestRunner>();
            timerDecorator = new TimerDecorator(testRunnerMock.Object);
        }

        [Fact]
        public void TimerDecoratorExecuteRunsMethod()
        {
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
        public void TimerDecoratorExecuteAddsDurationToResult()
        {
            bool methodRun = false;
            MethodInfo method = ((Action)(() => Thread.Sleep(1))).Method;
            testRunnerMock
                .Setup(x => x.Execute(method))
                .Callback(() => methodRun = true)
                .Returns(new TestResult());

            var result = timerDecorator.Execute(method);

            Assert.True(result.Duration > 0);
        }
    }
}