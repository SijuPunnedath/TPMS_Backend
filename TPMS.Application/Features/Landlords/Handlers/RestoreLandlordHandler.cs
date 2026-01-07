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
    public class RestoreLandlordHandler : IRequestHandler<RestoreLandlordCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public RestoreLandlordHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(RestoreLandlordCommand request, CancellationToken cancellationToken)
        {
            var landlord = await _db.Landlords
                .FirstOrDefaultAsync(l => l.LandlordID == request.LandlordId, cancellationToken);

            if (landlord == null || !landlord.IsDeleted) return false; // not found or not deleted

            landlord.IsDeleted = false;
            landlord.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
