namespace TestMoya.Runners
{
    using System;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Factories;
    using Moya.Models;
    using Moya.Runners;
    using Xunit;

    public class MoyaConfigurationTestRunnerTests
    {
        private IMoyaConfigurationTestRunner Runner;

        public MoyaConfigurationTestRunnerTests()
        {
            Runner = new MoyaConfigurationTestRunner();
        }

        [Fact]
        public void UseConfigurationToAddTestRunnerShouldWork()
        {
            Runner.MoyaTestRunnerFactory = new MoyaTestRunnerFactory();
            MethodInfo method = typeof(TestClass).GetMethod("MyConfiguration");

            Runner.Execute(method);

            Assert.NotNull(Runner.MoyaTestRunnerFactory.GetTestRunnerForAttribute(typeof(TestAttribute)));
        }

        [Fact]
        public void UseConfigurationToAddTestRunnerWithBadSetupShouldNotWork()
        {
            MethodInfo method = typeof(TestClass).GetMethod("MyConfiguration");

            var result = Runner.Execute(method);

            Assert.Equal(TestOutcome.Failure, result.TestOutcome);
        }

        [Fact]
        public void MoyaConfigurationTestsArePreTests()
        {

            Runner.MoyaTestRunnerFactory = new MoyaTestRunnerFactory();
            MethodInfo method = typeof(TestClass).GetMethod("MyConfiguration");

            var result = Runner.Execute(method);

            Assert.Equal(TestType.PreTest, result.TestType);
        }

        public class TestClass
        {
            [MoyaConfiguration]
            public void MyConfiguration(IMoyaTestRunnerFactory factory)
            {
                factory.AddTestRunnerForAttribute(typeof(TestRunner), typeof(TestAttribute));
            }
        }

        public class TestAttribute : MoyaAttribute
        {
            
        }

        public class TestRunner : ITestRunner
        {
            public ITestResult Execute(MethodInfo methodInfo)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}