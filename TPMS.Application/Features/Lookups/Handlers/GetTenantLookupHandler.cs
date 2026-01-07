using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Lookups.DTOs;
using TPMS.Application.Features.Lookups.Queries;
using TPMS.Application.Features.Lookups.Services;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Lookups.Handlers;

public class GetTenantLookupHandler : IRequestHandler<GetTenantLookupQuery, List<TenantLookupDto>>
{
    //private readonly TPMSDBContext _db;
   // public GetTenantLookupHandler(TPMSDBContext db) => _db = db;
    
    private readonly ILookupCacheService _cache;
    public GetTenantLookupHandler(TPMSDBContext db,ILookupCacheService cache)
    {
        _cache = cache;
    }

    public async Task<List<TenantLookupDto>> Handle(GetTenantLookupQuery request, CancellationToken cancellationToken)
    {

        return await _cache.GetTenantsAsync();
        /*  return await _db.Tenants
              .Where(t => !t.IsDeleted)
              .OrderBy(t => t.Name)
              .Select(t => new TenantLookupDto
              {
                  TenantID = t.TenantID,
                  Name = t.Name!
              })
              .ToListAsync(cancellationToken);*/
    }
}