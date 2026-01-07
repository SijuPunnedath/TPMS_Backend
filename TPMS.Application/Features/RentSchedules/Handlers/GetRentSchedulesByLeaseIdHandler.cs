using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Application.Features.RentSchedules.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RentSchedules.Handlers;

public class GetRentSchedulesByLeaseIdHandler : IRequestHandler<GetRentSchedulesByLeaseIdQuery, List<RentScheduleDtoCrud>>
{
    private readonly TPMSDBContext _db;
    public GetRentSchedulesByLeaseIdHandler(TPMSDBContext db) => _db = db;

    public async Task<List<RentScheduleDtoCrud>> Handle(GetRentSchedulesByLeaseIdQuery request, CancellationToken cancellationToken)
    {
        return await _db.RentSchedules
            .Where(r => r.LeaseID == request.LeaseID)
            .Select(r => new RentScheduleDtoCrud()
            {
                ScheduleID = r.ScheduleID,
                LeaseID = r.LeaseID,
                DueDate = r.DueDate,
                Amount = r.Amount,
                Status = r.Status,
                IsPaid = r.IsPaid,
                PaidDate = r.PaidDate,
                Penalty = r.Penalty
            })
            .OrderBy(r => r.DueDate)
            .ToListAsync(cancellationToken);
    }
}