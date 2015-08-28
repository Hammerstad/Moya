namespace Moya.Runners
{
    internal interface IStressTestRunner : ITestRunner
    {
        int Users { get; }
        int Times { get; }
    }
}