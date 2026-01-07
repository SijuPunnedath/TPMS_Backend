using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.LeaseAlert.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.LeaseAlert.Handlers;

public class SoftDeleteLeaseAlertHandler :IRequestHandler<SoftDeleteLeaseAlertCommand, bool>
{
    private readonly TPMSDBContext _db;
    public SoftDeleteLeaseAlertHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(SoftDeleteLeaseAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await _db.LeaseAlerts.FirstOrDefaultAsync(a => a.AlertID == request.AlertID, cancellationToken);
        if (alert == null) return false;

        alert.IsDeleted = true;
        alert.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}