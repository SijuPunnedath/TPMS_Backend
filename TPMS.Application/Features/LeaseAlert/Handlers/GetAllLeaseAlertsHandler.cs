using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.LeaseAlert.DTOs;
using TPMS.Application.Features.LeaseAlert.Queries;
using TPMS.Infrastructure.Persistence.Configurations;


namespace TPMS.Application.Features.LeaseAlert.Handlers;

public class GetAllLeaseAlertsHandler : IRequestHandler<GetAllLeaseAlertsQuery, List<LeaseAlertDtoCrud>>
{
    private readonly TPMSDBContext _db;
    public GetAllLeaseAlertsHandler(TPMSDBContext db) => _db = db;

    public async Task<List<LeaseAlertDtoCrud>> Handle(GetAllLeaseAlertsQuery request, CancellationToken cancellationToken)
    {
        return await _db.LeaseAlerts
            .Where(a => !a.IsDeleted)
            .Select(a => new LeaseAlertDtoCrud()
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
            })
            .OrderByDescending(a => a.AlertDate)
            .ToListAsync(cancellationToken);
    }
}