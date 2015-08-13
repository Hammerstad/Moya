namespace Moya.Runners
{
    public interface IWarmupTestRunner : IPreTestRunner
    {
        int Duration { get; set; } 
    }
}