using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Web.DataLayer.Entities;

namespace DependenciesMockingDemoProject.Web.Services.WeatherForecastService
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecastEntity[]> GetWeatherForecastsAsync(CancellationToken ct);
    }
}