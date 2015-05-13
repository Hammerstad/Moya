using System;
using System.Reflection;
using Moya.Attributes;

namespace Moya.Utility
{
    public class Reflection
    {
        public static bool TypeHasMoyaAttribute(MethodInfo type)
        {
            return MethodInfoHasAttribute(type, typeof(LoadTestAttribute))
                    || MethodInfoHasAttribute(type, typeof(DurationShouldBeAttribute));
        }

        public static bool MethodInfoHasAttribute(MethodInfo info, Type type)
        {
            return info.GetCustomAttributes(type, false).Length > 0;
        } 
    }
}