namespace TestMoya.Extensions
{
    using Moya.Extensions;
    using Xunit;

    public class StringExtensionsTests
    {
        public class FormatWith
        {
            [Fact]
            public void WorksWithStrings()
            {
                const string Expected = "Hello world!";

                var actual = "{0} {1}".FormatWith("Hello", "world!");

                Assert.Equal(Expected, actual);
            }

            [Fact]
            public void WorksWithIntegers()
            {
                const string Expected = "1 2 3 4";

                var actual = "{0} {1} {2} {3}".FormatWith(1, 2, 3, 4);

                Assert.Equal(Expected, actual);
            }

            [Fact]
            public void WorksWithIntegersAndStrings()
            {
                const string Expected = "1 2 Hello World!";

                var actual = "{0} {1} {2} {3}".FormatWith(1, 2, "Hello", "World!");

                Assert.Equal(Expected, actual);
            }
        }
    }
}