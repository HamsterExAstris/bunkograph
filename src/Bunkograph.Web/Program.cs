
using Bunkograph.DAL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

namespace Bunkograph.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();

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
            builder.Services.AddDbContext<BunkographContext>(dbContextOptions =>
            {
                DbContextOptionsBuilder? result = dbContextOptions.UseMySql(connectionString, serverVersion);

                if (builder.Environment.IsDevelopment())
                {
                    result = result.LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                }
            });

            builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);

            WebApplication? app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            // Apply EF Core migrations.
            await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
            {
                await using BunkographContext? db = scope.ServiceProvider.GetRequiredService<BunkographContext>();
                await db.Database.MigrateAsync();
            }

            app.Run();
        }
    }
}