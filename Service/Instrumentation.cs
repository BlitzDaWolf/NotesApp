using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Service
{
    public static class Instrumentation
    {
        public static ActivitySource GetActivitySource(string name)
        {
            string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
            return new ActivitySource(name, version);
        }

        public static ActivitySource GetActivitySource<T>() => GetActivitySource(typeof(T).FullName!);
        public static ActivitySource GetActivitySource(Type t) => GetActivitySource(t.FullName!);
    }
}
