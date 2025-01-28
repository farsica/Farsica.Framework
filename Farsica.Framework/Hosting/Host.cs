namespace Farsica.Framework.Hosting
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Destructurama;
    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Context;
    using Farsica.Framework.Logging;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public static class Host
    {
        public static async Task RunAsync<TStartup>(string[] args)
            where TStartup : class
        {
            await RunInternal<TStartup>(args, false);
        }

        public static async Task RunAsync<TStartup, TUser, TRole>(string[] args)
            where TStartup : Startup.Startup<TUser, TRole>
            where TUser : class
            where TRole : class
        {
            await RunInternal<TStartup>(args, true);
        }

        public static IHost? CreateHost<TStartup>(string[] args)
            where TStartup : class
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Globals.ProviderType = config.GetValue<DbProviderType>("Connection:ProviderType");
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Destructure.UsingAttributes()
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .CreateLogger();

            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddFilelog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>()
                    .UseSetting(WebHostDefaults.ApplicationKey, typeof(TStartup).GetTypeInfo().Assembly.FullName)
                    .UseKestrel(options =>
                    {
                        options.AddServerHeader = false;
                    })
                    .UseIIS();
                }).Build();
        }

        private static async Task RunInternal<TStartup>(string[] args, bool checkMigration)
            where TStartup : class
        {
            var host = CreateHost<TStartup>(args);
            if (host is null)
            {
                return;
            }

            if (checkMigration)
            {
                using var scope = host.Services.CreateScope();
                if (scope.ServiceProvider.GetService(typeof(IEntityContext)) is DbContext context)
                {
                    await context.Database.MigrateAsync();
                }
            }

            await host.RunAsync();
        }
    }
}
