namespace Moya.Runners
{
    public interface ILoadTestRunner : ITestRunner
    {
        int Runners { get; }
        int Times { get; }
    }
}