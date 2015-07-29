using Moya.Extensions;

namespace Moya.Runner.Factories
{
    using System;
    using System.Collections.Generic;
    using Attributes;
    using Exceptions;
    using Runners;

    public class TestRunnerFactory : ITestRunnerFactory
    {
        private IDictionary<Type, Type> attributeTestRunnerMapping;

        public TestRunnerFactory()
        {
            MapAttributesToTestRunners();
        }

        private void MapAttributesToTestRunners()
        {
            attributeTestRunnerMapping = new Dictionary<Type, Type>
            {
                { typeof(StressAttribute), typeof(LoadTestRunner) }
            };
        }

        public ITestRunner GetTestRunnerForAttribute(Type type)
        {
            if (AttributeIsNotMoyaAttribute(type))
            {
                throw new MoyaException("Unable to provide moya test runner for type {0}".FormatWith(type));
            }

            Type typeOfTestRunner = attributeTestRunnerMapping[type];
            ITestRunner instance = (ITestRunner)Activator.CreateInstance(typeOfTestRunner);
            return instance;
        }

        private static bool AttributeIsNotMoyaAttribute(Type type)
        {
            return !type.IsAssignableFrom(typeof(MoyaAttribute));
        }
    }
}