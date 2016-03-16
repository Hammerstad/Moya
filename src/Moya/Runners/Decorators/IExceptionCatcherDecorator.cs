namespace Moya.Runners.Decorators
{
    /// <summary>
    /// A test runner used to wrap other test runners in order to catch 
    /// potential exceptions being thrown by the tests. We don't want them to
    /// boil up and destory our testing environment.
    /// </summary>
    internal interface IExceptionCatcherDecorator : IMoyaTestRunner
    {

        /// <summary>
        /// Another <see cref="IMoyaTestRunner"/> which we will surpress exceptions from.
        /// </summary>
        IMoyaTestRunner DecoratedTestRunner { get; set; }
    }
}