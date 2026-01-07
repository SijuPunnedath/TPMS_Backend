using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.RolePermissions.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RolePermissions.Handlers
{
    public class DeleteRolePermissionHandler : IRequestHandler<DeleteRolePermissionCommand, bool>
    {
        private readonly TPMSDBContext _db;
        public DeleteRolePermissionHandler(TPMSDBContext db) => _db = db;

        public async Task<bool> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var rp = await _db.RolePermissions.FindAsync(new object?[] { request.RolePermissionID }, cancellationToken);
            if (rp == null) return false;

            _db.RolePermissions.Remove(rp);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
