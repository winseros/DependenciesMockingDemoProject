using System;
using System.Linq;
using DependenciesMockingDemoProject.Web.DataLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependenciesMockingDemoProject.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            static bool Migrate(string p)
            {
                return string.Equals(p, "--migrate", StringComparison.InvariantCultureIgnoreCase);
            }

            if (args.FirstOrDefault(Migrate) == null)
                CreateHostBuilder(args).Build().Run();
            else
                RunMigrations(args);
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static void RunMigrations(string[] args)
        {
            IServiceCollection services = DesignTimeFactory.PrepareDesignTimeServices(args);
            services.AddSingleton<Migrations>();

            IServiceProvider provider = services.BuildServiceProvider();
            provider.GetRequiredService<Migrations>().Run();
        }
    }
}