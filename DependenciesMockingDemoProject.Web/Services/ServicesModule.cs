using DependenciesMockingDemoProject.Web.Services.DateTimeService;
using DependenciesMockingDemoProject.Web.Services.OpenWeatherClient;
using DependenciesMockingDemoProject.Web.Services.WeatherForecastService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependenciesMockingDemoProject.Web.Services
{
    internal static class ServicesModule
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDateTimeService();
            services.AddOpenWeatherClient(configuration);
            services.AddWeatherForecastService();
        }
    }
}