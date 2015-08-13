namespace Moya.Runner.Runners
{
    using System.Reflection;
    using Models;

    public interface IMoyaTestRunner
    {
        ITestResult Execute(MethodInfo methodInfo);
    }
}