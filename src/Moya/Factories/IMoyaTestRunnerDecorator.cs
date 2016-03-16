namespace Moya.Factories
{
    using Runners;
    using Runners.Decorators;

    /// <summary>
    /// A factory which decorates instances of <see cref="IMoyaTestRunner"/> with
    /// specific <see cref="IMoyaTestRunner"/> decorators.
    /// </summary>
    internal interface IMoyaTestRunnerDecorator
    {
        /// <summary>
        /// Decorates an instance of <see cref="IMoyaTestRunner"/> with the
        /// following <see cref="IMoyaTestRunner"/> decorators:
        ///  - <see cref="TimerDecorator"/> 
        ///  - <see cref="MethodNameDecorator"/> 
        /// </summary>
        /// <param name="testRunner">The <see cref="IMoyaTestRunner"/> to be decorated.</param>
        /// <returns>A decorated <see cref="IMoyaTestRunner"/>.</returns>
        IMoyaTestRunner DecorateTestRunner(IMoyaTestRunner testRunner);
    }
}