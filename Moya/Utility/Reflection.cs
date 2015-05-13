namespace Moya.Utility
{
    using System;
    using System.Reflection;
    using Attributes;

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