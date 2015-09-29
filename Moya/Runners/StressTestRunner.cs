namespace Moya.Runners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Attributes;
    using Models;

    /// <summary>
    /// A test runner for the <see cref="StressAttribute"/> attribute. Can run a method
    /// multiple times, both in parallel and sequence.
    /// </summary>
    internal class StressTestRunner : IStressTestRunner
    {
        private readonly IList<Thread> threadPool = new List<Thread>(); 

        private int m_times = 1;
        private int m_users = 1;

        public int Times
        {
            get { return m_times; }
            private set { m_times = value; }
        }

        public int Users
        {
            get { return m_users; }
            private set { m_users = value; }
        }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            var type = methodInfo.DeclaringType;

            if (!MethodHasStressAttribute(methodInfo))
            {
                return new TestResult
                {
                    TestOutcome = TestOutcome.NotFound,
                    TestType = TestType.Test
                };
            }

            SetUsersAndTimes(methodInfo);
            var countdownEvent = new CountdownEvent(Users);

            for (var i = 0; i < Users; i++)
            {
                var thread = new Thread(delegate()
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var instance = Activator.CreateInstance(type);
                    for (var j = 0; j < Times; j++)
                    {
                        methodInfo.Invoke(instance, null);
                    }
                    countdownEvent.Signal();
                });
                thread.Start();
                threadPool.Add(thread);
            }

            countdownEvent.Wait();
            WaitForThreadsToEnd();

            return new TestResult
            {
                TestOutcome = TestOutcome.Success,
                TestType = TestType.Test
            };
        }

        private void WaitForThreadsToEnd()
        {
            foreach (var thread in threadPool)
            {
                thread.Join();
            }
        }

        private static bool MethodHasStressAttribute(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(StressAttribute), true);

            return moyaAttributes.Length != 0;
        }

        private void SetUsersAndTimes(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(StressAttribute), true);

            StressAttribute stressAttribute = moyaAttributes.FirstOrDefault(x => x is StressAttribute) as StressAttribute;

            // ReSharper disable once PossibleNullReferenceException
            Users = stressAttribute.Users;
            Times = stressAttribute.Times;
        }
    }
}