namespace Moya.Runner.Factories
{
    using System;
    using System.Collections.Generic;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Utility;
    using Runners;

    public class TestRunnerFactory : ITestRunnerFactory
    {
        private readonly IDictionary<Type, Type> attributeTestRunnerMapping = new Dictionary<Type, Type>
        {
            { typeof(StressAttribute), typeof(StressTestRunner) }
        };

        public ITestRunner GetTestRunnerForAttribute(Type type)
        {
            if (!Reflection.TypeIsMoyaAttribute(type))
            {
                throw new MoyaException("Unable to provide moya test runner for type {0}".FormatWith(type));
            }

            Type typeOfTestRunner = attributeTestRunnerMapping[type];
            ITestRunner instance = (ITestRunner)Activator.CreateInstance(typeOfTestRunner);
            return instance;
        }
    }
}