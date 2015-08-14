namespace TestMoya.Factories
{
    using Moya.Attributes;
    using Moya.Exceptions;
    using Moya.Factories;
    using Moya.Runners;
    using Xunit;

    public class TestRunnerFactoryTests
    {
        private readonly ITestRunnerFactory testRunnerFactory = new TestRunnerFactory();
        
        [Fact]
        public void GetTestRunnerForStressAttributeReturnsTimerDecoratorContainingStressTestRunner()
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));

            Assert.Equal(typeof(TimerDecorator), testRunner.GetType());
            Assert.Equal(typeof(StressTestRunner), ((TimerDecorator)testRunner).DecoratedTestRunner.GetType());
        }

        [Fact]
        public void GetTestRunnerForInvalidAttributeThrowsMoyaException()
        {
            var exception = Record.Exception(() => testRunnerFactory.GetTestRunnerForAttribute(typeof(MoyaAttribute)));

            Assert.Equal(typeof(MoyaException), exception.GetType());
            Assert.Equal("Unable to provide moya test runner for type Moya.Attributes.MoyaAttribute", exception.Message);
        }

        [Fact]
        public void GetTestRunnerForWarmupAttributeReturnsTimerDecoratorContainingWarmupTestRunner()
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(WarmupAttribute));

            Assert.Equal(typeof(TimerDecorator), testRunner.GetType());
            Assert.Equal(typeof(WarmupTestRunner), ((TimerDecorator)testRunner).DecoratedTestRunner.GetType());
        }

        [Fact]
        public void GetTestRunnerForLongerThanAttributeReturnsTimerDecoratorContainingLongerThanTestRunner()
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(LongerThanAttribute));

            Assert.Equal(typeof(TimerDecorator), testRunner.GetType());
            Assert.Equal(typeof(LongerThanTestRunner), ((TimerDecorator)testRunner).DecoratedTestRunner.GetType());
        }

        [Fact]
        public void GetTestRunnerForLessThanAttributeReturnsTimerDecoratorContainingLessThanTestRunner()
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(LessThanAttribute));

            Assert.Equal(typeof(TimerDecorator), testRunner.GetType());
            Assert.Equal(typeof(LessThanTestRunner), ((TimerDecorator)testRunner).DecoratedTestRunner.GetType());
        }
    }
}