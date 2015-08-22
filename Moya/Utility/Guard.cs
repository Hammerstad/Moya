namespace Moya.Utility
{
    using System;
    using Exceptions;
    using Extensions;
    using Runners;

    public class Guard
    {
        public static void IsMoyaAttribute(Type type)
        {
            if (!Reflection.TypeIsMoyaAttribute(type))
            {
                throw new MoyaException("{0} is not a Moya Attribute.".FormatWith(type));
            }
        }

        public static void IsMoyaTestRunner(Type type)
        {
            if (!typeof(ITestRunner).IsAssignableFrom(type))
            {
                throw new MoyaException("{0} is not a Moya Test Runner.".FormatWith(type));
            }
        } 
    }
}