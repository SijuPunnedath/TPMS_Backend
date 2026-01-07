using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.OwnerTypes.DTOs;
using TPMS.Application.Features.OwnerTypes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.OwnerTypes.Handlers;

public class GetOwnerTypeByIdHandler : IRequestHandler<GetOwnerTypeByIdQuery, OwnerTypeDto?>
{
    private readonly TPMSDBContext _db;
    public GetOwnerTypeByIdHandler(TPMSDBContext db) => _db = db;

    public async Task<OwnerTypeDto?> Handle(GetOwnerTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _db.OwnerTypes.FirstOrDefaultAsync(o => o.OwnerTypeID == request.OwnerTypeID, cancellationToken);
        if (entity == null) return null;

        return new OwnerTypeDto
        {
            OwnerTypeID = entity.OwnerTypeID,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}