using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Tenants.DTOs;
using TPMS.Application.Features.Tenants.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Tenants.Handlers;

public class GetTenantLookupHandler : IRequestHandler<GetTenantLookupQuery, List<TenantLookupDto>>
{
    private readonly TPMSDBContext _db;

    public GetTenantLookupHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<List<TenantLookupDto>> Handle(GetTenantLookupQuery request, CancellationToken cancellationToken)
    {
        return await _db.Tenants
            .Where(t => !t.IsDeleted)
            .OrderBy(t => t.Name)
            .Select(t => new TenantLookupDto
            {
                TenantID = t.TenantID,
                Name = t.Name!
            })
            .ToListAsync(cancellationToken);
    }
}