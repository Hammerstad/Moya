namespace TestMoya.Runners.Decorators
{
    using System;
    using System.Reflection;
    using Moq;
    using Moya.Models;
    using Moya.Runners;
    using Moya.Runners.Decorators;
    using Xunit;

    public class ExceptionCatcherDecoratorTests
    {
        private readonly Mock<IMoyaTestRunner> _testRunnerMock; 
        private readonly IExceptionCatcherDecorator _exceptionCatcherDecorator;

        public ExceptionCatcherDecoratorTests()
        {
            _testRunnerMock = new Mock<IMoyaTestRunner>();
            _exceptionCatcherDecorator = new ExceptionCatcherDecorator(_testRunnerMock.Object);
        }

        [Fact]
        public void DecoratorCatchesThrownException()
        {
            MethodInfo method = new Action(() => { }).Method;
            _testRunnerMock
                .Setup(x => x.Execute(It.IsAny<MethodInfo>()))
                .Throws<Exception>();

            var exception = Record.Exception(() => _exceptionCatcherDecorator.Execute(method));

            Assert.Null(exception);
        }


        [Fact]
        public void DecoratorAddsCaughtExceptionToTestResult()
        {
            MethodInfo method = new Action(() => { }).Method;
            _testRunnerMock
                .Setup(x => x.Execute(It.IsAny<MethodInfo>()))
                .Throws<Exception>();

            ITestResult testResult = _exceptionCatcherDecorator.Execute(method);

            Assert.NotNull(testResult.Exception);
        }


        [Fact]
        public void DecoratorAddsFailureToTestResultUponCaughtException()
        {
            MethodInfo method = new Action(() => { }).Method;
            _testRunnerMock
                .Setup(x => x.Execute(It.IsAny<MethodInfo>()))
                .Throws<Exception>();

            ITestResult testResult = _exceptionCatcherDecorator.Execute(method);

            Assert.Equal(TestOutcome.Failure, testResult.Outcome);
        }
    }
}