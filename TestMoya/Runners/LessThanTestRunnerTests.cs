namespace TestMoya.Runners
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runners;
    using Xunit;

    public class LessThanTestRunnerTests
    {
        private readonly ILessThanTestRunner lessThanTestRunner;
        private readonly TestClass testClass;

        public LessThanTestRunnerTests()
        {
            lessThanTestRunner = new LessThanTestRunner();
            testClass = new TestClass();
            TestClass.ResetState();
        }

        [Fact]
        public void ExecuteMethodWithOnlyLessThanAttributeDoesNotRunMethod()
        {
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

            lessThanTestRunner.Execute(method);

            Assert.True(TestClass.MethodWithLessThanTenSecondsAttributeRun == 0);
        }

        [Fact]
        public void ExecuteMethodWithLessThanAttributeWithSecondsDefinedInAttributeAddsSecondsToTestRunner()
        {
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsConstructorAttribute).Method;

            lessThanTestRunner.Execute(method);

            Assert.Equal(10, lessThanTestRunner.Seconds);
        }

        [Fact]
        public void ExecuteMethodWithNoLessThanAttributeReturnsNotFound()
        {
            MethodInfo method = ((Action)testClass.MethodWithEmptyWarmupAttribute).Method;

            var result = lessThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.NotFound);
        }

        [Fact]
        public void ExecuteMethodWithOnlyLessThanAttributeReturnsFailure()
        {
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

            var result = lessThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Failure);
        }

        [Fact]
        public void ExecuteAfterOtherTestWhichRanOnTimeShouldReturnSuccess()
        {
            lessThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 10, TestType = TestType.Test}
            };
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

            var result = lessThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        [Fact]
        public void ExecuteAfterOtherTestWhichRanOnLessTimeShouldReturnSuccess()
        {
            lessThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 8, TestType = TestType.Test}
            };
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

            var result = lessThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        [Fact]
        public void TestResultsWhichAreNotTestTypeTestShouldBeIgnored()
        {
            lessThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 8, TestType = TestType.Test},
                new TestResult{Duration = 8, TestType = TestType.PostTest},
                new TestResult{Duration = 8, TestType = TestType.PreTest},
                new TestResult{Duration = 8, TestType = TestType.PostTest},
                new TestResult{Duration = 8, TestType = TestType.PreTest},
            };
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

            var result = lessThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        [Fact]
        public void ExecuteAfterMultipleTestsWhichHasRunForTooLongShouldReturnFailure()
        {
            lessThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 2, TestType = TestType.Test},
                new TestResult{Duration = 3, TestType = TestType.Test},
                new TestResult{Duration = 4, TestType = TestType.Test},
                new TestResult{Duration = 5, TestType = TestType.Test},
            };
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

            var result = lessThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Failure);
        }

        [Fact]
        public void ExecuteAfterMultipleTestsWhichHasRunForLessThanMaxShouldReturnSuccess()
        {
            lessThanTestRunner.previousTestResults = new Collection<ITestResult>
            {
                new TestResult{Duration = 2, TestType = TestType.Test},
                new TestResult{Duration = 3, TestType = TestType.Test},
                new TestResult{Duration = 4, TestType = TestType.Test},
            };
            MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

            var result = lessThanTestRunner.Execute(method);

            Assert.Equal(result.TestOutcome, TestOutcome.Success);
        }

        class TestClass
        {
            public static int MethodWithLessThanTenSecondsAttributeRun;

            public static void ResetState()
            {
                MethodWithLessThanTenSecondsAttributeRun = 0;
            }

            [Warmup]
            public void MethodWithEmptyWarmupAttribute()
            {
            }

            [LessThan(Seconds = 10)]
            public void MethodWithLessThanTenSecondsAttribute()
            {
                MethodWithLessThanTenSecondsAttributeRun++;
            }

            [LessThan(10)]
            public void MethodWithLessThanTenSecondsConstructorAttribute()
            {
                MethodWithLessThanTenSecondsAttributeRun++;
            }
        }
    }
}