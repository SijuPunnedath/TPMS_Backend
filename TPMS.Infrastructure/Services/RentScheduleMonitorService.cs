using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

public class RentScheduleMonitorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RentScheduleMonitorService> _logger;

    public RentScheduleMonitorService(
        IServiceProvider serviceProvider,
        ILogger<RentScheduleMonitorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Give the app time to fully start
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOverdueSchedules(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RentScheduleMonitorService failed");
                // IMPORTANT: swallow exception to prevent host crash
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task ProcessOverdueSchedules(CancellationToken token)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TPMSDBContext>();

        var today = DateTime.UtcNow.Date;

        var overdue = await db.RentSchedules
            .Include(rs => rs.Lease)
            .ThenInclude(l => l.PenaltyPolicy)
            .Where(rs => !rs.IsPaid &&
                         today > rs.DueDate.AddDays(rs.Lease.PenaltyPolicy.GracePeriodDays))
            .ToListAsync(token);

        foreach (var rs in overdue)
        {
            if (rs.Penalty.HasValue && rs.Penalty > 0)
                continue;

            decimal penalty = 0;

            if (rs.Lease.PenaltyPolicy.FixedAmount.HasValue)
                penalty += rs.Lease.PenaltyPolicy.FixedAmount.Value;

            if (rs.Lease.PenaltyPolicy.PercentageOfRent.HasValue)
                penalty += rs.Amount * rs.Lease.PenaltyPolicy.PercentageOfRent.Value / 100;

            rs.Penalty = penalty;
            rs.Status = "Overdue";
        }

        await db.SaveChangesAsync(token);

        await CreateAlerts(overdue, db, token);
    }

    private async Task CreateAlerts(
        List<RentSchedule> overdue,
        TPMSDBContext db,
        CancellationToken token)
    {
        foreach (var rs in overdue)
        {
            bool exists = await db.LeaseAlerts.AnyAsync(
                a => a.LeaseID == rs.LeaseID &&
                     a.AlertType == "OverdueRent" &&
                     a.AlertDate.Date == DateTime.UtcNow.Date,
                token);

            if (exists)
                continue;

            db.LeaseAlerts.Add(new LeaseAlert
            {
                LeaseID = rs.LeaseID,
                AlertType = "OverdueRent",
                AlertDate = DateTime.UtcNow,
                Status = "Pending",
                Message = $"Rent overdue for schedule {rs.ScheduleID}.",
                DeliveryMethod = "Dashboard"
            });
        }

        await db.SaveChangesAsync(token);
    }
}
