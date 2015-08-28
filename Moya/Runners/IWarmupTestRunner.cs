namespace Moya.Runners
{
    internal interface IWarmupTestRunner : IPreTestRunner
    {
        int Duration { get; set; } 
    }
}