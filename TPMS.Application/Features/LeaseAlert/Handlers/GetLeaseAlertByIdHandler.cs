using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.LeaseAlert.DTOs;
using TPMS.Application.Features.LeaseAlert.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.LeaseAlert.Handlers;

public class GetLeaseAlertByIdHandler : IRequestHandler<GetLeaseAlertByIdQuery, LeaseAlertDtoCrud?>
{
    private readonly TPMSDBContext _db;
    public GetLeaseAlertByIdHandler(TPMSDBContext db) => _db = db;

    public async Task<LeaseAlertDtoCrud?> Handle(GetLeaseAlertByIdQuery request, CancellationToken cancellationToken)
    {
        var a = await _db.LeaseAlerts.FirstOrDefaultAsync(x => x.AlertID == request.AlertID, cancellationToken);
        if (a == null) return null;

        return new LeaseAlertDtoCrud()
        {
            AlertID = a.AlertID,
            LeaseID = a.LeaseID,
            AlertType = a.AlertType,
            AlertDate = a.AlertDate,
            SentAt = a.SentAt,
            Status = a.Status,
            Message = a.Message,
            DeliveryMethod = a.DeliveryMethod,
            RetryCount = a.RetryCount,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
    
}