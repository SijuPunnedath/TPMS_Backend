using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.Properties.DTOs;
using TPMS.Application.Features.Properties.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Properties.Handlers
{
    public class GetAllPropertiesHandler : IRequestHandler<GetAllPropertiesQuery, IEnumerable<PropertyDto>>
    {
        private readonly TPMSDBContext _db;

        public GetAllPropertiesHandler(TPMSDBContext db) => _db = db;

        public async Task<IEnumerable<PropertyDto>> Handle(
            GetAllPropertiesQuery request,
            CancellationToken cancellationToken)
        {
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Property")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            var properties = await (
                    from p in _db.Properties.AsNoTracking()
                    where !p.IsDeleted

                    join l in _db.Landlords.AsNoTracking()
                        on p.LandlordID equals l.LandlordID into landlordGroup
                    from landlord in landlordGroup.DefaultIfEmpty()

                    select new PropertyDto
                    {
                        PropertyID = p.PropertyID,
                        PropertyName = p.PropertyName,
                        PropertyNumber = p.PropertyNumber,
                        SerialNo = p.SerialNo,
                        Type = p.Type,
                        Size = p.Size,
                        Notes = p.Notes,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,

                        LandlordID = p.LandlordID,
                        LandlordName = landlord != null ? landlord.Name : null,

                        Address = _db.Addresses
                            .Where(a =>
                                a.OwnerTypeID == ownerTypeId &&
                                a.OwnerID == p.PropertyID &&
                                a.IsPrimary)
                            .Select(a => new PropertyAddressDto
                            {
                                AddressLine1 = a.AddressLine1,
                                AddressLine2 = a.AddressLine2,
                                City = a.City,
                                State = a.State,
                                Country = a.Country,
                                PostalCode = a.PostalCode,
                                Phone1 = a.Phone1,
                                Phone2 = a.Phone2,
                                Email = a.Email,
                                IsPrimary = a.IsPrimary
                            })
                            .FirstOrDefault() ?? new PropertyAddressDto()
                    })
                .ToListAsync(cancellationToken);

            return properties;
        }

        }

 }

