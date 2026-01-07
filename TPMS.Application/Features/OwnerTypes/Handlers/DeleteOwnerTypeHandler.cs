using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.OwnerTypes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.OwnerTypes.Handlers;

public class DeleteOwnerTypeHandler : IRequestHandler<DeleteOwnerTypeCommand, bool>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _cache;

    public DeleteOwnerTypeHandler(TPMSDBContext db,IOwnerTypeCacheService cache)
    {
        _db = db; 
        _cache = cache;
    } 

    public async Task<bool> Handle(DeleteOwnerTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.OwnerTypes.FindAsync(new object?[] { request.OwnerTypeID }, cancellationToken);
        if (entity == null) return false;

        _db.OwnerTypes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        
        _cache.RefreshCache();
        return true;
    } 
}