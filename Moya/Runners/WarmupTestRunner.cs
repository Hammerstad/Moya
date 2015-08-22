namespace Moya.Runners
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using Models;

    internal class WarmupTestRunner : IWarmupTestRunner
    {
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

        private static bool MethodHasWarmupAttribute(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(WarmupAttribute), true);

            return moyaAttributes.Length != 0;
        }

        private void DetectDurationFromMethod(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(WarmupAttribute), true);
            
            WarmupAttribute warmupAttribute = moyaAttributes.FirstOrDefault(x => x.GetType() == typeof(WarmupAttribute)) as WarmupAttribute;

            // ReSharper disable once PossibleNullReferenceException
            Duration = warmupAttribute.Duration;
        }

        private void ExecuteWarmup(Action codeBlock)
        {
            if (Duration == 0)
            {
                codeBlock();
            }
            else
            {
                ExecuteWithTimeLimit(Duration * 1000, codeBlock);
            }
        }

        private static void ExecuteWithTimeLimit(int milliseconds, Action codeBlock)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
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