using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DependenciesMockingDemoProject.Web.Services.OpenWeatherClient
{
    public static class OpenWeatherClientModule
    {
        public static void AddOpenWeatherClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<OpenWeatherClientOptions>()
                .Bind(configuration.GetSection("OpenWeatherApi"))
                .ValidateDataAnnotations();
            services.AddHttpClient(OpenWeatherClientImpl.CLIENT_NAME, (provider, client) =>
            {
                var opts = provider.GetRequiredService<IOptions<OpenWeatherClientOptions>>();
                client.BaseAddress = opts.Value.BaseAddress;
            });
            services.AddSingleton<IOpenWeatherClient, OpenWeatherClientImpl>();
        }
    }
}