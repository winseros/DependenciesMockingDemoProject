using DependenciesMockingDemoProject.Client.Models;
using DependenciesMockingDemoProject.Web.DataLayer.Entities;

namespace DependenciesMockingDemoProject.Web.Controllers
{
    public static class WeatherForecastModelExtensions
    {
        public static WeatherForecastGetModel ToGetModel(this WeatherForecastEntity entity)
        {
            return new()
            {
                Date = entity.Date,
                TemperatureC = entity.TemperatureC,
                Summary = entity.Summary
            };
        }
    }
}