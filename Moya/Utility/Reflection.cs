namespace Moya.Utility
{
    using System.Reflection;
    using Attributes;

    public class Reflection
    {
        public static bool MethodInfoHasMoyaAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(typeof(MoyaAttribute), false).Length > 0;
        }
    }
}