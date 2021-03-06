﻿namespace TestMoya.Runners
{
    using System;
    using System.Reflection;
    using Moya.Attributes;
    using Moya.Models;
    using Moya.Runners;
    using Xunit;

    public class StressTestRunnerTests
    {
        public class Execute
        {
            private readonly StressTestRunner stressTestRunner;
            private readonly TestClass testClass = new TestClass();

            public Execute()
            {
                stressTestRunner = new StressTestRunner();
                testClass.ResetState();
            }

            [Fact]
            public void RunsMethod()
            {
                MethodInfo method = ((Action)testClass.MethodWithTwoTimes).Method;

                stressTestRunner.Execute(method);

                Assert.True(TestClass.MethodWithTwoTimesRun > 0);
            }

            [Fact]
            public void WithTwoUsersRunsMethodTwice()
            {
                MethodInfo method = ((Action)testClass.MethodWithTwoUsers).Method;

                stressTestRunner.Execute(method);

                Assert.Equal(2, TestClass.MethodWithTwoUsersRun);
            }

            [Fact]
            public void WithTwoTimesRunsMethodTwice()
            {
                MethodInfo method = ((Action)testClass.MethodWithTwoTimes).Method;

                stressTestRunner.Execute(method);

                Assert.Equal(2, TestClass.MethodWithTwoTimesRun);
            }

            [Fact]
            public void WithTwoTimesAndTwoUsersRunsMethodFourTimes()
            {
                MethodInfo method = ((Action)testClass.MethodWithTwoTimesAndTwoUsers).Method;

                stressTestRunner.Execute(method);

                Assert.Equal(4, TestClass.MethodWithTwoTimesAndTwoUsersRun);
            }

            [Fact]
            public void WithNoAttributeShouldThrowMoyaAttributeNotFoundException()
            {
                MethodInfo method = ((Action)testClass.ResetState).Method;

                var result = stressTestRunner.Execute(method);

                Assert.Equal(TestOutcome.NotFound, result.Outcome);
            }

            [Fact]
            public void WarmupAttributeShouldThrowMoyaAttributeNotFoundException()
            {
                MethodInfo method = ((Action)testClass.MethodWithWarmupAttribute).Method;

                var result = stressTestRunner.Execute(method);

                Assert.Equal(TestOutcome.NotFound, result.Outcome);
            }
        }

        class TestClass
        {
            public static int MethodWithTwoUsersRun;
            public static int MethodWithTwoTimesRun;
            public static int MethodWithTwoTimesAndTwoUsersRun;

            private static readonly object MyLock = new object();

            public void ResetState()
            {
                MethodWithTwoUsersRun = 0;
                MethodWithTwoTimesRun = 0;
                MethodWithTwoTimesAndTwoUsersRun = 0;
            }

            [Warmup(1)]
            public void MethodWithWarmupAttribute()
            {
            }

            [Stress(Users = 2)]
            public void MethodWithTwoUsers()
            {
                lock (MyLock)
                {
                    MethodWithTwoUsersRun++;
                }
            }

            [Stress(Times = 2)]
            public void MethodWithTwoTimes()
            {
                MethodWithTwoTimesRun++;
            }

            [Stress(Times = 2, Users = 2)]
            public void MethodWithTwoTimesAndTwoUsers()
            {
                lock (MyLock)
                {
                    MethodWithTwoTimesAndTwoUsersRun++;
                }
            }
        }
    }
}