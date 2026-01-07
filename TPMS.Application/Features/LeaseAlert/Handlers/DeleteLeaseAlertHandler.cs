using MediatR;
using TPMS.Application.Features.LeaseAlert.Commands;
using  System.Threading;
using System.Threading.Tasks;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.LeaseAlert.Handlers;

public class DeleteLeaseAlertHandler :IRequestHandler<DeleteLeaseAlertCommand, bool>
{
    private readonly TPMSDBContext _db;
    public DeleteLeaseAlertHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(DeleteLeaseAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await _db.LeaseAlerts.FindAsync(new object?[] { request.AlertID }, cancellationToken);
        if (alert == null) return false;

        _db.LeaseAlerts.Remove(alert);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}