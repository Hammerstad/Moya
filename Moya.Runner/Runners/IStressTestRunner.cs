namespace Moya.Runner.Runners
{
    public interface IStressTestRunner : ITestRunner
    {
        int Runners { get; set; }
        int Times { get; set; }
    }
}