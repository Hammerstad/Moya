namespace Moya.Runner.VisualStudio
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

    using Utility;

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri(Constants.ExecutorUriString)]
    public class MoyaTestDiscoverer : ITestDiscoverer
    {
        private readonly IContainer container;

        private MoyaTestDiscoverer()
        {
            container = Container.DefaultInstance;
        }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            GetTests(sources, discoverySink);
        }

        internal IList<TestCase> GetTests(IEnumerable<string> sources, ITestCaseDiscoverySink discoverySink)
        {
            ITestContainer testContainer = container.RegisterAndResolve<ITestContainer, TestContainer>(new TestContainer());
            IList<TestCase> tests = container.RegisterAndResolve<IList<TestCase>, List<TestCase>>(new List<TestCase>());

            foreach (var assemblyFileName in sources)
            {
                Assembly assembly = Assembly.LoadFrom(assemblyFileName);

                var methodsWithMoyaAttributes = assembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(Reflection.TypeHasMoyaAttribute)
                      .ToArray();

                foreach (MethodInfo methodWithMoyaAttributes in methodsWithMoyaAttributes)
                {
                    var testCase = new TestCase(methodWithMoyaAttributes.Name, Constants.ExecutorUri, assemblyFileName);
                    tests.Add(testCase);
                    testContainer.AddTestCaseAndMethod(testCase, methodWithMoyaAttributes);
                    if (discoverySink != null)
                    {
                        discoverySink.SendTestCase(testCase);
                    }
                }
            }

            return tests;
        }
    }
}