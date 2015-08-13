namespace Moya.Runners
{
    public interface IStressTestRunner : ITestRunner
    {
        int Users { get; }
        int Times { get; }
    }
}