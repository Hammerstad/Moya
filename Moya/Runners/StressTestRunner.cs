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
    /// The test runner responsible for running methods marked with
    /// the <see cref="StressAttribute"/> attribute. Can run a method
    /// multiple times, both in parallel and sequence.
    /// </summary>
    internal class StressTestRunner : IStressTestRunner
    {
        /// <summary>
        /// A list of threads, each running the same method, potentially multiple times. 
        /// The amount of threads will equal the value of the <see cref="Users"/> property.
        /// </summary>
        private readonly IList<Thread> threadPool = new List<Thread>(); 

        private int m_times = 1;
        private int m_users = 1;

        /// <summary>
        /// Represents the amount of sequential executions.
        /// </summary>
        public int Times
        {
            get { return m_times; }
            private set { m_times = value; }
        }

        /// <summary>
        /// Represents the amount of parallel executions.
        /// </summary>
        public int Users
        {
            get { return m_users; }
            private set { m_users = value; }
        }

        /// <summary>
        /// Executes a method several times in sequence or parallel, or both.
        /// </summary>
        /// <param name="methodInfo">A method attributed with a <see cref="StressAttribute"/> attribute.</param>
        /// <returns>A <see cref="ITestResult"/> object containing information about the test run.</returns>
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

            DetectUsersAndTimesFromMethod(methodInfo);
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

        /// <summary>
        /// Waits for all the threads in the <see cref="threadPool"/> to end.
        /// </summary>
        private void WaitForThreadsToEnd()
        {
            foreach (var thread in threadPool)
            {
                thread.Join();
            }
        }

        /// <summary>
        /// Utility method which checks if a method is attributed with a <see cref="StressAttribute"/> attribute.
        /// </summary>
        /// <param name="methodInfo">A method we want to investigate.</param>
        /// <returns>Returns <see cref="c:true"/> if the method is attributed with 
        /// a <see cref="StressAttribute"/> attribute.</returns>
        private static bool MethodHasStressAttribute(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(StressAttribute), true);

            return moyaAttributes.Length != 0;
        }

        /// <summary>
        /// Utility method which finds the amount of parallel execution(users) and sequential execution(times) of a method.
        /// These values are extracted from the <see cref="StressAttribute"/> attribute surrounding the method.
        /// The values are stored in this object's <see cref="Users"/> and <see cref="Times"/> properties.
        /// </summary>
        /// <param name="methodInfo">A method attributed with a <see cref="StressAttribute"/> attribute.</param>
        private void DetectUsersAndTimesFromMethod(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(StressAttribute), true);

            StressAttribute stressAttribute = moyaAttributes.FirstOrDefault(x => x is StressAttribute) as StressAttribute;

            // ReSharper disable once PossibleNullReferenceException
            Users = stressAttribute.Users;
            Times = stressAttribute.Times;
        }
    }
}