namespace Moya.Utility
{
    using System;
    using System.Reflection;
    using Attributes;

    internal class Reflection
    {
        public static bool MethodInfoHasMoyaAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(typeof(MoyaAttribute), false).Length > 0;
        }

        public static bool TypeIsMoyaAttribute(Type type)
        {
            return type.IsSubclassOf(typeof(MoyaAttribute));
        }
    }
}