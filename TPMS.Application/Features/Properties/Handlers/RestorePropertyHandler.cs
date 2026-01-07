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
    public class RestorePropertyHandler : IRequestHandler<RestorePropertyCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public RestorePropertyHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(RestorePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _db.Properties
                .FirstOrDefaultAsync(p => p.PropertyID == request.PropertyId, cancellationToken);

            if (property == null || !property.IsDeleted) return false;

            property.IsDeleted = false;
            property.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
