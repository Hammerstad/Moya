namespace Moya.Runner.VisualStudio
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

    [ExtensionUri(Constants.ExecutorUriString)]
    public class MoyaTestExecutor : ITestExecutor
    {
        private bool _cancelled;
        private readonly IAttributeExecuter _attributeExecuter;
        private readonly IContainer container;
        private readonly ITestContainer testContainer;
        
        public MoyaTestExecutor()
        {
            _attributeExecuter = new AttributeExecuter();
            container = Container.DefaultInstance;
            testContainer = container.Resolve<ITestContainer>();
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            _cancelled = false;

            foreach (var testCase in tests)
            {
                if (_cancelled)
                {
                    break;
                }

                MethodInfo methodInfo = testContainer.GetMethodInfoFromTestCase(testCase);
                var moyaTestResult = _attributeExecuter.RunTest(methodInfo);
                TestResult testResult = ConvertToTestResult(testCase, moyaTestResult);
                frameworkHandle.RecordResult(testResult);
            }
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            IList<TestCase> tests = container.Resolve<IList<TestCase>>();

            if (tests == null)
            {
                return;
            }

            RunTests(tests, runContext, frameworkHandle);
        }

        public void Cancel()
        {
            _cancelled = true;
        }

        private static TestResult ConvertToTestResult(TestCase testResult, Datamodels.TestResult moyaTestResult)
        {
            return new TestResult(testResult)
            {
                ErrorMessage = moyaTestResult.ErrorMessage,
                Outcome = ConvertToOutcome(moyaTestResult.TestOutcome)
            };
        }

        private static TestOutcome ConvertToOutcome(Datamodels.TestOutcome testOutcome)
        {
            switch (testOutcome)
            {
                case Datamodels.TestOutcome.Failure:
                    return TestOutcome.Failed;
                case Datamodels.TestOutcome.Ignored:
                    return TestOutcome.Skipped;
                case Datamodels.TestOutcome.Success:
                    return TestOutcome.Passed;
                case Datamodels.TestOutcome.NotFound:
                    return TestOutcome.NotFound;
                default:
                    return TestOutcome.None;
            }
        }
    }
}