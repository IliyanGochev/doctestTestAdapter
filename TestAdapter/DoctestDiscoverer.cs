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
        public List<TestCase> GetTests(IEnumerable<string> sources)
        {
            testCases = new List<TestCase>();
            foreach (var source in sources)
            {
                mMessageLogger.SendMessage(TestMessageLevel.Informational,$"Source: {source}{Environment.NewLine}");
                if (!File.Exists(source))
                {
                    mMessageLogger.SendMessage(TestMessageLevel.Error,$" File does not exist!{Environment.NewLine}");
                }
                else if (CheckSource(source))
                {

                }
            }

            Log = mMessageLogger.ToString();
            return testCases;
        }

        private bool CheckSource(string source)
        {
            return false;
        }
    }
}
