using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.LeaseAlert.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.LeaseAlert.Handlers;

public class RestoreLeaseAlertHandler :IRequestHandler<RestoreLeaseAlertCommand, bool>
{
    private readonly TPMSDBContext _db;
    public RestoreLeaseAlertHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(RestoreLeaseAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await _db.LeaseAlerts.FirstOrDefaultAsync(a => a.AlertID == request.AlertID, cancellationToken);
        if (alert == null) return false;

        alert.IsDeleted = false;
        alert.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}