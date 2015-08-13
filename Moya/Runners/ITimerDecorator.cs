namespace Moya.Runners
{
    public interface ITimerDecorator : IMoyaTestRunner
    {
        IMoyaTestRunner DecoratedTestRunner { get; set; }
    }
}