namespace Moya.Runner.Runners
{
    using System.Diagnostics;
    using System.Reflection;
    using Models;

    public class TimerDecorator : ITimerDecorator
    {
        public IMoyaTestRunner DecoratedTestRunner { get; set; }
        
        public TimerDecorator(IMoyaTestRunner testRunner)
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