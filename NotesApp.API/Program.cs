using Database.Context;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotesApp.API.Swagger;
using OpenTelemetry.Exporter.InfluxDB;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Service;
using Service.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

const string serviceName = "NotesApp";

// builder.Services.AddSingleton<Instrumentation>();

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
        .AddSource("NotesApp.*", "Service.*")
        .SetSampler(new AlwaysOnSampler())
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(opts => { opts.Endpoint = new Uri("http://localhost:4317"); }))
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
        metrics.AddPrometheusExporter();
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<NotesContext>(x => x.UseSqlite("DataSource=app.db", b => b.MigrationsAssembly("NotesApp.API")));
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<NotesContext>()
    .AddApiEndpoints();

builder.Services.AddScoped<INoteService, NoteService>();

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
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();