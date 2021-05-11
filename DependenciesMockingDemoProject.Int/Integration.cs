using System;
using System.Net.Http;
using DependenciesMockingDemoProject.Client;
using DependenciesMockingDemoProject.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WireMock.Server;

namespace DependenciesMockingDemoProject.Int
{
    [SetUpFixture]
    public class Integration
    {
        private static readonly int AppPort = 6000;
        private IHost _webApp;

        internal static WireMockServer WeatherApi { get; private set; }
        internal const string MockWeatherApiKey = "mock-weather-api-key";
        private const string MockWeatherApiAddress = "http://localhost:6001";

        [OneTimeSetUp]
        public void StartApplication()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("ASPNETCORE_URLS", $"http://localhost:{AppPort}");
            Environment.SetEnvironmentVariable("OpenWeatherApi__BaseAddress", MockWeatherApiAddress);
            Environment.SetEnvironmentVariable("OpenWeatherApi__ApiKey", MockWeatherApiKey);

            IHostBuilder builder = Program.CreateHostBuilder(Array.Empty<string>());
            builder.ConfigureServices(services => { services.AddSingleton(IntegrationLogger.Factory); });

            _webApp = builder.Build();
            _webApp.Start();
        }

        [OneTimeTearDown]
        public void StopApplication()
        {
            _webApp.StopAsync().Wait();
            _webApp.Dispose();
        }

        [OneTimeSetUp]
        public void StartApiMocks()
        {
            WeatherApi = WireMockServer.Start(MockWeatherApiAddress);
        }

        [OneTimeTearDown]
        public void StopApiMocks()
        {
            WeatherApi.Stop();
        }

        public static IWeatherForecastClient CreateClient()
        {
            return new TestWeatherForecastClient(new Logger<WeatherForecastClient>(IntegrationLogger.Factory));
        }

        private class TestWeatherForecastClient : WeatherForecastClient
        {
            public TestWeatherForecastClient(ILogger<WeatherForecastClient> logger)
                : base(logger)
            {
            }

            protected override HttpClient GetHttpClient()
            {
                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(20),
                    BaseAddress = new Uri($"http://localhost:{AppPort}")
                };
                return client;
            }
        }
    }
}