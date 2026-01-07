using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class RestoreLeaseHandler : IRequestHandler<RestoreLeaseCommand, bool>
    {
        private readonly TPMSDBContext _db;
        public RestoreLeaseHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(RestoreLeaseCommand request, CancellationToken cancellationToken)
        {
            var lease = await _db.Leases.FirstOrDefaultAsync(l => l.LeaseID == request.LeaseId, cancellationToken);
            if (lease == null || !lease.IsDeleted) return false;

            lease.IsDeleted = false;
            lease.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
