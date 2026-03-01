using Microsoft.EntityFrameworkCore;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Infrastructure.Services;

public class LeaseAlertService : ILeaseAlertService
{
    private readonly TPMSDBContext _context;

    public LeaseAlertService(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task GenerateAlertsAsync(DateTime runDate)
    {
        var today = runDate.Date;

        var rules = await _context.LeaseAlertRules
            .Where(r => r.IsActive)
            .ToListAsync();

        var leases = await _context.Leases
            .Where(l => !l.IsDeleted && !l.IsTerminated)
            .ToListAsync();

        foreach (var lease in leases)
        {
            foreach (var rule in rules)
            {
                if (!IsRuleApplicable(lease, rule, today))
                    continue;

                await CreateAlertIfNotExists(lease, rule, today);
            }
        }

        await _context.SaveChangesAsync();
    }

    
    private bool IsRuleApplicable(
        Lease lease,
        LeaseAlertRule rule,
        DateTime today)
    {
        if (rule.LeaseType.HasValue && lease.LeaseType != rule.LeaseType)
            return false;

        if (rule.PaymentFrequency != lease.PaymentFrequency)
            return false;

        return rule.AlertType switch
        {
            "LeaseExpiry" =>
                (lease.EndDate.Date - today).Days == rule.TriggerDays,

            "RentDue" =>
                GetRentDueDate(lease, today) == today.AddDays(rule.TriggerDays),

            "RentOverdue" =>
                GetRentDueDate(lease, today).AddDays(-rule.TriggerDays) == today,

            _ => false
        };
    }

    private DateTime GetRentDueDate(Lease lease, DateTime referenceDate)
    {
        var startDate = lease.StartDate.Date;

        return lease.PaymentFrequency switch
        {
            "Monthly"   => GetMonthlyDueDate(startDate, referenceDate),
            "Quarterly" => GetQuarterlyDueDate(startDate, referenceDate),
            "Yearly"    => GetYearlyDueDate(startDate, referenceDate),
            _ => throw new InvalidOperationException(
                $"Unsupported payment frequency: {lease.PaymentFrequency}")
        };
    }

    private DateTime GetMonthlyDueDate(DateTime leaseStart, DateTime referenceDate)
    {
        int dueDay = leaseStart.Day;

        int daysInMonth = DateTime.DaysInMonth(
            referenceDate.Year,
            referenceDate.Month);

        // Clamp day (handles 29/30/31 safely)
        dueDay = Math.Min(dueDay, daysInMonth);

        return new DateTime(
            referenceDate.Year,
            referenceDate.Month,
            dueDay);
    }

    private DateTime GetQuarterlyDueDate(DateTime leaseStart, DateTime referenceDate)
    {
        int monthsSinceStart =
            ((referenceDate.Year - leaseStart.Year) * 12)
            + referenceDate.Month - leaseStart.Month;

        int currentQuarterStartMonth =
            leaseStart.Month + (monthsSinceStart / 3) * 3;

        int year = leaseStart.Year + (currentQuarterStartMonth - 1) / 12;
        int month = ((currentQuarterStartMonth - 1) % 12) + 1;

        int dueDay = Math.Min(
            leaseStart.Day,
            DateTime.DaysInMonth(year, month));

        return new DateTime(year, month, dueDay);
    }

    private DateTime GetYearlyDueDate(DateTime leaseStart, DateTime referenceDate)
    {
        int year = referenceDate.Year;

        int dueDay = Math.Min(
            leaseStart.Day,
            DateTime.DaysInMonth(year, leaseStart.Month));

        return new DateTime(year, leaseStart.Month, dueDay);
    }

    
    private async Task CreateAlertIfNotExists(
        Lease lease,
        LeaseAlertRule rule,
        DateTime today)
    {
        bool exists = await _context.LeaseAlerts.AnyAsync(a =>
            a.LeaseID == lease.LeaseID &&
            a.AlertType == rule.AlertType &&
            a.AlertDate == today &&
            !a.IsDeleted);

        if (exists)
            return;

        var message = rule.MessageTemplate
            .Replace("{LeaseName}", lease.LeaseName)
            .Replace("{Days}", rule.TriggerDays.ToString());

        _context.LeaseAlerts.Add(new LeaseAlert
        {
            LeaseID = lease.LeaseID,
            AlertType = rule.AlertType,
            AlertDate = today,
            Message = message,
            DeliveryMethod = rule.DeliveryMethod,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
    }

    
    
}