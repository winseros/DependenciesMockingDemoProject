using System;
using DependenciesMockingDemoProject.Web.DataLayer;
using DependenciesMockingDemoProject.Web.DataLayer.Entities;
using NUnit.Framework;

namespace DependenciesMockingDemoProject.Test.DataLayer.Mappings
{
    public class WeatherForecastEntityTest
    {
        [Test]
        public void Test_Mapping_Functional()
        {
            Db.Recreate();

            using WeatherDbContext ctx1 = Db.GetAppContext();
            var entity = new WeatherForecastEntity
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Summary = "some summary",
                TemperatureC = 10
            };
            ctx1.Set<WeatherForecastEntity>().Add(entity);
            ctx1.SaveChanges();

            using WeatherDbContext ctx2 = Db.GetAppContext();
            WeatherForecastEntity saved = ctx2.Set<WeatherForecastEntity>().Find(entity.Id);

            Assert.NotNull(saved);
            Assert.AreEqual(entity.Date, saved.Date);
            Assert.AreEqual(entity.Summary, saved.Summary);
            Assert.AreEqual(entity.TemperatureC, saved.TemperatureC);
        }
    }
}