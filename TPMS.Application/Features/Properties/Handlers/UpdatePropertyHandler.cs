using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using TPMS.Application.Features.Properties.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Properties.Handlers
{
    public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public UpdatePropertyHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _db.Properties
                .FirstOrDefaultAsync(p => p.PropertyID == request.PropertyId && !p.IsDeleted, cancellationToken);

            if (property == null) return false;

            property.PropertyName = request.Property.PropertyName;
            property.SerialNo = request.Property.SerialNo;
            property.Type = request.Property.Type;
            property.Size = request.Property.Size;
            property.Notes = request.Property.Notes;
            property.UpdatedAt = DateTime.UtcNow;

            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Property")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            var address = await _db.Addresses
                .FirstOrDefaultAsync(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == property.PropertyID, cancellationToken);

            if (address != null)
            {
                address.AddressLine1 = request.Property.Address.AddressLine1;
                address.AddressLine2 = request.Property.Address.AddressLine2;
                address.City = request.Property.Address.City;
                address.State = request.Property.Address.State;
                address.Country = request.Property.Address.Country;
                address.PostalCode = request.Property.Address.PostalCode;
                address.Phone1 = request.Property.Address.Phone1;
                address.Phone2 = request.Property.Address.Phone2;
                address.Email = request.Property.Address.Email;
                address.IsPrimary = true;
            }

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
