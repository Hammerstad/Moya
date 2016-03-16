namespace Moya.Runner.VisualStudio
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;

    public interface ITestContainer
    {
        void AddTestCaseAndMethod(TestCase testCase, MethodInfo methodInfo);
        IDictionary<TestCase, MethodInfo> GetMethodsWithMoyaAttributes();
        MethodInfo GetMethodInfoFromTestCase(TestCase testCase);
    }
}