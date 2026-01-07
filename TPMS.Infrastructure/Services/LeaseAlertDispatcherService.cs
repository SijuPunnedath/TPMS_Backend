using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.POCO;
using TPMS.Infrastructure.Services;

public class LeaseAlertDispatcherService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<LeaseAlertDispatcherService> _logger;

    private const int MAX_RETRY = 3;
    private const int BATCH_SIZE = 100;

    public LeaseAlertDispatcherService(
        IServiceProvider provider,
        ILogger<LeaseAlertDispatcherService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("LeaseAlertDispatcherService started");

        // Allow app + DB to fully start
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingAlerts(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LeaseAlertDispatcherService loop failure");
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task ProcessPendingAlerts(CancellationToken token)
    {
        using var scope = _provider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<TPMSDBContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();

        var alerts = await db.LeaseAlerts
            .Include(a => a.Lease)
                .ThenInclude(l => l.Tenant)
            .Include(a => a.Lease)
                .ThenInclude(l => l.Landlord)
            .Where(a =>
                a.Status == AlertStatus.Pending &&
                a.AlertDate <= DateTime.UtcNow &&
                !a.IsDeleted)
            .OrderBy(a => a.AlertDate)
            .Take(BATCH_SIZE)
            .ToListAsync(token);

        if (!alerts.Any())
            return;

        var recipientKeys = alerts
            .Select(a =>
            {
                var isInbound = a.Lease.LeaseType == LeaseType.Inbound;

                if (isInbound && !a.Lease.LandlordID.HasValue)
                {
                    _logger.LogWarning(
                        "Skipping alert {AlertId}: inbound lease has no landlord",
                        a.AlertID);
                    return null;
                }

                if (!isInbound && !a.Lease.TenantID.HasValue)
                {
                    _logger.LogWarning(
                        "Skipping alert {AlertId}: outbound lease has no tenant",
                        a.AlertID);
                    return null;
                }

                return new
                {
                    OwnerTypeId = isInbound ? 1 : 2,
                    OwnerId = isInbound
                        ? a.Lease.LandlordID!.Value
                        : a.Lease.TenantID!.Value
                };
            })
            .Where(x => x != null)
            .Distinct()
            .ToList();

        if (!recipientKeys.Any())
            return;

        var addressMap = await db.Addresses
            .Where(a =>
                recipientKeys.Select(r => r!.OwnerId).Contains(a.OwnerID) &&
                recipientKeys.Select(r => r!.OwnerTypeId).Contains(a.OwnerTypeID))
            .ToDictionaryAsync(
                a => (a.OwnerTypeID, a.OwnerID),
                token);

        foreach (var alert in alerts)
        {
            try
            {
                alert.Status = AlertStatus.Processing;
                await db.SaveChangesAsync(token);

                var message = alert.Message ?? "You have a lease notification.";
                var isInbound = alert.Lease.LeaseType == LeaseType.Inbound;
                var ownerTypeId = isInbound ? 1 : 2;
                var ownerId = isInbound
                    ? alert.Lease.LandlordID
                    : alert.Lease.TenantID;

                if (!ownerId.HasValue ||
                    !addressMap.TryGetValue((ownerTypeId, ownerId.Value), out var address))
                {
                    throw new Exception("Recipient address not found");
                }

                if (alert.DeliveryMethod is DeliveryMethod.Email or DeliveryMethod.Both &&
                    !string.IsNullOrWhiteSpace(address.Email))
                {
                    await emailService.SendEmailAsync(
                        address.Email,
                        "TPMS Alert",
                        message);
                }

                if (alert.DeliveryMethod is DeliveryMethod.Sms or DeliveryMethod.Both &&
                    !string.IsNullOrWhiteSpace(address.Phone1))
                {
                    await smsService.SendSmsAsync(
                        address.Phone1,
                        message);
                }

                alert.Status = AlertStatus.Sent;
                alert.SentAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                alert.RetryCount++;

                alert.Status = alert.RetryCount >= MAX_RETRY
                    ? AlertStatus.Failed
                    : AlertStatus.Pending;

                _logger.LogError(ex,
                    "Alert {AlertId} failed (retry {Retry})",
                    alert.AlertID,
                    alert.RetryCount);
            }
        }

        await db.SaveChangesAsync(token);
    }
}
