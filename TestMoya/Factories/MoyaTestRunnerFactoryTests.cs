namespace TestMoya.Factories
{
    using System;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Exceptions;
    using Moya.Factories;
    using Moya.Models;
    using Moya.Runners;
    using Xunit;

    public class MoyaTestRunnerFactoryTests
    {
        private readonly IMoyaTestRunnerFactory testRunnerFactory = new MoyaTestRunnerFactory();
        
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

        [Fact]
        public void AddExistingTestRunnerShouldThrowMoyaException()
        {
            Type stressAttributeType = typeof(StressAttribute);
            Type stressTestRunnerType = typeof(StressTestRunner);
            const string ExpectedExceptionMessage = "There already exists an entry for Moya.Runners.StressTestRunner - Moya.Attributes.StressAttribute.";
            
            var exception = Record.Exception(() => testRunnerFactory.AddTestRunnerForAttribute(stressTestRunnerType, stressAttributeType));

            Assert.Equal(typeof(MoyaException), exception.GetType());
            Assert.Equal(ExpectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void AddNewTestRunnerForAttributeShouldAddTimerDecoratedTestRunner()
        {
            Type attributeType = typeof(DummyAttribute);
            Type testRunnerType = typeof(DummyTestRunner);

            testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType);
            var actualTestRunner = testRunnerFactory.GetTestRunnerForAttribute(attributeType);

            Assert.Equal(typeof(TimerDecorator), actualTestRunner.GetType());
        }

        [Fact]
        public void AddNewTestRunnerForAttributeShouldAddMapping()
        {
            Type attributeType = typeof(DummyAttribute);
            Type testRunnerType = typeof(DummyTestRunner);

            testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType);
            var actualTestRunner = testRunnerFactory.GetTestRunnerForAttribute(attributeType);

            Assert.Equal(testRunnerType, ((ITimerDecorator)actualTestRunner).DecoratedTestRunner.GetType());
        }

        [Fact]
        public void AddInvalidTestRunnerShouldThrowException()
        {
            Type attributeType = typeof(DummyAttribute);
            Type testRunnerType = typeof(Object);
            const string ExpectedExceptionMessage = "System.Object is not a Moya Test Runner.";

            var exception = Record.Exception(() => testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType));

            Assert.Equal(typeof(MoyaException), exception.GetType());
            Assert.Equal(ExpectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void AddInvalidAttributeShouldThrowException()
        {
            Type attributeType = typeof(Object);
            Type testRunnerType = typeof(DummyTestRunner);
            const string ExpectedExceptionMessage = "System.Object is not a Moya Attribute.";

            var exception = Record.Exception(() => testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType));

            Assert.Equal(typeof(MoyaException), exception.GetType());
            Assert.Equal(ExpectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void GetInstanceReturnsAValidTestRunnerFactory()
        {
            IMoyaTestRunnerFactory defaultFactory = MoyaTestRunnerFactory.DefaultInstance;

            Assert.IsType(typeof(MoyaTestRunnerFactory), defaultFactory);
        }

        [Fact]
        public void GetInstanceReturnsTheSameObjectTwice()
        {
            IMoyaTestRunnerFactory firstInstance = MoyaTestRunnerFactory.DefaultInstance;
            IMoyaTestRunnerFactory secondInstance = MoyaTestRunnerFactory.DefaultInstance;

            Assert.Same(firstInstance,secondInstance);
        }

        class DummyAttribute : MoyaAttribute
        {
            
        }

        class DummyTestRunner : ITestRunner
        {
            public ITestResult Execute(MethodInfo methodInfo)
            {
                throw new NotImplementedException();
            }
        }
    }
}