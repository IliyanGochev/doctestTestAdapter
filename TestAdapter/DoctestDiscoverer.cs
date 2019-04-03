using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace TestAdapter
{
    class DoctestDiscoverer
    {
        private IMessageLogger mMessageLogger;
        private List<TestCase> testCases;

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
                    var tests = ExtractTestsFrom(executable);
                    testCases.AddRange( tests );
                }
            }

            return testCases;
        }

        private List<TestCase> ExtractTestsFrom(string executable)
        {
            var tempXmlFile = executable + ".doctest.xml";

            var process = new Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = "--reporters=xml --out=" + tempXmlFile;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            var output = process.StandardOutput.ReadToEndAsync();
            var erroroutput = process.StandardError.ReadToEndAsync();

            // Wait for 60 seconds for discovery
            // TODO: extract it to a settings file
            process.WaitForExit(60000);

            if (!process.HasExited)
            {
                process.Kill();
                mMessageLogger.SendMessage(TestMessageLevel.Warning, "Timeout reached!");
            }
            else
            {
                if (!string.IsNullOrEmpty(erroroutput.Result))
                {
                    mMessageLogger.SendMessage(TestMessageLevel.Error, $"  Error Occurred (exit code {process.ExitCode}):{Environment.NewLine}{erroroutput.Result}");
                    mMessageLogger.SendMessage(TestMessageLevel.Error, $"  output:{Environment.NewLine}{output.Result}");
                }
                if (File.Exists(tempXmlFile)) {
                    var xmlOutput = File.ReadAllText(tempXmlFile);
                    File.Delete(tempXmlFile);
                    var result = ProcessTestsOutput(xmlOutput, executable);
                    return result;
                }
            }
            return new List<TestCase>();
        }

        private List<TestCase> ProcessTestsOutput(string xmlOutput, string executable)
        {
            var results = new List<TestCase>();

            var xml = new XmlDocument();
            var executableName = Path.GetFileName(executable);
            try
            {
                xml.LoadXml(xmlOutput);
            }
            catch (XmlException e)
            {
                Debugger.Launch();
                mMessageLogger.SendMessage(TestMessageLevel.Error, "XML Error: " + e.Message);
            }
            try
            {
                var testSuits = xml.SelectNodes("//TestSuite");
                Uri executorUri = new Uri("executor://doctestTestExecutor");
                for (int i = 0; i < testSuits.Count; i++)
                {
                    try
                    {
                        string testSuiteName = executableName + " | ";
                        if (testSuits[i].Attributes.Count > 0 && testSuits[i].Attributes["name"].Value != null)
                        {
                            testSuiteName +=  testSuits[i].Attributes["name"].Value;
                        }
                        else
                        {
                            testSuiteName += "Anonymous Suite";
                        }
                        testSuiteName += " | ";

                        var testCases = testSuits.Item(i).SelectNodes("//TestCase");
                        foreach (XmlNode testCase in testCases)
                        {
                            try
                            {
                                var tc = new TestCase();
                                tc.Source = executable;
                                tc.DisplayName = testCase.Attributes["name"]?.Value;
                                tc.FullyQualifiedName = testSuiteName + tc.DisplayName;
                                tc.CodeFilePath = testCase.Attributes["filename"]?.Value;
                                tc.ExecutorUri = executorUri;

                                int line;
                                if (int.TryParse(testCase.Attributes["line"]?.Value, out line))
                                {
                                    tc.LineNumber = line;
                                }

                                results.Add(tc);
                            }
                            catch (XmlException e)
                            {
                                Debugger.Launch();
                                mMessageLogger.SendMessage(TestMessageLevel.Error, "XML Error: " + e.Message);
                            }
                        }
                    }
                    catch (XmlException e)
                    {
                        Debugger.Launch();
                        mMessageLogger.SendMessage(TestMessageLevel.Error, "XML Error: " + e.Message);
                    }
                }
            }
            catch (XmlException e)
            {
                Debugger.Launch();
                mMessageLogger.SendMessage(TestMessageLevel.Error, "XML Error: " + e.Message);
            }
            return results;
        }
        private bool CheckExecutableForTests(string executable)
        {
            return true;
        }
    }
}
