namespace Moya.Runner.Runners
{
    public interface IWarmupTestRunner : IPreTestRunner
    {
        int Duration { get; set; } 
    }
}