namespace Moya.Runners
{
    using System.Diagnostics;
    using System.Reflection;
    using Attributes;
    using Models;

    /// <summary>
    /// A test runner used to wrap other test runners in order to time
    /// their duration.
    /// </summary>
    public class TimerDecorator : ITimerDecorator
    {
        /// <summary>
        /// Another <see cref="IMoyaTestRunner"/> which will be measured by 
        /// this <see cref="TimerDecorator"/>.
        /// </summary>
        public IMoyaTestRunner DecoratedTestRunner { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerDecorator"/> class.
        /// </summary>
        /// <param name="testRunner">An instance of a <see cref="IMoyaTestRunner"/>
        /// which will be measured.</param>
        public TimerDecorator(IMoyaTestRunner testRunner)
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = DecoratedTestRunner.Execute(methodInfo);
            stopwatch.Stop();

            result.Duration = stopwatch.ElapsedMilliseconds;
            return result;
        }
    }
}