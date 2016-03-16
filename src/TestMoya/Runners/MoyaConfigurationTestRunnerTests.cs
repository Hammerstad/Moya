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
        public class Execute
        {
            private readonly IMoyaConfigurationTestRunner Runner;

            public Execute()
            {
                Runner = new MoyaConfigurationTestRunner();
            }

            [Fact]
            public void UseConfigurationToAddTestRunnerShouldWork()
            {
                MethodInfo method = typeof(TestClass).GetMethod("MyGoodConfiguration");

                var result = Runner.Execute(method);

                Assert.Equal(TestOutcome.Success, result.TestOutcome);
                Assert.NotNull(MoyaTestRunnerFactory.DefaultInstance.GetTestRunnerForAttribute(typeof(TestAttribute)));
            }

            [Fact]
            public void UseConfigurationToAddTestRunnerWithBadSetupShouldNotWork()
            {
                MethodInfo method = typeof(TestClass).GetMethod("MyBadConfiguration");

                var result = Runner.Execute(method);

                Assert.Equal(TestOutcome.Failure, result.TestOutcome);
            }

            [Fact]
            public void MoyaConfigurationTestsArePreTests()
            {

                MethodInfo method = typeof(TestClass).GetMethod("MyBadConfiguration");

                var result = Runner.Execute(method);

                Assert.Equal(TestType.PreTest, result.TestType);
            }
        }

        public class TestClass
        {
            [MoyaConfiguration]
            public void MyGoodConfiguration(IMoyaTestRunnerFactory factory)
            {
                factory.AddTestRunnerForAttribute(typeof(TestRunner), typeof(TestAttribute));
            }

            [MoyaConfiguration]
            public void MyBadConfiguration(IMoyaTestRunnerFactory factory)
            {
                factory.AddTestRunnerForAttribute(typeof(TestRunner), typeof(Object));
            }
        }

        public class TestAttribute : MoyaAttribute
        {
            
        }

        public class TestRunner : ITestRunner
        {
            ITestResult IMoyaTestRunner.Execute(MethodInfo methodInfo)
            {
                throw new NotImplementedException();
            }
        }
    }
}