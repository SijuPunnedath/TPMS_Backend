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
    public class UpdateLandlordHandler : IRequestHandler<UpdateLandlordCommand, bool>
    {
        private readonly TPMSDBContext _db;
        private readonly ILookupCacheService _cache;
        public UpdateLandlordHandler(TPMSDBContext db, ILookupCacheService cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<bool> Handle(UpdateLandlordCommand request, CancellationToken cancellationToken)
        {
            var landlord = await _db.Landlords.FirstOrDefaultAsync(
                l => l.LandlordID == request.LandlordId, cancellationToken);

            if (landlord == null) return false;

            landlord.Name = request.Landlord.Name;
            landlord.Notes = request.Landlord.Notes;
            landlord.UpdatedAt = DateTime.UtcNow;

            // Find OwnerTypeID for Landlord
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Landlord")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            var address = await _db.Addresses
                .FirstOrDefaultAsync(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == landlord.LandlordID && a.IsPrimary, cancellationToken);

            if (address != null)
            {
                address.AddressLine1 = request.Landlord.LandlordAddress.AddressLine1;
                address.AddressLine2 = request.Landlord.LandlordAddress.AddressLine2;
                address.City = request.Landlord.LandlordAddress.City;
                address.State = request.Landlord.LandlordAddress.State;
                address.Country = request.Landlord.LandlordAddress.Country;
                address.PostalCode = request.Landlord.LandlordAddress.PostalCode;
                address.Phone1 = request.Landlord.LandlordAddress.Phone1;
                address.Phone2 = request.Landlord.LandlordAddress.Phone2;
                address.Email = request.Landlord.LandlordAddress.Email;
            }

            await _db.SaveChangesAsync(cancellationToken);
            await _cache.RefreshLandlordsAsync();
            return true;
        }
    }
}
