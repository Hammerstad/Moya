using System.Reflection;
using Moya.Models;
using Moya.Runner.Utility;

namespace Moya.Runner
{
    using Attributes;
    using Factories;
    using Runners;

    public class TestCaseExecuter : ITestCaseExecuter
    {
        private readonly ITestRunnerFactory testRunnerFactory;

        public TestCaseExecuter()
        {
            testRunnerFactory = new TestRunnerFactory();
        }

        public TestResult RunTest(TestCase testCase)
        {
            RunPreTestAttributes(testCase);
            RunTestAttributes(testCase);
            RunPostTestAttributes(testCase);
            return new TestResult();
        }

        private void RunPreTestAttributes(TestCase testCase)
        {
            
        }

        private void RunTestAttributes(TestCase testCase)
        {
            MethodInfo methodInfo = ConvertTestCaseToMethodInfo(testCase);

            ITestRunner loadTestRunner = testRunnerFactory.GetTestRunnerForAttribute(typeof(StressAttribute));
            loadTestRunner.Execute(methodInfo);
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