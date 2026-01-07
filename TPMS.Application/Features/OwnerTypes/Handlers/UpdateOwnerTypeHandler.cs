using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.OwnerTypes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.OwnerTypes.Handlers;

public class UpdateOwnerTypeHandler : IRequestHandler<UpdateOwnerTypeCommand, bool>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _cache;

    public UpdateOwnerTypeHandler(TPMSDBContext db,IOwnerTypeCacheService cache)
    {
        _db = db;
        _cache = cache;

    } 
    public async Task<bool> Handle(UpdateOwnerTypeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.OwnerType;
        var entity = await _db.OwnerTypes.FirstOrDefaultAsync(o => o.OwnerTypeID == dto.OwnerTypeID, cancellationToken);
        if (entity == null) return false;

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.IsActive = dto.IsActive;
        entity.UpdatedBy = request.UpdatedBy;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        
        _cache.RefreshCache();
        return true;
    }  
}