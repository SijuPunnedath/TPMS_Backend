using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Properties.Commands;
using TPMS.Application.Features.Properties.DTOs;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

public class CreatePropertyHandler
    : IRequestHandler<CreatePropertyCommand, PropertyDto>
{
    private readonly TPMSDBContext _db;
    private readonly IOwnerTypeCacheService _ownerTypeCache;

    public CreatePropertyHandler(
        TPMSDBContext db,
        IOwnerTypeCacheService ownerTypeCache)
    {
        _db = db;
        _ownerTypeCache = ownerTypeCache;
    }

    public async Task<PropertyDto> Handle(
        CreatePropertyCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // --------------------------------------------------
            // 1 Create Property
            // --------------------------------------------------
            var property = new Property
            {
                PropertyName = dto.PropertyName.Trim(),
                PropertyNumber = dto.PropertyNumber,
                SerialNo = dto.SerialNo,
                Type = dto.Type,
                Size = dto.Size,
                Notes = dto.Notes,
                LandlordID = dto.LandlordID,
                Status = PropertyStatus.Draft,
                ActiveInboundLeaseId = null,
                ActiveOutboundLeaseId = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Properties.Add(property);
            await _db.SaveChangesAsync(cancellationToken);

            // --------------------------------------------------
            // 2 Create Address
            // --------------------------------------------------
            int propertyOwnerTypeId =
                _ownerTypeCache.GetOwnerTypeId("Property");

            var address = new Address
            {
                OwnerTypeID = propertyOwnerTypeId,
                OwnerID = property.PropertyID,
                AddressLine1 = dto.Address.AddressLine1,
                AddressLine2 = dto.Address.AddressLine2,
                City = dto.Address.City,
                State = dto.Address.State,
                Country = dto.Address.Country,
                PostalCode = dto.Address.PostalCode,
                Phone1 = dto.Address.Phone1,
                Email = dto.Address.Email,
                IsPrimary = true
            };

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync(cancellationToken);

            // --------------------------------------------------
            // 3 Reload with Navigation (Landlord)
            // --------------------------------------------------
            var savedProperty = await _db.Properties
                .Include(p => p.Landlord)
                .FirstAsync(
                    p => p.PropertyID == property.PropertyID,
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            // --------------------------------------------------
            // 4 Map DTO
            // --------------------------------------------------
            return MapToDto(savedProperty, address);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    // ------------------------------------------------------
    // Mapping Logic (isolated & reusable)
    // ------------------------------------------------------
    private static PropertyDto MapToDto(
        Property property,
        Address address)
    {
        return new PropertyDto
        {
            PropertyID = property.PropertyID,
            PropertyName = property.PropertyName,
            PropertyNumber = property.PropertyNumber,
            SerialNo = property.SerialNo,
            Type = property.Type,
            Size = property.Size,
            Notes = property.Notes,
            Status = property.Status,
            ActiveInboundLeaseId = property.ActiveInboundLeaseId,
            ActiveOutboundLeaseId = property.ActiveOutboundLeaseId,
            LandlordID = property.LandlordID,
            LandlordName = property.Landlord?.Name,

            Address = new PropertyAddressDto
            {
                AddressID = address.AddressID,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,
                Phone1 = address.Phone1,
                Email = address.Email,
                IsPrimary = address.IsPrimary
            },

            CreatedAt = property.CreatedAt,
            UpdatedAt = property.UpdatedAt
        };
    }
}
