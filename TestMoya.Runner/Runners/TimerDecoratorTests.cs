namespace TestMoya.Runner.Runners
{
    using System;
    using System.Reflection;
    using Moq;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runner.Runners;
    using Xunit;

    public class TimerDecoratorTests
    {
        private readonly Mock<ITestRunner> testRunnerMock;
        private TimerDecorator timerDecorator;

        public TimerDecoratorTests()
        {
            testRunnerMock = new Mock<ITestRunner>();
        }

        [Fact]
        public void TimerDecoratorExecuteRunsMethod()
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
        public void TimerDecoratorExecuteAddsDurationToResult()
        {
            MethodInfo method = ((Action)TestClass.MethodWithMoyaAttribute).Method;
            timerDecorator = new TimerDecorator(new StressTestRunner());

            var result = timerDecorator.Execute(method);

            Assert.True(result.Duration > 0);
        }

        public class TestClass
        {
            [Stress]
            public static void MethodWithMoyaAttribute()
            {

            }
        }
    }
}