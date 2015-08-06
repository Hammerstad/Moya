using Shouldly;

namespace TestMoya.Extensions
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Moya.Extensions;

    public class DictionaryExtensionsTests
    {
        public Dictionary<string, string> originalDictionary = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };
            
        [Fact]
        public void AddingSeveralUniqueElementsWithAddRangeWorks()
        {
            var otherDictionary = new Dictionary<string, string>
            {
                { "key3", "value3" },
                { "key4", "value4" }
            };

            otherDictionary.AddRange(originalDictionary);
            
            Assert.Equal(4, otherDictionary.Count);
            Assert.Equal("value1", otherDictionary["key1"]);
            Assert.Equal("value2", otherDictionary["key2"]);
            Assert.Equal("value3", otherDictionary["key3"]);
            Assert.Equal("value4", otherDictionary["key4"]);
        }

        [Fact]
        public void TryingToAddRangeWithNullShouldThrowArgumentException()
        {
            var exception = Record.Exception(() => originalDictionary.AddRange(null));

            Assert.Equal(typeof(ArgumentNullException), exception.GetType());
#if __MonoCS__
            exception.Message.ShouldStartWith("Argument cannot be null.");
#else
            exception.Message.ShouldStartWith("Value cannot be null.");
#endif
            exception.Message.ShouldEndWith("Parameter name: collection");
        }

        [Fact]
        public void AddingSeveralEqualElementsWithAddRangeIgnoresDuplicates()
        {
            var otherDictionary = new Dictionary<string, string>
            {
                { "key1", "value3" },
                { "key2", "value4" },
            };

            otherDictionary.AddRange(originalDictionary);

            Assert.Equal(2, otherDictionary.Count);
            Assert.Equal("value3", otherDictionary["key1"]);
            Assert.Equal("value4", otherDictionary["key2"]);
        }
    }
}