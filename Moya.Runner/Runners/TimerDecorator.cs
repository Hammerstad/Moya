namespace Moya.Runner.Runners
{
    using System.Diagnostics;
    using System.Reflection;
    using Models;
    using Statistics;

    public class TimerDecorator : ITestRunner
    {
        private readonly IDurationManager duration;

        public ITestRunner DecoratedTestRunner { get; set; }
        
        public TimerDecorator(ITestRunner testRunner)
        {
            DecoratedTestRunner = testRunner;
            duration = DurationManager.DefaultInstance;
        }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = DecoratedTestRunner.Execute(methodInfo);

            stopwatch.Stop();
            duration.AddOrUpdateDuration(methodInfo.GetHashCode(), stopwatch.ElapsedMilliseconds);

            result.Duration = stopwatch.ElapsedMilliseconds;
            return result;
        }
    }
}