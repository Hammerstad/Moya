namespace Moya.Runner.Runners
{
    public interface ILoadTestRunner : ITestRunner
    {
        int Runners { get; set; }
        int Times { get; set; }
    }
}