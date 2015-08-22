namespace TestMoya.Runners
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runners;
    using Xunit;

    public class LongerThanTestRunnerTests
    {
        private readonly ILongerThanTestRunner longerThanTestRunner;
        private readonly TestClass testClass;

        public LongerThanTestRunnerTests()
        {
            longerThanTestRunner = new LongerThanTestRunner();
            testClass = new TestClass();
            TestClass.ResetState();
        }

        [Fact]
        public void ExecuteMethodWithLongerThanAttributeDoesNotRunMethod()
        {
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            longerThanTestRunner.Execute(method);

            Assert.True(TestClass.MethodWithLongerThanTenSecondsAttributeRun == 0);
        }

        [Fact]
        public void ExecuteMethodWithLongerThanAttributeWithSecondsDefinedInAttributeAddsSecondsToTestRunner()
        {
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsConstructorAttribute).Method;

            longerThanTestRunner.Execute(method);

            Assert.Equal(10, longerThanTestRunner.Seconds);
        }

        [Fact]
        public void ExecuteMethodWithNoLongerThanAttributeReturnsNotFound()
        {
            MethodInfo method = ((Action)testClass.MethodWithWarmupAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.NotFound);
        }

        [Fact]
        public void ExecuteMethodWithOnlyLongerThanAttributeReturnsFailure()
        {
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Failure);
        }

        [Fact]
        public void ExecuteAfterOtherTestWhichRanOnTimeShouldReturnSuccess()
        {
            longerThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 10, TestType = TestType.Test}
            };
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        [Fact]
        public void ExecuteAfterOtherTestWhichRanOnLessTimeShouldReturnFailure()
        {
            longerThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 8, TestType = TestType.Test}
            };
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Failure);
        }

        [Fact]
        public void ExecuteAfterOtherTestWhichRanOnLongerTimeShouldReturnSuccess()
        {
            longerThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 12, TestType = TestType.Test}
            };
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        [Fact]
        public void TestResultsWhichAreNotTestTypeTestShouldBeIgnored()
        {
            longerThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 8, TestType = TestType.Test},
                new TestResult{Duration = 8, TestType = TestType.PostTest},
                new TestResult{Duration = 8, TestType = TestType.PreTest},
                new TestResult{Duration = 8, TestType = TestType.PostTest},
                new TestResult{Duration = 8, TestType = TestType.PreTest},
            };
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Failure);
        }

        [Fact]
        public void ExecuteAfterMultipleTestsWhichHasRunForTooLongShouldReturnSuccess()
        {
            longerThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 2, TestType = TestType.Test},
                new TestResult{Duration = 3, TestType = TestType.Test},
                new TestResult{Duration = 4, TestType = TestType.Test},
                new TestResult{Duration = 5, TestType = TestType.Test},
            };
            MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

            var result = longerThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        class TestClass
        {
            public static int MethodWithLongerThanTenSecondsAttributeRun;

            public static void ResetState()
            {
                MethodWithLongerThanTenSecondsAttributeRun = 0;
            }

            [Warmup(1)]
            public void MethodWithWarmupAttribute()
            {
            }

            [LongerThan(10)]
            public void MethodWithLongerThanTenSecondsAttribute()
            {
                MethodWithLongerThanTenSecondsAttributeRun++;
            }

            [LongerThan(10)]
            public void MethodWithLongerThanTenSecondsConstructorAttribute()
            {
                MethodWithLongerThanTenSecondsAttributeRun++;
            }
        }
    }
}