using System;
using System.Reflection;
using Moya.Attributes;
using Moya.Exceptions;
using Moya.Runner.Runners;
using Xunit;

namespace TestMoya.Runner.Runners
{
    public class StressTestRunnerTests
    {
        private readonly StressTestRunner stressTestRunner;
        private readonly TestClass testClass = new TestClass();

        public StressTestRunnerTests()
        {
            stressTestRunner = new StressTestRunner();
            TestClass.ResetState();
        }

        [Fact]
        public void ExecuteRunsMethod()
        {
            MethodInfo method = ((Action)testClass.MethodWithTwoTimes).Method;

            stressTestRunner.Execute(method);

            Assert.True(TestClass.MethodWithTwoTimesRun > 0);
        }

        [Fact]
        public void ExecuteStressWithTwoUsersRunsMethodTwice()
        {
            MethodInfo method = ((Action)testClass.MethodWithTwoUsers).Method;

            stressTestRunner.Execute(method);

            Assert.Equal(2, TestClass.MethodWithTwoUsersRun);
        }

        [Fact]
        public void ExecuteStressWithTwoTimesRunsMethodTwice()
        {
            MethodInfo method = ((Action)testClass.MethodWithTwoTimes).Method;

            stressTestRunner.Execute(method);

            Assert.Equal(2, TestClass.MethodWithTwoTimesRun);
        }

        [Fact]
        public void ExecuteStressWithTwoTimesAndTwoUsersRunsMethodFourTimes()
        {
            MethodInfo method = ((Action)testClass.MethodWithTwoTimesAndTwoUsers).Method;

            stressTestRunner.Execute(method);

            Assert.Equal(4, TestClass.MethodWithTwoTimesAndTwoUsersRun);
        }

        [Fact]
        public void ExecuteMethodWithNoAttributeShouldThrowMoyaAttributeNotFoundException()
        {
            MethodInfo method = ((Action)TestClass.ResetState).Method;

            var exception = Record.Exception(() => stressTestRunner.Execute(method));

            Assert.Equal(typeof(MoyaAttributeNotFoundException), exception.GetType());
            Assert.Equal("Unable to find Moya.Attributes.StressAttribute in Void ResetState()", exception.Message);
        }

        [Fact]
        public void ExecuteMethodWithWarmupAttributeShouldThrowMoyaAttributeNotFoundException()
        {
            MethodInfo method = ((Action)TestClass.MethodWithWarmupAttribute).Method;

            var exception = Record.Exception(() => stressTestRunner.Execute(method));

            Assert.Equal(typeof(MoyaAttributeNotFoundException), exception.GetType());
            Assert.Equal("Unable to find Moya.Attributes.StressAttribute in Void MethodWithWarmupAttribute()", exception.Message);
        }

        public class TestClass
        {
            public static int MethodWithTwoUsersRun;
            public static int MethodWithTwoTimesRun;
            public static int MethodWithTwoTimesAndTwoUsersRun;
            private static readonly object MyLock = new object();

            public static void ResetState()
            {
                MethodWithTwoUsersRun = 0;
                MethodWithTwoTimesRun = 0;
                MethodWithTwoTimesAndTwoUsersRun = 0;
            }

            [Warmup]
            public static void MethodWithWarmupAttribute()
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