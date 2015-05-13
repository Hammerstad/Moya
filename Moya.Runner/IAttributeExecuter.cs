namespace Moya.Runner
{
    using Datamodels;
    using System.Reflection;

    public interface IAttributeExecuter
    {
        TestResult RunTest(MethodInfo testCase);
    }
}