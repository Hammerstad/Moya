namespace Moya.Runners
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Models;

    /// <summary>
    /// The test runner responsible for running tests marked with
    /// the <see cref="LongerThanAttribute"/> attribute.
    /// </summary>
    internal class LongerThanTestRunner : ILongerThanTestRunner
    {
        /// <summary>
        /// How many seconds the tests are supposed to take longer than.
        /// </summary>
        public int Seconds { get; private set; }

        /// <summary>
        /// Contains a list of previous test results. Used to determine if the tests
        /// in fact took longer than a given time.
        /// </summary>
        public ICollection<ITestResult> previousTestResults { get; set; }

        /// <summary>
        /// Checks that a given method takes longer than a designated period of time to execute.
        /// </summary>
        /// <param name="methodInfo">A method attributed with a <see cref="LongerThanAttribute"/> attribute.</param>
        /// <returns>A <see cref="ITestResult"/> object containing information 
        /// about the test run.</returns>
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

        /// <summary>
        /// Utility method which checks whether or not the executed method executed
        /// within time. The result is calculated by adding the durations of each <see cref="ITestResult"/>
        /// in <see cref="previousTestResults"/>, and comparing it to this objects <see cref="Seconds"/>
        /// property. The sum is executed within time if it takes longer than or equal to the value
        /// of <see cref="Seconds"/>.
        /// </summary>
        /// <returns>Returns <see cref="c:true"/> if the method executes within time.</returns>
        private bool ExecutedWithinTime()
        {
            if (previousTestResults == null)
            {
                return false;
            }

            var usedTime = previousTestResults.Where(x => x.TestType == TestType.Test).Sum(x => x.Duration);

            return usedTime >= Seconds;
        }

        /// <summary>
        /// Utility method which checks if a method is attributed with a <see cref="LongerThanAttribute"/> attribute.
        /// </summary>
        /// <param name="methodInfo">A method we want to investigate.</param>
        /// <returns>Returns <see cref="c:true"/> if the method is attributed with 
        /// a <see cref="LongerThanAttribute"/> attribute.</returns>
        private static bool MethodHasLongerThanAttribute(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(LongerThanAttribute), true);

            return moyaAttributes.Length != 0;
        }

        /// <summary>
        /// Utility method which checks how many seconds the method can minimally take. This value
        /// is extracted from the <see cref="LongerThanAttribute"/> attribute surrounding the method.
        /// The value is stored in this object's <see cref="Seconds"/> property.
        /// </summary>
        /// <param name="methodInfo">A method attributed with a <see cref="LongerThanAttribute"/> attribute.</param>
        private void DetectSecondsFromMethod(MethodInfo methodInfo)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(typeof(LongerThanAttribute), true);

            LongerThanAttribute longerThanAttribute = moyaAttributes.FirstOrDefault(x => x is LongerThanAttribute) as LongerThanAttribute;

            // ReSharper disable once PossibleNullReferenceException
            Seconds = longerThanAttribute.Seconds;
        }
    }
}