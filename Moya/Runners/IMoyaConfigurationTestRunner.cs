namespace Moya.Runners
{
    using Attributes;

    /// <summary>
    /// Represents the test runner responsible for running methods marked with
    /// the <see cref="MoyaConfigurationAttribute"/> attribute.
    /// </summary>
    public interface IMoyaConfigurationTestRunner : IPreTestRunner
    {

    }
}