namespace Moya.Factories
{
    using System;
    using System.Collections.Generic;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Runners;
    using Utility;

    /// <summary>
    /// A factory containing mappings between children of <see cref="MoyaAttribute"/> and
    /// implementations of <see cref="IMoyaTestRunner"/>.
    /// </summary>
    public class MoyaTestRunnerFactory : IMoyaTestRunnerFactory
    {
        private readonly IDictionary<Type, Type> attributeTestRunnerMapping = new Dictionary<Type, Type>
        {
            { typeof(StressAttribute), typeof(StressTestRunner) },
            { typeof(WarmupAttribute), typeof(WarmupTestRunner) },
            { typeof(LongerThanAttribute), typeof(LongerThanTestRunner) },
            { typeof(LessThanAttribute), typeof(LessThanTestRunner) },
        };

        /// <summary>
        /// Gets a <see cref="IMoyaTestRunner"/> implementation for a subclass of <see cref="MoyaAttribute"/>.
        /// Throws a <see cref="MoyaException"/> if <paramref name="type"/> is not a subclass of <see cref="MoyaAttribute"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> which is a subclass of <see cref="MoyaAttribute"/>.</param>
        /// <returns>An implementation of <see cref="IMoyaTestRunner"/> for <paramref name="type"/>.</returns>
        public IMoyaTestRunner GetTestRunnerForAttribute(Type type)
        {
            if (!Reflection.TypeIsMoyaAttribute(type))
            {
                throw new MoyaException("Unable to provide moya test runner for type {0}".FormatWith(type));
            }

            Type typeOfTestRunner = attributeTestRunnerMapping[type];
            IMoyaTestRunner instance = (IMoyaTestRunner)Activator.CreateInstance(typeOfTestRunner);
            IMoyaTestRunner timerDecoratedInstance = new TimerDecorator(instance);
            return timerDecoratedInstance;
        }

        /// <summary>
        /// Adds a mapping between a custom <see cref="MoyaAttribute"/> and a custom
        /// <see cref="IMoyaTestRunner"/>. Throws an exception if the parameters
        /// are of incorrect type.
        /// </summary>
        /// <param name="testRunner">A <see cref="IMoyaTestRunner"/> to be mapped.</param>
        /// <param name="attribute">A <see cref="MoyaAttribute"/> to be mapped.</param>
        public void AddTestRunnerForAttribute(Type testRunner, Type attribute)
        {
            Guard.IsMoyaTestRunner(testRunner);
            Guard.IsMoyaAttribute(attribute);

            EnsureMappingDoesNotExist(testRunner, attribute);

            attributeTestRunnerMapping.Add(attribute, testRunner);
        }

        /// <summary>
        /// Ensures a mapping between <paramref name="testRunner"/> and <paramref name="attribute"/>
        /// exists. If it does not, a <see cref="MoyaException"/> is thrown.
        /// </summary>
        /// <param name="testRunner">A <see cref="IMoyaTestRunner"/> mapped to a <see cref="MoyaAttribute"/></param>
        /// <param name="attribute">A <see cref="MoyaAttribute"/> mapped to a <see cref="IMoyaTestRunner"/></param>
        private void EnsureMappingDoesNotExist(Type testRunner, Type attribute)
        {
            Type existingTestRunner;
            if (attributeTestRunnerMapping.TryGetValue(attribute, out existingTestRunner) && existingTestRunner == testRunner)
            {
                throw new MoyaException("There already exists an entry for {0} - {1}.".FormatWith(testRunner, attribute));
            }
        }
    }
}