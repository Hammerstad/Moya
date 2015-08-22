namespace Moya.Runners
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Attributes;
    using Models;

    internal class StressTestRunner : IStressTestRunner
    {
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
                new Thread(delegate()
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
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
                TestOutcome = TestOutcome.Success,
                TestType = TestType.Test
            };
        }

        private static bool MethodHasStressAttribute(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(StressAttribute), true);

            return moyaAttributes.Length != 0;
        }

        private void SetUsersAndTimes(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(StressAttribute), true);

            StressAttribute stressAttribute = moyaAttributes.FirstOrDefault(x => x.GetType() == typeof(StressAttribute)) as StressAttribute;

            Users = stressAttribute.Users;
            Times = stressAttribute.Times;
        }
    }
}