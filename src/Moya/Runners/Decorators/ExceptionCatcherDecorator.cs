namespace Moya.Runners.Decorators
{
    using System;
    using System.Reflection;
    using Attributes;
    using Models;

    internal class ExceptionCatcherDecorator : IExceptionCatcherDecorator
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCatcherDecorator"/> class.
        /// </summary>
        /// <param name="testRunner">An instance of a <see cref="IMoyaTestRunner"/>
        /// which we will surpress exceptions from.</param>
        public ExceptionCatcherDecorator(IMoyaTestRunner testRunner)
        {
            DecoratedTestRunner = testRunner;
        }

        /// <summary>
        /// Another <see cref="IMoyaTestRunner"/> which we will surpress exceptions from.
        /// </summary>
        public IMoyaTestRunner DecoratedTestRunner { get; set; }

        /// <summary>
        /// Executes a method and surpresses potential exceptions. If an exception is thrown, the test will fail.
        /// </summary>
        /// <param name="methodInfo">A method attributed with a derivative of the 
        /// <see cref="MoyaAttribute"/> attribute.</param>
        /// <returns>A <see cref="ITestResult"/> object containing information about the test run.</returns>
        public ITestResult Execute(MethodInfo methodInfo)
        {
            try
            {
                return DecoratedTestRunner.Execute(methodInfo);
            }
            catch (TargetInvocationException e)
            {
                return new TestResult
                {
                    TestOutcome = TestOutcome.Failure,
                    Exception = e
                };
            }
            catch (Exception e)
            {
                return new TestResult
                {
                    TestOutcome = TestOutcome.Failure,
                    Exception = e
                };
            }
        }
    }
}