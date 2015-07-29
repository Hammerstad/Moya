namespace Moya.Runner.Runners
{
    using System;
    using System.Reflection;
    using System.Threading;

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

        public void Execute<T>(Func<T> function)
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
        }

        public void Execute(Action action)
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
        }

        public void Execute(MethodInfo methodInfo, Type type)
        {
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
        }
    }
}