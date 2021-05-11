using System;
using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Web.DataLayer;
using DependenciesMockingDemoProject.Web.DataLayer.Entities;
using DependenciesMockingDemoProject.Web.Services.DateTimeService;
using DependenciesMockingDemoProject.Web.Services.WeatherForecastService;
using NUnit.Framework;

namespace DependenciesMockingDemoProject.Test.Services
{
    public class WeatherForecastServiceTest
    {
        [Test]
        public async Task Test_GetForecastsAsync_Returns_Correct_Results()
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
                    }, new WeatherForecastEntity
                    {
                        Id = Guid.NewGuid(),
                        Date = now.AddDays(-30),
                        Summary = "sum-3",
                        TemperatureC = 1
                    }, new WeatherForecastEntity
                    {
                        Id = Guid.NewGuid(),
                        Date = now.AddDays(-31),
                        Summary = "sum-4",
                        TemperatureC = 1
                    });
                await ctx.SaveChangesAsync();
            }

            using var mock = Mock.Auto();
            mock.Mock<IDateTimeService>().Setup(p => p.Now()).Returns(now);

            var service = mock.Create<WeatherForecastServiceImpl>();

            WeatherForecastEntity[] entities = await service.GetWeatherForecastsAsync(CancellationToken.None);

            Assert.AreEqual(3, entities.Length);
        }
    }
}