using Moya.Models;

namespace Moya.Runner.Runners
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
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

        public ITestResult Execute<T>(Func<T> function)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = DecoratedTestRunner.Execute(function);

            stopwatch.Stop();
            duration.AddOrUpdateDuration(function.GetHashCode(), stopwatch.ElapsedMilliseconds);

            result.Duration = stopwatch.ElapsedMilliseconds;
            return result;
        }

        public ITestResult Execute(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = DecoratedTestRunner.Execute(action);

            stopwatch.Stop();
            duration.AddOrUpdateDuration(action.GetHashCode(), stopwatch.ElapsedMilliseconds);

            result.Duration = stopwatch.ElapsedMilliseconds;
            return result;
        }

        public ITestResult Execute(MethodInfo methodInfo, Type type = null)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = DecoratedTestRunner.Execute(methodInfo, type);

            stopwatch.Stop();
            duration.AddOrUpdateDuration(methodInfo.GetHashCode(), stopwatch.ElapsedMilliseconds);

            result.Duration = stopwatch.ElapsedMilliseconds;
            return result;
        }
    }
}