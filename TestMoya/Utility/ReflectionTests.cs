﻿namespace TestMoya.Utility
{
    using System;
    using Moya.Attributes;
    using Moya.Utility;
    using Xunit;

    public class ReflectionTests
    {
        [Fact]
        public void ResultAttributeIsMoyaAttribute()
        {
            var isMoyaAttribute = Reflection.TypeIsMoyaAttribute(typeof(ResultAttribute));

            Assert.True(isMoyaAttribute);
        }

        [Fact]
        public void StressAttributeIsMoyaAttribute()
        {
            var isMoyaAttribute = Reflection.TypeIsMoyaAttribute(typeof(StressAttribute));

            Assert.True(isMoyaAttribute);
        }

        [Fact]
        public void WarmupAttributeIsMoyaAttribute()
        {
            var isMoyaAttribute = Reflection.TypeIsMoyaAttribute(typeof(WarmupAttribute));

            Assert.True(isMoyaAttribute);
        }

        [Fact]
        public void TestClassHasMembersWithMoyaAttribute()
        {
            var MethodWithMoyaAttribute = ((Action)TestClass.MethodWithMoyaAttribute).Method;

            var hasMoyaAttributes = Reflection.MethodInfoHasMoyaAttribute(MethodWithMoyaAttribute);

            Assert.True(hasMoyaAttributes);
        }

        static class TestClass
        {
            [Warmup]
            public static void MethodWithMoyaAttribute()
            {
                
            }
        }
    }
}