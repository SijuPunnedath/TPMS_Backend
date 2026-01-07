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
    public class SoftDeletePropertyHandler : IRequestHandler<SoftDeletePropertyCommand, bool>
    {
        private readonly TPMSDBContext _db;
        public SoftDeletePropertyHandler(TPMSDBContext db) => _db = db;
       
        public async Task<bool> Handle(SoftDeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _db.Properties
                .FirstOrDefaultAsync(p => p.PropertyID == request.PropertyId, cancellationToken);

            if (property == null || property.IsDeleted) return false;

            property.IsDeleted = true;
            property.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
