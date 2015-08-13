namespace Moya.Runner
{
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
        private readonly ITestRunnerFactory testRunnerFactory = new TestRunnerFactory();
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

            RunPreTestAttributes(methodInfo);
            RunTestAttributes(methodInfo);
            RunPostTestAttributes(methodInfo);
            return testResults;
        }

        private void RunPreTestAttributes(MethodInfo methodInfo)
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(WarmupAttribute));
            testResults.Add(testRunner.Execute(methodInfo));
        }

        private void RunTestAttributes(MethodInfo methodInfo)
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));
            testResults.Add(testRunner.Execute(methodInfo));
        }

        private void RunPostTestAttributes(MethodInfo methodInfo)
        {
        }

        private static MethodInfo ConvertTestCaseToMethodInfo(TestCase testCase)
        {
            var assemblyHelper = new AssemblyHelper(testCase.FilePath);
            return assemblyHelper.GetMethodFromAssembly(testCase.ClassName, testCase.MethodName);
        }
    }
}