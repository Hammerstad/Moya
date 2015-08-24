namespace Moya.Runners
{
    using Factories;

    public interface IMoyaConfigurationTestRunner : IPreTestRunner
    {
        IMoyaTestRunnerFactory MoyaTestRunnerFactory { get; set; }
    }
}