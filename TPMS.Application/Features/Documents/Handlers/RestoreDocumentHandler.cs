using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Documents.Handlers
{
    public class RestoreDocumentHandler : IRequestHandler<RestoreDocumentCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public RestoreDocumentHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(RestoreDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _db.Documents.FirstOrDefaultAsync(d => d.DocumentID == request.DocumentID, cancellationToken);

            if (doc == null)
                return false;

            // Restore soft-deleted document
            doc.IsDeleted = false;
            doc.IsActive = true;
            _db.Documents.Update(doc);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
