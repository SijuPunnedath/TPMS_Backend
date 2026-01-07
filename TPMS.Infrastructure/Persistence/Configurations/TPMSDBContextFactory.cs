using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class TPMSDBContextFactory : IDesignTimeDbContextFactory<TPMSDBContext>
    {
        public TPMSDBContext CreateDbContext(string[] args)
        {
           // var optionsBuilder = new DbContextOptionsBuilder<TPMSDBContext>();

            // ✅ Clean, known working connection string
          //  optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=Tenants;Username=postgres;Password=Admin@123;SslMode=Disable;");

           // return new TPMSDBContext(optionsBuilder.Options);
           
           var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

           var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TPMS.API"))
               .AddJsonFile("appsettings.json", optional: false)
               .AddJsonFile($"appsettings.{environment}.json", optional: true)
               .Build();

           var connectionString = configuration.GetConnectionString("DefaultConnection");

           var optionsBuilder = new DbContextOptionsBuilder<TPMSDBContext>();
           optionsBuilder.UseNpgsql(connectionString);

           return new TPMSDBContext(optionsBuilder.Options);
        }
    }
}