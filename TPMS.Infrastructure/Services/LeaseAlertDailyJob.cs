using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TPMS.Infrastructure.Services;

public class LeaseAlertDailyJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LeaseAlertDailyJob> _logger;

    public LeaseAlertDailyJob(
        IServiceScopeFactory scopeFactory,
        ILogger<LeaseAlertDailyJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("LeaseAlertDailyJob started");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunOncePerDay(stoppingToken);
        }
    }

    private async Task RunOncePerDay(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider
                .GetRequiredService<ILeaseAlertService>();

            _logger.LogInformation("Running Lease Alert generation");

            await service.GenerateAlertsAsync(DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lease alert job failed");
        }

        // Sleep for 24 hours
        await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
    }
}
