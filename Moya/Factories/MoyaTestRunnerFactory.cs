namespace Moya.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Runners;
    using Utility;

    /// <summary>
    /// A factory containing mappings between children of <see cref="MoyaAttribute"/> and
    /// implementations of <see cref="IMoyaTestRunner"/>.
    /// </summary>
    internal class MoyaTestRunnerFactory : IMoyaTestRunnerFactory
    {
        /// <summary>
        /// The default MoyaTestRunnerFactory. Simplifies registring of custom test runners, as
        /// well as fetching them.
        /// </summary>
        private static readonly Lazy<IMoyaTestRunnerFactory> defaultInstance;

        /// <summary>
        /// The default MoyaTestRunnerFactory. Simplifies registring of custom test runners, as
        /// well as fetching them.
        /// </summary>
        internal static IMoyaTestRunnerFactory DefaultInstance { get { return defaultInstance.Value; } }

        static MoyaTestRunnerFactory()
        {
            defaultInstance = new Lazy<IMoyaTestRunnerFactory>(() => new MoyaTestRunnerFactory());
        }

        /// <summary>
        /// A mapping between attributes and test runner for that attribute. There is a one-to-one relationship
        /// between them. The key of the dictionary is the attribute, the value is the test runner.
        /// </summary>
        private readonly IDictionary<Type, Type> attributeTestRunnerMapping = new Dictionary<Type, Type>
        {
            { typeof(StressAttribute), typeof(StressTestRunner) },
            { typeof(WarmupAttribute), typeof(WarmupTestRunner) },
            { typeof(LongerThanAttribute), typeof(LongerThanTestRunner) },
            { typeof(LessThanAttribute), typeof(LessThanTestRunner) },
            { typeof(MoyaConfigurationAttribute), typeof(MoyaConfigurationTestRunner) },
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
        /// Gets an <see cref="IEnumerable{T}"/> containing <see cref="ICustomPreTestRunner"/> which are custom made test runners 
        /// added by the user. These runners run in the pre test phase, after the original moya pre test runners.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with custom pre test runners which are made by the user.</returns>
        public IEnumerable<ICustomPreTestRunner> GetCustomPreTestRunners()
        {
            return attributeTestRunnerMapping.Values
                .Where(x => x.GetInterfaces()
                    .Contains(typeof(ICustomPreTestRunner)))
                .Select(x => (ICustomPreTestRunner)Activator.CreateInstance(x));
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing <see cref="ICustomTestRunner"/> which are custom made test runners 
        /// added by the user. These runners run in the test phase, after the original moya test runners.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with custom test runners which are made by the user.</returns>
        public IEnumerable<ICustomTestRunner> GetCustomTestRunners()
        {
            return attributeTestRunnerMapping.Values
                .Where(x => x.GetInterfaces()
                    .Contains(typeof(ICustomTestRunner)))
                .Select(x => (ICustomTestRunner)Activator.CreateInstance(x));
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing <see cref="ICustomPostTestRunner"/> which are custom made test runners 
        /// added by the user. These runners run in the post test phase, after the original moya post test runners.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with custom post test runners which are made by the user.</returns>
        public IEnumerable<ICustomPostTestRunner> GetCustomPostTestRunners()
        {
            return attributeTestRunnerMapping.Values
                .Where(x => x.GetInterfaces()
                    .Contains(typeof(ICustomPostTestRunner)))
                .Select(x => (ICustomPostTestRunner)Activator.CreateInstance(x));
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