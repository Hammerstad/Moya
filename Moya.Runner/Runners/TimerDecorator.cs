namespace Moya.Runner.Runners
{
    using System.Diagnostics;
    using System.Reflection;
    using Models;

    public class TimerDecorator : ITestRunner
    {
        public ITestRunner DecoratedTestRunner { get; set; }
        
        public TimerDecorator(ITestRunner testRunner)
        {
            DecoratedTestRunner = testRunner;
        }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = DecoratedTestRunner.Execute(methodInfo);

            stopwatch.Stop();

            result.Duration = stopwatch.ElapsedMilliseconds;
            return result;
        }
    }
}