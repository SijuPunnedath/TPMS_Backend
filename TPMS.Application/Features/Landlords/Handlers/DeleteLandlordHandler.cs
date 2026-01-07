using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.Commands;
using TPMS.Application.Features.Lookups.Services;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Landlords.Handlers
{
    public class DeleteLandlordHandler : IRequestHandler<DeleteLandlordCommand, bool>
    {
        private readonly TPMSDBContext _db;
        private readonly ILookupCacheService _cache;

        public DeleteLandlordHandler(TPMSDBContext db, ILookupCacheService cache)
        {
            _db = db;
            _cache = cache;
        }


        public async Task<bool> Handle(DeleteLandlordCommand request, CancellationToken cancellationToken)
        {
            var landlord = await _db.Landlords
                .FirstOrDefaultAsync(l => l.LandlordID == request.LandlordId, cancellationToken);

            if (landlord == null) return false;

            // Find OwnerTypeID
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Landlord")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            // Delete addresses
            var addresses = await _db.Addresses
                .Where(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == landlord.LandlordID)
                .ToListAsync(cancellationToken);

            _db.Addresses.RemoveRange(addresses);

            // Delete landlord
            _db.Landlords.Remove(landlord);

            await _db.SaveChangesAsync(cancellationToken);
            await _cache.RefreshLandlordsAsync();
            return true;
        }
    }
}
