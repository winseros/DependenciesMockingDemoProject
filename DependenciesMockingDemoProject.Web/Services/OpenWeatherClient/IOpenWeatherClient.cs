using System.Threading;
using System.Threading.Tasks;
using DependenciesMockingDemoProject.Client.Models;

namespace DependenciesMockingDemoProject.Web.Services.OpenWeatherClient
{
    public interface IOpenWeatherClient
    {
        Task<WeatherGetModel> GetCurrentWeatherInAsync(string city, CancellationToken ct);
    }
}