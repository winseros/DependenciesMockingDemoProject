using DependenciesMockingDemoProject.Web.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DependenciesMockingDemoProject.Web.DataLayer
{
    public class WeatherDbContext : DbContext
    {
        private readonly DataLayerOptions _options;
        private readonly ILoggerFactory _loggerFactory;

        public WeatherDbContext(IOptions<DataLayerOptions> options, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _options = options.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeatherForecastEntity).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_options.ConnectionString,
                builder => { builder.MigrationsHistoryTable("__EFMigrationsHistory", SchemaConstants.SchemaName); });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            if (_loggerFactory != null)
                optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}