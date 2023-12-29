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
        public async Task<string> TestingTrace()
        {
            using var activity = Instrumentation.GetActivitySource().StartActivity("Testing out the traces");
            activity!.AddTag("my-value", "test");
            await Task.Delay(400);

            using var activity2 = Instrumentation.GetActivitySource().StartActivity("Testing out the traces");
            await Task.Delay(600);
            activity2!.Dispose();

            await Task.Delay(400);
            return "Testing with traces";
        }
    }
}
