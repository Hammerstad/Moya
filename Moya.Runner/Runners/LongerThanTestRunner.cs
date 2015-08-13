namespace Moya.Runner.Runners
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Models;

    public class LongerThanTestRunner : ILongerThanTestRunner
    {
        public int Seconds { get; private set; }

        public ICollection<ITestResult> previousTestResults { get; set; }

        public ITestResult Execute(MethodInfo methodInfo)
        {
            if (!MethodHasLongerThanAttribute(methodInfo))
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

        private static bool MethodHasLongerThanAttribute(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(LongerThanAttribute), true);

            return moyaAttributes.Length != 0;
        }

        private void DetectSecondsFromMethod(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(LongerThanAttribute), true);

            LongerThanAttribute longerThanAttribute = moyaAttributes.FirstOrDefault(x => x.GetType() == typeof(LongerThanAttribute)) as LongerThanAttribute;

            // ReSharper disable once PossibleNullReferenceException
            Seconds = longerThanAttribute.Seconds;
        }
    }
}