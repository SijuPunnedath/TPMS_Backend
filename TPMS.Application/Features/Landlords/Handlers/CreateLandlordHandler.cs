using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.Commands;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.Lookups.Services;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Landlords.Handlers
{
    public class CreateLandlordHandler : IRequestHandler<CreateLandlordCommand, int>
    {
        private readonly TPMSDBContext _db;
        private readonly ILookupCacheService _cache;

        public CreateLandlordHandler(TPMSDBContext db,ILookupCacheService cache)
        {
            _db = db;
            _cache = cache;
        } 

        public async Task<int> Handle(CreateLandlordCommand request, CancellationToken cancellationToken)
        {
            var landlord = new Landlord
            {
                Name = request.Landlord.Name,
                Notes = request.Landlord.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Landlords.Add(landlord);
            await _db.SaveChangesAsync(cancellationToken);

            var ownerTypeId = await _db.OwnerTypes
            .Where(o => o.Name == "Landlord")
            .Select(o => o.OwnerTypeID)
            .FirstAsync(cancellationToken);

            var address = new Address
            {
                OwnerTypeID = ownerTypeId,
                OwnerID = landlord.LandlordID,
                AddressLine1 = request.Landlord.LandlordAddress.AddressLine1,
                City = request.Landlord.LandlordAddress.City,
                State = request.Landlord.LandlordAddress.State,
                Country = request.Landlord.LandlordAddress.Country,
                PostalCode = request.Landlord.LandlordAddress.PostalCode,
                Phone1 = request.Landlord.LandlordAddress.Phone1,
                Email = request.Landlord.LandlordAddress.Email,
                IsPrimary = true
            };

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync(cancellationToken);
            await _cache.RefreshLandlordsAsync();
            return landlord.LandlordID;
        }
    }
}
