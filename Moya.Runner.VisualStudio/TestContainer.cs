namespace Moya.Runner.VisualStudio
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;

    public class TestContainer : ITestContainer
    {
        private readonly IDictionary<TestCase, MethodInfo> methodsWithMoyaAttributes;

        public TestContainer()
        {
            methodsWithMoyaAttributes = new Dictionary<TestCase, MethodInfo>();
        }

        public void AddTestCaseAndMethod(TestCase testCase, MethodInfo methodInfo)
        {
            methodsWithMoyaAttributes.Add(testCase, methodInfo);
        }

        IDictionary<TestCase, MethodInfo> ITestContainer.GetMethodsWithMoyaAttributes()
        {
            return methodsWithMoyaAttributes;
        }

        public MethodInfo GetMethodInfoFromTestCase(TestCase testCase)
        {
            return methodsWithMoyaAttributes[testCase];
        }
    }
}