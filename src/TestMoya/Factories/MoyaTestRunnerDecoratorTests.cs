namespace TestMoya.Factories
{
    using Moq;
    using Moya.Factories;
    using Moya.Runners;
    using Moya.Runners.Decorators;
    using Xunit;

    public class MoyaTestRunnerDecoratorTests
    {
        private readonly IMoyaTestRunnerDecorator _moyaTestRunnerDecorator;
        private readonly Mock<IMoyaTestRunner> _mockTestRunner;

        public MoyaTestRunnerDecoratorTests()
        {
            _moyaTestRunnerDecorator = new MoyaTestRunnerDecorator();
            _mockTestRunner = new Mock<IMoyaTestRunner>();
        }

        [Fact]
        public void DecoratingAddsTimerDecorator()
        {
            IMoyaTestRunner decoratedTestRunner = _moyaTestRunnerDecorator.DecorateTestRunner(_mockTestRunner.Object);

            Assert.Equal(typeof(TimerDecorator), decoratedTestRunner.GetType());
        }

        [Fact]
        public void DecoratingAddsMethodNameDecorator()
        {
            IMoyaTestRunner decoratedTestRunner = _moyaTestRunnerDecorator.DecorateTestRunner(_mockTestRunner.Object);

            Assert.Equal(typeof(MethodNameDecorator), ((ITimerDecorator)decoratedTestRunner).DecoratedTestRunner.GetType());
        }
    }
}