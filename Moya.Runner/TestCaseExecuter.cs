namespace Moya.Runner
{
    using System.Collections.Generic;
    using System.Reflection;
    using Attributes;
    using Factories;
    using Models;
    using Runners;
    using Utility;

    public class TestCaseExecuter : ITestCaseExecuter
    {
        private readonly ITestRunnerFactory testRunnerFactory= new TestRunnerFactory();
        private readonly ICollection<ITestResult> testResults = new List<ITestResult>();

        public ICollection<ITestResult> RunTest(TestCase testCase)
        {
            RunPreTestAttributes(testCase);
            RunTestAttributes(testCase);
            RunPostTestAttributes(testCase);
            return testResults;
        }

        private void RunPreTestAttributes(TestCase testCase)
        {
            
        }

        private void RunTestAttributes(TestCase testCase)
        {
            MethodInfo methodInfo = ConvertTestCaseToMethodInfo(testCase);

            ITestRunner loadTestRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));
            testResults.Add(loadTestRunner.Execute(methodInfo));
        }

        private void RunPostTestAttributes(TestCase testCase)
        {
        }

        private MethodInfo ConvertTestCaseToMethodInfo(TestCase testCase)
        {
            var assemblyHelper = new AssemblyHelper(testCase.FilePath);
            return assemblyHelper.GetMethodFromAssembly(testCase.ClassName, testCase.MethodName);
        }
    }
}