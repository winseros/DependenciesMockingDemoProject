using System;

namespace DependenciesMockingDemoProject.Web.DataLayer.Entities
{
    public class WeatherForecastEntity
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}