namespace Moya.Runner.Runners
{
    using System.Reflection;
    using Models;

    public interface ITestRunner
    {
        ITestResult Execute(MethodInfo methodInfo);
    }
}