namespace Moya.Factories
{
    using System.Collections.Generic;
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
        /// Throws a <see cref="MoyaException"/> if <paramref name="attribute"/> is not a subclass of <see cref="MoyaAttribute"/>.
        /// </summary>
        /// <param name="attribute">A <see cref="Type"/> which is a subclass of <see cref="MoyaAttribute"/>.</param>
        /// <returns>An implementation of <see cref="IMoyaTestRunner"/> for <paramref name="attribute"/>.</returns>
        IMoyaTestRunner GetTestRunnerForAttribute(Type attribute);

        /// <summary>
        /// Gets a <see cref="MoyaAttribute"/> which can be run by the supplied <see cref="IMoyaTestRunner"/>.
        /// Throws a <see cref="MoyaException"/> if <paramref name="testRunner"/> is not an <see cref="IMoyaTestRunner"/>.
        /// </summary>
        /// <param name="testRunner">A <see cref="Type"/> which is an <see cref="IMoyaTestRunner"/>.</param>
        /// <returns>An attribute which can be run by <paramref name="testRunner"/>.</returns>
        MoyaAttribute GetAttributeForTestRunner(Type testRunner);

        /// <summary>
        /// Adds a mapping between a custom <see cref="MoyaAttribute"/> and a custom
        /// <see cref="IMoyaTestRunner"/>. Throws an exception if the parameters
        /// are of incorrect type.
        /// </summary>
        /// <param name="testRunner">A <see cref="IMoyaTestRunner"/> to be mapped.</param>
        /// <param name="attribute">A <see cref="MoyaAttribute"/> to be mapped.</param>
        void AddTestRunnerForAttribute(Type testRunner, Type attribute);

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing <see cref="ICustomPreTestRunner"/> which are custom made test runners 
        /// added by the user. These runners run in the pre test phase, after the original moya pre test runners.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with custom pre test runners which are made by the user.</returns>
        IEnumerable<ICustomPreTestRunner> GetCustomPreTestRunners();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing <see cref="ICustomTestRunner"/> which are custom made test runners 
        /// added by the user. These runners run in the test phase, after the original moya test runners.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with custom test runners which are made by the user.</returns>
        IEnumerable<ICustomTestRunner> GetCustomTestRunners();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing <see cref="ICustomPostTestRunner"/> which are custom made test runners 
        /// added by the user. These runners run in the post test phase, after the original moya post test runners.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with custom post test runners which are made by the user.</returns>
        IEnumerable<ICustomPostTestRunner> GetCustomPostTestRunners();
    }
}