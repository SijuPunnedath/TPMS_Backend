using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Infrastructure.Common.DataSeed;

public class PermissionSeederHostedService : IHostedService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<PermissionSeederHostedService> _logger;

    public PermissionSeederHostedService(
        IServiceProvider provider,
        ILogger<PermissionSeederHostedService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TPMSDBContext>();

            if (!await context.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogError("Database unreachable. Skipping permission seeding.");
                return;
            }

            await PermissionSeeder.SeedAsync(context);
            _logger.LogInformation("Permission seeding completed successfully.");
        }
        catch (Exception ex)
        {
            // 🔥 DO NOT crash the app
            _logger.LogError(ex, "Permission seeding failed.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
