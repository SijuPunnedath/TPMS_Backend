using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.OwnerTypes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.OwnerTypes.Handlers;

public class RestoreOwnerTypeHandler : IRequestHandler<RestoreOwnerTypeCommand, bool>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _cache;

    public RestoreOwnerTypeHandler(TPMSDBContext db, IOwnerTypeCacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<bool> Handle(RestoreOwnerTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.OwnerTypes.FirstOrDefaultAsync(o => o.OwnerTypeID == request.OwnerTypeID, cancellationToken);
        if (entity == null) return false;

        entity.IsDeleted = false;
        entity.IsActive = true;
        entity.UpdatedBy = request.UpdatedBy;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        _cache.RefreshCache();

        return true;
    }
}
