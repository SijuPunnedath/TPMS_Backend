using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Deposit.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Deposit.Handlers;

public class DeleteDepositMasterHandler :
    IRequestHandler<DeleteDepositMasterCommand, bool>
{
    private readonly TPMSDBContext _db;
    public DeleteDepositMasterHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(DeleteDepositMasterCommand request, CancellationToken token)
    {
        var master = await _db.DepositMasters
            .FirstOrDefaultAsync(dm => dm.DepositMasterID == request.DepositMasterID, token);

        if (master == null)
            return false;

        _db.DepositMasters.Remove(master);
        await _db.SaveChangesAsync(token);

        return true;
    }
}
