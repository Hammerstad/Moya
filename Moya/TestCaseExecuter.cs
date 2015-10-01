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

    /// <summary>
    /// Represents an instance used to run <see cref="TestCase"/>s.
    /// </summary>
    public class TestCaseExecuter : ITestCaseExecuter
    {
        private readonly IMoyaTestRunnerFactory testRunnerFactory = MoyaTestRunnerFactory.DefaultInstance;
        private ICollection<ITestResult> testResults;

        /// <summary>
        /// Runs a <see cref="TestCase"/> for a specific method.
        /// Returns a <see cref="ICollection{ITestResult}"/> containing one 
        /// <see cref="ITestResult"/> per <see cref="MoyaAttribute"/> surrounding
        /// the run method.
        /// </summary>
        /// <param name="testCase">A <see cref="TestCase"/> to be run.</param>
        /// <returns>A collection of <see cref="ITestResult"/> from the test run.</returns>
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

        /// <summary>
        /// Runs a specific test runner on a method. The test runner is deduced from the specified 
        /// argument <paramref name="attributeType"/> . The <see cref="Type"/> of <paramref name="attributeType"/> 
        /// must be a subclass of <see cref="MoyaAttribute"/>. It must also have a mapping to a test runner
        /// which either explicitly or implicitly implement <see cref="IMoyaTestRunner"/>.
        /// </summary>
        /// <param name="methodInfo">A method we want to test.</param>
        /// <param name="attributeType">A </param>
        private void RunTest(MethodInfo methodInfo, Type attributeType)
        {
            IMoyaTestRunner testRunner = testRunnerFactory.GetTestRunnerForAttribute(attributeType);
            if (MethodHasAttribute(methodInfo, attributeType))
            {
                testResults.Add(testRunner.Execute(methodInfo));
            }
        }

        /// <summary>
        /// Gets a <see cref="MethodInfo"/> instance from the values specified in a <see cref="TestCase"/>.
        /// </summary>
        /// <param name="testCase">The test case we want to convert.</param>
        /// <returns>The <see cref="MethodInfo"/> from the values specified in the test case.</returns>
        private static MethodInfo ConvertTestCaseToMethodInfo(TestCase testCase)
        {
            var assemblyHelper = new AssemblyHelper(testCase.FilePath);
            return assemblyHelper.GetMethodFromAssembly(testCase.ClassName, testCase.MethodName);
        }

        /// <summary>
        /// Utility method to check if a <see cref="MethodInfo"/> has a certain <see cref="Attribute"/>.
        /// </summary>
        /// <param name="methodInfo">The method we want to check.</param>
        /// <param name="attribute">The <see cref="Type"/> we want the argument <paramref name="methodInfo"/>
        /// to be attributed with.</param>
        /// <returns></returns>
        private static bool MethodHasAttribute(MethodInfo methodInfo, Type attribute)
        {
            object[] moyaAttributes = methodInfo.GetCustomAttributes(attribute, true);

            return moyaAttributes.Length != 0;
        }

        /// <summary>
        /// Runs all user made test runners implementing <see cref="ICustomPreTestRunner"/>.
        /// </summary>
        /// <param name="methodInfo">The method to be run.</param>
        private void RunCustomPreTests(MethodInfo methodInfo)
        {
            var customTestRunners = testRunnerFactory.GetCustomPreTestRunners();
            RunCustomTestRunners(customTestRunners, methodInfo);
        }

        /// <summary>
        /// Runs all user made test runners implementing <see cref="ICustomTestRunner"/>.
        /// </summary>
        /// <param name="methodInfo">The method to be run.</param>
        private void RunCustomTests(MethodInfo methodInfo)
        {
            var customTestRunners = testRunnerFactory.GetCustomTestRunners();
            RunCustomTestRunners(customTestRunners, methodInfo);
        }

        /// <summary>
        /// Runs all user made test runners implementing <see cref="ICustomPostTestRunner"/>.
        /// </summary>
        /// <param name="methodInfo">The method to be run.</param>
        private void RunCustomPostTests(MethodInfo methodInfo)
        {
            var customTestRunners = testRunnerFactory.GetCustomPostTestRunners();
            RunCustomTestRunners(customTestRunners, methodInfo);
        }

        /// <summary>
        /// Runs one or more user made test runners.
        /// </summary>
        /// <param name="testRunners">The test runner(s) to run.</param>
        /// <param name="methodInfo">The targeted <see cref="MethodInfo"/> which will be run by the test runners.</param>
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