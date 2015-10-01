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
        public class Seconds
        {
            private readonly ILongerThanTestRunner longerThanTestRunner;
            private readonly TestClass testClass;

            public Seconds()
            {
                longerThanTestRunner = new LongerThanTestRunner();
                testClass = new TestClass();
                TestClass.ResetState();
            }

            [Fact]
            public void MethodWithLongerThanAttributeWithSecondsDefinedAddsSecondsToTestRunner()
            {
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsConstructorAttribute).Method;

                longerThanTestRunner.Execute(method);

                Assert.Equal(10, longerThanTestRunner.Seconds);
            }
        }

        public class Execute
        {
            private readonly ILongerThanTestRunner longerThanTestRunner;
            private readonly TestClass testClass;

            public Execute()
            {
                longerThanTestRunner = new LongerThanTestRunner();
                testClass = new TestClass();
                TestClass.ResetState();
            }

            [Fact]
            public void MethodWithLongerThanAttributeDoesNotRunMethod()
            {
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

                longerThanTestRunner.Execute(method);

                Assert.True(TestClass.MethodWithLongerThanTenSecondsAttributeRun == 0);
            }

            [Fact]
            public void MethodWithNoLongerThanAttributeReturnsNotFound()
            {
                MethodInfo method = ((Action)testClass.MethodWithWarmupAttribute).Method;

                var result = longerThanTestRunner.Execute(method);

                Assert.Equal(result.TestOutcome, TestOutcome.NotFound);
            }

            [Fact]
            public void MethodWithOnlyLongerThanAttributeReturnsFailure()
            {
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

                var result = longerThanTestRunner.Execute(method);

                Assert.Equal(result.TestOutcome, TestOutcome.Failure);
            }

            [Fact]
            public void AfterOtherTestWhichRanOnTimeShouldReturnSuccess()
            {
                longerThanTestRunner.previousTestResults = new Collection<ITestResult> { new TestResult { Duration = 10, TestType = TestType.Test } };
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

                var result = longerThanTestRunner.Execute(method);

                Assert.Equal(result.TestOutcome, TestOutcome.Success);
            }

            [Fact]
            public void AfterOtherTestWhichRanOnLessTimeShouldReturnFailure()
            {
                longerThanTestRunner.previousTestResults = new Collection<ITestResult> { new TestResult { Duration = 8, TestType = TestType.Test } };
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

                var result = longerThanTestRunner.Execute(method);

                Assert.Equal(result.TestOutcome, TestOutcome.Failure);
            }

            [Fact]
            public void AfterOtherTestWhichRanOnLongerTimeShouldReturnSuccess()
            {
                longerThanTestRunner.previousTestResults = new Collection<ITestResult> { new TestResult { Duration = 12, TestType = TestType.Test } };
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

                var result = longerThanTestRunner.Execute(method);

                Assert.Equal(result.TestOutcome, TestOutcome.Success);
            }

            [Fact]
            public void TestResultsWhichAreNotTestTypeTestShouldBeIgnored()
            {
                longerThanTestRunner.previousTestResults = new Collection<ITestResult>
                {
                    new TestResult { Duration = 8, TestType = TestType.Test },
                    new TestResult { Duration = 8, TestType = TestType.PostTest },
                    new TestResult { Duration = 8, TestType = TestType.PreTest },
                    new TestResult { Duration = 8, TestType = TestType.PostTest },
                    new TestResult { Duration = 8, TestType = TestType.PreTest },
                };
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

                var result = longerThanTestRunner.Execute(method);

                Assert.Equal(result.TestOutcome, TestOutcome.Failure);
            }

            [Fact]
            public void AfterMultipleTestsWhichHasRunForTooLongShouldReturnSuccess()
            {
                longerThanTestRunner.previousTestResults = new Collection<ITestResult>
                {
                    new TestResult { Duration = 2, TestType = TestType.Test },
                    new TestResult { Duration = 3, TestType = TestType.Test },
                    new TestResult { Duration = 4, TestType = TestType.Test },
                    new TestResult { Duration = 5, TestType = TestType.Test },
                };
                MethodInfo method = ((Action)testClass.MethodWithLongerThanTenSecondsAttribute).Method;

                var result = longerThanTestRunner.Execute(method);

                Assert.Equal(result.TestOutcome, TestOutcome.Success);
            }
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