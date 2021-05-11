using Microsoft.Extensions.DependencyInjection;

namespace DependenciesMockingDemoProject.Web.Services.DateTimeService
{
    public static class DateTimeServiceModule
    {
        public static void AddDateTimeService(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeServiceImpl>();
        }
    }
}