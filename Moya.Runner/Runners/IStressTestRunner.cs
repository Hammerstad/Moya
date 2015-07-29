namespace Moya.Runner.Runners
{
    public interface IStressTestRunner : ITestRunner
    {
        int Users { get; }
        int Times { get; }
    }
}