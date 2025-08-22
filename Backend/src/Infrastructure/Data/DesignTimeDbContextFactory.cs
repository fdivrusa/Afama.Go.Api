using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Afama.Go.Api.Infrastructure.Data
{
    /// <summary>
    /// Design-time factory so EF Core tools can create ApplicationDbContext
    /// without booting the host (and without needing environment variables).
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                   ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                   ?? "Development";

            var infraDir = Directory.GetCurrentDirectory();

            var apiHostDir = Path.GetFullPath(Path.Combine(infraDir, "..", "Api.Host"));

            if (!Directory.Exists(apiHostDir))
            {
                var probe = new DirectoryInfo(infraDir);
                for (int i = 0; i < 4 && probe != null; i++, probe = probe.Parent)
                {
                    var candidate = Path.Combine(probe.FullName, "Api.Host");
                    if (Directory.Exists(candidate))
                    {
                        apiHostDir = candidate;
                        break;
                    }
                }
            }

            var configBasePath = Directory.Exists(apiHostDir) ? apiHostDir : infraDir;

            var config = new ConfigurationBuilder()
                .SetBasePath(configBasePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();


            var connStr = config.GetConnectionString("Afama.Go.ApiDb")
                      ?? config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connStr))
                throw new InvalidOperationException(
                    "Connection string 'Afama.Go.ApiDb' (or 'DefaultConnection') not found in configuration.");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connStr)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
