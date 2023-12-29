using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Service
{
    public static class Instrumentation
    {
        public const string ActivitySourceName = "Examples.AspNetCore";
        internal const string MeterName = "Examples.AspNetCore";

        public static ActivitySource GetActivitySource()
        {
            string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
            return new ActivitySource(ActivitySourceName, version);
        }

        public static ActivitySource GetActivitySource<T>()
        {
            string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
            return new ActivitySource(typeof(T).FullName!, version);
        }
    }
}
