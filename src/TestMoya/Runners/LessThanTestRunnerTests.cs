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
        public class Seconds
        {
            private readonly ILessThanTestRunner lessThanTestRunner;
            private readonly TestClass testClass;

            public Seconds()
            {
                lessThanTestRunner = new LessThanTestRunner();
                testClass = new TestClass();
                TestClass.ResetState();
            }

            [Fact]
            public void LessThanAttributeWithSecondsDefinedAddsSecondsToTestRunner()
            {
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsConstructorAttribute).Method;

                lessThanTestRunner.Execute(method);

                Assert.Equal(10, lessThanTestRunner.Seconds);
            }
        }

        public class Execute
        {
            private readonly ILessThanTestRunner lessThanTestRunner;
            private readonly TestClass testClass;

            public Execute()
            {
                lessThanTestRunner = new LessThanTestRunner();
                testClass = new TestClass();
                TestClass.ResetState();
            }

            [Fact]
            public void OnlyLessThanAttributeDoesNotRunMethod()
            {
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

                lessThanTestRunner.Execute(method);

                Assert.True(TestClass.MethodWithLessThanTenSecondsAttributeRun == 0);
            }

            [Fact]
            public void NoLessThanAttributeReturnsNotFound()
            {
                MethodInfo method = ((Action)testClass.MethodWithWarmupAttribute).Method;

                var result = lessThanTestRunner.Execute(method);

                Assert.Equal(result.Outcome, TestOutcome.NotFound);
            }

            [Fact]
            public void OnlyLessThanAttributeReturnsFailure()
            {
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

                var result = lessThanTestRunner.Execute(method);

                Assert.Equal(result.Outcome, TestOutcome.Failure);
            }

            [Fact]
            public void AfterOtherTestWhichRanOnTimeShouldReturnSuccess()
            {
                lessThanTestRunner.PreviousTestResults = new Collection<ITestResult>
                {
                    new TestResult{Duration = 10, TestType = TestType.Test}
                };
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

                var result = lessThanTestRunner.Execute(method);

                Assert.Equal(result.Outcome, TestOutcome.Success);
            }

            [Fact]
            public void AfterOtherTestWhichRanOnLessTimeShouldReturnSuccess()
            {
                lessThanTestRunner.PreviousTestResults = new Collection<ITestResult>
                {
                    new TestResult{Duration = 8, TestType = TestType.Test}
                };
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

                var result = lessThanTestRunner.Execute(method);

                Assert.Equal(result.Outcome, TestOutcome.Success);
            }

            [Fact]
            public void TestResultsWhichAreNotTestTypeTestShouldBeIgnored()
            {
                lessThanTestRunner.PreviousTestResults = new Collection<ITestResult>
                {
                    new TestResult{Duration = 8, TestType = TestType.Test},
                    new TestResult{Duration = 8, TestType = TestType.PostTest},
                    new TestResult{Duration = 8, TestType = TestType.PreTest},
                    new TestResult{Duration = 8, TestType = TestType.PostTest},
                    new TestResult{Duration = 8, TestType = TestType.PreTest},
                };
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

                var result = lessThanTestRunner.Execute(method);

                Assert.Equal(result.Outcome, TestOutcome.Success);
            }

            [Fact]
            public void AfterMultipleTestsWhichHasRunForTooLongShouldReturnFailure()
            {
                lessThanTestRunner.PreviousTestResults = new Collection<ITestResult>
                {
                    new TestResult{Duration = 2, TestType = TestType.Test},
                    new TestResult{Duration = 3, TestType = TestType.Test},
                    new TestResult{Duration = 4, TestType = TestType.Test},
                    new TestResult{Duration = 5, TestType = TestType.Test},
                };
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

                var result = lessThanTestRunner.Execute(method);

                Assert.Equal(result.Outcome, TestOutcome.Failure);
            }

            [Fact]
            public void AfterMultipleTestsWhichHasRunForLessThanMaxShouldReturnSuccess()
            {
                lessThanTestRunner.PreviousTestResults = new Collection<ITestResult>
                {
                    new TestResult{Duration = 2, TestType = TestType.Test},
                    new TestResult{Duration = 3, TestType = TestType.Test},
                    new TestResult{Duration = 4, TestType = TestType.Test},
                };
                MethodInfo method = ((Action)testClass.MethodWithLessThanTenSecondsAttribute).Method;

                var result = lessThanTestRunner.Execute(method);

                Assert.Equal(result.Outcome, TestOutcome.Success);
            }
        }

        class TestClass
        {
            public static int MethodWithLessThanTenSecondsAttributeRun;

            public static void ResetState()
            {
                MethodWithLessThanTenSecondsAttributeRun = 0;
            }

            [Warmup(1)]
            public void MethodWithWarmupAttribute()
            {
            }

            [LessThan(10)]
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