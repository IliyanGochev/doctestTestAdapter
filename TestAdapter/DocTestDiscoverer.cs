using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TestAdapter
{
    class DoctestDiscoverer
    {
        private StringBuilder log;
        private List<TestCase> testCases;

        public string Log { get; private set; }

        public DoctestDiscoverer()
        {
            log = new StringBuilder();
        }
        public List<TestCase> GetTests(IEnumerable<string> sources)
        {
            testCases = new List<TestCase>();
            log.Append(Environment.NewLine);
            foreach (var source in sources)
            {
                log.Append($"Source: {source}{Environment.NewLine}");
                if (!File.Exists(source))
                {
                    log.Append($" File does not exist!{Environment.NewLine}");
                }
                else if (CheckSource(source))
                {

                }
            }

            Log = log.ToString();
            return testCases;
        }

        private bool CheckSource(string source)
        {
            return false;
        }
    }
}
