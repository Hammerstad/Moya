namespace TestMoya.Runner.Factories
{
    using Moya.Attributes;
    using Moya.Exceptions;
    using Moya.Runner.Factories;
    using Moya.Runner.Runners;
    using Xunit;

    public class TestRunnerFactoryTests
    {
        private readonly ITestRunnerFactory testRunnerFactory = new TestRunnerFactory();

        [Fact]
        public void GetTestRunnerForStressAttributeReturnsTimerDecorator()
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));

            Assert.Equal(typeof(TimerDecorator), testRunner.GetType());
        }

        [Fact]
        public void GetTestRunnerForStressAttributeReturnsTimerDecoratorContainingStressTestRunner()
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));

            Assert.Equal(typeof(StressTestRunner), ((TimerDecorator)testRunner).DecoratedTestRunner.GetType());
        }

        [Fact]
        public void GetTestRunnerForInvalidAttributeThrowsMoyaException()
        {
            IMoyaTestRunner testRunner;
            
            var exception = Record.Exception(() => testRunnerFactory.GetTestRunnerForAttribute(typeof(MoyaAttribute)));

            Assert.Equal(typeof(MoyaException), exception.GetType());
            Assert.Equal("Unable to provide moya test runner for type Moya.Attributes.MoyaAttribute", exception.Message);
        }
    }
}