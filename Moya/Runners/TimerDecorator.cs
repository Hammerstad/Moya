using System;
using System.Diagnostics;
using Moya.Statistics;

namespace Moya.Runners
{
    public class TimerDecorator : ITestRunner
    {
        public TimerDecorator(ITestRunner testRunner)
        {
            this.testRunner = testRunner;
            duration = DurationManager.DefaultInstance;
        }

        private readonly ITestRunner testRunner;
        private readonly IDurationManager duration;
        
        public void Execute<T>(Func<T> function)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            testRunner.Execute(function);

            stopwatch.Stop();
            duration.RegisterDuration(function.GetHashCode(), stopwatch.ElapsedMilliseconds);
        }

        public void Execute(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            testRunner.Execute(action);

            stopwatch.Stop();
            duration.RegisterDuration(action.GetHashCode(), stopwatch.ElapsedMilliseconds);
        }
    }
}