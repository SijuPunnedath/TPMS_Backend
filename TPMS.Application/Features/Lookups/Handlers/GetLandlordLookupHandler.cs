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

public class GetLandlordLookupHandler 
    : IRequestHandler<GetLandlordLookupQuery, List<LandlordLookupDto>>
{
   /* private readonly TPMSDBContext _db;

    public GetLandlordLookupHandler(TPMSDBContext db) => _db = db; */
   
   private readonly ILookupCacheService _cache;

   public GetLandlordLookupHandler(ILookupCacheService cache)
   {
       _cache = cache;
   }

    public async Task<List<LandlordLookupDto>> Handle(GetLandlordLookupQuery request, CancellationToken cancellationToken)
    {
        
        return await _cache.GetLandlordsAsync();
        
      /*  return await _db.Landlords
            .Where(l => !l.IsDeleted)
            .OrderBy(l => l.Name)
            .Select(l => new LandlordLookupDto 
            {
                LandlordID = l.LandlordID,
                Name = l.Name!
            })
            .ToListAsync(cancellationToken); */
    }
}