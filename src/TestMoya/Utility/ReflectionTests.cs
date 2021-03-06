﻿namespace TestMoya.Utility
{
    using System;
    using Moya.Attributes;
    using Moya.Utility;
    using Xunit;

    public class ReflectionTests
    {
        public class TypeIsMoyaAttribute
        {
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
            public void MoyaConfigurationAttributeIsMoyaAttribute()
            {
                var isMoyaAttribute = Reflection.TypeIsMoyaAttribute(typeof(MoyaConfigurationAttribute));

                Assert.True(isMoyaAttribute);
            }

            [Fact]
            public void LessThanAttributeIsMoyaAttribute()
            {
                var isMoyaAttribute = Reflection.TypeIsMoyaAttribute(typeof(LessThanAttribute));

                Assert.True(isMoyaAttribute);
            }

            [Fact]
            public void LongerThanAttributeIsMoyaAttribute()
            {
                var isMoyaAttribute = Reflection.TypeIsMoyaAttribute(typeof(LongerThanAttribute));

                Assert.True(isMoyaAttribute);
            }

            [Fact]
            public void MoyaAttributeIsNotMoyaAttribute()
            {
                var isMoyaAttribute = Reflection.TypeIsMoyaAttribute(typeof(MoyaAttribute));

                Assert.False(isMoyaAttribute);
            }
        }

        public class MethodInfoHasMoyaAttribute
        {
            [Fact]
            public void TestClassHasMembersWithMoyaAttribute()
            {
                var MethodWithMoyaAttribute = ((Action)TestClass.MethodWithMoyaAttribute).Method;

                var hasMoyaAttributes = Reflection.MethodInfoHasMoyaAttribute(MethodWithMoyaAttribute);

                Assert.True(hasMoyaAttributes);
            }
        }

        static class TestClass
        {
            [Warmup(1)]
            public static void MethodWithMoyaAttribute()
            {
                
            }
        }
    }
}