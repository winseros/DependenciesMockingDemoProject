using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DependenciesMockingDemoProject.Web.DataLayer
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IOptions<DataLayerOptions> _options;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextFactory(IOptions<DataLayerOptions> options, ILoggerFactory loggerFactory)
        {
            _options = options;
            _loggerFactory = loggerFactory;
        }

        public WeatherDbContext GetWeatherDbContext()
        {
            return new(_options, _loggerFactory);
        }
    }
}