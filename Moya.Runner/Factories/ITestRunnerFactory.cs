namespace Moya.Runner.Factories
{
    using Runners;
    using System;

    public interface ITestRunnerFactory
    {
        IMoyaTestRunner GetTestRunnerForAttribute(Type type);
    }
}