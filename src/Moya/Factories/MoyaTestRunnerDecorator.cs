namespace Moya.Factories
{
    using Runners;
    using Runners.Decorators;

    /// <summary>
    /// A factory which decorates instances of <see cref="IMoyaTestRunner"/> with
    /// specific <see cref="IMoyaTestRunner"/> decorators.
    /// </summary>
    internal class MoyaTestRunnerDecorator : IMoyaTestRunnerDecorator
    {
        /// <summary>
        /// Decorates an instance of <see cref="IMoyaTestRunner"/> with the
        /// following <see cref="IMoyaTestRunner"/> decorators:
        ///  - <see cref="TimerDecorator"/> 
        ///  - <see cref="MethodNameDecorator"/> 
        ///  - <see cref="ExceptionCatcherDecorator"/> 
        /// </summary>
        /// <param name="testRunner">The <see cref="IMoyaTestRunner"/> to be decorated.</param>
        /// <returns>A decorated <see cref="IMoyaTestRunner"/>.</returns>
        public IMoyaTestRunner DecorateTestRunner(IMoyaTestRunner testRunner)
        {
            return new TimerDecorator(new MethodNameDecorator(new ExceptionCatcherDecorator(testRunner)));
        }
    }
}