
using Bunkograph.DAL;
using Microsoft.EntityFrameworkCore;

namespace Bunkograph.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();

            string? connectionString = "server=hamsterexastris.synology.me;port=3307;user=BunkographWeb;password=&34f!E3E*6uiMF#u;database=bunkograph";
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

            WebApplication? app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            // Apply EF Core migrations.
            using (IServiceScope? scope = app.Services.CreateScope())
            {
                BunkographContext? db = scope.ServiceProvider.GetRequiredService<BunkographContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}