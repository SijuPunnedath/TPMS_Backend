using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Properties.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Properties.Handlers
{
    public class DeletePropertyHandler :  IRequestHandler<DeletePropertyCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public DeletePropertyHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _db.Properties
                .FirstOrDefaultAsync(p => p.PropertyID == request.PropertyId, cancellationToken);

            if (property == null) return false;

            _db.Properties.Remove(property);

            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Property")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            var addresses = await _db.Addresses
                .Where(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == property.PropertyID)
                .ToListAsync(cancellationToken);

            _db.Addresses.RemoveRange(addresses);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
