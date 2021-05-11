using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Client.Models;

namespace DependenciesMockingDemoProject.Client
{
    public interface IWeatherForecastClient
    {
        public Task<WeatherForecastGetModel[]> GetWeatherForecasts(CancellationToken ct);

        public Task<WeatherGetModel> GetCurrentWeatherIn(string city, CancellationToken ct);
    }
}