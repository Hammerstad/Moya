using System.Diagnostics;

namespace Moya.Runner.Runners
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Models;

    public class WarmupTestRunner : ITestRunner
    {
        public int Duration { get; set; }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            var type = methodInfo.DeclaringType;

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

        private void DetectDurationFromMethod(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(MoyaAttribute), true);

            if (moyaAttributes.Length == 0)
            {
                throw new MoyaAttributeNotFoundException("Unable to find {0} in {1}".FormatWith(typeof(WarmupAttribute), methodInfo));
            }

            WarmupAttribute warmupAttribute = moyaAttributes.FirstOrDefault(x => x.GetType() == typeof(WarmupAttribute)) as WarmupAttribute;

            if (warmupAttribute == null)
            {
                throw new MoyaAttributeNotFoundException("Unable to find {0} in {1}".FormatWith(typeof(WarmupAttribute), methodInfo));
            }

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
                ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(Duration), codeBlock);
            }
        }

        private static void ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            Task task = Task.Factory.StartNew(codeBlock);
            task.Wait(timeSpan);
        }
    }
}