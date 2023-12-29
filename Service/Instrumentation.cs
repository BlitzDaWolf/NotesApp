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

        /*private readonly Meter meter;

        public Instrumentation()
        {
            string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
            this.ActivitySource = new ActivitySource(ActivitySourceName, version);
            this.meter = new Meter(MeterName, version);
            this.FreezingDaysCounter = this.meter.CreateCounter<long>("weather.days.freezing", description: "The number of days where the temperature is below freezing");
        }

        public ActivitySource ActivitySource { get; }

        public Counter<long> FreezingDaysCounter { get; }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.meter.Dispose();
        }*/
    }
}
