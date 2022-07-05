
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bunkograph.DAL
{
    public class BunkographContextFactory : IDesignTimeDbContextFactory<BunkographContext>
    {
        public BunkographContext CreateDbContext(string[] args)
        {
            // Create a configuration using the user secrets from the web project, where we expect
            // developers to store their connection string.
            Assembly? assembly = Assembly.Load("Bunkograph.Web");
            IConfigurationBuilder? configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(assembly);
            IConfigurationRoot? configuration = configurationBuilder.Build();

            string? connectionString = configuration.GetConnectionString("localdb")
                ?? throw new InvalidOperationException("Database connection string 'localdb' not found.");

            ServerVersion? serverVersion = ServerVersion.AutoDetect(connectionString);

            DbContextOptionsBuilder<BunkographContext>? optionsBuilder = new DbContextOptionsBuilder<BunkographContext>();
            optionsBuilder.UseMySql(connectionString, serverVersion);

            return new BunkographContext(optionsBuilder.Options);
        }
    }
}
