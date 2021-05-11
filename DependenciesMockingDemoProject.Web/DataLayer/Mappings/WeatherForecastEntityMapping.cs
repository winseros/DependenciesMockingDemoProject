using DependenciesMockingDemoProject.Web.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DependenciesMockingDemoProject.Web.DataLayer.Mappings
{
    public class WeatherForecastEntityMapping : IEntityTypeConfiguration<WeatherForecastEntity>
    {
        public void Configure(EntityTypeBuilder<WeatherForecastEntity> builder)
        {
            builder.ToTable("WeatherForecasts", SchemaConstants.SchemaName);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Date).IsRequired();
            builder.Property(p => p.TemperatureC).IsRequired();
            builder.Property(p => p.Summary).IsRequired().HasMaxLength(100);
        }
    }
}