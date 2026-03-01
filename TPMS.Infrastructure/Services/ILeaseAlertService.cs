namespace TPMS.Infrastructure.Services;

public interface ILeaseAlertService
{
    Task GenerateAlertsAsync(DateTime runDate);
}