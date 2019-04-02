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
    class TestDiscoverer :ITestDiscoverer
    {
        private IDiscoveryContext mDiscoveryContext;
        private IMessageLogger    mMessageLogger;
        private ITestCaseDiscoverySink mDiscoverySink;

        public void DiscoverTests(IEnumerable<string> executables, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            mDiscoveryContext = discoveryContext;
            mDiscoverySink = discoverySink;
            mMessageLogger = logger;

            Log(TestMessageLevel.Informational, "Starting discovery...");
            //var settingsProvider = mDiscoveryContext.RunSettings.GetSettings("s") as SettingsProvider;

            DiscoverTests(executables);
        }

        private void DiscoverTests(IEnumerable<string> executables)
        {
            var discoverer = new DoctestDiscoverer(mMessageLogger);
            var testCases = discoverer.GetTests(executables);

            Log(TestMessageLevel.Informational, discoverer.Log);

            Log(TestMessageLevel.Informational, "Start adding discovered tests");
            foreach (var testCase in testCases)
            {
                mDiscoverySink.SendTestCase(testCase);
            }
            Log(TestMessageLevel.Informational, "Finished adding tests");
        }

        private void Log(TestMessageLevel level, string message)
        {
            mMessageLogger.SendMessage(level, message );
        }
    }
}
