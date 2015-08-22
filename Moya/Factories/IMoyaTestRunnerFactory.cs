namespace Moya.Factories
{
    using Attributes;
    using Exceptions;
    using Runners;
    using System;

    /// <summary>
    /// A factory containing mappings between subclasses of <see cref="MoyaAttribute"/> and
    /// implementations of <see cref="IMoyaTestRunner"/>.
    /// </summary>
    public interface IMoyaTestRunnerFactory
    {
        /// <summary>
        /// Gets a <see cref="IMoyaTestRunner"/> implementation for a subclass of <see cref="MoyaAttribute"/>.
        /// Throws a <see cref="MoyaException"/> if <paramref name="type"/> is not a subclass of <see cref="MoyaAttribute"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> which is a subclass of <see cref="MoyaAttribute"/>.</param>
        /// <returns>An implementation of <see cref="IMoyaTestRunner"/> for <paramref name="type"/>.</returns>
        IMoyaTestRunner GetTestRunnerForAttribute(Type type);

        /// <summary>
        /// Adds a mapping between a custom <see cref="MoyaAttribute"/> and a custom
        /// <see cref="IMoyaTestRunner"/>. Throws an exception if the parameters
        /// are of incorrect type.
        /// </summary>
        /// <param name="testRunner">A <see cref="IMoyaTestRunner"/> to be mapped.</param>
        /// <param name="attribute">A <see cref="MoyaAttribute"/> to be mapped.</param>
        void AddTestRunnerForAttribute(Type testRunner, Type attribute);
    }
}