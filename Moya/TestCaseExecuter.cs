namespace Moya
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Factories;
    using Models;
    using Runners;
    using Utility;

    public class TestCaseExecuter : ITestCaseExecuter
    {
        private readonly IMoyaTestRunnerFactory testRunnerFactory = MoyaTestRunnerFactory.DefaultInstance;
        private readonly ICollection<ITestResult> testResults = new List<ITestResult>();

        public ICollection<ITestResult> RunTest(TestCase testCase)
        {
            MethodInfo methodInfo = ConvertTestCaseToMethodInfo(testCase);

            if (methodInfo == null)
            {
                throw new MoyaException(
                    "Unable to find method from assembly.Assembly file path: {0}\nClass name: {1}\nMethod name: {2}"
                     .FormatWith(testCase.FilePath, testCase.ClassName, testCase.MethodName)
                );
            }

            // Pre test
            RunTest(methodInfo, typeof(MoyaConfigurationAttribute));
            RunTest(methodInfo, typeof(WarmupAttribute));
            // Test
            RunTest(methodInfo, typeof(StressAttribute));
            // Post test
            RunTest(methodInfo, typeof(LessThanAttribute));
            RunTest(methodInfo, typeof(LongerThanAttribute));
            return testResults;
        }

        private void RunTest(MethodInfo methodInfo, Type attributeType)
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(attributeType);
            if (MethodHasAttribute(methodInfo, attributeType))
            {
                testResults.Add(testRunner.Execute(methodInfo));
            }
        }

        private static MethodInfo ConvertTestCaseToMethodInfo(TestCase testCase)
        {
            var assemblyHelper = new AssemblyHelper(testCase.FilePath);
            return assemblyHelper.GetMethodFromAssembly(testCase.ClassName, testCase.MethodName);
        }

        private static bool MethodHasAttribute(MethodInfo methodInfo, Type attribute)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(attribute, true);

            return moyaAttributes.Length != 0;
        }
    }
}