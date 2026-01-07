using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Common.Services;
using TPMS.Application.Features.OwnerTypes.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.OwnerTypes.Handlers;

public class CreateOwnerTypeHandler : IRequestHandler<CreateOwnerTypeCommand, int>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _cache;
    
    public CreateOwnerTypeHandler(TPMSDBContext db, IOwnerTypeCacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<int> Handle(CreateOwnerTypeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.OwnerType;
        var entity = new OwnerType
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedBy
        };

        _db.OwnerTypes.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        _cache.RefreshCache();
        return entity.OwnerTypeID;
    }
}