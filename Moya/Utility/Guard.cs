namespace Moya.Utility
{
    using System;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Runners;

    /// <summary>
    /// Utility class used to check that arguments fulfill certain conditions.
    /// </summary>
    internal class Guard
    {
        /// <summary>
        /// Makes sure a <see cref="Type"/> is a <see cref="MoyaAttribute"/>.
        /// Throws a <see cref="MoyaException"/> if it is not.
        /// </summary>
        /// <exception cref="MoyaException">Throws if <paramref name="type"/>  is not a <see cref="MoyaAttribute"/>.</exception>
        /// <param name="type">A <see cref="Type"/> which should be a <see cref="MoyaAttribute"/>.</param>
        public static void IsMoyaAttribute(Type type)
        {
            IsMoyaAttribute(type, "{0} is not a Moya Attribute.".FormatWith(type));
        }


        /// <summary>
        /// Makes sure a <see cref="Type"/> is a <see cref="MoyaAttribute"/>.
        /// Throws a <see cref="MoyaException"/> if it is not.
        /// </summary>
        /// <exception cref="MoyaException">Throws if <paramref name="type"/>  is not a <see cref="MoyaAttribute"/>.</exception>
        /// <param name="type">A <see cref="Type"/> which should be a <see cref="MoyaAttribute"/>.</param>
        /// <param name="exceptionMessage">The message describing the error.</param>
        public static void IsMoyaAttribute(Type type, string exceptionMessage)
        {
            if (!Reflection.TypeIsMoyaAttribute(type))
            {
                throw new MoyaException(exceptionMessage);
            }
        }

        /// <summary>
        /// Makes sure a <see cref="Type"/> is assignable from <see cref="IMoyaTestRunner"/>.
        /// Throws a <see cref="MoyaException"/> if it is not.
        /// </summary>
        /// <exception cref="MoyaException">Throws if <paramref name="type"/> is not assignable 
        /// from <see cref="IMoyaTestRunner"/>.</exception>
        /// <param name="type">A <see cref="Type"/> which should be a <see cref="IMoyaTestRunner"/>.</param>
        public static void IsMoyaTestRunner(Type type)
        {
            if (!typeof(IMoyaTestRunner).IsAssignableFrom(type))
            {
                throw new MoyaException("{0} is not a Moya Test Runner.".FormatWith(type));
            }
        } 
    }
}