namespace Moya.Factories
{
    using Runners;
    using System;

    public interface ITestRunnerFactory
    {
        IMoyaTestRunner GetTestRunnerForAttribute(Type type);
    }
}