using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Tenants.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Tenants.Handlers
{
    public class DeleteTenantHandler : IRequestHandler<DeleteTenantCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public DeleteTenantHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _db.Tenants
                .FirstOrDefaultAsync(t => t.TenantID == request.TenantId, cancellationToken);

            if (tenant == null) return false;

            _db.Tenants.Remove(tenant);

            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Tenant")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            var addresses = await _db.Addresses
                .Where(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == tenant.TenantID)
                .ToListAsync(cancellationToken);

            _db.Addresses.RemoveRange(addresses);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
