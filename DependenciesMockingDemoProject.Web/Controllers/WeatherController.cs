using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Client.Models;
using DependenciesMockingDemoProject.Web.DataLayer.Entities;
using DependenciesMockingDemoProject.Web.Services.OpenWeatherClient;
using DependenciesMockingDemoProject.Web.Services.WeatherForecastService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DependenciesMockingDemoProject.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly IOpenWeatherClient _openWeatherClient;
        private readonly ILogger _logger;

        public WeatherController(
            IWeatherForecastService weatherForecastService,
            IOpenWeatherClient openWeatherClient,
            ILogger<WeatherController> logger)
        {
            _weatherForecastService = weatherForecastService;
            _openWeatherClient = openWeatherClient;
            _logger = logger;
        }

        [HttpGet("forecasts")]
        [ProducesResponseType(typeof(WeatherForecastGetModel[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWeatherForecastsAsync(CancellationToken ct)
        {
            _logger.LogDebug("Retrieving weather forecasts");

            WeatherForecastEntity[] entities = await _weatherForecastService.GetWeatherForecastsAsync(ct);
            WeatherForecastGetModel[] models = entities.Select(p => p.ToGetModel()).ToArray();

            _logger.LogDebug("Retrieved {0} items", models.Length);

            return Ok(models);
        }

        [HttpGet("current")]
        [ProducesResponseType(typeof(WeatherGetModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentWeatherAsync([Required] [FromQuery] string city,
            CancellationToken ct)
        {
            _logger.LogDebug("Retrieving current weather");

            try
            {
                WeatherGetModel result = await _openWeatherClient.GetCurrentWeatherInAsync(city, ct);
                _logger.LogDebug("Retrieved the result");
                return Ok(result);
            }
            catch (HttpStatusException ex)
            {
                _logger.LogWarning("The service returned an unexpected status code: {0}", ex.StatusCode);
                return StatusCode((int) ex.StatusCode);
            }
        }
    }
}