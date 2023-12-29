using Database.Context;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotesApp.API;
using OpenTelemetry;
using OpenTelemetry.Exporter.InfluxDB;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Service;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

const string serviceName = "roll-dice";

builder.Services.AddSingleton<Instrumentation>();
builder.Services.AddSingleton<TestService>();

builder.Logging.AddOpenTelemetry(options =>
{
    options
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName))
    .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddSource(Instrumentation.ActivitySourceName)
        .SetSampler(new AlwaysOnSampler())
        .AddAspNetCoreInstrumentation()
        .AddZipkinExporter())
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation();
        if (config.GetValue<bool>("InfluxDB:Use"))
        {
            metrics.AddInfluxDBMetricsExporter(options =>
            {
                options.Org = config["InfluxDB:Org"];
                options.Bucket = config["InfluxDB:Bucket"];
                options.Token = config["InfluxDB:Token"];
                options.Endpoint = new Uri(config["InfluxDB:url"]!);
                options.MetricsSchema = MetricsSchema.TelegrafPrometheusV2;
            });
        }
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<NotesContext>(x => x.UseSqlite("DataSource=app.db", b => b.MigrationsAssembly("NotesApp.API")));
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<NotesContext>()
    .AddApiEndpoints();

var app = builder.Build();

app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();