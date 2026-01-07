using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Users.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Users.Handlers
{
    public class SoftDeleteUserHandler : IRequestHandler<SoftDeleteUserCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public SoftDeleteUserHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == request.UserId, cancellationToken);

            if (user == null)
                return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
