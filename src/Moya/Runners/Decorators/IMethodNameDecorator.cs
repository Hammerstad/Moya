namespace Moya.Runners.Decorators
{
    /// <summary>
    /// A test runner used to wrap other test runners in order to detect
    /// method name and namespace of run methods and append to the result.
    /// </summary>
    internal interface IMethodNameDecorator : IMoyaTestRunner
    {

        /// <summary>
        /// Another <see cref="IMoyaTestRunner"/> whose results will be appended
        /// method name and namespace by this <see cref="IMethodNameDecorator"/>.
        /// </summary>
        IMoyaTestRunner DecoratedTestRunner { get; set; }
    }
}