using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Tenants.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Tenants.Handlers
{
   public class UpdateTenantHandler : IRequestHandler<UpdateTenantCommand, bool>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _ownerTypeCache;

    public UpdateTenantHandler(TPMSDBContext db, IOwnerTypeCacheService ownerTypeCache)
    {
        _db = db;
        _ownerTypeCache = ownerTypeCache;
    }

    public async Task<bool> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _db.Tenants
            .FirstOrDefaultAsync(t => t.TenantID == request.TenantID && !t.IsDeleted, cancellationToken);

        if (tenant == null)
            throw new KeyNotFoundException("Tenant not found or deleted.");

        // Begin transaction
        using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // ✅ Update tenant fields
            tenant.Name = request.Tenant.Name ?? tenant.Name;
            tenant.Notes = request.Tenant.Notes ?? tenant.Notes;
            tenant.UpdatedAt = DateTime.UtcNow;

            // ✅ If address info provided, update address too
            if (request.Tenant.Address != null)
            {
                int tenantTypeId = _ownerTypeCache.GetOwnerTypeId("Tenant");

                var address = await _db.Addresses
                    .FirstOrDefaultAsync(a => a.OwnerTypeID == tenantTypeId && a.OwnerID == tenant.TenantID, cancellationToken);

                if (address != null)
                {
                    // Update existing address
                    address.AddressLine1 = request.Tenant.Address.AddressLine1 ?? address.AddressLine1;
                    address.AddressLine2 = request.Tenant.Address.AddressLine2 ?? address.AddressLine2;
                    address.City = request.Tenant.Address.City ?? address.City;
                    address.State = request.Tenant.Address.State ?? address.State;
                    address.Country = request.Tenant.Address.Country ?? address.Country;
                    address.PostalCode = request.Tenant.Address.PostalCode ?? address.PostalCode;
                    address.Phone1 = request.Tenant.Address.Phone1 ?? address.Phone1;
                    address.Phone2 = request.Tenant.Address.Phone2 ?? address.Phone2;
                    address.Email = request.Tenant.Address.Email ?? address.Email;
                    //address.IsPrimary = request.Tenant.Address.IsPrimary ?? address.IsPrimary;  
                }
                else
                {
                    // Create new address if missing
                    var newAddress = new Address
                    {
                        OwnerTypeID = tenantTypeId,
                        OwnerID = tenant.TenantID,
                        AddressLine1 = request.Tenant.Address.AddressLine1,
                        AddressLine2 = request.Tenant.Address.AddressLine2,
                        City = request.Tenant.Address.City,
                        State = request.Tenant.Address.State,
                        Country = request.Tenant.Address.Country,
                        PostalCode = request.Tenant.Address.PostalCode,
                        Phone1 = request.Tenant.Address.Phone1,
                        Phone2 = request.Tenant.Address.Phone2,
                        Email = request.Tenant.Address.Email,
                        IsPrimary = true
                    };
                    _db.Addresses.Add(newAddress);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

}
