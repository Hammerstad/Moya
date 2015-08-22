namespace Moya.Factories
{
    using Attributes;
    using Runners;
    using System;

    public interface ITestRunnerFactory
    {
        IMoyaTestRunner GetTestRunnerForAttribute(Type type);

        void AddTestRunnerForAttribute(Type testRunner, Type attribute);
    }
}