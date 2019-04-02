using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace TestAdapter
{
    class DoctestDiscoverer
    {
        private IMessageLogger mMessageLogger;
        private List<TestCase> testCases;

        public string Log { get; private set; }

        public DoctestDiscoverer(IMessageLogger logger)
        {
            mMessageLogger = logger;
        }
        public List<TestCase> GetTests(IEnumerable<string> executables)
        {
            testCases = new List<TestCase>();
            foreach (var executable in executables)
            {
                mMessageLogger.SendMessage(TestMessageLevel.Informational,$"Source: {executable}{Environment.NewLine}");
                if (!File.Exists(executable))
                {
                    mMessageLogger.SendMessage(TestMessageLevel.Error,$" File does not exist!{Environment.NewLine}");
                }
                else if (CheckExecutableForTests(executable))
                {

                }
            }

            Log = mMessageLogger.ToString();
            return testCases;
        }

        private bool CheckExecutableForTests(string executable)
        {
            return false;
        }
    }
}
