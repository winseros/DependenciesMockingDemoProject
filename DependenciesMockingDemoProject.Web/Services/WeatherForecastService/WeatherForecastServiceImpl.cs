using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using DependenciesMockingDemoProject.Web.DataLayer;
using DependenciesMockingDemoProject.Web.DataLayer.Entities;
using DependenciesMockingDemoProject.Web.Services.DateTimeService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DependenciesMockingDemoProject.Web.Services.WeatherForecastService
{
    public class WeatherForecastServiceImpl : IWeatherForecastService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<WeatherForecastServiceImpl> _logger;

        public WeatherForecastServiceImpl(
            IDbContextFactory dbContextFactory,
            IDateTimeService dateTimeService,
            ILogger<WeatherForecastServiceImpl> logger)
        {
            _dbContextFactory = dbContextFactory;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public async Task<WeatherForecastEntity[]> GetWeatherForecastsAsync(CancellationToken ct)
        {
            _logger.LogDebug("Retrieving weather forecast items from the database");

            using var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
                TransactionScopeAsyncFlowOption.Enabled);

            using WeatherDbContext ctx = _dbContextFactory.GetWeatherDbContext();

            WeatherForecastEntity[] entities = await ctx.Set<WeatherForecastEntity>()
                .Where(p => p.Date >= _dateTimeService.Now().AddDays(-30))
                .OrderByDescending(p => p.Date)
                .ToArrayAsync(ct);

            scope.Complete();

            _logger.LogDebug("Retrieved the items: {0}", entities.Length);

            return entities;
        }
    }
}