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
    public class DeleteLeaseHandler : IRequestHandler<DeleteLeaseCommand, bool>
    {
        private readonly TPMSDBContext _db;
        public DeleteLeaseHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(DeleteLeaseCommand request, CancellationToken cancellationToken)
        {
            var lease = await _db.Leases
                .Include(l => l.RentSchedules)
                .FirstOrDefaultAsync(l => l.LeaseID == request.LeaseId, cancellationToken);

            if (lease == null) return false;

            _db.RentSchedules.RemoveRange(lease.RentSchedules);
            _db.Leases.Remove(lease);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
