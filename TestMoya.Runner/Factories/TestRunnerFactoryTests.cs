namespace TestMoya.Runner.Factories
{
    using Moya.Attributes;
    using Moya.Runner.Factories;
    using Moya.Runner.Runners;
    using Xunit;

    public class TestRunnerFactoryTests
    {
        private readonly ITestRunnerFactory testRunnerFactory = new TestRunnerFactory();

        [Fact]
        public void GetTestRunnerForStressAttributeReturnsTimerDecorator()
        {
            ITestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));

            Assert.Equal(typeof(TimerDecorator), testRunner.GetType());
        }

        [Fact]
        public void GetTestRunnerForStressAttributeReturnsTimerDecoratorContainingStressTestRunner()
        {
            ITestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));

            Assert.Equal(typeof(StressTestRunner), ((TimerDecorator)testRunner).DecoratedTestRunner.GetType());
        }
    }
}