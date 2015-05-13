namespace Moya.Runner.Runners
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Statistics;

    public class TimerDecorator
    {
        public TimerDecorator()
        {
            duration = DurationManager.DefaultInstance;
        }

        public TimerDecorator(ITestRunner testRunner) : this()
        {
            DecoratedTestRunner = testRunner;
        }

        private readonly IDurationManager duration;
        public ITestRunner DecoratedTestRunner { get; set; }

        public void Execute<T>(Func<T> function)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            DecoratedTestRunner.Execute(function);

            stopwatch.Stop();
            duration.AddOrUpdateDuration(function.GetHashCode(), stopwatch.ElapsedMilliseconds);
        }

        public void Execute(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            DecoratedTestRunner.Execute(action);

            stopwatch.Stop();
            duration.AddOrUpdateDuration(action.GetHashCode(), stopwatch.ElapsedMilliseconds);
        }

        public void Execute(MethodInfo methodInfo, Type type = null)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            DecoratedTestRunner.Execute(methodInfo, type);

            stopwatch.Stop();
            duration.AddOrUpdateDuration(methodInfo.GetHashCode(), stopwatch.ElapsedMilliseconds);
        }
    }
}