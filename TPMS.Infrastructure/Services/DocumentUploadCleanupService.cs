using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;



using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Infrastructure.Services;
public class DocumentUploadCleanupService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DocumentUploadCleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(30); // run every 30 minutes

    public DocumentUploadCleanupService(IServiceProvider services, ILogger<DocumentUploadCleanupService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TPMSDBContext>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                var cutoff = DateTime.UtcNow.AddHours(-24); // configurable
                var stale = await db.DocumentUploadSessions
                    .Where(s => !s.IsCompleted && s.StartedAt < cutoff)
                    .ToListAsync(stoppingToken);

                foreach (var s in stale)
                {
                    try
                    {
                        string tempFolder = Path.Combine(env.ContentRootPath, "Uploads", "Temp", s.SessionId.ToString());
                        if (Directory.Exists(tempFolder))
                        {
                            Directory.Delete(tempFolder, true);
                        }

                        s.IsCompleted = false;
                        s.Status = "Expired";
                        s.ErrorMessage = "Session expired and cleaned by background job.";
                        s.UpdatedAt = DateTime.UtcNow;
                        db.DocumentUploadSessions.Update(s);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to cleanup session {SessionId}", s.SessionId);
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DocumentUploadCleanupService");
            }

            //await Task.Delay(_interval, stoppingToken);  --  here we can use this for setting a different refresh interval
            
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
