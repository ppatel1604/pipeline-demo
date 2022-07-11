using Microsoft.AspNetCore.Mvc;

namespace Web.api.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(int maxNumberOfDays = 5)
        {
            _logger.LogInformation("Getting weather forcast information");

            if(maxNumberOfDays <= 0)
            {
                _logger.LogError("Max number of days are set less than 0.");
                return Array.Empty<WeatherForecast>();
            }

            if(maxNumberOfDays > 5)
            {
                // Assumption: 5 is the maximum number of days for the weather forecast
                _logger.LogWarning("Max number of days were more than 5. Set it 5");
                maxNumberOfDays = 5;
            }

            return Enumerable.Range(1, maxNumberOfDays).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}