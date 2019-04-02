using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace TestAdapter
{
    [ExtensionUri("executor://doctestTestExecutor")]
    public class TestExecutor : ITestExecutor
    {
        public static readonly Uri ExecutorUri = new Uri("executor://doctestTestExecutor");


        private bool executionCanceled;
        private IFrameworkHandle frameworkHandle;
        private IRunContext runContext;
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            executionCanceled = true;
        }
    }
}
