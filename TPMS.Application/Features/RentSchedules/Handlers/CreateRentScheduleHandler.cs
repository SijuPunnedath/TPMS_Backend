using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.RentSchedules.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RentSchedules.Handlers;

public class CreateRentScheduleHandler : IRequestHandler<CreateRentScheduleCommand, int>
{
    private readonly TPMSDBContext _db;
    public CreateRentScheduleHandler(TPMSDBContext db) => _db = db;

    public async Task<int> Handle(CreateRentScheduleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RentSchedule;
        var entity = new RentSchedule
        {
            LeaseID = dto.LeaseID,
            DueDate = dto.DueDate,
            Amount = dto.Amount,
            Status = dto.Status,
            IsPaid = dto.IsPaid,
            PaidDate = dto.PaidDate,
            Penalty = dto.Penalty
        };

        _db.RentSchedules.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.ScheduleID;
    }
}