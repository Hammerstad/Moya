namespace Moya.Runners
{
    /// <summary>
    /// A test runner used to wrap other test runners in order to time
    /// their duration.
    /// </summary>
    public interface ITimerDecorator : IMoyaTestRunner
    {
        IMoyaTestRunner DecoratedTestRunner { get; set; }
    }
}