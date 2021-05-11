using Microsoft.Extensions.DependencyInjection;

namespace DependenciesMockingDemoProject.Web.Services.WeatherForecastService
{
    internal static class WeatherForecastServiceModule
    {
        public static void AddWeatherForecastService(this IServiceCollection services)
        {
            services.AddSingleton<IWeatherForecastService, WeatherForecastServiceImpl>();
        }
    }
}