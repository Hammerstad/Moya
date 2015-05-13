namespace Moya.Runner
{
    using System;
    using System.Reflection;
    using Attributes;
    using Datamodels;
    using Runners;

    public class AttributeExecuter : IAttributeExecuter
    {
        public TestResult RunTest(MethodInfo testCase)
        {
            ITestRunner testRunner;
            TimerDecorator timedTestRunner = new TimerDecorator();
            Type typeOfMethod = testCase.DeclaringType;
            if (typeOfMethod == typeof(LoadTestAttribute))
            {
                testRunner = new LoadTestRunner();
                timedTestRunner.DecoratedTestRunner = testRunner;
                timedTestRunner.Execute(testCase, typeOfMethod);
            }

            //TODO:
            return new TestResult();
        }
    }
}