namespace Moya.Runners
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Models;

    public class LessThanTestRunner : ILessThanTestRunner
    {
        public int Seconds { get; private set; }

        public ICollection<ITestResult> previousTestResults { get; set; }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            if (!MethodHasLessThanAttribute(methodInfo))
            {
                return new TestResult
                {
                    TestOutcome = TestOutcome.NotFound,
                    TestType = TestType.PostTest
                };
            }

            DetectSecondsFromMethod(methodInfo);

            var executedWithinTime = ExecutedWithinTime();

            return new TestResult
            {
                TestOutcome = executedWithinTime ? TestOutcome.Success : TestOutcome.Failure,
                TestType = TestType.PostTest
            };
        }

        private bool ExecutedWithinTime()
        {
            if (previousTestResults == null)
            {
                return false;
            }

            var usedTime = previousTestResults.Where(x => x.TestType == TestType.Test).Sum(x => x.Duration);

            return usedTime <= Seconds;
        }

        private static bool MethodHasLessThanAttribute(MethodInfo methodInfo)
        {
            object[] lessThanAttributes = methodInfo.GetCustomAttributes(typeof(LessThanAttribute), true);

            return lessThanAttributes.Length != 0;
        }

        private void DetectSecondsFromMethod(MethodInfo methodInfo)
        {
            object[] lessThanAttributes = methodInfo.GetCustomAttributes(typeof(LessThanAttribute), true);

            LessThanAttribute lessThanAttribute = lessThanAttributes.FirstOrDefault(x => x.GetType() == typeof(LessThanAttribute)) as LessThanAttribute;

            // ReSharper disable once PossibleNullReferenceException
            Seconds = lessThanAttribute.Seconds;
        }
    }
}