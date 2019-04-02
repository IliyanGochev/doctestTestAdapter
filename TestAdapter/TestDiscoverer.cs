using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace TestAdapter
{
    [DefaultExecutorUri("executor://doctestTestExecutor")]
    [FileExtension(".exe")]
    class DoctestTestDiscoverer :ITestDiscoverer
    {
        private IDiscoveryContext m_discoveryContext;
        private IMessageLogger    m_messageLogger;
        private ITestCaseDiscoverySink m_discoverySink;

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            m_discoveryContext = discoveryContext;
            m_discoverySink = discoverySink;
            m_messageLogger = logger;

            Log(TestMessageLevel.Informational, "Starting discovery...");
            //var settingsProvider = m_discoveryContext.RunSettings.GetSettings("s") as SettingsProvider;

            DiscoverTests(sources);
        }

        private void DiscoverTests(IEnumerable<string> sources)
        {
            var discoverer = new DoctestDiscoverer();
            var testCases = discoverer.GetTests(sources);

            Log(TestMessageLevel.Informational, discoverer.Log);

            Log(TestMessageLevel.Informational, "Start adding discovered tests");
            foreach (var testCase in testCases)
            {
                m_discoverySink.SendTestCase(testCase);
            }
            Log(TestMessageLevel.Informational, "Finished adding tests");
        }

        private void Log(TestMessageLevel level, string message)
        {
            m_messageLogger.SendMessage(level, message );
        }
    }
}
