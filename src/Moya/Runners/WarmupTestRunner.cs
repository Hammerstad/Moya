namespace Moya.Runners
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using Models;

    /// <summary>
    /// A test runner which runs a method decorated with a <see cref="WarmupAttribute"/>
    /// attribute for a given time without recording any statistics.
    /// </summary>
    internal class WarmupTestRunner : IWarmupTestRunner
    {
        /// <summary>
        /// How long the attributed method will be run. Measured in seconds.
        /// </summary>
        public int Duration { get; set; }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            var type = methodInfo.DeclaringType;

            if (!MethodHasWarmupAttribute(methodInfo))
            {
                return new TestResult
                {
                    TestOutcome = TestOutcome.NotFound,
                    TestType = TestType.PreTest
                };
            }

            DetectDurationFromMethod(methodInfo);

            ExecuteWarmup(() =>
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var instance = Activator.CreateInstance(type);
                do
                {
                    methodInfo.Invoke(instance, null);
                }
                while (Duration > 0);
            });

            return new TestResult
            {
                TestOutcome = TestOutcome.Success,
                TestType = TestType.PreTest
            };
        }

        /// <summary>
        /// Utility method which checks if a method is attributed with a <see cref="WarmupAttribute"/> attribute.
        /// </summary>
        /// <param name="methodInfo">A method we want to investigate.</param>
        /// <returns>Returns <see cref="c:true"/> if the method is attributed with 
        /// a <see cref="WarmupAttribute"/> attribute.</returns>
        private static bool MethodHasWarmupAttribute(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(WarmupAttribute), true);

            return moyaAttributes.Length != 0;
        }

        /// <summary>
        /// Utility method which checks if a method has a <see cref="WarmupAttribute"/> surrounding it,
        /// and how long the duration of the warmup phase is set to.
        /// The value is stored in this object's <see cref="Duration"/> property.
        /// </summary>
        /// <param name="methodInfo">A method attributed with <see cref="WarmupAttribute"/>.</param>
        private void DetectDurationFromMethod(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(WarmupAttribute), true);
            
            WarmupAttribute warmupAttribute = moyaAttributes.FirstOrDefault(x => x is WarmupAttribute) as WarmupAttribute;

            // ReSharper disable once PossibleNullReferenceException
            Duration = warmupAttribute.Duration;
        }

        /// <summary>
        /// Executes an action. If the <see cref="Duration"/> is set to 0,
        /// the codeblock is executed once. Else, it will be executed for the
        /// defined <see cref="Duration"/>.
        /// </summary>
        /// <param name="codeBlock">An <see cref="Action"/> which will be executed.</param>
        private void ExecuteWarmup(Action codeBlock)
        {
            if (Duration == 0)
            {
                codeBlock();
            }
            else
            {
                ExecuteWithTimeLimit(codeBlock, Duration);
            }
        }

        /// <summary>
        /// Executes an <see cref="Action"/>, and stops execution after a specified amount of seconds.
        /// If the <see cref="Action"/> takes less than the specified duration, it will be run again.
        /// </summary>
        /// <param name="codeBlock">An <see cref="Action"/> which will be executed.</param>
        /// <param name="seconds">How long the specified <see cref="Action"/> should be run. Defined in seconds.</param>
        private static void ExecuteWithTimeLimit(Action codeBlock, int seconds)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int milliseconds = seconds * 1000;
            bool done = false;
            while (!done)
            {
                Task task = Task.Factory.StartNew(codeBlock);
                done = task.Wait(milliseconds);

                milliseconds -= (int)stopwatch.ElapsedMilliseconds;
                if (milliseconds <= 0)
                {
                    done = true;
                }
            }
        }
    }
}