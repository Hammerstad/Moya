//using System.Diagnostics;

//namespace Moya.Runner.VisualStudio
//{
//    using System.Collections.Generic;
//    using System.Reflection;
//    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
//    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

//    [ExtensionUri(Constants.ExecutorUriString)]
//    public class MoyaTestExecutor : ITestExecutor
//    {
//        private bool cancelled;
//        private readonly ITestCaseExecuter attributeExecuter;
//        private readonly ITestContainer testContainer;
        
//        public MoyaTestExecutor()
//        {
//            attributeExecuter = new TestCaseExecuter();
//            testContainer = new TestContainer();
//        }

//        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
//        {
//            Debugger.Launch();
//            cancelled = false;

//            foreach (var testCase in tests)
//            {
//                if (cancelled)
//                {
//                    break;
//                }

//                MethodInfo methodInfo = GetMethodInfoFromTestCase(testCase);
//                var moyaTestResult = attributeExecuter.RunTest(methodInfo);
//                TestResult testResult = ConvertToTestResult(testCase, moyaTestResult);
//                frameworkHandle.RecordResult(testResult);
//            }
//        }

//        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
//        {
//            IList<TestCase> tests = MoyaTestDiscoverer.GetTests(sources, null, testContainer);

//            if (tests == null)
//            {
//                return;
//            }

//            RunTests(tests, runContext, frameworkHandle);
//        }

//        public void Cancel()
//        {
//            cancelled = true;
//        }

//        private static TestResult ConvertToTestResult(TestCase testResult, Models.TestResult moyaTestResult)
//        {
//            return new TestResult(testResult)
//            {
//                ErrorMessage = moyaTestResult.ErrorMessage,
//                Outcome = ConvertToOutcome(moyaTestResult.TestOutcome)
//            };
//        }

//        private static TestOutcome ConvertToOutcome(Models.TestOutcome testOutcome)
//        {
//            switch (testOutcome)
//            {
//                case Models.TestOutcome.Failure:
//                    return TestOutcome.Failed;
//                case Models.TestOutcome.Ignored:
//                    return TestOutcome.Skipped;
//                case Models.TestOutcome.Success:
//                    return TestOutcome.Passed;
//                case Models.TestOutcome.NotFound:
//                    return TestOutcome.NotFound;
//                default:
//                    return TestOutcome.None;
//            }
//        }

//        private MethodInfo GetMethodInfoFromTestCase(TestCase testCase)
//        {
//            return testContainer.GetMethodInfoFromTestCase(testCase);
//        }
//    }
//}