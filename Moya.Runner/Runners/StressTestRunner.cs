namespace Moya.Runner.Runners
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Models;

    public class StressTestRunner : IStressTestRunner
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
            SetUsersAndTimes(methodInfo);
            var countdownEvent = new CountdownEvent(Users);

            for (var i = 0; i < Users; i++)
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

        private void SetUsersAndTimes(MethodInfo methodInfo)
        {
            StressAttribute stressAttribute = methodInfo
                .GetCustomAttributes(typeof(MoyaAttribute), true)
                .First(x => x.GetType() == typeof(StressAttribute)) as StressAttribute;
            
            if (stressAttribute == null)
            {
                throw new MoyaAttributeNotFoundException("Unable to find {0} in {1}".FormatWith(typeof(StressAttribute), methodInfo));
            }

            Users = stressAttribute.Users;
            Times = stressAttribute.Times;
        }
    }
}