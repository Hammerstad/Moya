namespace Moya.Factories
{
    using System;
    using System.Collections.Generic;
    using Attributes;
    using Exceptions;
    using Extensions;
    using Runners;
    using Utility;

    public class TestRunnerFactory : ITestRunnerFactory
    {
        private readonly IDictionary<Type, Type> attributeTestRunnerMapping = new Dictionary<Type, Type>
        {
            { typeof(StressAttribute), typeof(StressTestRunner) },
            { typeof(WarmupAttribute), typeof(WarmupTestRunner) },
            { typeof(LongerThanAttribute), typeof(LongerThanTestRunner) },
            { typeof(LessThanAttribute), typeof(LessThanTestRunner) },
        };

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

        public void AddTestRunnerForAttribute(Type testRunner, Type attribute)
        {
            Guard.IsMoyaTestRunner(testRunner);
            Guard.IsMoyaAttribute(attribute);

            EnsureMappingDoesNotExist(testRunner, attribute);
            

            attributeTestRunnerMapping.Add(attribute, testRunner);
        }

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