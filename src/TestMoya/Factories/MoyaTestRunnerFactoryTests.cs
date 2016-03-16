namespace TestMoya.Factories
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Exceptions;
    using Moya.Factories;
    using Moya.Models;
    using Moya.Runners;
    using Shouldly;
    using Xunit;

    public class MoyaTestRunnerFactoryTests
    {
        public class GetTestRunnerForAttribute
        {
            private readonly IMoyaTestRunnerFactory testRunnerFactory = new MoyaTestRunnerFactory();
            
            [Fact]
            public void InvalidAttributeThrowsMoyaException()
            {
                var exception = Record.Exception(() => testRunnerFactory.GetTestRunnerForAttribute(typeof(MoyaAttribute)));

                Assert.Equal(typeof(MoyaException), exception.GetType());
                Assert.Equal("Unable to provide moya test runner for type Moya.Attributes.MoyaAttribute", exception.Message);
            }
        }

        public class AddTestRunnerForAttribute
        {
            private readonly IMoyaTestRunnerFactory testRunnerFactory = new MoyaTestRunnerFactory();

            [Fact]
            public void ExistingTestRunnerShouldThrowMoyaException()
            {
                Type stressAttributeType = typeof(StressAttribute);
                Type stressTestRunnerType = typeof(StressTestRunner);
                const string ExpectedExceptionMessage = "There already exists an entry for Moya.Runners.StressTestRunner - Moya.Attributes.StressAttribute.";

                var exception = Record.Exception(() => testRunnerFactory.AddTestRunnerForAttribute(stressTestRunnerType, stressAttributeType));

                Assert.Equal(typeof(MoyaException), exception.GetType());
                Assert.Equal(ExpectedExceptionMessage, exception.Message);
            }

            [Fact]
            public void NewTestRunnerShouldAddMapping()
            {
                Type attributeType = typeof(DummyAttribute);
                Type testRunnerType = typeof(DummyTestRunner);

                testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType);
                var actualTestRunner = testRunnerFactory.GetTestRunnerForAttribute(attributeType);

                Assert.Equal(testRunnerType, actualTestRunner.GetType());
            }

            [Fact]
            public void InvalidTestRunnerShouldThrowException()
            {
                Type attributeType = typeof(DummyAttribute);
                Type testRunnerType = typeof(Object);
                const string ExpectedExceptionMessage = "System.Object is not a Moya Test Runner.";

                var exception = Record.Exception(() => testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType));

                Assert.Equal(typeof(MoyaException), exception.GetType());
                Assert.Equal(ExpectedExceptionMessage, exception.Message);
            }

            [Fact]
            public void InvalidAttributeShouldThrowException()
            {
                Type attributeType = typeof(Object);
                Type testRunnerType = typeof(DummyTestRunner);
                const string ExpectedExceptionMessage = "System.Object is not a Moya Attribute.";

                var exception = Record.Exception(() => testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType));

                Assert.Equal(typeof(MoyaException), exception.GetType());
                Assert.Equal(ExpectedExceptionMessage, exception.Message);
            }

            [Fact]
            public void CustomPreTestRunnerShouldBeReturnedByGetCustomPreTestRunners()
            {
                Type attributeType = typeof(DummyAttribute);
                Type testRunnerType = typeof(DummyPreTestRunner);

                testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType);
                var testRunners = testRunnerFactory.GetCustomPreTestRunners().ToList();

                Assert.NotEmpty(testRunners);
                Assert.True(testRunners.Count() == 1);
            }

            [Fact]
            public void CustomTestRunnerShouldBeReturnedByGetCustomTestRunners()
            {
                Type attributeType = typeof(DummyAttribute);
                Type testRunnerType = typeof(DummyCustomTestRunner);

                testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType);
                var testRunners = testRunnerFactory.GetCustomTestRunners().ToList();

                Assert.NotEmpty(testRunners);
                Assert.True(testRunners.Count() == 1);
            }

            [Fact]
            public void CustomPostTestRunnerShouldBeReturnedByGetCustomPostTestRunners()
            {
                Type attributeType = typeof(DummyAttribute);
                Type testRunnerType = typeof(DummyPostTestRunner);

                testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType);
                var testRunners = testRunnerFactory.GetCustomPostTestRunners().ToList();

                Assert.NotEmpty(testRunners);
                Assert.True(testRunners.Count() == 1);
            }
        }

        public class DefaultInstance
        {
            [Fact]
            public void ReturnsAValidTestRunnerFactory()
            {
                IMoyaTestRunnerFactory defaultFactory = MoyaTestRunnerFactory.DefaultInstance;

                Assert.IsType(typeof(MoyaTestRunnerFactory), defaultFactory);
            }

            [Fact]
            public void ReturnsTheSameObjectTwice()
            {
                IMoyaTestRunnerFactory firstInstance = MoyaTestRunnerFactory.DefaultInstance;
                IMoyaTestRunnerFactory secondInstance = MoyaTestRunnerFactory.DefaultInstance;

                Assert.Same(firstInstance, secondInstance);
            }
        }

        public class GetAttributeForTestRunner
        {
            private readonly IMoyaTestRunnerFactory testRunnerFactory = new MoyaTestRunnerFactory();

            [Fact]
            public void InvalidTypeThrowsException()
            {
                Type testRunnerType = typeof(Object);
                const string ExpectedExceptionMessage = "System.Object is not a Moya Test Runner.";

                var exception = Record.Exception(() => testRunnerFactory.GetAttributeForTestRunner(testRunnerType));

                exception.ShouldBeOfType<MoyaException>();
                Assert.Equal(ExpectedExceptionMessage, exception.Message);
            }

            [Fact]
            public void UnregistredTestRunnerThrowsException()
            {
                Type testRunnerType = typeof(DummyTestRunner);

                var exception = Record.Exception(() => testRunnerFactory.GetAttributeForTestRunner(testRunnerType));

                exception.ShouldBeOfType<ArgumentNullException>();
            }

            [Fact]
            public void RegisteredTestRunnerReturnsAttribute()
            {
                Type attributeType = typeof(DummyAttribute);
                Type testRunnerType = typeof(DummyTestRunner);

                testRunnerFactory.AddTestRunnerForAttribute(testRunnerType, attributeType);
                var attribute = testRunnerFactory.GetAttributeForTestRunner(testRunnerType);

                attribute.ShouldBeOfType<DummyAttribute>();
            }
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

        class DummyPreTestRunner : ICustomPreTestRunner
        {
            public ITestResult Execute(MethodInfo methodInfo)
            {
                throw new NotImplementedException();
            }
        }

        class DummyPostTestRunner : ICustomPostTestRunner
        {
            public ITestResult Execute(MethodInfo methodInfo)
            {
                throw new NotImplementedException();
            }
        }

        class DummyCustomTestRunner : ICustomTestRunner
        {
            public ITestResult Execute(MethodInfo methodInfo)
            {
                throw new NotImplementedException();
            }
        }
    }
}