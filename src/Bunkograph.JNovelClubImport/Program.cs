
using Bunkograph.DAL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bunkograph.JNovelClubImport
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using IHost? host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .Build();

            // Apply EF Core migrations.
            await using (AsyncServiceScope scope = host.Services.CreateAsyncScope())
            {
                await using BunkographContext? db = scope.ServiceProvider.GetRequiredService<BunkographContext>();
                await db.Database.MigrateAsync();
            }

            await host.RunAsync();
        }

        private static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
        {
            string? connectionString = builder.Configuration.GetConnectionString("localdb");
            if (connectionString is null)
            {
                throw new InvalidOperationException("Database connection string 'localdb' not found.");
            }
            string? port = builder.Configuration.GetValue<string>("WEBSITE_MYSQL_PORT");
            if (port != null)
            {
                // Azure App Services do not create a valid connection string. We need to tweak it
                // and move the port # into a separate key-value pair.
                connectionString = connectionString.Replace(":" + port, string.Empty) + ";Port=" + port;
            }

            ServerVersion? serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<BunkographContext>(dbContextOptions =>
            {
                DbContextOptionsBuilder? result = dbContextOptions.UseMySql(connectionString, serverVersion);

                if (builder.HostingEnvironment.IsDevelopment())
                {
                    result = result.LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                }
            });

            services.AddHostedService<JNovelClubSyncService>();

            services.AddHttpClient<JNovelClubClient>();

            services.AddOptions<JNovelClubOptions>()
                .Bind(builder.Configuration.GetSection("JNC"));
        }
    }
}