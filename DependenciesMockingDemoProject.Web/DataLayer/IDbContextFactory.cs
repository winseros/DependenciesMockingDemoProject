namespace DependenciesMockingDemoProject.Web.DataLayer
{
    public interface IDbContextFactory
    {
        WeatherDbContext GetWeatherDbContext();
    }
}