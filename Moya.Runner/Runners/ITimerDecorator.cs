namespace Moya.Runner.Runners
{
    public interface ITimerDecorator : IMoyaTestRunner
    {
        IMoyaTestRunner DecoratedTestRunner { get; set; }
    }
}