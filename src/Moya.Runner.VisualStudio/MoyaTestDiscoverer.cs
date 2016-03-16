namespace Moya.Runner.VisualStudio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

    using Moya.Utility;

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri(Constants.ExecutorUriString)]
    public class MoyaTestDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            GetTests(sources, discoverySink);
        }

        internal static IList<TestCase> GetTests(IEnumerable<string> sources, ITestCaseDiscoverySink discoverySink = null, ITestContainer testContainer = null)
        {
            IList<TestCase> tests = new List<TestCase>();

            foreach (var assemblyFileName in sources)
            {
                Assembly assembly = Assembly.LoadFrom(assemblyFileName);

                var methodsWithMoyaAttributes = assembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(Reflection.MethodInfoHasMoyaAttribute)
                      .ToArray();

                foreach (MethodInfo methodWithMoyaAttributes in methodsWithMoyaAttributes)
                {
                    var testCase = new TestCase(methodWithMoyaAttributes.Name, Constants.ExecutorUri, assemblyFileName)
                    {
                        Id = Guid.NewGuid()
                    };

                    tests.Add(testCase);
                    if (discoverySink != null)
                    {
                        discoverySink.SendTestCase(testCase);
                    }
                    if (testContainer != null)
                    {
                        testContainer.AddTestCaseAndMethod(testCase, methodWithMoyaAttributes);
                    }
                }
            }

            return tests;
        }
    }
}