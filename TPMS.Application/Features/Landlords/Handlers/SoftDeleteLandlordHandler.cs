using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Landlords.Handlers
{
    public class SoftDeleteLandlordHandler : IRequestHandler<SoftDeleteLandlordCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public SoftDeleteLandlordHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(SoftDeleteLandlordCommand request, CancellationToken cancellationToken)
        {
            var landlord = await _db.Landlords
            .FirstOrDefaultAsync(l => l.LandlordID == request.LandlordId, cancellationToken);

            if (landlord == null || landlord.IsDeleted) return false;

            landlord.IsDeleted = true;
            landlord.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
