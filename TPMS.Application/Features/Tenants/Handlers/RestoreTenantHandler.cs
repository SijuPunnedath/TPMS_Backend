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
    public class RestoreTenantHandler : IRequestHandler<RestoreTenantCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public RestoreTenantHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(RestoreTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _db.Tenants
                .FirstOrDefaultAsync(t => t.TenantID == request.TenantId, cancellationToken);

            if (tenant == null || !tenant.IsDeleted) return false;

            tenant.IsDeleted = false;
            tenant.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
