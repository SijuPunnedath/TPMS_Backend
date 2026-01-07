using System;
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

public class GetPendingLeaseAlertsHandler : IRequestHandler<GetPendingLeaseAlertsQuery, List<LeaseAlertDtoCrud>>
{
    private readonly TPMSDBContext _db;
    public GetPendingLeaseAlertsHandler(TPMSDBContext db) => _db = db;

    public async Task<List<LeaseAlertDtoCrud>> Handle(GetPendingLeaseAlertsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        return await _db.LeaseAlerts
            .Where(a => a.Status == "Pending" && a.AlertDate <= now && !a.IsDeleted)
            .Select(a => new LeaseAlertDtoCrud()
            {
                AlertID = a.AlertID,
                LeaseID = a.LeaseID,
                AlertType = a.AlertType,
                AlertDate = a.AlertDate,
                Status = a.Status,
                Message = a.Message,
                DeliveryMethod = a.DeliveryMethod,
                RetryCount = a.RetryCount
            })
            .ToListAsync(cancellationToken);
    }  
}