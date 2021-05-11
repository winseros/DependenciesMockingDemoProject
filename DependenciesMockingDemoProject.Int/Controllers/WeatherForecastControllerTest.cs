using System;
using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Client;
using DependenciesMockingDemoProject.Client.Models;
using DependenciesMockingDemoProject.Web.DataLayer;
using DependenciesMockingDemoProject.Web.DataLayer.Entities;
using DependenciesMockingDemoProject.Test;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace DependenciesMockingDemoProject.Int.Controllers
{
    public class WeatherForecastControllerTest : IntegrationTest
    {
        [Test]
        public async Task Test_GetWeatherForecastsAsync_Returns_Weather()
        {
            Db.Recreate();

            var now = DateTime.UtcNow;
            using (WeatherDbContext ctx = Db.GetAdminContext())
            {
                ctx.Set<WeatherForecastEntity>()
                    .AddRange(new WeatherForecastEntity
                    {
                        Id = Guid.NewGuid(),
                        Date = now,
                        Summary = "sum-1",
                        TemperatureC = 1
                    }, new WeatherForecastEntity
                    {
                        Id = Guid.NewGuid(),
                        Date = now.AddDays(-1),
                        Summary = "sum-2",
                        TemperatureC = 1
                    });
                await ctx.SaveChangesAsync();
            }

            IWeatherForecastClient client = Integration.CreateClient();

            WeatherForecastGetModel[] models = await client.GetWeatherForecasts(CancellationToken.None);

            Assert.AreEqual(2, models.Length);
            Assert.AreEqual("sum-1", models[0].Summary);
            Assert.AreEqual("sum-2", models[1].Summary);
        }

        [Test]
        public async Task Test_GetCurrentWeatherAsync_Returns_Weather()
        {
            const string city = "Mountain View";

            Integration.WeatherApi.Given(Request.Create()
                    .WithPath("/data/2.5/weather")
                    .WithParam("appid", Integration.MockWeatherApiKey)
                    .WithParam("q", city))
                .RespondWith(Response.Create()
                    .WithSuccess()
                    .WithBodyFromFile("Controllers\\mockWeatherResponse.json"));

            IWeatherForecastClient client = Integration.CreateClient();

            WeatherGetModel weather = await client.GetCurrentWeatherIn(city, CancellationToken.None);

            Assert.AreEqual(weather.Name, "Mountain View");
        }
    }
}