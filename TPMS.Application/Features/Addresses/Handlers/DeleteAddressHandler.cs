using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Addresses.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Addresses.Handlers;

public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, bool>
{
    private readonly TPMSDBContext _db;
    public DeleteAddressHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _db.Addresses.FindAsync(new object?[] { request.AddressID }, cancellationToken);
        if (address == null) return false;

        _db.Addresses.Remove(address);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
