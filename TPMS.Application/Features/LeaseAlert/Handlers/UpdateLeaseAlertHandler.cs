using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.LeaseAlert.Commands;
using TPMS.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace TPMS.Application.Features.LeaseAlert.Handlers;

public class UpdateLeaseAlertHandler : IRequestHandler<UpdateLeaseAlertCommand, bool>
{
    private readonly TPMSDBContext _db;
    public UpdateLeaseAlertHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(UpdateLeaseAlertCommand request, CancellationToken cancellationToken)
    {
        var dto = request.LeaseAlert;
        var alert = await _db.LeaseAlerts.FirstOrDefaultAsync(a => a.AlertID == dto.AlertID, cancellationToken);
        if (alert == null) return false;

        alert.AlertType = dto.AlertType;
        alert.AlertDate = dto.AlertDate;
        alert.SentAt = dto.SentAt;
        alert.Status = dto.Status;
        alert.Message = dto.Message;
        alert.DeliveryMethod = dto.DeliveryMethod;
        alert.RetryCount = dto.RetryCount;
        alert.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}