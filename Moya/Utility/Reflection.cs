namespace Moya.Utility
{
    using System;
    using System.Reflection;
    using Attributes;

    /// <summary>
    /// Utility class which provides reflection support.
    /// </summary>
    internal class Reflection
    {
        /// <summary>
        /// Checks if a <see cref="MethodInfo"/> is attributed with a <see cref="MoyaAttribute"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> we want to check.</param>
        /// <returns><see cref="c:true"/> if the <see cref="MethodInfo"/> is attributed with a 
        /// <see cref="MoyaAttribute"/>, <see cref="c:false"/> otherwise.</returns>
        public static bool MethodInfoHasMoyaAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(typeof(MoyaAttribute), false).Length > 0;
        }

        /// <summary>
        /// Checks if a <see cref="Type"/> is a <see cref="MoyaAttribute"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> we want to check.</param>
        /// <returns><see cref="c:true"/> if the <see cref="Type"/> is a 
        /// <see cref="MoyaAttribute"/>, <see cref="c:false"/> otherwise.</returns>
        public static bool TypeIsMoyaAttribute(Type type)
        {
            return type.IsSubclassOf(typeof(MoyaAttribute));
        }
    }
}