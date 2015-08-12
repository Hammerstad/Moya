namespace Moya.Runner.Runners
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using Models;

    public class WarmupTestRunner : ITestRunner
    {
        public int Duration { get; set; }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            var type = methodInfo.DeclaringType;

            if (!MethodHasWarmupAttribute(methodInfo))
            {
                return new TestResult
                {
                    TestOutcome = TestOutcome.NotFound
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
                TestOutcome = TestOutcome.Success
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
                ExecuteWithTimeLimit(TimeSpan.FromSeconds(Duration), codeBlock);
            }
        }

        private static void ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            Task task = Task.Factory.StartNew(codeBlock);
            task.Wait(timeSpan);
        }
    }
}