namespace Moya.Runners.Decorators
{
    using System.Reflection;
    using Attributes;
    using Models;

    /// <summary>
    /// A test runner used to wrap other test runners in order to detect
    /// method name and namespace of run methods and append to the result.
    /// </summary>
    internal class MethodNameDecorator : IMethodNameDecorator
    {
        /// <summary>
        /// Another <see cref="IMoyaTestRunner"/> whose results will be appended
        /// method name and namespace by this <see cref="IMethodNameDecorator"/>.
        /// </summary>
        public IMoyaTestRunner DecoratedTestRunner { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNameDecorator"/> class.
        /// </summary>
        /// <param name="testRunner"> An instance of <see cref="IMoyaTestRunner"/> whose results will 
        /// be appended method name and namespace by this <see cref="IMethodNameDecorator"/></param>.
        public MethodNameDecorator(IMoyaTestRunner testRunner)
        {
            DecoratedTestRunner = testRunner;
        }

        /// <summary>
        /// Executes a method and adds the duration that method took to the returned <see cref="ITestResult"/>.
        /// </summary>
        /// <param name="methodInfo">A method attributed with a derivative of the 
        /// <see cref="MoyaAttribute"/> attribute.</param>
        /// <returns>A <see cref="ITestResult"/> object containing information about the test run.</returns>
        public ITestResult Execute(MethodInfo methodInfo)
        {
            var result = DecoratedTestRunner.Execute(methodInfo);

            ((TestResult)result).MethodName = methodInfo.Name;
            ((TestResult)result).Namespace = methodInfo.DeclaringType?.FullName + "." + result.MethodName;
            return result;
        }
    }
}