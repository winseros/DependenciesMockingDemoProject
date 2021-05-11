using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Client.Models;
using Microsoft.Extensions.Logging;

namespace DependenciesMockingDemoProject.Client
{
    public abstract class WeatherForecastClient : IWeatherForecastClient
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly ILogger _logger;

        protected WeatherForecastClient(ILogger logger)
        {
            _logger = logger;
        }

        protected abstract HttpClient GetHttpClient();

        public async Task<WeatherForecastGetModel[]> GetWeatherForecasts(CancellationToken ct)
        {
            _logger.LogDebug("Retrieving the set of forecasts");

            using HttpClient httpClient = GetHttpClient();

            //Do proper exception handling here!!!
            HttpResponseMessage message = await httpClient.GetAsync("weather/forecasts", ct);

            //Do proper response handling here!!!
            if (!message.IsSuccessStatusCode)
                throw new ApplicationException("The service responded with an unexpected status code");

            using Stream data = await message.Content.ReadAsStreamAsync(ct);

            //Do proper exception handling here!!!
            var items = await JsonSerializer.DeserializeAsync<WeatherForecastGetModel[]>(data, Options, ct);

            _logger.LogDebug("Retrieved {0} items", items?.Length);

            return items;
        }

        public async Task<WeatherGetModel> GetCurrentWeatherIn(string city, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(city))
                throw new ArgumentNullException(nameof(city));

            _logger.LogDebug("Retrieving the set of forecasts");

            using HttpClient httpClient = GetHttpClient();

            //Do proper exception handling here!!!
            HttpResponseMessage message = await httpClient.GetAsync($"weather/current?city={city}", ct);

            //Do proper response handling here!!!
            if (!message.IsSuccessStatusCode)
                throw new ApplicationException("The service responded with an unexpected status code");

            using Stream data = await message.Content.ReadAsStreamAsync(ct);

            //Do proper exception handling here!!!
            var items = await JsonSerializer.DeserializeAsync<WeatherGetModel>(data, Options, ct);

            _logger.LogDebug("Retrieved the weather");

            return items;
        }
    }
}