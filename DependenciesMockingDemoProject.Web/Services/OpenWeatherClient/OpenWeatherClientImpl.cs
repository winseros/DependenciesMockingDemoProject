using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Client.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DependenciesMockingDemoProject.Web.Services.OpenWeatherClient
{
    public class OpenWeatherClientImpl : IOpenWeatherClient
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static readonly string CLIENT_NAME = "open-weather-client";

        private readonly OpenWeatherClientOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public OpenWeatherClientImpl(IOptions<OpenWeatherClientOptions> options,
            IHttpClientFactory httpClientFactory,
            ILogger<OpenWeatherClientImpl> logger)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<WeatherGetModel> GetCurrentWeatherInAsync(string city,
            CancellationToken ct)
        {
            _logger.LogDebug("Retrieving the current weather in the city: {0}", city);

            using HttpClient client = _httpClientFactory.CreateClient(CLIENT_NAME);

            //Do proper exception handling here!!!
            HttpResponseMessage message =
                await client.GetAsync($"data/2.5/weather?q={city}&appid={_options.ApiKey}", ct);

            //Do proper response handling here!!!
            if (!message.IsSuccessStatusCode)
                throw new HttpStatusException(message.StatusCode);

            using Stream data = await message.Content.ReadAsStreamAsync(ct);

            //Do proper exception handling here!!!
            var items = await JsonSerializer.DeserializeAsync<WeatherGetModel>(data, Options, ct);

            _logger.LogDebug("Retrieved the response");

            return items;
        }
    }
}