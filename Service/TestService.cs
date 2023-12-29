using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TestService
    {
        readonly ActivitySource source;

        public TestService(Instrumentation instrumentation)
        {
            source = instrumentation.ActivitySource;
        }

        public async Task<string> TestingTrace()
        {
            using var activity = source.StartActivity("Testing out the traces");
            activity!.AddTag("my-value", "test");
            await Task.Delay(400);

            using var activity2 = source.StartActivity("Testing out the traces");
            await Task.Delay(600);
            //activity2!.Dispose();
            await Task.Delay(400);
            return "Testing with traces";
        }
    }
}
