namespace Moya.Runner.Runners
{
    using System;
    using System.Reflection;
    using Models;

    public interface ITestRunner
    {
        ITestResult Execute<T>(Func<T> function);
        ITestResult Execute(Action action);
        ITestResult Execute(MethodInfo methodInfo, Type type = null);
    }
}