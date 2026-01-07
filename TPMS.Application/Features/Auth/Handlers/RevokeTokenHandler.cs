using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Auth.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Auth.Handlers
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public RevokeTokenHandler(TPMSDBContext db) { _db = db; }

        public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == request.Token, cancellationToken);
            if (token == null) return false;
            token.Revoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.CreatedByIp = request.IpAddress;
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
