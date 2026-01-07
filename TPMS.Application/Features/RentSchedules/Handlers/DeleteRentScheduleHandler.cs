using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.RentSchedules.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RentSchedules.Handlers;

public class DeleteRentScheduleHandler : IRequestHandler<DeleteRentScheduleCommand, bool>
{
    private readonly TPMSDBContext _db;
    public DeleteRentScheduleHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(DeleteRentScheduleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.RentSchedules.FindAsync(new object?[] { request.ScheduleID }, cancellationToken);
        if (entity == null) return false;

        _db.RentSchedules.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}