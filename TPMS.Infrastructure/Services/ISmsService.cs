namespace TPMS.Infrastructure.Services;

public interface ISmsService
{
    Task SendSmsAsync(string phone, string message);
}