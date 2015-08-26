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
        private ICollection<ITestResult> testResults;

        public ICollection<ITestResult> RunTest(TestCase testCase)
        {
            testResults = new List<ITestResult>();
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
            RunCustomPreTests(methodInfo);
            // Test
            RunTest(methodInfo, typeof(StressAttribute));
            RunCustomTests(methodInfo);
            // Post test
            RunTest(methodInfo, typeof(LessThanAttribute));
            RunTest(methodInfo, typeof(LongerThanAttribute));
            RunCustomPostTests(methodInfo);
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

        private void RunCustomPreTests(MethodInfo methodInfo)
        {
            var customTestRunners = testRunnerFactory.GetCustomPreTestRunners();
            RunCustomTestRunners(customTestRunners, methodInfo);
        }
        private void RunCustomTests(MethodInfo methodInfo)
        {
            var customTestRunners = testRunnerFactory.GetCustomTestRunners();
            RunCustomTestRunners(customTestRunners, methodInfo);
        }
        private void RunCustomPostTests(MethodInfo methodInfo)
        {
            var customTestRunners = testRunnerFactory.GetCustomPostTestRunners();
            RunCustomTestRunners(customTestRunners, methodInfo);
        }

        private void RunCustomTestRunners(IEnumerable<IMoyaTestRunner> testRunners, MethodInfo methodInfo)
        {
            foreach (var testRunner in testRunners)
            {
                var attributeType = testRunnerFactory.GetAttributeForTestRunner(testRunner.GetType());
                if (MethodHasAttribute(methodInfo, attributeType.GetType()))
                {
                    testResults.Add(testRunner.Execute(methodInfo));
                }
            }
        }
    }
}