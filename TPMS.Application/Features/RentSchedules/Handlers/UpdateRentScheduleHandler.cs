using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RentSchedules.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RentSchedules.Handlers;

public class UpdateRentScheduleHandler : IRequestHandler<UpdateRentScheduleCommand, bool>
{
    private readonly TPMSDBContext _db;
    public UpdateRentScheduleHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(UpdateRentScheduleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RentSchedule;
        var entity = await _db.RentSchedules.FirstOrDefaultAsync(r => r.ScheduleID == dto.ScheduleID, cancellationToken);
        if (entity == null) return false;

        entity.LeaseID = dto.LeaseID;
        entity.DueDate = dto.DueDate;
        entity.Amount = dto.Amount;
        entity.Status = dto.Status;
        entity.IsPaid = dto.IsPaid;
        entity.PaidDate = dto.PaidDate;
        entity.Penalty = dto.Penalty;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}