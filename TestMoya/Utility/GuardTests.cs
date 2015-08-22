namespace TestMoya.Utility
{
    using System;
    using Moya.Attributes;
    using Moya.Exceptions;
    using Moya.Runners;
    using Moya.Utility;
    using Xunit;

    public class GuardTests
    {
        [Fact]
        public void IsMoyaAttributeWithStressAttributeThrowsNoException()
        {
            var exception = Record.Exception(() => Guard.IsMoyaAttribute(typeof(StressAttribute)));

            Assert.Null(exception);
        }

        [Fact]
        public void IsMoyaAttributeWithInvalidAttributeThrowsException()
        {
            const string ExpectedExceptionMessage = "System.Object is not a Moya Attribute.";

            var exception = Record.Exception(() => Guard.IsMoyaAttribute(typeof(Object)));

            Assert.NotNull(exception);
            Assert.Equal(typeof(MoyaException), exception.GetType());
            Assert.Equal(ExpectedExceptionMessage, exception.Message);
        }
        [Fact]
        public void IsMoyaTestRunnerWithStressTestRunnerThrowsNoException()
        {
            var exception = Record.Exception(() => Guard.IsMoyaTestRunner(typeof(StressTestRunner)));

            Assert.Null(exception);
        }

        [Fact]
        public void IsMoyaTestRunnerWithInvalidTestRunnerThrowsException()
        {
            const string ExpectedExceptionMessage = "System.Object is not a Moya Test Runner.";

            var exception = Record.Exception(() => Guard.IsMoyaTestRunner(typeof(Object)));

            Assert.NotNull(exception);
            Assert.Equal(typeof(MoyaException), exception.GetType());
            Assert.Equal(ExpectedExceptionMessage, exception.Message);
        }
    }
}