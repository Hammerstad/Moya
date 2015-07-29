namespace Moya.Runner.Factories
{
    using Runners;
    using System;

    public interface ITestRunnerFactory
    {
        ITestRunner GetTestRunnerForAttribute(Type type);
    }
}