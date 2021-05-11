using DependenciesMockingDemoProject.Web.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace DependenciesMockingDemoProject.Web
{
    public class Migrations
    {
        private readonly IDbContextFactory _dbContextFactory;

        public Migrations(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void Run()
        {
            using WeatherDbContext ctx = _dbContextFactory.GetWeatherDbContext();
            ctx.Database.Migrate();
        }
    }
}