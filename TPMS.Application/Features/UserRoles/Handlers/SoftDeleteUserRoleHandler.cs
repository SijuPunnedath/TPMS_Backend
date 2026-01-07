using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.UserRoles.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.UserRoles.Handlers
{
    public class SoftDeleteUserRoleHandler : IRequestHandler<SoftDeleteUserRoleCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public SoftDeleteUserRoleHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(SoftDeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userRole = await _db.UserRoles.FirstOrDefaultAsync(u => u.UserRoleID == request.UserRoleID, cancellationToken);
            if (userRole == null)
                return false;

            userRole.IsActive = false;
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
