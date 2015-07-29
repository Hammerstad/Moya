namespace Moya.Runner.Runners
{
    using System;
    using System.Reflection;
    using System.Threading;
    using Models;

    public class StressTestRunner : IStressTestRunner
    {
        private int m_times = 1;
        private int m_runners = 1;

        public int Times
        {
            get { return m_times; }
            set { m_times = value; }
        }

        public int Runners
        {
            get { return m_runners; }
            set { m_runners = value; }
        }

        public ITestResult Execute<T>(Func<T> function)
        {
            var countdownEvent = new CountdownEvent(Runners);

            for (var i = 0; i < Runners; i++)
            {
                new Thread(delegate()
                {
                    for (var j = 0; j < Times; j++)
                    {
                        function();
                    }
                    countdownEvent.Signal();
                }).Start();
            }

            countdownEvent.Wait();

            //TODO: Implement
            return null;
        }

        public ITestResult Execute(Action action)
        {
            var countdownEvent = new CountdownEvent(Runners);

            for (var i = 0; i < Runners; i++)
            {
                new Thread(delegate()
                {
                    for (var j = 0; j < Times; j++)
                    {
                        action();
                    }
                    countdownEvent.Signal();
                }).Start();
            }

            countdownEvent.Wait();

            //TODO: Implement
            return null;
        }

        public ITestResult Execute(MethodInfo methodInfo, Type type)
        {
            type = type ?? methodInfo.DeclaringType;

            var countdownEvent = new CountdownEvent(Runners);

            for (var i = 0; i < Runners; i++)
            {
                new Thread(delegate()
                {
                    var instance = Activator.CreateInstance(type);
                    for (var j = 0; j < Times; j++)
                    {
                        methodInfo.Invoke(instance, null);
                    }
                    countdownEvent.Signal();
                }).Start();
            }

            countdownEvent.Wait();

            return new TestResult
            {
                TestOutcome = TestOutcome.Success
            };
        }
    }
}