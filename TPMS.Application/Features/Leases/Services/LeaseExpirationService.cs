using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Services;

public class LeaseExpirationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public LeaseExpirationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TPMSDBContext>();

            var now = DateTime.UtcNow;

            var expiredLeases = await db.Leases
                .Where(l => l.Status == LeaseStatus.Active && l.EndDate < now)
                .ToListAsync(stoppingToken);

            foreach (var lease in expiredLeases)
            {
                lease.Status = LeaseStatus.Expired;

                var property = await db.Properties
                    .FirstOrDefaultAsync(p => p.PropertyID == lease.PropertyID, stoppingToken);

                if (property != null)
                {
                    if (lease.LeaseType == LeaseType.Inbound)
                        property.ActiveInboundLeaseId = null;
                    else
                        property.ActiveOutboundLeaseId = null;
                }
            }

            await db.SaveChangesAsync(stoppingToken);

            // Run once per day
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
