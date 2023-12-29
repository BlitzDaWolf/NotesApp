using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Diagnostics;

namespace NotesApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ActivitySource activitySource;
        readonly TestService testService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, Instrumentation instrumentation, TestService testService)
        {
            _logger = logger;
            this.activitySource = instrumentation.ActivitySource;
            this.testService = testService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(int runs)
        {
            var curId = Activity.Current?.Id;
            using var activity = this.activitySource.StartActivity("calculate forecast");
            for (int i = 0; i < runs; i++)
            {
                await Task.Delay(200);
                await testService.TestingTrace();
            }
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
