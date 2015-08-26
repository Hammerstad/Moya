namespace Moya.Dummy.Test.Project
{
    using Attributes;
    using Factories;
    using Moya.Attributes;
    using TestRunners;

    public class Configuration
    {
        [MoyaConfiguration]
        public void Configure(IMoyaTestRunnerFactory factory)
        {
            factory.AddTestRunnerForAttribute(typeof(CustomPreTestRunner), typeof(CustomPreAttribute));
            factory.AddTestRunnerForAttribute(typeof(CustomTestRunner), typeof(CustomAttribute));
            factory.AddTestRunnerForAttribute(typeof(CustomPostTestRunner), typeof(CustomPostAttribute));
        } 
    }
}