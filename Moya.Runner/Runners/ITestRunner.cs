namespace Moya.Runner.Runners
{
    using System;
    using System.Reflection;

    public interface ITestRunner
    {
        void Execute<T>(Func<T> function);
        void Execute(Action action);
        void Execute(MethodInfo methodInfo, Type type = null);
    }
}