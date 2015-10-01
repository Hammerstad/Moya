namespace Moya.Runners
{
    /// <summary>
    /// A test runner used to wrap other test runners in order to time
    /// their duration.
    /// </summary>
    public interface ITimerDecorator : IMoyaTestRunner
    {
        /// <summary>
        /// Another <see cref="IMoyaTestRunner"/> which will be measured by 
        /// this <see cref="ITimerDecorator"/>.
        /// </summary>
        IMoyaTestRunner DecoratedTestRunner { get; set; }
    }
}