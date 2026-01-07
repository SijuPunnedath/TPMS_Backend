using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Application.Features.RentSchedules.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RentSchedules.Handlers;

public class GetRentScheduleByIdHandler : IRequestHandler<GetRentScheduleByIdQuery, RentScheduleDtoCrud?>
{
    private readonly TPMSDBContext _db;
    public GetRentScheduleByIdHandler(TPMSDBContext db) => _db = db;

    public async Task<RentScheduleDtoCrud?> Handle(GetRentScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _db.RentSchedules.FirstOrDefaultAsync(r => r.ScheduleID == request.ScheduleID, cancellationToken);
        if (entity == null) return null;

        return new RentScheduleDtoCrud()
        {
            ScheduleID = entity.ScheduleID,
            LeaseID = entity.LeaseID,
            DueDate = entity.DueDate,
            Amount = entity.Amount,
            Status = entity.Status,
            IsPaid = entity.IsPaid,
            PaidDate = entity.PaidDate,
            Penalty = entity.Penalty
        };
    }
}