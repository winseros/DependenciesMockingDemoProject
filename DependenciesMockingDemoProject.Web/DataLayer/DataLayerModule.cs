using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependenciesMockingDemoProject.Web.DataLayer
{
    internal static class DataLayerModule
    {
        public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DataLayerOptions>()
                .Bind(configuration.GetSection("Database"))
                .ValidateDataAnnotations();

            services.AddSingleton<IDbContextFactory, DbContextFactory>();
        }
    }
}