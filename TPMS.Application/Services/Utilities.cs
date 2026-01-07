using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Services;

public class Utilities
{
    public TPMSDBContext _db { get; set; }

    public Utilities(TPMSDBContext db)
    {
        _db = db;
    }
  
    public async Task<int> GetOwnerTypeIdOrThrowAsync(string ownerTypeName, CancellationToken cancellationToken)
    {
        var id = await _db.OwnerTypes
            .Where(o => o.Name == ownerTypeName)
            .Select(o => (int?)o.OwnerTypeID)
            .FirstOrDefaultAsync(cancellationToken);

        if (id == null)
            throw new InvalidOperationException($"OwnerType '{ownerTypeName}' not found. Please seed it in the OwnerTypes table.");

        return id.Value;
    }

}