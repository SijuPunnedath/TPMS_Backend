using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.LeaseAlert.Commands;
using TPMS.Application.Features.LeaseAlert.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;
using System;
using TPMS.Domain.Entities;

namespace TPMS.Application.Features.LeaseAlert.Handlers;


public class CreateLeaseAlertHandler : IRequestHandler<CreateLeaseAlertCommand, int>
{
    private readonly TPMSDBContext _db;
    public CreateLeaseAlertHandler(TPMSDBContext db) => _db = db;

    public async Task<int> Handle(CreateLeaseAlertCommand request, CancellationToken cancellationToken)
    {
        var dto = request.LeaseAlert;

        var alert = new Domain.Entities.LeaseAlert()
        {
            LeaseID = dto.LeaseID,
            AlertType = dto.AlertType,
            AlertDate = dto.AlertDate,
            SentAt = dto.SentAt,
            Status = dto.Status,
            Message = dto.Message,
            DeliveryMethod = dto.DeliveryMethod,
            RetryCount = dto.RetryCount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.LeaseAlerts.Add(alert);
        await _db.SaveChangesAsync(cancellationToken);
        return alert.AlertID;
    }
}