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
        /// Lock used when setting exception from potentially many threads.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// A list of threads, each running the same method, potentially multiple times. 
        /// The amount of threads will equal the value of the <see cref="Users"/> property.
        /// </summary>
        private readonly IList<Thread> _threadPool = new List<Thread>();

        /// <summary>
        /// Represents the amount of sequential executions.
        /// </summary>
        public int Times { get; private set; } = 1;

        /// <summary>
        /// Represents the amount of parallel executions.
        /// </summary>
        public int Users { get; private set; } = 1;

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

            Exception latestException = null;
            for (var i = 0; i < Users; i++)
            {
                var thread = new Thread(delegate()
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var instance = Activator.CreateInstance(type);
                    for (var j = 0; j < Times; j++)
                    {
                        SafeExecute(() => methodInfo.Invoke(instance, null), out latestException);
                    }
                    countdownEvent.Signal();
                });
                thread.Start();
                _threadPool.Add(thread);
            }

            countdownEvent.Wait();
            WaitForThreadsToEnd();

            return new TestResult
            {
                TestOutcome = (latestException == null) ? TestOutcome.Success : TestOutcome.Failure,
                TestType = TestType.Test, 
                Exception = latestException
            };
        }

        /// <summary>
        /// Waits for all the threads in the <see cref="_threadPool"/> to end.
        /// </summary>
        private void WaitForThreadsToEnd()
        {
            foreach (var thread in _threadPool)
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

        /// <summary>
        /// Ensures that exceptions thrown by an action is caught and passed out.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exception"></param>
        private void SafeExecute(Action action, out Exception exception)
        {
            exception = null;

            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                lock (_lock)
                {
                    exception = ex;
                }
            }
        }
    }
}