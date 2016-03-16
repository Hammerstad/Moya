namespace TestMoya.Runners.Decorators
{
    using System.Reflection;
    using Moq;
    using Moya.Models;
    using Moya.Runners;
    using Moya.Runners.Decorators;
    using Xunit;

    public class MethodNameDecoratorTests
    {
        private readonly IMethodNameDecorator _methodNameDecorator;
        private readonly Mock<IMoyaTestRunner> _mockTestRunner; 

        public MethodNameDecoratorTests()
        {
            _mockTestRunner = new Mock<IMoyaTestRunner>();
            _methodNameDecorator = new MethodNameDecorator(_mockTestRunner.Object);
        }

        [Fact]
        public void DecoratorExecutesUnderlyingTestRunner()
        {
            bool underlyingTestRunnerCalled = false;
            MethodInfo method = typeof(TestClass).GetMethod("MyTestMethod");
            _mockTestRunner
                .Setup(x => x.Execute(method))
                .Callback(() => underlyingTestRunnerCalled = true)
                .Returns(new TestResult());

            _methodNameDecorator.Execute(method);

            Assert.True(underlyingTestRunnerCalled);
        }

        [Fact]
        public void DecoratorAddsDataToTestResult()
        {
            const string MethodName = "MyTestMethod";
            string expectedNamespace = typeof(TestClass).FullName + "." + MethodName;
            MethodInfo method = typeof(TestClass).GetMethod(MethodName);
            _mockTestRunner
                .Setup(x => x.Execute(method))
                .Returns(new TestResult());

            ITestResult testResult = _methodNameDecorator.Execute(method);

            Assert.Equal(MethodName, testResult.MethodName);
            Assert.Equal(expectedNamespace, testResult.Namespace);
        }

        class TestClass
        {
            // ReSharper disable once UnusedMember.Local
            public void MyTestMethod()
            {

            }
        }
    }
}